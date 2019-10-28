using NuclearPhysicsProgram.Models;
using NuclearPhysicsProgram.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NuclearPhysicsProgram.ViewModels {
    class PeriodicTableViewModel {
        public ElementViewModel ElementViewModel { get; private set; }
        public ElementInfoViewModels.ElementInfoViewModel ElementInfoViewModel { get; private set; }
        public double? ActualWidth { get => 1142; }
        public double? ActualHeight { get => 550; }
        public Thickness PeriodicTableItemMargin { get => new Thickness(4); }
        public double? PeriodicTableItemWidth { get => (double)ActualHeight / 7 - (PeriodicTableItemMargin.Bottom * 2) - 14; }
        public double? PeriodicTableItemHeight { get => (double)ActualHeight / 7 - (PeriodicTableItemMargin.Bottom * 2) - 14; }

        public PeriodicTableViewModel() {
            ElementViewModel = new ElementViewModel();
            ElementInfoViewModel = new ElementInfoViewModels.ElementInfoViewModel();
        }
    }
}
