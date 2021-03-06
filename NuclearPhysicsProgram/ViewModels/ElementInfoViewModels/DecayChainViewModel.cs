﻿using NuclearPhysicsProgram.Models;
using NuclearPhysicsProgram.ViewModels.Commands.ElementInfoCommands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NuclearPhysicsProgram.ViewModels.ElementInfoViewModels {
    public class DecayChainViewModel : PropertyHandler.NotifyPropertyChanged {
        /// decayType indicates type of decay of which led to said isotope in the decayChain
        private static List<List<(IsotopeModel isotope, int index, string decayType)>> decayChains;
        private ElementInfoViewModel elementInfoViewModel;
        private PlotViewModel plotViewModel;
        private IsotopeDataModel currentIsotopeData;

        private int currentDecayChainIndex = 0;
        private bool? isArrowUpEnabled;
        private bool? isArrowDownEnabled;

        ///Related to fix for multiple-decays-of-same-isotope-but-different-decay-type problem causing
        ///multiple consecutive isotopes of the same symbol to show up in the same decay chain instead
        ///of showing up as two different decay chains respectively
        ValueTuple<IsotopeModel, int, string> initialIsotope;
        int lastIndex;
        bool conflict;
        bool rerun;

        public ICommand SwitchDecayChainIsotopeCommand { get; private set; }
        /// Binding is only possible to Properties, such as the Tuple's. ValueTuples
        /// only expose fields and as such can not raise PropertyChangedEvent or bind
        public ObservableCollection<Tuple<IsotopeModel, string, string>> IsotopeDecayChain { get; private set; }
        public bool? IsArrowUpEnabled {
            get => isArrowUpEnabled; 
            private set {
                isArrowUpEnabled = value; 
                SetPropertyChanged(this, "IsArrowUpEnabled");
                SetPropertyChanged(this, "ArrowUpOpacity");
            }
        }
        public bool? IsArrowDownEnabled {
            get => isArrowDownEnabled; 
            private set {
                isArrowDownEnabled = value; 
                SetPropertyChanged(this, "IsArrowDownEnabled");
                SetPropertyChanged(this, "ArrowDownOpacity");
            }
        }
        public double? ArrowUpOpacity { get => isArrowUpEnabled.GetValueOrDefault() ? 1 : 0.25; }
        public double? ArrowDownOpacity { get => isArrowDownEnabled.GetValueOrDefault() ? 1 : 0.25; }

        public DecayChainViewModel(ElementInfoViewModel elementInfoViewModel, PlotViewModel plotViewModel) {
            this.elementInfoViewModel = elementInfoViewModel;
            this.plotViewModel = plotViewModel;
            currentIsotopeData = new IsotopeDataModel("", new IsotopeModel[0]);
            decayChains = new List<List<(IsotopeModel, int, string)>>();
            SwitchDecayChainIsotopeCommand = new SwitchDecayChainIsotopeCommand(this);
            IsotopeDecayChain = new ObservableCollection<Tuple<IsotopeModel, string, string>>();
        }

        public static string GetDecaySymbol(string decayType) {
            switch (decayType) {
                case "Alpha":
                    return "α";
                case "Beta+":
                    return "β+";
                case "Beta-":
                    return "β-";
                case "Beta-Beta-":
                    return "β-,β-";
                case "Gamma":
                    return "γ";
                case "Beta-Gamma":
                    return "β-,γ";
                case "ECGamma":
                    return "EC,γ";
                case "ECEC":
                    return "EC,EC";
                default:
                    return decayType;
            }
        }

        public static double GetAvarageEnergyReleased(ElementDataModel element) {
            var energiesReleased = new List<double>();
            int firstProtonAmount = element.AtomicNumber;
            int firstNeutronAmount = element.MassNumber - element.AtomicNumber;
            double firstMass = (firstProtonAmount * Constants.Proton.Mass.Kilograms) + (firstNeutronAmount * Constants.Neutron.Mass.Kilograms);
            foreach (var decayChain in decayChains) {
                if (decayChain.Count < 2)
                    continue;

                if (!ElementViewModel.ElementDataDictionary.TryGetValue(decayChain[1].isotope.Symbol, out var secondElementData))
                    continue;

                int secondProtonAmount = secondElementData.AtomicNumber;
                int secondNeutronAmount = decayChain[1].isotope.MassNumber - secondElementData.AtomicNumber;
                double secondMass = (secondProtonAmount * Constants.Proton.Mass.Kilograms) + (secondNeutronAmount * Constants.Neutron.Mass.Kilograms);
                double massDifference = firstMass - secondMass;
                if (massDifference < 0)
                    continue;
                energiesReleased.Add(massDifference * (Constants.Photon.Speed * Constants.Photon.Speed));
            }

            double avarageEnergyReleased = 0;
            energiesReleased.ForEach(energy => avarageEnergyReleased += energy);
            if (energiesReleased.Count == 0)
                return 0;
            avarageEnergyReleased /= energiesReleased.Count;
            return avarageEnergyReleased;
        }

        public void SwitchDecayChainIsotope(string direction) {
            if (direction == "next")
                SetupObservableDecayChain(++currentDecayChainIndex);
            else
                SetupObservableDecayChain(--currentDecayChainIndex);
        }

        /// <returns>Avarage energy released out of all of the element's isotopes' first decays</returns>
        public double SetupDecayChains(ElementDataModel element, IsotopeModel selectIsotope = null, bool constructSortedDecayChain = false) {
            if (!ElementViewModel.IsotopeDataDictionary.TryGetValue(element.Symbol, out var isotopeData))
                return 0;

            SetupDecayChains(isotopeData, selectIsotope, constructSortedDecayChain);
            return GetAvarageEnergyReleased(element);
        }

        public void SetupDecayChains(IsotopeDataModel isotopeData, IsotopeModel selectIsotope = null, bool constructSortedDecayChain = false) {
            currentIsotopeData = isotopeData;
            if (currentIsotopeData.Isotopes.Length == 0)
                return;

            decayChains.Clear();
            lastIndex = -1;

            foreach (var isotope in currentIsotopeData.Isotopes) {
                decayChains.Add(new List<(IsotopeModel, int, string)>());
                AnalyzeDecays(isotope, 0, null);   //analyze first base isotope of decay tree
                if (!conflict) {
                    lastIndex = -1;
                    continue;
                }

                rerun = true;
                decayChains.Add(new List<(IsotopeModel, int, string)>());
                //analyze decays of isotope again but on a new chain instead including the conflicting decay
                AnalyzeDecays(isotope, 0, null);
                lastIndex = -1;
                conflict = false; 
                rerun = false;
            }

            currentDecayChainIndex = decayChains.FindIndex(decayChain => decayChain.First().isotope == selectIsotope);
            if (currentDecayChainIndex == -1)
                currentDecayChainIndex = 0;

            if (constructSortedDecayChain)
                SetupObservableDecayChain(currentDecayChainIndex);
        }

        private void AnalyzeDecays(IsotopeModel isotope, int index, string isotopeDecayType) {
            if (index == 0)
                initialIsotope = (isotope, index, isotopeDecayType);

            if (lastIndex == index) {
                conflict = true;
                return;
            }

            lastIndex = index;
            decayChains.Last().Add((isotope, index, isotopeDecayType));  //add isotope as branch to decay tree
            if (isotope.Decays.Length < 1)  //does isotope not contain any decays?
                return;

            int j = 0;
            for (int i = rerun ? 1 : 0; i < isotope.Decays.Length; i++) {  //iterate through decays of isotope
                rerun = false;
                if (j > 0) {
                    if (decayChains.Last().First() == initialIsotope)
                        continue;

                    decayChains.Add(new List<(IsotopeModel isotope, int index, string decayType)>());
                    decayChains.Last().Add(initialIsotope);
                    index = 0;
                }

                if (isotope.Decays[i].ProductSymbol == "-")
                    return;

                var nextIsotope = ElementViewModel.GetIsotope(isotope.Decays[i].ProductSymbol, isotope.Decays[i].MassNumber);
                AnalyzeDecays(nextIsotope, index + 1, isotope.Decays[i].Type);  //
                j++;
            }
        }

        private void SetupObservableDecayChain(int decayChainIndex) {
            IsotopeDecayChain.Clear();
            var decayChain = decayChains[decayChainIndex];
            for (int i = 0; i < decayChain.Count; i++) {
                var decayChainItem = decayChain.Where(decayChainItemData => decayChainItemData.index == i).First();
                string decaySymbol = GetDecaySymbol(decayChainItem.decayType);
                ElementViewModel.ElementDataDictionary.TryGetValue(decayChainItem.isotope.Symbol, out var elementDataModel);
                IsotopeDecayChain.Add(new Tuple<IsotopeModel, string, string>(decayChainItem.isotope, decaySymbol, elementDataModel.Name));
            }

            IsArrowUpEnabled = decayChainIndex == 0 ? false : true;
            IsArrowDownEnabled = decayChainIndex == decayChains.Count - 1 ? false : true;
            IsotopeModel isotope = IsotopeDecayChain.First().Item1;
            ElementViewModel.ElementDataDictionary.TryGetValue(isotope.Symbol, out var elementData);
            elementInfoViewModel.InfoIsotopeName = elementData.Name;
            elementInfoViewModel.InfoProtons = elementData.AtomicNumber;
            elementInfoViewModel.InfoNeutrons = -elementData.AtomicNumber + isotope.MassNumber;
            plotViewModel.SetupPlot(isotope);
            double roundedHalfLife = CalculateRoundedHalfLife();
            string roundedHalfLifeString = double.IsNaN(roundedHalfLife) ? "-" : roundedHalfLife.ToString();
            elementInfoViewModel.InfoHalfLife = $"{roundedHalfLifeString}\n{plotViewModel.Unit}";
        }

        private double CalculateRoundedHalfLife() {
            double roundedHalfLife = plotViewModel.UnitHalfLife;
            if (roundedHalfLife.ToString().Contains("E"))
                roundedHalfLife = double.Parse(roundedHalfLife.ToString("E2"));
            else
                roundedHalfLife = Math.Round(plotViewModel.UnitHalfLife, 1);

            roundedHalfLife = roundedHalfLife == 0 ? double.PositiveInfinity : roundedHalfLife;
            if (roundedHalfLife == 0)
                roundedHalfLife = double.PositiveInfinity;
            else if (roundedHalfLife == -1)
                roundedHalfLife = double.NaN;

            return roundedHalfLife;
        }
    }
}
