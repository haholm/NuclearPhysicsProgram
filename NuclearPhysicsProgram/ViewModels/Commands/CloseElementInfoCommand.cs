using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NuclearPhysicsProgram.ViewModels.Commands {
    class CloseElementInfoCommand : ICommand {
        private MainViewModel mainViewModel;

        public CloseElementInfoCommand(MainViewModel mainViewModel) {
            this.mainViewModel = mainViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true; // TEMPORARY?

        public void Execute(object parameter) => mainViewModel.CloseElementInfo();
    }
}
