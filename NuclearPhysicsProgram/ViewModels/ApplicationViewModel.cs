using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NuclearPhysicsProgram.ViewModels {
    class ApplicationViewModel : PropertyHandler.NotifyPropertyChanged {
        private ICommand beginCommand;
        private Visibility? introductionViewVisibility;

        public string ApplicationName { get => "Nuclear Physics Program"; }
        public int FirstRun = 1;
        public ICommand BeginCommand { get => beginCommand; private set => beginCommand = value; }
        public Visibility? IntroductionViewVisibility { get => introductionViewVisibility; private set { introductionViewVisibility = value; SetPropertyChanged(this, "IntroductionViewVisibility"); } }

        public ApplicationViewModel() {
            BeginCommand = new Commands.IntroductionCommands.BeginCommand(this);
            Initialize();
        }

        public void Begin() => File.WriteAllText("conf", "0");

        private void Initialize() {
            string conf = "conf";
            if (!File.Exists(conf))
                File.WriteAllText(conf, "1");

            string firstRun = File.ReadAllText(conf);
            int.TryParse(firstRun, out FirstRun);
            if (FirstRun == 0)
                IntroductionViewVisibility = Visibility.Collapsed;
        }
    }
}
