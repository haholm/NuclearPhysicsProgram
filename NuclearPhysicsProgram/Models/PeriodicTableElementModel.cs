using System.Windows.Media;

namespace NuclearPhysicsProgram.Models {
    //public struct PeriodicTableElementModel {
    //    public ElementDataModel Element { get; private set; }
    //    public SolidColorBrush AERColor { get; private set; }

    //    public PeriodicTableElementModel(ElementDataModel element, SolidColorBrush avarageEnergyReleased) {
    //        Element = element;
    //        AERColor = avarageEnergyReleased;
    //    }
    //}

    public class PeriodicTableElementModel : ViewModels.PropertyHandler.NotifyPropertyChanged {
        private ElementDataModel element;
        private SolidColorBrush aerColor;

        public ElementDataModel Element { get => element; private set { element = value; SetPropertyChanged(this, "Element"); } }
        public SolidColorBrush AERColor { get => aerColor; private set { aerColor = value; SetPropertyChanged(this, "AERColor"); } }

        public PeriodicTableElementModel(ElementDataModel element, SolidColorBrush avarageEnergyReleasedColor) {
            Element = element;
            AERColor = avarageEnergyReleasedColor;
        }
    }
}
