using NuclearPhysicsProgram;
using NuclearPhysicsProgram.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using OxyPlot;
using System.Windows.Media.Effects;

namespace NuclearPhysicsProgram.ViewModels.ElementInfoViewModels {
    public class PlotViewModel : PropertyHandler.NotifyPropertyChanged {
        private Effect effect;
        private double? maximumTime;
        private string unit;
        private double? unitTitlePosition;
        private BlurEffect blurryEffect;

        public Effect Effect { get => effect; private set { effect = value; SetPropertyChanged(this, "Effect"); } }
        public double? MaximumTime { get => maximumTime; private set { maximumTime = value; SetPropertyChanged(this, "MaximumTime"); } }
        public string Unit { get => unit; private set { unit = value; SetPropertyChanged(this, "Unit"); } }
        public double? UnitTitlePosition { get => unitTitlePosition; set { unitTitlePosition = value; SetPropertyChanged(this, "UnitTitlePosition"); } }
        public ObservableCollection<DataPoint> DataPoints { get; private set; }

        public PlotViewModel() {
            blurryEffect = new BlurEffect();
            blurryEffect.Radius = 8;
            DataPoints = new ObservableCollection<DataPoint>();
        }

        public void SetupPlot(IsotopeModel isotope) {
            DataPoints.Clear();
            double halfLife = GetHalfLife(isotope);
            if (halfLife == 0) {
                Effect = blurryEffect;
                Unit = " ";
                MaximumTime = 10;
                return;
            }

            Effect = null;
            string unit = ConvertToAppropriateUnit(ref halfLife);
            Unit = unit;
            UnitTitlePosition = 1.02 + ((double)(unit.Length - 5) / 1000 * 4);
            MaximumTime = 2 * Math.PI * halfLife;
            double time = 0;
            double timeStep = 1;
            if (unit == "years" && halfLife > 1000)
                timeStep = halfLife / 100;

            double numberOfNuclides = CalculateNumberOfNuclides(halfLife, 0);
            for (; numberOfNuclides > 0.1; time += timeStep) {
                numberOfNuclides = CalculateNumberOfNuclides(halfLife, time);
                DataPoints.Add(new DataPoint(time, numberOfNuclides));
            }
        }

        private double CalculateNumberOfNuclides(double halfLife, double time) => 10 * Math.Pow(0.5, time / halfLife);

        private double GetHalfLife(IsotopeModel isotope) {
            string halfLifeString = isotope.HalfLife.Replace(",", "").Replace(".", ",");
            if (halfLifeString == string.Empty || halfLifeString == "?")
                return 0;

            if (halfLifeString.Contains("e")) {
                double.TryParse(halfLifeString.Substring(0, halfLifeString.IndexOf("e")), out double firstNumber);
                int.TryParse(halfLifeString.Substring(halfLifeString.IndexOf("e") + 1), out int power);
                return firstNumber * Math.Pow(10, power);
            }
            else {
                return double.Parse(halfLifeString);
            }
        }

        private string ConvertToAppropriateUnit(ref double halfLife) {
            if (halfLife > TimeSpan.MaxValue.TotalSeconds) {
                halfLife /= 31536000;
                return "years";
            }

            TimeSpan halfLifeInSeconds = TimeSpan.FromSeconds(halfLife);
            if (halfLife >= 60 && halfLife < 3600) {
                halfLife = halfLifeInSeconds.TotalMinutes;
                return "minutes";
            }
            else if (halfLife >= 3600 && halfLife < 86400) {
                halfLife = halfLifeInSeconds.TotalHours;
                return "hours";
            }
            else if (halfLife >= 86400 && halfLife < 31536000) {
                halfLife = halfLifeInSeconds.TotalDays;
                return "days";
            }
            else if (halfLife >= 31536000) {
                halfLife = halfLifeInSeconds.TotalDays / 365;
                return "years";
            }
            else {
                halfLife = halfLifeInSeconds.TotalSeconds;
                return "seconds";
            }
        }
    }
}
