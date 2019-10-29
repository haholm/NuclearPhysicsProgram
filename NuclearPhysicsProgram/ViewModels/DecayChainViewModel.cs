using NuclearPhysicsProgram.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuclearPhysicsProgram.ViewModels {
    public class DecayChainViewModel : PropertyHandler.NotifyPropertyChanged {
        private IsotopeDataModel currentIsotopeData;
        private List<List<(IsotopeModel, int)>> decayChains;

        public IsotopeDataModel CurrentIsotopeData { get => currentIsotopeData; set { currentIsotopeData = value; SetupDecayChain(); } }
        public ObservableCollection<IsotopeModel> IsotopeDecayChain { get; private set; }

        public DecayChainViewModel() {
            currentIsotopeData = new IsotopeDataModel("", new IsotopeModel[0]);
            decayChains = new List<List<(IsotopeModel, int)>>();
            IsotopeDecayChain = new ObservableCollection<IsotopeModel>();
        }

        //make multi-threaded
        private void SetupDecayChain() {
            decayChains.Clear();
            IsotopeDecayChain.Clear();
            if (CurrentIsotopeData.Isotopes.Length < 1)
                return;

            foreach (var isotope in CurrentIsotopeData.Isotopes) {
                decayChains.Add(new List<(IsotopeModel, int)>());
                AnalyzeDecays(isotope, 0);   //analyze first base isotope of decay tree
            }

            for (int i = 0; i < decayChains[0].Count; i++)
                ConstructSortedDecayChain(i);
        }

        private void AnalyzeDecays(IsotopeModel isotope, int index) {
            decayChains.Last().Add((isotope, index));  //add isotope as branch to decay tree
            if (isotope.Decays.Length < 1)  //does isotope contain any decays?
                return;

            for (int i = 0; i < isotope.Decays.Length; i++) {  //iterate through decays of isotope
                if (isotope.Decays[i].ProductSymbol == "-")
                    break;  ///hmm should this break;

                var nextIsotope = ElementViewModel.GetIsotope(isotope.Decays[i].ProductSymbol, isotope.Decays[i].MassNumber);
                AnalyzeDecays(nextIsotope, index + 1);
            }
        }

        private void ConstructSortedDecayChain(int index) {
            foreach (var decayChainItem in decayChains[0].Where(isotopeModel => isotopeModel.Item2 == index))
                IsotopeDecayChain.Add(decayChainItem.Item1);
        }
    }
}
