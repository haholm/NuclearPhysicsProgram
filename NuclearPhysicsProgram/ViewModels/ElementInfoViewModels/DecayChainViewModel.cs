using NuclearPhysicsProgram.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuclearPhysicsProgram.ViewModels.ElementInfoViewModels {
    public class DecayChainViewModel : PropertyHandler.NotifyPropertyChanged {
        private IsotopeDataModel currentIsotopeData;
        private List<List<(IsotopeModel isotope, int index)>> decayChains;
        private double? itemWidth = 58;
        private double? itemHeight = 58;

        ///Related to fix for multiple-decays-of-same-isotope-but-different-decay-type problem causing
        ///multiple consecutive isotopes of the same symbol to show up in the same decay chain instead
        ///of showing up as two different decay chains respectively
        int lastIndex;
        bool conflict;
        bool rerun;

        //public IsotopeDataModel CurrentIsotopeData { get => currentIsotopeData; set { currentIsotopeData = value; } }
        public ObservableCollection<IsotopeModel> IsotopeDecayChain { get; private set; }
        public double? ItemWidth { get => itemWidth; set { itemWidth = value; SetPropertyChanged(this, "ItemWidth"); } }
        public double? ItemHeight { get => itemHeight; set { itemHeight = value; SetPropertyChanged(this, "ItemHeight"); } }

        public DecayChainViewModel() {
            currentIsotopeData = new IsotopeDataModel("", new IsotopeModel[0]);
            decayChains = new List<List<(IsotopeModel, int)>>();
            IsotopeDecayChain = new ObservableCollection<IsotopeModel>();
        }

        //make multi-threaded
        public void SetupDecayChain(IsotopeDataModel isotopeData) {
            currentIsotopeData = isotopeData;
            if (currentIsotopeData.Isotopes.Length < 1)
                return;

            decayChains.Clear();
            lastIndex = -1;

            foreach (var isotope in currentIsotopeData.Isotopes) {
                decayChains.Add(new List<(IsotopeModel, int)>());
                AnalyzeDecays(isotope, 0, null);   //analyze first base isotope of decay tree
                if (!conflict) {
                    lastIndex = -1;
                    continue;
                }

                rerun = true;
                decayChains.Add(new List<(IsotopeModel, int)>());
                //analyze decays of isotope again but on new chain and analyzing 
                AnalyzeDecays(isotope, 0, null);
                lastIndex = -1;
                conflict = false; 
                rerun = false;
            }

            ConstructSortedDecayChain(0);
        }

        private void AnalyzeDecays(IsotopeModel isotope, int index, string isotopeDecayType) {
            if (lastIndex == index) {
                conflict = true;
                return;
            }

            lastIndex = index;
            decayChains.Last().Add((isotope, index));  //add isotope as branch to decay tree

            if (isotope.Decays.Length < 1)  //does isotope contain any decays?
                return;

            for (int i = 0; i < isotope.Decays.Length; i++) {  //iterate through decays of isotope
                if (isotope.Decays[i].ProductSymbol == "-")
                    return;

                var nextIsotope = ElementViewModel.GetIsotope(isotope.Decays[i].ProductSymbol, isotope.Decays[i].MassNumber);
                AnalyzeDecays(nextIsotope, index + 1, isotope.Decays[rerun && nextIsotope == isotope ? i + 1 : i].Type);  //rerun && nextIsotope == isotope ? i + 1 : i to get the second decay in the second decay chain
            }
        }

        private void ConstructSortedDecayChain(int decayChainIndex) {
            IsotopeDecayChain.Clear();
            var decayChain = decayChains[decayChainIndex];
            for (int i = 0; i < decayChain.Count; i++) {
                var decayChainItem = decayChain.Where(isotopeModel => isotopeModel.index == i).First();
                IsotopeDecayChain.Add(decayChainItem.isotope);
            }
        }
    }
}
