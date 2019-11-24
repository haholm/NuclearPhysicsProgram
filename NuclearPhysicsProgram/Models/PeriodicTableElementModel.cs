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
        private SolidColorBrush instabilityColor;

        public ElementDataModel Element { get => element; private set { element = value; SetPropertyChanged(this, "Element"); } }
        public SolidColorBrush AERColor { get => aerColor; private set { aerColor = value; SetPropertyChanged(this, "AERColor"); } }
        public SolidColorBrush InstabilityColor { get => instabilityColor; private set { instabilityColor = value; SetPropertyChanged(this, "InstabilityColor"); } }

        public PeriodicTableElementModel(ElementDataModel element, SolidColorBrush avarageEnergyReleasedColor, SolidColorBrush instabilityColor) {
            Element = element;
            AERColor = avarageEnergyReleasedColor;
            InstabilityColor = instabilityColor;
        }
    }
}
