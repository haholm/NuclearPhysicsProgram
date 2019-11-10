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

    class ElementInfoViewModel : PropertyHandler.NotifyPropertyChanged {
        private const int infoPages = 2;
        private int currentInfoPage = 0;
        private double? infoProtons;
        private double? infoNeutrons;
        private double? plotViewCanvasLeft;
        private double? plotViewOpacity;
        private Storyboard plotViewStoryboard;

        public static IsotopeDataModel CurrentIsotopeData { get; set; }
        public ICommand SwitchInfoCommand { get; private set; }
        /// set from mainvm
        public double? InfoProtons { get => infoProtons; set { infoProtons = value; SetPropertyChanged(this, "InfoProtons"); } }
        public double? InfoNeutrons { get => infoNeutrons; set { infoNeutrons = value; SetPropertyChanged(this, "InfoNeutrons"); } }
        public double? PlotViewOpacity { get => plotViewOpacity; set { plotViewOpacity = value; SetPropertyChanged(this, "PlotViewOpacity"); } }
        public Storyboard PlotViewStoryboard { get => plotViewStoryboard; set { plotViewStoryboard = value; SetPropertyChanged(this, "PlotViewStoryboard"); } }

        public ElementInfoViewModel() {
            SwitchInfoCommand = new SwitchInfoCommand(this);
        }

        public void SwitchInfo(string direction) {
            if (direction == "right" && currentInfoPage < infoPages) {
                /// 
                /// Quick prototype, will clean up
                ///

                ++currentInfoPage;
                plotViewStoryboard = new Storyboard();
                Storyboard.SetTargetName(plotViewStoryboard, "PlotView");
                Storyboard.SetTargetProperty(plotViewStoryboard, new PropertyPath("(Canvas.Left)"));
                DoubleAnimation da = new DoubleAnimation();
                ElasticEase ef = new ElasticEase();
                ef.Springiness = 12;
                ef.Oscillations = 1;
                da.EasingFunction = ef;
                da.FillBehavior = FillBehavior.HoldEnd;
                 
                da.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 600));
                plotViewStoryboard.Children.Add(da);
                //NotifyPropertyChanged after configuration
                SetPropertyChanged(this, "PlotViewStoryboard");
            }
            else if (currentInfoPage > 0) {
                --currentInfoPage;
            }
            else
                return;
        }
    }
}
