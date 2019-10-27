using NuclearPhysicsProgram.ViewModels.Commands.ElementInfoCommands;
using NuclearPhysicsProgram.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuclearPhysicsProgram.ViewModels.ElementInfoViewModels {
    class IsotopeDropDownViewModel : PropertyHandler.NotifyPropertyChanged {
        private double newHeight;

        public OpenIsotopeDropDownCommand OpenIsotopeDropDownCommand { get; private set; }
        public double ActualWidth { get; private set; }
        public double NewHeight { get => newHeight; set { newHeight = value; SetPropertyChanged(this, "NewHeight"); } } //NotifyPropertyChanged

        public IsotopeDropDownViewModel() {
            OpenIsotopeDropDownCommand = new OpenIsotopeDropDownCommand(this);
            ActualWidth = 80;
            NewHeight = 80;
        }

        public void OpenIsotopeDropDown() {
            NewHeight = ElementInfoViewModel.CurrentIsotopeData.Isotopes.Length * ActualWidth;
        }
    }
}
