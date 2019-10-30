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
        private double? itemWidth = 58;
        private double? itemHeight = 58;

        //public IsotopeDataModel CurrentIsotopeData { get => currentIsotopeData; set { currentIsotopeData = value; } }
        public ObservableCollection<Tuple<IsotopeModel, ElementDataModel>> IsotopeDecayChain { get; private set; }
        public double? ItemWidth { get => itemWidth; set { itemWidth = value; SetPropertyChanged(this, "ItemWidth"); } }
        public double? ItemHeight { get => itemHeight; set { itemHeight = value; SetPropertyChanged(this, "ItemHeight"); } }

        public DecayChainViewModel() {
            currentIsotopeData = new IsotopeDataModel("", new IsotopeModel[0]);
            decayChains = new List<List<(IsotopeModel, int)>>();
            IsotopeDecayChain = new ObservableCollection<Tuple<IsotopeModel, ElementDataModel>>();
        }

        //make multi-threaded
        public void SetupDecayChain(IsotopeDataModel isotopeData) {
            currentIsotopeData = isotopeData;
            decayChains.Clear();
            IsotopeDecayChain.Clear();
            if (currentIsotopeData.Isotopes.Length < 1)
                return;

            foreach (var isotope in currentIsotopeData.Isotopes) {
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
            foreach (var decayChainItem in decayChains[0].Where(isotopeModel => isotopeModel.Item2 == index)) {
                ElementViewModel.ElementDataDictionary.TryGetValue(decayChainItem.Item1.Symbol, out ElementDataModel element);
                IsotopeDecayChain.Add(new Tuple<IsotopeModel, ElementDataModel>(decayChainItem.Item1, element));
            }
        }
    }
}
