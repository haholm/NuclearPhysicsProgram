using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NuclearPhysicsProgram.ViewModels {
    public class PeriodicTablePlotViewModel : PropertyHandler.NotifyPropertyChanged {
        private readonly OxyColor[] rangeColors = {
            OxyColors.OrangeRed,
            OxyColors.LightPink,
            OxyColors.PeachPuff,
            OxyColors.PapayaWhip,
            OxyColors.Linen,
            OxyColors.Gold,
            OxyColors.Gainsboro,
            OxyColors.MediumAquamarine,
            OxyColors.OliveDrab,
            OxyColors.Olive,
            OxyColors.DarkTurquoise,
            OxyColors.LightSkyBlue,
            OxyColors.DeepSkyBlue,
            OxyColors.LightSeaGreen,
            OxyColors.DodgerBlue,
            OxyColors.Blue,
            OxyColors.DarkBlue,
            OxyColors.Black
        };
        private readonly double[] masses = {
            1.008, 4.0026,
            6.94, 9.0122, 10.81, 12.011, 14.007, 15.999, 18.998, 20.180,
            22.990, 24.305, 26.982, 28.085, 30.974, 32.06, 35.45, 39.948,
            39.098, 40.078, 44.956, 47.867, 50.942, 51.996, 54.938, 55.845, 58.933, 58.693, 63.546, 65.38, 69.723, 72.630, 74.922, 78.971, 79.904, 83.798,
            85.468, 87.62, 88.906, 91.224, 92.906, 95.95, 98, 101.07, 102.91, 106.42, 107.87, 112.41, 114.82, 118.71, 121.76, 127.60, 126.90, 131.29,
            132.91, 137.33, 138.91, 140.12, 140.91, 144.24, 145, 150.36, 151.96, 157.25, 158.93, 162.50, 164.93, 167.26, 168.93, 173.04, 174.97, 178.49, 180.95, 183.84, 186.21, 190.23, 192.22, 195.08, 196.97, 200.59, 204.38, 207.2, 208.98, 209, 210, 222,
            223, 226, 227, 232.04, 231.04, 238.03, 237, 244, 243, 247, 247, 251, 252, 257, 258, 259, 266, 267, 268, 269, 270, 270, 278, 281, 282, 285, 286, 289, 290, 293, 294, 294
        };
        private ObservableCollection<DataPoint> bindingEnergyDataPoints;
        private ObservableCollection<ScatterPoint> stabilityScatterPoints;
        private string currentPlotInfo;
        private Visibility? visibility;
        private PlotModel instabilityPlotModel;
        private PlotModel bindingEnergyPlotModel;
        private PlotModel plotModel;

        public static string CurrentPlotType { get; private set; }
        public static string LeftInstabilityTitle { get => "Number of protons (Z)"; }
        public static string BottomInstabilityTitle { get => "Number of neutrons (N)"; }
        public static string LeftBindingEnergyTitle { get => "Binding energy per nucleon (MeV)"; }
        public static string BottomBindingEnergyTitle { get => "Nucleons in nuclei of elements in periodic table"; }
        public string CurrentPlotInfo { get => currentPlotInfo; set { currentPlotInfo = value; SetPropertyChanged(this, "CurrentPlotInfo"); } }
        public Visibility? Visibility { get => visibility; set { visibility = value; SetPropertyChanged(this, "Visibility"); } }
        public ObservableCollection<DataPoint> DataPoints { get; private set; }
        public ObservableCollection<ScatterPoint> ScatterPoints { get; private set; }
        public static List<(string elementName, int massNumber, int atomicNumber)> OpenDataPoints { get; private set; }
        public static List<(string elementName, int massNumber, int atomicNumber, double halfLife)> OpenScatterPoints { get; private set; }
        public PlotModel PlotModel { get => plotModel; private set { plotModel = value; SetPropertyChanged(this, "PlotModel"); } }

        public PeriodicTablePlotViewModel() {
            bindingEnergyDataPoints = new ObservableCollection<DataPoint>();
            stabilityScatterPoints = new ObservableCollection<ScatterPoint>();
            instabilityPlotModel = new PlotModel();
            bindingEnergyPlotModel = new PlotModel();
            DataPoints = new ObservableCollection<DataPoint>();
            ScatterPoints = new ObservableCollection<ScatterPoint>();
            OpenDataPoints = new List<(string, int, int)>();
            OpenScatterPoints = new List<(string, int, int, double)>();
            PlotModel = new PlotModel();
            Setup();
        }

        public async void InitializePlot(string type) {
            CurrentPlotType = type;
            if (type == "instability") {
                CurrentPlotInfo = "https://www.nuclear-power.net/nuclear-power/reactor-physics/atomic-nuclear-physics/nuclear-stability/";
                PlotModel = instabilityPlotModel;
            }
            else if (type == "energy") {
                CurrentPlotInfo = "https://wikipedia.org/wiki/Nuclear_binding_energy#Nuclear_binding_energy_curve";
                PlotModel = bindingEnergyPlotModel;
            }
        }

        private async void Setup() {
            await SetupInstabilityScatterPoints();
            SetupInstabilityPlot();
            await SetupBindingEnergyDataPoints();
            SetupBindingEnergyPlot();
        }

        private async Task SetupInstabilityScatterPoints() {
            await WaitForElements();

            for (int i = 0; i < ElementViewModel.IsotopeDataDictionary.Count; i++) {
                var isotopeDataModel = ElementViewModel.IsotopeDataDictionary.ElementAt(i).Value;
                var elementDataModel = ElementViewModel.ElementDataDictionary.ElementAt(i).Value;

                foreach (var isotopeModel in isotopeDataModel.Isotopes) {
                    double halfLife = ElementViewModel.GetHalfLife(isotopeModel);
                    ScatterPoints.Add(new ScatterPoint(isotopeModel.MassNumber - elementDataModel.AtomicNumber, elementDataModel.AtomicNumber, double.NaN, halfLife == 0 ? double.PositiveInfinity : halfLife));
                    OpenScatterPoints.Add((elementDataModel.Name, isotopeModel.MassNumber, elementDataModel.AtomicNumber, ScatterPoints.Last().Value));
                }
            }
        }

        private async Task SetupBindingEnergyDataPoints() {
            await WaitForElements();

            double lastBindingEnergy = 7;
            for (int i = 0; i < ElementViewModel.ElementDataDictionary.Count; i++) {
                var element = ElementViewModel.ElementDataDictionary.Values.ElementAt(i);
                double elementMass = ElementViewModel.GetMassInAMU(element);
                double massDefect = elementMass - masses[i];
                double bindingEnergyMeV = massDefect * 931.5;
                double bindingEnergyMeVPerNucleon = bindingEnergyMeV / element.MassNumber;
                if (element.Symbol == "H")
                    bindingEnergyDataPoints.Add(new DataPoint(0, 0));
                else if (bindingEnergyMeVPerNucleon < 0 || bindingEnergyMeVPerNucleon > 9 || bindingEnergyMeVPerNucleon < lastBindingEnergy - 2 || bindingEnergyMeVPerNucleon > lastBindingEnergy + 2)
                    continue;
                else {
                    bindingEnergyDataPoints.Add(new DataPoint(element.MassNumber, bindingEnergyMeVPerNucleon));
                }

                ElementViewModel.ElementDataDictionary.TryGetValue(element.Symbol, out var elementData);
                OpenDataPoints.Add((elementData.Name, (int)bindingEnergyDataPoints.Last().X, elementData.AtomicNumber));
            }

            var list = bindingEnergyDataPoints.ToList();
            List<double> xList = list.Select(item => item.X).ToList();
            xList.Sort();
            for (int i = 0; i < list.Count; i++)
                bindingEnergyDataPoints[i] = new DataPoint(xList[i], bindingEnergyDataPoints[i].Y);

            DataPoints = bindingEnergyDataPoints;
        }

        private void SetupInstabilityPlot() {
            var transparent = OxyColor.FromArgb(0, 0, 0, 0);
            instabilityPlotModel.PlotAreaBorderColor = transparent;
            instabilityPlotModel.Background = transparent;
            instabilityPlotModel.DefaultFont = "Open Sans";
            var leftAxis = new OxyPlot.Axes.LinearAxis {
                Position = OxyPlot.Axes.AxisPosition.Left,
                Title = LeftInstabilityTitle,
                IsZoomEnabled = false,
                Maximum = 120,
                IsPanEnabled = false,
                MinorTickSize = 0,
                MajorTickSize = 6,
                IntervalLength = 22,
                AxisTitleDistance = 10,
                FontSize = 14
            };
            var bottomAxis = new OxyPlot.Axes.LinearAxis {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                Title = BottomInstabilityTitle,
                IsZoomEnabled = false,
                Maximum = 180,
                IsPanEnabled = false,
                MinorTickSize = 0,
                MajorTickSize = 6,
                IntervalLength = 30,
                FontSize = 14
            };
            var secondBottomAxis = new OxyPlot.Axes.RangeColorAxis {
                StartPosition = 0.4,
                EndPosition = 0.8,
                HighColor = OxyColors.Black,
                LowColor = OxyColors.White,
                Minimum = Math.Pow(10, -15),
                Maximum = Math.Pow(10, 15)
            };

            //Add color range for unknown half life periods (-1)
            secondBottomAxis.AddRange(-1, -1, OxyColors.Silver);
            double lowerBound = secondBottomAxis.Minimum;
            for (int i = 0; i < 18; i++) {
                double upperBound = Math.Pow(10, -15 + (i * 3 / 1.7));
                secondBottomAxis.AddRange(lowerBound, upperBound, rangeColors[i]);
                lowerBound = upperBound;
            }

            instabilityPlotModel.Axes.Add(leftAxis);
            instabilityPlotModel.Axes.Add(bottomAxis);
            instabilityPlotModel.Axes.Add(secondBottomAxis);
            instabilityPlotModel.Series.Add(new ScatterSeries {
                ItemsSource = ScatterPoints,
                MarkerType = MarkerType.Square,
                MarkerSize = 1.2f
            });
        }

        private void SetupBindingEnergyPlot() {
            var transparent = OxyColor.FromArgb(0, 0, 0, 0);
            bindingEnergyPlotModel.PlotAreaBorderColor = transparent;
            bindingEnergyPlotModel.Background = transparent;
            bindingEnergyPlotModel.DefaultFont = "Open Sans";
            var leftAxis = new OxyPlot.Axes.LinearAxis {
                MinorStep = 1,
                Position = OxyPlot.Axes.AxisPosition.Left,
                Title = LeftBindingEnergyTitle,
                IsZoomEnabled = false,
                Maximum = 9,
                IsPanEnabled = false,
                MajorTickSize = 6,
                IntervalLength = 22,
                AxisTitleDistance = 10,
                FontSize = 14
            };
            var bottomAxis = new OxyPlot.Axes.LinearAxis {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                Title = BottomBindingEnergyTitle,
                IsZoomEnabled = false,
                Maximum = 294,
                IsPanEnabled = false,
                MinorTickSize = 0,
                MajorTickSize = 6,
                IntervalLength = 30,
                FontSize = 14
            };
            bindingEnergyPlotModel.Axes.Add(leftAxis);
            bindingEnergyPlotModel.Axes.Add(bottomAxis);
            bindingEnergyPlotModel.Series.Add(new OxyPlot.Series.LineSeries {
                ItemsSource = DataPoints,
                MarkerFill = OxyColors.White,
                MarkerType = MarkerType.Diamond,
                LineJoin = LineJoin.Round,
                LineStyle = LineStyle.Dash,
                Color = OxyColors.White
            });
        }

        private async Task WaitForElements() {
            while (ElementViewModel.ElementDataDictionary == null)
                await Task.Delay(10);
        }
    }
}
