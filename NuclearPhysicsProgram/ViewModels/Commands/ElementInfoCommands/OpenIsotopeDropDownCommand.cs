using NuclearPhysicsProgram.ViewModels.ElementInfoViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NuclearPhysicsProgram.ViewModels.Commands.ElementInfoCommands {
    class OpenIsotopeDropDownCommand : ICommand {
        private IsotopeDropDownViewModel isotopeDropDownViewModel;
        
        public OpenIsotopeDropDownCommand(IsotopeDropDownViewModel isotopeDropDownViewModel) {
            this.isotopeDropDownViewModel = isotopeDropDownViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => isotopeDropDownViewModel.OpenIsotopeDropDown();
    }
}
