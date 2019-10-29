﻿using NuclearPhysicsProgram.Models;
using NuclearPhysicsProgram.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NuclearPhysicsProgram.ViewModels {
    public class MainViewModel : PropertyHandler.NotifyPropertyChanged {
        private const double magnificationMultiplier = 1;
        private const double blurMultiplier = 2.5;

        private double? mainWindowMagnification;
        private double? mainWindowBlurRadius;
        private double? elementInfoViewOpacity;
        private Visibility elementInfoViewVisibility;
        private bool periodicTableViewIsEnabled;
        private IsotopeDataModel currentIsotopeData;

        public static AnimationViewModel AnimationViewModel { get; set; }
        public DecayChainViewModel DecayChainViewModel { get; private set; }
        public static ICommand OpenElementInfoCommand { get; private set; }
        public static ICommand CloseElementInfoCommand { get; private set; }
        //public double? MainWindowMagnification { get => (elementInfoViewOpacity / 100) + magnificationMultiplier; }
        //public double MainWindowBlurRadius { get => elementInfoViewOpacity * blurMultiplier; }
        public double? MainWindowMagnification { get => mainWindowMagnification; set { mainWindowMagnification = value; SetPropertyChanged(this, "MainWindowMagnification"); } }
        public double? MainWindowBlurRadius { get => mainWindowBlurRadius; set { mainWindowBlurRadius = value; SetPropertyChanged(this, "MainWindowBlurRadius"); } }
        public double? ElementInfoViewOpacity {
            get => elementInfoViewOpacity;
            private set {
                elementInfoViewOpacity = value; 
                SetPropertyChanged(this, "ElementInfoViewOpacity");
            }
        }
        public Visibility ElementInfoViewVisibility { get => elementInfoViewVisibility; private set { elementInfoViewVisibility = value; SetPropertyChanged(this, "ElementInfoViewVisibility"); } }
        public bool PeriodicTableViewIsEnabled { get => periodicTableViewIsEnabled; private set { periodicTableViewIsEnabled = value; SetPropertyChanged(this, "PeriodicTableViewIsEnabled"); } }
        public IsotopeDataModel CurrentIsotopeData { get => currentIsotopeData; private set { currentIsotopeData = value; SetPropertyChanged(this, "CurrentIsotope"); } }

        public MainViewModel() {
            elementInfoViewVisibility = Visibility.Collapsed; //ahem...
            periodicTableViewIsEnabled = true; //AHEM...
            AnimationViewModel = new AnimationViewModel();
            DecayChainViewModel = new DecayChainViewModel();

            OpenElementInfoCommand = new OpenElementInfoCommand(this); 
            CloseElementInfoCommand = new CloseElementInfoCommand(this);
        }

        public async void OpenElementInfo(string symbol) {
            PeriodicTableViewIsEnabled = false;
            InitializeIsotopeData(symbol);

            ElementInfoViewVisibility = Visibility.Visible;
            AnimationViewModel.TransitionEffect(UpdateElementInfoViewOpacity, 0, 1, 0.1, 1.5);
            AnimationViewModel.TransitionEffect(UpdateMainWindowBlurRadius, 0, 2.5, 0.1, 1.5);
            await AnimationViewModel.AsyncTransitionEffect(UpdateMainWindowMagnification, 0, -1, 0.1, 1.5);
        }

        public async void CloseElementInfo() {
            AnimationViewModel.TransitionEffect(UpdateMainWindowBlurRadius, 2.5, 0, 0.1, 1.5);
            AnimationViewModel.TransitionEffect(UpdateMainWindowMagnification, -1, 0, 0.1, 1.5);
            await AnimationViewModel.AsyncTransitionEffect(UpdateElementInfoViewOpacity, 1, 0, 0.1, 1.5);
            CurrentIsotopeData = null;
            ElementInfoViewVisibility = Visibility.Collapsed;
            PeriodicTableViewIsEnabled = true;
        }

        public void InitializeIsotopeData(string symbol) {
            ElementViewModel.IsotopeDataDictionary.TryGetValue(symbol, out IsotopeDataModel isotopeData);
            CurrentIsotopeData = isotopeData;
            ElementInfoViewModels.ElementInfoViewModel.CurrentIsotopeData = CurrentIsotopeData;
            DecayChainViewModel.CurrentIsotopeData = CurrentIsotopeData;
        }

        private void UpdateElementInfoViewOpacity(double? value) => ElementInfoViewOpacity = value;

        private void UpdateMainWindowBlurRadius(double? value) => MainWindowBlurRadius = value;

        private void UpdateMainWindowMagnification(double? value) => MainWindowMagnification = value;
    }
}
