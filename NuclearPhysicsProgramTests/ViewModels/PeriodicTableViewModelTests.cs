using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuclearPhysicsProgram.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NuclearPhysicsProgram.ViewModels.Tests {
    [TestClass()]
    public class PeriodicTableViewModelTests {
        private MainViewModel mainViewModel;
        private PeriodicTableViewModel periodicTableViewModel;

        [TestInitialize]
        public void TestInitialize() {
            mainViewModel = new MainViewModel();
            periodicTableViewModel = new PeriodicTableViewModel();
        }

        [TestCleanup]
        public void TestCleanup() {
            mainViewModel.MainWindowSizeUpdateTaskCTS.Cancel();
            mainViewModel = null;
            periodicTableViewModel = null;
        }

        [TestMethod()]
        public void ExpandHiddenElementsTest() {
            Assert.IsTrue(periodicTableViewModel.ExpandHiddenElements(1, true).Wait(1000) &&
                          periodicTableViewModel.FirstHiddenElementsWidth == 870 &&
                          periodicTableViewModel.FirstHiddenElementsVisibility == Visibility.Visible &&
                          periodicTableViewModel.ExpandHiddenElements(2, true).Wait(1000) &&
                          periodicTableViewModel.SecondHiddenElementsWidth == 870 &&
                          periodicTableViewModel.SecondHiddenElementsVisibility == Visibility.Visible);
        }

        [TestMethod()]
        public void ContractHiddenElementsTest() {
            Assert.IsTrue(periodicTableViewModel.ContractHiddenElements(1, true).Wait(1000) &&
                          periodicTableViewModel.FirstHiddenElementsWidth == 58 &&
                          periodicTableViewModel.FirstHiddenElementsVisibility == Visibility.Collapsed);
            Assert.IsTrue(periodicTableViewModel.ContractHiddenElements(2, true).Wait(1000) &&
                          periodicTableViewModel.SecondHiddenElementsWidth == 58 &&
                          periodicTableViewModel.SecondHiddenElementsVisibility == Visibility.Collapsed);
        }
    }
}