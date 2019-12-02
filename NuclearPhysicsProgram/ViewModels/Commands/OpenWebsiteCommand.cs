using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NuclearPhysicsProgram.ViewModels.Commands {
    class OpenWebsiteCommand : ICommand {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) {
            string link = parameter as string;
            if (link.StartsWith("https://"))
                System.Diagnostics.Process.Start($"{link}");
        }
    }
}
