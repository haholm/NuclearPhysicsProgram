using NuclearPhysicsProgram.Models;
using NuclearPhysicsProgram.ViewModels.Commands;
using NuclearPhysicsProgram.ViewModels.ElementInfoViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NuclearPhysicsProgram.ViewModels {
    public class MainViewModel : PropertyHandler.NotifyPropertyChanged {
        private double? mainWindowMagnification;
        private double? mainWindowBlurRadius;
        private double? elementInfoViewOpacity;
        private Visibility? elementInfoViewVisibility;
        private Visibility? periodicTableViewVisibility;
        private double? periodicTableViewOpacity;
        private IsotopeDataModel currentIsotopeData;

        /// Ta med binding energy

        public static AnimationViewModel AnimationViewModel { get; set; }
        public ElementViewModel ElementViewModel { get; private set; }
        public PlotViewModel PlotViewModel { get; private set; }
        public DecayChainViewModel DecayChainViewModel { get; private set; }
        public static ICommand OpenElementInfoCommand { get; private set; }
        public static ICommand CloseElementInfoCommand { get; private set; }
        public double? MainWindowMagnification { get => mainWindowMagnification; set { mainWindowMagnification = value; SetPropertyChanged(this, "MainWindowMagnification"); } }
        public double? MainWindowBlurRadius { get => mainWindowBlurRadius; set { mainWindowBlurRadius = value; SetPropertyChanged(this, "MainWindowBlurRadius"); } }
        public double? ElementInfoViewOpacity {
            get => elementInfoViewOpacity;
            private set {
                elementInfoViewOpacity = value; 
                SetPropertyChanged(this, "ElementInfoViewOpacity");
            }
        }
        public Visibility? ElementInfoViewVisibility { get => elementInfoViewVisibility; private set { elementInfoViewVisibility = value; SetPropertyChanged(this, "ElementInfoViewVisibility"); } }
        public Visibility? PeriodicTableViewVisibility { get => periodicTableViewVisibility; private set { periodicTableViewVisibility = value; SetPropertyChanged(this, "PeriodicTableViewVisibility"); } }
        public double? PeriodicTableViewOpacity { get => periodicTableViewOpacity; private set { periodicTableViewOpacity = value; SetPropertyChanged(this, "PeriodicTableViewOpacity"); } }
        public IsotopeDataModel CurrentIsotopeData { get => currentIsotopeData; private set { currentIsotopeData = value; SetPropertyChanged(this, "CurrentIsotopeData"); } }

        public MainViewModel() {
            AnimationViewModel = new AnimationViewModel();
            PlotViewModel = new PlotViewModel();
            DecayChainViewModel = new DecayChainViewModel(PlotViewModel);
            ElementViewModel = new ElementViewModel(this);

            OpenElementInfoCommand = new OpenElementInfoCommand(this); 
            CloseElementInfoCommand = new CloseElementInfoCommand(this);
        }

        public async void OpenElementInfo(string symbol, int massNumber) {
            await AnimationViewModel.AsyncTransition(UpdatePeriodicTableViewOpacity, 1, 0, 0.05, 1);
            PeriodicTableViewVisibility = Visibility.Collapsed;
            InitializeIsotopeData(symbol, massNumber);

            ElementInfoViewVisibility = Visibility.Visible;
            AnimationViewModel.Transition(UpdateMainWindowBlurRadius, 0, 2.5, 0.05, 1.5);
            AnimationViewModel.Transition(UpdateMainWindowMagnification, 0, -1, 0.05, 1.5);
            await AnimationViewModel.AsyncTransition(UpdateElementInfoViewOpacity, 0, 1, 0.05, 1.5);
        }

        public async void CloseElementInfo() {
            AnimationViewModel.Transition(UpdateElementInfoViewOpacity, 1, 0, 0.05, 1.5);
            AnimationViewModel.Transition(UpdateMainWindowMagnification, -1, 0, 0.05, 1.5);
            await AnimationViewModel.AsyncTransition(UpdateMainWindowBlurRadius, 2.5, 0, 0.05, 1.5);

            CurrentIsotopeData = null;
            PeriodicTableViewVisibility = Visibility.Visible;
            await AnimationViewModel.AsyncTransition(UpdatePeriodicTableViewOpacity, 0, 1, 0.05, 1);
            ElementInfoViewVisibility = Visibility.Collapsed;
        }

        public void InitializeIsotopeData(string symbol, int massNumber) {
            if (ElementViewModel.IsotopeDataDictionary.TryGetValue(symbol, out IsotopeDataModel isotopeData))
                SetIsotopeDatas(isotopeData, massNumber);
            else if(ElementViewModel.ElementDataDictionary.TryGetValue(symbol, out ElementDataModel elementData)) {
                var templateIsotope = new IsotopeModel[] { new IsotopeModel(symbol, elementData.MassNumber, "?", new DecayModel[0]) };
                SetIsotopeDatas(new IsotopeDataModel(symbol, templateIsotope), massNumber);
            }
        }

        private void SetIsotopeDatas(IsotopeDataModel isotopeData, int massNumber) {
            CurrentIsotopeData = isotopeData;
            ElementInfoViewModels.ElementInfoViewModel.CurrentIsotopeData = isotopeData;
            DecayChainViewModel.SetupDecayChains(isotopeData, true);
        }

        private void UpdateElementInfoViewOpacity(double? value) => ElementInfoViewOpacity = value;

        private void UpdateMainWindowBlurRadius(double? value) => MainWindowBlurRadius = value;

        private void UpdateMainWindowMagnification(double? value) => MainWindowMagnification = value;

        private void UpdatePeriodicTableViewOpacity(double? value) => PeriodicTableViewOpacity = value;
    }
}
