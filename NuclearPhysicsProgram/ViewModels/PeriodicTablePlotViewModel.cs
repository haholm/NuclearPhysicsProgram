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
        public static List<(string symbol, string elementName, int massNumber, int atomicNumber, double halfLife)> OpenScatterPoints { get; private set; }
        public PlotModel PlotModel { get => plotModel; private set { plotModel = value; SetPropertyChanged(this, "PlotModel"); } }

        public PeriodicTablePlotViewModel() {
            bindingEnergyDataPoints = new ObservableCollection<DataPoint>();
            stabilityScatterPoints = new ObservableCollection<ScatterPoint>();
            instabilityPlotModel = new PlotModel();
            bindingEnergyPlotModel = new PlotModel();
            DataPoints = new ObservableCollection<DataPoint>();
            ScatterPoints = new ObservableCollection<ScatterPoint>();
            OpenDataPoints = new List<(string, int, int)>();
            OpenScatterPoints = new List<(string, string, int, int, double)>();
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
                    OpenScatterPoints.Add((elementDataModel.Symbol, elementDataModel.Name, isotopeModel.MassNumber, elementDataModel.AtomicNumber, ScatterPoints.Last().Value));
                }
            }
        }

        private async Task SetupBindingEnergyDataPoints() {
            await WaitForElements();

            double lastBindingEnergy = 7;
            for (int i = 0; i < ElementViewModel.ElementDataDictionary.Count; i++) {
                var element = ElementViewModel.ElementDataDictionary.Values.ElementAt(i);
                double elementMass = ElementViewModel.GetMassInAMU(element);
                double massDefect = elementMass - ElementViewModel.PeriodicTableMasses[i];
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
