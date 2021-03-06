﻿using NuclearPhysicsProgram.Models;
using NuclearPhysicsProgram.ViewModels.Commands;
using NuclearPhysicsProgram.ViewModels.ElementInfoViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace NuclearPhysicsProgram.ViewModels {
    public class MainViewModel : PropertyHandler.NotifyPropertyChanged {
        private double? mainWindowWidth;
        private double? mainWindowHeight;
        private WindowState mainWindowState;
        private double? mainWindowMagnification;
        private double? mainWindowBlurRadius;
        private double? elementInfoViewOpacity;
        private Visibility? elementInfoViewVisibility;
        private Visibility? periodicTableViewVisibility;
        private double? periodicTableViewOpacity;
        private IsotopeDataModel currentIsotopeData;
        private double? periodicTableScale;
        private bool mainWindowSizeChanged;
        private Visibility? itemViewAERColorVisibility;
        private Visibility? itemViewInstabilityColorVisibility;
        private Thickness? periodicTableAreaMargin;

        public static AnimationViewModel AnimationViewModel { get; set; }
        public ElementViewModel ElementViewModel { get; private set; }
        public PeriodicTablePlotViewModel PeriodicTablePlotViewModel { get; private set; }
        public ElementInfoViewModel ElementInfoViewModel { get; private set; }
        public PlotViewModel PlotViewModel { get; private set; }
        public DecayChainViewModel DecayChainViewModel { get; private set; }
        public static ICommand OpenElementInfoCommand { get; private set; }
        public static ICommand CloseElementInfoCommand { get; private set; }
        public static ICommand TogglePropertyColorCommand { get; private set; }
        public ICommand OpenWebsiteCommand { get; private set; }
        public ICommand OpenWikipediaCommand { get; private set; }
        public double? MainWindowWidth { get => mainWindowWidth; set { mainWindowWidth = value; SetPropertyChanged(this, "MainWindowWidth"); mainWindowSizeChanged = true; } }
        public double? MainWindowHeight { get => mainWindowHeight; set { mainWindowHeight = value; SetPropertyChanged(this, "MainWindowHeight"); mainWindowSizeChanged = true; } }
        public WindowState MainWindowState { get => mainWindowState; set { mainWindowState = value; SetPropertyChanged(this, "MainWindowState"); mainWindowSizeChanged = true; } }
        public double? MainWindowMagnification { get => mainWindowMagnification; set { mainWindowMagnification = value; SetPropertyChanged(this, "MainWindowMagnification"); } }
        public double? MainWindowBlurRadius { get => mainWindowBlurRadius; set { mainWindowBlurRadius = value; SetPropertyChanged(this, "MainWindowBlurRadius"); } }
        public Thickness? PeriodicTableAreaMargin { get => periodicTableAreaMargin; private set { periodicTableAreaMargin = value; SetPropertyChanged(this, "PeriodicTableAreaMargin"); } }
        public double? ElementInfoViewOpacity { get => elementInfoViewOpacity; private set { elementInfoViewOpacity = value; SetPropertyChanged(this, "ElementInfoViewOpacity"); } }
        public Visibility? ElementInfoViewVisibility { get => elementInfoViewVisibility; private set { elementInfoViewVisibility = value; SetPropertyChanged(this, "ElementInfoViewVisibility"); } }
        public Visibility? PeriodicTableViewVisibility { get => periodicTableViewVisibility; private set { periodicTableViewVisibility = value; SetPropertyChanged(this, "PeriodicTableViewVisibility"); } }
        public double? PeriodicTableViewOpacity { get => periodicTableViewOpacity; private set { periodicTableViewOpacity = value; SetPropertyChanged(this, "PeriodicTableViewOpacity"); } }
        public double? PeriodicTableScale { get => periodicTableScale; private set { periodicTableScale = value; SetPropertyChanged(this, "PeriodicTableScale"); } }
        public Visibility? ItemViewAERColorVisibility { get => itemViewAERColorVisibility; set { itemViewAERColorVisibility = value; SetPropertyChanged(this, "ItemViewAERColorVisibility"); } }
        public Visibility? ItemViewInstabilityColorVisibility { get => itemViewInstabilityColorVisibility; set { itemViewInstabilityColorVisibility = value; SetPropertyChanged(this, "ItemViewInstabilityColorVisibility"); } }
        public IsotopeDataModel CurrentIsotopeData { get => currentIsotopeData; private set { currentIsotopeData = value; SetPropertyChanged(this, "CurrentIsotopeData"); } }
        public CancellationTokenSource MainWindowSizeUpdateTaskCTS { get; private set; }
        public Task MainWindowSizeUpdateTask { get; private set; }

        public MainViewModel() {
            AnimationViewModel = new AnimationViewModel();
            ElementInfoViewModel = new ElementInfoViewModel();
            PeriodicTablePlotViewModel = new PeriodicTablePlotViewModel();
            PlotViewModel = new PlotViewModel();
            DecayChainViewModel = new DecayChainViewModel(ElementInfoViewModel, PlotViewModel);
            ElementViewModel = new ElementViewModel(this);

            OpenElementInfoCommand = new OpenElementInfoCommand(this);
            CloseElementInfoCommand = new CloseElementInfoCommand(this);
            TogglePropertyColorCommand = new TogglePropertyColorCommand(this);
            OpenWebsiteCommand = new OpenWebsiteCommand();
            OpenWikipediaCommand = new OpenWikipediaCommand(ElementInfoViewModel);
            MainWindowSizeUpdateTaskCTS = new CancellationTokenSource();
            MainWindowSizeUpdateTask = Task.Run(() => {
                while (true) {
                    Thread.Sleep(100);
                    if (!mainWindowSizeChanged)
                        continue;

                    mainWindowSizeChanged = false;
                    if (MainWindowState == WindowState.Maximized)
                        PeriodicTableScale = (double)System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 1340;
                    else if (!double.IsNaN(MainWindowWidth.GetValueOrDefault()))
                        PeriodicTableScale = ((MainWindowWidth / 1200) + (MainWindowHeight / 800)) / 2;
                }
            }, MainWindowSizeUpdateTaskCTS.Token);
        }

        public async Task OpenElementInfo(string symbol, int massNumber, bool test = false) {
            // check if ElementInfoView is open or unit testing, if true, do not play transitions
            bool opened = ElementInfoViewOpacity == 1 || test;
            if (!opened) await AnimationViewModel.AsyncTransition((opacity) => PeriodicTableViewOpacity = opacity, 1, 0, 0.05, 1);
            PeriodicTableViewVisibility = Visibility.Collapsed;
            InitializeIsotopeData(symbol, massNumber);

            ElementInfoViewVisibility = Visibility.Visible;
            if (!opened) await AnimationViewModel.AsyncTransition((opacity) => ElementInfoViewOpacity = opacity, 0, 1, 0.1, 1.5);
            ElementInfoViewOpacity = 1;
        }

        public async Task CloseElementInfo(bool test = false) {
            if (!test) await AnimationViewModel.AsyncTransition((opacity) => ElementInfoViewOpacity = opacity, 1, 0, 0.1, 1.5);
            PeriodicTableViewVisibility = Visibility.Visible;
            if (!test) await AnimationViewModel.AsyncTransition((opacity) => PeriodicTableViewOpacity = opacity, 0, 1, 0.05, 1);
            ElementInfoViewVisibility = Visibility.Collapsed;
        }

        public void TogglePropertyColor(string property) {
            switch (property) {
                case "instability":
                    ToggleInstabilityColor();
                    ToggleInstabilityPlot();
                    return;
                case "energy":
                    ToggleEnergyColor();
                    ToggleBindingEnergyPlot();
                    return;
            }
        }

        private void ToggleInstabilityColor() {
            if (ItemViewAERColorVisibility == Visibility.Visible)
                ToggleEnergyColor();

            switch (ItemViewInstabilityColorVisibility) {
                case Visibility.Visible:
                    PeriodicTableAreaMargin = new Thickness(0, 50, 0, 0);
                    ItemViewInstabilityColorVisibility = Visibility.Hidden;
                    return;
                case Visibility.Hidden:
                    PeriodicTableAreaMargin = new Thickness(0, 100, 0, 0);
                    ItemViewInstabilityColorVisibility = Visibility.Visible;
                    return;
                default:
                    PeriodicTableAreaMargin = new Thickness(0, 100, 0, 0);
                    //In case ItemViewAERColorVisibility was never set;
                    ItemViewInstabilityColorVisibility = Visibility.Visible;
                    return;
            }
        }

        private void ToggleEnergyColor() {
            if (ItemViewInstabilityColorVisibility == Visibility.Visible)
                ToggleInstabilityColor();

            switch (ItemViewAERColorVisibility) {
                case Visibility.Visible:
                    PeriodicTableAreaMargin = new Thickness(0, 50, 0, 0);
                    ItemViewAERColorVisibility = Visibility.Hidden;
                    return;
                case Visibility.Hidden:
                    PeriodicTableAreaMargin = new Thickness(0, 100, 0, 0);
                    ItemViewAERColorVisibility = Visibility.Visible;
                    return;
                default:
                    PeriodicTableAreaMargin = new Thickness(0, 100, 0, 0);
                    //In case ItemViewAERColorVisibility was never set;
                    ItemViewAERColorVisibility = Visibility.Visible;
                    return;
            }
        }

        private void ToggleInstabilityPlot() {
            PeriodicTablePlotViewModel.Visibility = ItemViewInstabilityColorVisibility;
            if (ItemViewInstabilityColorVisibility == Visibility.Visible)
                PeriodicTablePlotViewModel.InitializePlot("instability");
        }

        private void ToggleBindingEnergyPlot(bool plot = true) {
            PeriodicTablePlotViewModel.Visibility = ItemViewAERColorVisibility;
            if (ItemViewAERColorVisibility == Visibility.Visible)
                PeriodicTablePlotViewModel.InitializePlot("energy");
        }

        public void InitializeIsotopeData(string symbol, int massNumber) {
            if (ElementViewModel.IsotopeDataDictionary.TryGetValue(symbol, out IsotopeDataModel isotopeData)) {
                var selectIsotope = ElementViewModel.GetIsotope(isotopeData, massNumber);
                SetIsotopeDatas(isotopeData, selectIsotope, massNumber);
            }
            else if (ElementViewModel.ElementDataDictionary.TryGetValue(symbol, out ElementDataModel elementData)) {
                var templateIsotope = new IsotopeModel[] { new IsotopeModel(symbol, elementData.MassNumber, "?", new DecayModel[0]) };
                SetIsotopeDatas(new IsotopeDataModel(symbol, templateIsotope), templateIsotope[0], massNumber);
            }
        }

        private void SetIsotopeDatas(IsotopeDataModel isotopeData, IsotopeModel selectIsotope, int massNumber) {
            CurrentIsotopeData = isotopeData;
            ElementInfoViewModel.CurrentIsotopeData = isotopeData;
            DecayChainViewModel.SetupDecayChains(isotopeData, selectIsotope, true);
        }
    }
}
