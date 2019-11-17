using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NuclearPhysicsProgram.ViewModels.Commands {
    class TogglePropertyColorCommand : ICommand {
        private MainViewModel mainViewModel;
        public event EventHandler CanExecuteChanged;

        public TogglePropertyColorCommand(MainViewModel mainViewModel) {
            this.mainViewModel = mainViewModel;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => mainViewModel.TogglePropertyColor(parameter as string);
    }
}
