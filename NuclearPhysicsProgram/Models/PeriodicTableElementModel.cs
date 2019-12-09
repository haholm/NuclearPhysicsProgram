using System.Windows.Media;

namespace NuclearPhysicsProgram.Models {
    public class PeriodicTableElementModel : ViewModels.PropertyHandler.NotifyPropertyChanged {
        private ElementDataModel element;
        private SolidColorBrush aerColor;
        private SolidColorBrush instabilityColor;
        private double? mass;

        public ElementDataModel Element { get => element; private set { element = value; SetPropertyChanged(this, "Element"); } }
        public SolidColorBrush AERColor { get => aerColor; private set { aerColor = value; SetPropertyChanged(this, "AERColor"); } }
        public SolidColorBrush InstabilityColor { get => instabilityColor; private set { instabilityColor = value; SetPropertyChanged(this, "InstabilityColor"); } }
        public double? Mass { get => mass; private set { mass = value; SetPropertyChanged(this, "Mass"); } }

        public PeriodicTableElementModel(ElementDataModel element, SolidColorBrush avarageEnergyReleasedColor, SolidColorBrush instabilityColor, double mass) {
            Element = element;
            AERColor = avarageEnergyReleasedColor;
            InstabilityColor = instabilityColor;
            Mass = mass;
        }
    }
}
