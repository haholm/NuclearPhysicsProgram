using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuclearPhysicsProgram.ViewModels.PropertyHandler {
    public class NotifyPropertyChanged : INotifyPropertyChanged {
        public void SetPropertyChanged(object senderClass, string propertyName) {
            PropertyChanged?.Invoke(senderClass, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
