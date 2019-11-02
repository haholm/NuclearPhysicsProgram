using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuclearPhysicsProgram.ViewModels.PropertyHandler {
    public class NotifyPropertyChanged : INotifyPropertyChanged {
        private Dictionary<string, PropertyChangedEventArgs> previousEventArgs;

        public NotifyPropertyChanged() {
            previousEventArgs = new Dictionary<string, PropertyChangedEventArgs>();
        }

        public void SetPropertyChanged(object senderClass, string propertyName) {
            PropertyChanged?.Invoke(senderClass, ConstructEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private PropertyChangedEventArgs ConstructEventArgs(string propertyName) {
            if (previousEventArgs.TryGetValue(propertyName, out var existingEventArgs))
                return existingEventArgs;

            var eventArgs = new PropertyChangedEventArgs(propertyName);
            previousEventArgs.Add(propertyName, eventArgs);
            return eventArgs;
        }
    }
}
