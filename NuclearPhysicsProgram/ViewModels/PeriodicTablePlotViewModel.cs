using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuclearPhysicsProgram.ViewModels {
    public class PeriodicTablePlotViewModel : PropertyHandler.NotifyPropertyChanged {
        private readonly double[] masses = { 
            1.008, 4.0026, 
            6.94, 9.0122, 10.81, 12.011, 14.007, 15.999, 18.998, 20.180,
            22.990, 24.305, 26.982, 28.085, 30.974, 32.06, 35.45, 39.948, 
            39.098, 40.078, 44.956, 47.867, 50.942, 51.996, 54.938, 55.845, 58.933, 58.693, 63.546, 65.38, 69.723, 72.630, 74.922, 78.971, 79.904, 83.798,
            85.468, 87.62, 88.906, 91.224, 92.906, 95.95, 98, 101.07, 102.91, 106.42, 107.87, 112.41, 114.82, 118.71, 121.76, 127.60, 126.90, 131.29,
            132.91, 137.33, 138.91, 140.12, 140.91, 144.24, 145, 150.36, 151.96, 157.25, 158.93, 162.50, 164.93, 167.26, 168.93, 173.05, 174.97, 178.49, 180.95, 183.84, 186.21, 190.23, 192.22, 195.08, 196.97, 200.59, 204.38, 207.2, 208.98, 209, 210, 222,
            223, 226, 227, 232.04, 231.04, 238.03, 237, 244, 243, 247, 247, 251, 252, 257, 258, 259, 266, 267, 268, 269, 270, 270, 278, 281, 282, 285, 286, 289, 290, 293, 294, 294};
        private double[] massDefects;
        private ObservableCollection<DataPoint> bindingEnergyDataPoints;

        public ObservableCollection<DataPoint> DataPoints { get; private set; }
        public PlotModel PlotModel { get; private set; }

        public PeriodicTablePlotViewModel() {
            massDefects = new double[118];
            bindingEnergyDataPoints = new ObservableCollection<DataPoint>();
            DataPoints = new ObservableCollection<DataPoint>();
            PlotModel = new PlotModel();
            SetupDataPoints();

            var transparent = OxyColor.FromArgb(0, 0, 0, 0);
            PlotModel.PlotAreaBorderColor = transparent;
            PlotModel.Background = transparent;
            PlotModel.PlotMargins = new OxyThickness(2);
            PlotModel.Padding = new OxyThickness(-1, 10, -2, 0);
            PlotModel.DefaultFont = "Open Sans";
            var leftAxis = new OxyPlot.Axes.LinearAxis {
                MinorStep = 1,
                Position = OxyPlot.Axes.AxisPosition.Left,
                Title = "Binding energy per nucleon (MeV)",
                Maximum = 9,
                MajorTickSize = 6,
                IntervalLength = 22,
                AxisTitleDistance = 10,
                FontSize = 10
            };
            var rightAxis = new OxyPlot.Axes.LinearAxis {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                Title = "Nucleons in nucleus",
                Maximum = 294,
                MinorTickSize = 0,
                MajorTickSize = 6,
                IntervalLength = 30,
                FontSize = 10
            };
            PlotModel.Axes.Add(leftAxis);
            PlotModel.Axes.Add(rightAxis);
        }

        private async void SetupDataPoints() {
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
                if (bindingEnergyMeVPerNucleon < 0 || bindingEnergyMeVPerNucleon > 9 || bindingEnergyMeVPerNucleon < lastBindingEnergy - 1 || bindingEnergyMeVPerNucleon > lastBindingEnergy + 1)
                    continue;
                else {
                    bindingEnergyDataPoints.Add(new DataPoint(element.MassNumber, bindingEnergyMeVPerNucleon));
                }
            }

            DataPoints = bindingEnergyDataPoints;
        }

        private async Task WaitForElements() {
            while (ElementViewModel.ElementDataDictionary == null)
                await Task.Delay(10);
        }
    }
}
