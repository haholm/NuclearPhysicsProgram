using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NuclearPhysicsProgram.ViewModels.Commands.ElementInfoCommands {
    class ContractHiddenElementsCommand : ICommand {
        private PeriodicTableViewModel periodicTableViewModel;

        public ContractHiddenElementsCommand(PeriodicTableViewModel periodicTableViewModel) {
            this.periodicTableViewModel = periodicTableViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => periodicTableViewModel.ContractHiddenElements(int.Parse(parameter as string));
    }
}
