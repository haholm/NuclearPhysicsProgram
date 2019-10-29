using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NuclearPhysicsProgram.ViewModels.Commands.ElementInfoCommands {
    class ExpandHiddenElementsCommand : ICommand {
        private PeriodicTableViewModel periodicTableViewModel;

        public ExpandHiddenElementsCommand(PeriodicTableViewModel periodicTableViewModel) {
            this.periodicTableViewModel = periodicTableViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => periodicTableViewModel.ExpandHiddenElements(int.Parse(parameter as string));
    }
}
