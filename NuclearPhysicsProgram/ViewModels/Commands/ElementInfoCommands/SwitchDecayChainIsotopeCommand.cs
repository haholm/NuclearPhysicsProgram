using NuclearPhysicsProgram.ViewModels.ElementInfoViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NuclearPhysicsProgram.ViewModels.Commands.ElementInfoCommands {
    class SwitchDecayChainIsotopeCommand : ICommand {
        private DecayChainViewModel decayChainViewModel;
        public event EventHandler CanExecuteChanged;

        public SwitchDecayChainIsotopeCommand(DecayChainViewModel decayChainViewModel) {
            this.decayChainViewModel = decayChainViewModel;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => decayChainViewModel.SwitchDecayChainIsotope(parameter as string);
    }
}
