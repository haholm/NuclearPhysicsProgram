using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuclearPhysicsProgram.ViewModels.ElementInfoViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;

namespace NuclearPhysicsProgram.ViewModels.ElementInfoViewModels.Tests {
    [TestClass()]
    public class PlotViewModelTests {
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
        public void PlotViewModelTest() {

        }

        [TestMethod()]
        public void SetupPlotTest() {
            foreach (var isotopeData in ElementViewModel.IsotopeDataDictionary.Values) {
                foreach (var isotope in isotopeData.Isotopes) {
                    mainViewModel.PlotViewModel.SetupPlot(isotope);
                    SetupPlotVerify(isotope);
                }
            }
        }

        private void SetupPlotVerify(Models.IsotopeModel isotope) {
            double halfLife = ElementViewModel.GetHalfLife(isotope);
            if (halfLife <= 0) {
                Assert.IsInstanceOfType(mainViewModel.PlotViewModel.Effect, 
                                        typeof(BlurEffect));
                Assert.AreEqual((halfLife, " ", 10), 
                                (mainViewModel.PlotViewModel.UnitHalfLife, mainViewModel.PlotViewModel.Unit, mainViewModel.PlotViewModel.MaximumTime));
                return;
            }

            Assert.AreEqual((null, PlotViewModel.ConvertToAppropriateUnit(ref halfLife), halfLife, 998), 
                            (mainViewModel.PlotViewModel.Effect, mainViewModel.PlotViewModel.Unit, mainViewModel.PlotViewModel.UnitHalfLife, mainViewModel.PlotViewModel.DataPoints.Count));
        }
    }
}