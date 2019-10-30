using NuclearPhysicsProgram.Models;
using NuclearPhysicsProgram.ViewModels.Commands;
using NuclearPhysicsProgram.ViewModels.Commands.ElementInfoCommands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NuclearPhysicsProgram.ViewModels {
    class PeriodicTableViewModel : PropertyHandler.NotifyPropertyChanged {
        private double? periodicTableOpacity = 1;
        private Size? firstHiddenElementsSize;
        private Size? secondHiddenElementsSize;
        private double? firstHiddenElementsOpacity = 0;
        private double? secondHiddenElementsOpacity = 0;
        private Visibility? firstHiddenElementsVisibility;
        private Visibility? secondHiddenElementsVisibility;

        public ElementViewModel ElementViewModel { get; private set; }
        public ElementInfoViewModels.ElementInfoViewModel ElementInfoViewModel { get; private set; }
        public ICommand ExpandHiddenElementsCommand { get; private set; }
        public ICommand ContractHiddenElementsCommand { get; private set; }
        public double? ActualWidth { get => 1210; }
        public double? ActualHeight { get => 480; }
        public Thickness PeriodicTableItemMargin { get => new Thickness(4); }
        public double? PeriodicTableItemWidth { get => (double)ActualWidth / 18 - (PeriodicTableItemMargin.Bottom * 2) - 10; }
        public double? PeriodicTableItemHeight { get => (double)ActualWidth / 18 - (PeriodicTableItemMargin.Bottom * 2) - 10; }
        public double? PeriodicTableOpacity { get => periodicTableOpacity; set { periodicTableOpacity = value; SetPropertyChanged(this, "PeriodicTableOpacity"); } }
        public double? FirstHiddenElementsWidth { get => firstHiddenElementsSize.Value.Width; set { firstHiddenElementsSize = new Size((double)value, firstHiddenElementsSize.Value.Height); SetPropertyChanged(this, "FirstHiddenElementsWidth"); } }
        public double? FirstHiddenElementsHeight { get => firstHiddenElementsSize.Value.Height; }
        public double? SecondHiddenElementsWidth { get => secondHiddenElementsSize.Value.Width; set { secondHiddenElementsSize = new Size((double)value, secondHiddenElementsSize.Value.Height); SetPropertyChanged(this, "SecondHiddenElementsWidth"); } }
        public double? SecondHiddenElementsHeight { get => secondHiddenElementsSize.Value.Height; }
        public double? FirstHiddenElementsOpacity { get => firstHiddenElementsOpacity; set { firstHiddenElementsOpacity = value; SetPropertyChanged(this, "FirstHiddenElementsOpacity"); } }
        public double? SecondHiddenElementsOpacity { get => secondHiddenElementsOpacity; set { secondHiddenElementsOpacity = value; SetPropertyChanged(this, "SecondHiddenElementsOpacity"); } }
        public Visibility? FirstHiddenElementsVisibility { get => firstHiddenElementsVisibility; set { firstHiddenElementsVisibility = value; SetPropertyChanged(this, "FirstHiddenElementsVisibility"); } }
        public Visibility? SecondHiddenElementsVisibility { get => secondHiddenElementsVisibility; set { secondHiddenElementsVisibility = value; SetPropertyChanged(this, "secondHiddenElementsVisibility"); } }

        public PeriodicTableViewModel() {
            firstHiddenElementsSize = new Size(58, (double)ActualWidth / 18);
            secondHiddenElementsSize = new Size(58, (double)ActualWidth / 18);

            ElementViewModel = new ElementViewModel();
            ElementInfoViewModel = new ElementInfoViewModels.ElementInfoViewModel();
            ExpandHiddenElementsCommand = new ExpandHiddenElementsCommand(this);
            ContractHiddenElementsCommand = new ContractHiddenElementsCommand(this);
        }

        public async void ExpandHiddenElements(int item) {
            MainViewModel.AnimationViewModel.TransitionEffect(UpdatePeriodicTableOpacity, 1, 0.5, 0.1, 1);
            if (item == 1) {
                FirstHiddenElementsWidth = 870;
                FirstHiddenElementsVisibility = Visibility.Visible;
                await MainViewModel.AnimationViewModel.AsyncTransitionEffect(UpdateFirstHiddenElementsOpacity, 0, 1, 0.1, 1);
            }
            else if (item == 2) {
                SecondHiddenElementsWidth = 870;
                SecondHiddenElementsVisibility = Visibility.Visible;
                await MainViewModel.AnimationViewModel.AsyncTransitionEffect(UpdateSecondHiddenElementsOpacity, 0, 1, 0.1, 1);
            }
        }

        public async void ContractHiddenElements(int item) {
            MainViewModel.AnimationViewModel.TransitionEffect(UpdatePeriodicTableOpacity, 0.5, 1, 0.1, 1);
            if (item == 1) {
                FirstHiddenElementsWidth = 58;
                await MainViewModel.AnimationViewModel.AsyncTransitionEffect(UpdateFirstHiddenElementsOpacity, 1, 0, 0.1, 1);
                FirstHiddenElementsVisibility = Visibility.Collapsed;
            }
            else if (item == 2) {
                SecondHiddenElementsWidth = 58;
                await MainViewModel.AnimationViewModel.AsyncTransitionEffect(UpdateSecondHiddenElementsOpacity, 1, 0, 0.1, 1);
                SecondHiddenElementsVisibility = Visibility.Collapsed;
            }
        }

        private void UpdatePeriodicTableOpacity(double? value) => PeriodicTableOpacity = value;
        private void UpdateFirstHiddenElementsOpacity(double? value) => FirstHiddenElementsOpacity = value;
        private void UpdateSecondHiddenElementsOpacity(double? value) => SecondHiddenElementsOpacity = value;
    }
}
