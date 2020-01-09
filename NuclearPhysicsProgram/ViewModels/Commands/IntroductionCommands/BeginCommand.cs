using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NuclearPhysicsProgram.ViewModels.Commands.IntroductionCommands {
    class BeginCommand : ICommand {
        ApplicationViewModel applicationViewModel;

        public BeginCommand(ApplicationViewModel applicationViewModel) {
            this.applicationViewModel = applicationViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => applicationViewModel.Begin();
    }
}
