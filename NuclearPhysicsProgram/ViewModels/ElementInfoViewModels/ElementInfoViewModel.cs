using NuclearPhysicsProgram.Models;
using NuclearPhysicsProgram.ViewModels.Commands.ElementInfoCommands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace NuclearPhysicsProgram.ViewModels.ElementInfoViewModels {

    public class ElementInfoViewModel : PropertyHandler.NotifyPropertyChanged {
        private const int infoPages = 2;
        private int currentInfoPage = 1;
        private string infoIsotopeName;
        private double? infoProtons;
        private double? infoNeutrons;
        private double? plotViewCanvasLeft;
        private double? plotViewOpacity;
        private Storyboard plotViewStoryboard;
        private bool? isArrowLeftEnabled;
        private bool? isArrowRightEnabled;
        private string infoHalfLife;

        public static IsotopeDataModel CurrentIsotopeData { get; set; }
        public ICommand SwitchInfoCommand { get; private set; }
        public string InfoIsotopeName { get => infoIsotopeName; set { infoIsotopeName = value; SetPropertyChanged(this, "InfoIsotopeName"); } }
        public double? InfoProtons { get => infoProtons; set { infoProtons = value; SetPropertyChanged(this, "InfoProtons"); } }
        public double? InfoNeutrons { get => infoNeutrons; set { infoNeutrons = value; SetPropertyChanged(this, "InfoNeutrons"); } }
        public string InfoHalfLife { get => infoHalfLife; set { infoHalfLife = value; SetPropertyChanged(this, "InfoHalfLife"); } }
        public double? PlotViewOpacity { get => plotViewOpacity; set { plotViewOpacity = value; SetPropertyChanged(this, "PlotViewOpacity"); } }
        public Storyboard PlotViewStoryboard { get => plotViewStoryboard; set { plotViewStoryboard = value; SetPropertyChanged(this, "PlotViewStoryboard"); } }
        public bool? IsArrowLeftEnabled {
            get => isArrowLeftEnabled;
            private set {
                isArrowLeftEnabled = value;
                SetPropertyChanged(this, "IsArrowLeftEnabled");
                SetPropertyChanged(this, "ArrowLeftOpacity");
            }
        }
        public bool? IsArrowRightEnabled {
            get => isArrowRightEnabled;
            private set {
                isArrowRightEnabled = value;
                SetPropertyChanged(this, "IsArrowRightEnabled");
                SetPropertyChanged(this, "ArrowRightOpacity");
            }
        }
        public double? ArrowLeftOpacity { get => isArrowLeftEnabled.GetValueOrDefault() ? 1 : 0.25; }
        public double? ArrowRightOpacity { get => isArrowRightEnabled.GetValueOrDefault() ? 1 : 0.25; }

        public ElementInfoViewModel() {
            SwitchInfoCommand = new SwitchInfoCommand(this);
            IsArrowRightEnabled = true;
        }

        public void SwitchInfo(string direction) {
            if (direction == "r" && currentInfoPage < infoPages)
                ++currentInfoPage;
            else if (currentInfoPage > 0)
                --currentInfoPage;
            else
                return;

            IsArrowRightEnabled = currentInfoPage != 2 ? true : false;
            IsArrowLeftEnabled = currentInfoPage != 1 ? true : false;
        }

        public void OpenWikipedia() => System.Diagnostics.Process.Start($"https://wikipedia.org/wiki/{InfoIsotopeName}");
    }
}
