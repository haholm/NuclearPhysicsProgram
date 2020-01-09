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
    public class ApplicationViewModel : PropertyHandler.NotifyPropertyChanged {
        private ICommand beginCommand;
        private Visibility? introductionViewVisibility;

        public static string ConfigPath { get => "conf"; }
        public string ApplicationName { get => "Nuclear Physics Program"; }
        public ICommand BeginCommand { get => beginCommand; private set => beginCommand = value; }
        public Visibility? IntroductionViewVisibility { get => introductionViewVisibility; private set { introductionViewVisibility = value; SetPropertyChanged(this, "IntroductionViewVisibility"); } }
        public int FirstRun = 1;

        public ApplicationViewModel() {
            BeginCommand = new Commands.IntroductionCommands.BeginCommand(this);
            Initialize();
        }

        public void Begin() => File.WriteAllText(ConfigPath, "0");

        private void Initialize() {
            if (!File.Exists(ConfigPath))
                File.WriteAllText(ConfigPath, "1");

            string firstRun = File.ReadAllText(ConfigPath);
            int.TryParse(firstRun, out FirstRun);
            if (FirstRun == 0)
                IntroductionViewVisibility = Visibility.Collapsed;
        }
    }
}
