﻿using NuclearPhysicsProgram.Models;
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
    public class PeriodicTableViewModel : PropertyHandler.NotifyPropertyChanged {
        private double? periodicTableOpacity = 1;
        private Size? firstHiddenElementsSize;
        private Size? secondHiddenElementsSize;
        private double? firstHiddenElementsOpacity = 0;
        private double? secondHiddenElementsOpacity = 0;
        private Visibility? firstHiddenElementsVisibility;
        private Visibility? secondHiddenElementsVisibility;

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

            ElementInfoViewModel = new ElementInfoViewModels.ElementInfoViewModel();
            ExpandHiddenElementsCommand = new ExpandHiddenElementsCommand(this);
            ContractHiddenElementsCommand = new ContractHiddenElementsCommand(this);
        }

        public async Task ExpandHiddenElements(int item, bool test = false) {
            Action<double?> propertyUpdater = null;
            MainViewModel.AnimationViewModel.Transition(UpdatePeriodicTableOpacity, 1, 0.5, 0.1, 1);
            if (item == 1 && FirstHiddenElementsVisibility != Visibility.Visible) {
                FirstHiddenElementsWidth = 870;
                FirstHiddenElementsVisibility = Visibility.Visible;
                propertyUpdater = UpdateFirstHiddenElementsOpacity;
            }
            else if (item == 2 && SecondHiddenElementsVisibility != Visibility.Visible) {
                SecondHiddenElementsWidth = 870;
                SecondHiddenElementsVisibility = Visibility.Visible;
                propertyUpdater = UpdateSecondHiddenElementsOpacity;
            }
            else return;

            if (!test)
                await MainViewModel.AnimationViewModel.AsyncTransition(propertyUpdater, 0, 1, 0.1, 1);
        }

        public async Task ContractHiddenElements(int item, bool test = false) {
            MainViewModel.AnimationViewModel.Transition(UpdatePeriodicTableOpacity, 0.5, 1, 0.1, 1);
            if (item == 1 && FirstHiddenElementsVisibility != Visibility.Collapsed) {
                FirstHiddenElementsWidth = 58;
                if (!test)
                    await MainViewModel.AnimationViewModel.AsyncTransition(UpdateFirstHiddenElementsOpacity, 1, 0, 0.1, 1);

                FirstHiddenElementsVisibility = Visibility.Collapsed;
            }
            else if (item == 2 && SecondHiddenElementsVisibility != Visibility.Collapsed) {
                SecondHiddenElementsWidth = 58;
                if (!test)
                    await MainViewModel.AnimationViewModel.AsyncTransition(UpdateSecondHiddenElementsOpacity, 1, 0, 0.1, 1);

                SecondHiddenElementsVisibility = Visibility.Collapsed;
            }
        }

        private void UpdatePeriodicTableOpacity(double? value) => PeriodicTableOpacity = value;
        private void UpdateFirstHiddenElementsOpacity(double? value) => FirstHiddenElementsOpacity = value;
        private void UpdateSecondHiddenElementsOpacity(double? value) => SecondHiddenElementsOpacity = value;
    }
}
