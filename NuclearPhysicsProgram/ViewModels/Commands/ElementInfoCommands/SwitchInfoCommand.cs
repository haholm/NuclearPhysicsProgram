using NuclearPhysicsProgram.ViewModels.ElementInfoViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NuclearPhysicsProgram.ViewModels.Commands.ElementInfoCommands {
    class SwitchInfoCommand : ICommand {
        private ElementInfoViewModel elementInfoViewModel;
        public event EventHandler CanExecuteChanged;

        public SwitchInfoCommand(ElementInfoViewModel elementInfoViewModel) {
            this.elementInfoViewModel = elementInfoViewModel;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) {
            elementInfoViewModel.SwitchInfo(parameter as string);
        }
    }
}
