using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NuclearPhysicsProgram.ViewModels.Commands
{
    class OpenElementInfoCommand : ICommand {
        private MainViewModel mainViewModel;

        public OpenElementInfoCommand(MainViewModel mainViewModel) {
            this.mainViewModel = mainViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true; // TEMPORARY?

        public void Execute(object parameter) {
            var values = parameter as object[];
            var symbol = values[0] as string;
            var massNumber = values[1] as int?;
            mainViewModel.OpenElementInfo(symbol as string, massNumber.GetValueOrDefault());
        }
    }
}
