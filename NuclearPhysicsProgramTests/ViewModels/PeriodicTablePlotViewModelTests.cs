using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuclearPhysicsProgram.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuclearPhysicsProgram.ViewModels.Tests {
    [TestClass()]
    public class PeriodicTablePlotViewModelTests {
        private MainViewModel mainViewModel;

        [TestInitialize]
        public void TestInitialize() {
            mainViewModel = new MainViewModel();
        }

        [TestCleanup]
        public void TestCleanup() {
            mainViewModel.MainWindowSizeUpdateTaskCTS.Cancel();
            mainViewModel = null;
        }

        [TestMethod()]
        public void PeriodicTablePlotViewModelTest() {
            Task.Run(() => { 
                while (!mainViewModel.PeriodicTablePlotViewModel.SetupFinished)
                    System.Threading.Thread.Sleep(10);
            }).Wait(1000);

            Assert.IsTrue(mainViewModel.PeriodicTablePlotViewModel.ScatterPoints.Count > 0 &&
                          PeriodicTablePlotViewModel.OpenScatterPoints.Count > 0 &&
                          PeriodicTablePlotViewModel.OpenDataPoints.Count > 0 &&
                          mainViewModel.PeriodicTablePlotViewModel.DataPoints.Count > 0);
        }

        [TestMethod()]
        public void InitializePlotTest() {
            mainViewModel.PeriodicTablePlotViewModel.InitializePlot("instability");
            Assert.AreEqual(("instability", "https://www.nuclear-power.net/nuclear-power/reactor-physics/atomic-nuclear-physics/nuclear-stability/"), 
                            (PeriodicTablePlotViewModel.CurrentPlotType, mainViewModel.PeriodicTablePlotViewModel.CurrentPlotInfo));
            Assert.IsInstanceOfType(mainViewModel.PeriodicTablePlotViewModel.PlotModel.Series[0], typeof(OxyPlot.Series.ScatterSeries));

            mainViewModel.PeriodicTablePlotViewModel.InitializePlot("energy");
            Assert.AreEqual(("energy", "https://wikipedia.org/wiki/Nuclear_binding_energy#Nuclear_binding_energy_curve"),
                            (PeriodicTablePlotViewModel.CurrentPlotType, mainViewModel.PeriodicTablePlotViewModel.CurrentPlotInfo));
            Assert.IsInstanceOfType(mainViewModel.PeriodicTablePlotViewModel.PlotModel.Series[0], typeof(OxyPlot.Series.LineSeries));

        }
    }
}