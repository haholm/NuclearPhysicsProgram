using NuclearPhysicsProgram.ViewModels.ElementInfoViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NuclearPhysicsProgram.ViewModels.Commands {
    class OpenWikipediaCommand : ICommand {
        private ElementInfoViewModel elementInfoViewModel;

        public OpenWikipediaCommand(ElementInfoViewModel elementInfoViewModel) {
            this.elementInfoViewModel = elementInfoViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) {
            elementInfoViewModel.OpenWikipedia();
        }
    }
}
