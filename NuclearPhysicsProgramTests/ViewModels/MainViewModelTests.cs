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
    public class MainViewModelTests {
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
        public void MainViewModelTest() {
            Assert.AreEqual(TaskStatus.WaitingForActivation, mainViewModel.MainWindowSizeUpdateTask.Status, "MainViewModelTests.MainViewModelTest\nMainViewModel.MainWindowSizeUpdateTask");
        }

        [TestMethod()]
        public void OpenElementInfoTest() {
            mainViewModel.OpenElementInfo("H", 1, test: true).Wait();
            Assert.AreEqual(Visibility.Collapsed, mainViewModel.PeriodicTableViewVisibility);
            Assert.AreEqual(Visibility.Visible, mainViewModel.ElementInfoViewVisibility);
            Assert.AreEqual(1, mainViewModel.ElementInfoViewOpacity);
            Assert.IsInstanceOfType((mainViewModel.CurrentIsotopeData, ElementInfoViewModels.ElementInfoViewModel.CurrentIsotopeData), typeof(ValueTuple<Models.IsotopeDataModel, Models.IsotopeDataModel>));
            Assert.IsTrue(mainViewModel.DecayChainViewModel.IsotopeDecayChain.Count > 0 && mainViewModel.DecayChainViewModel.IsArrowUpEnabled.HasValue && mainViewModel.DecayChainViewModel.IsArrowDownEnabled.HasValue);
        }

        [TestMethod()]
        public void CloseElementInfoTest() {
            mainViewModel.CloseElementInfo(test: true).Wait();
            Assert.AreEqual(Visibility.Visible, mainViewModel.PeriodicTableViewVisibility);
            Assert.AreEqual(Visibility.Collapsed, mainViewModel.ElementInfoViewVisibility);
        }

        [TestMethod()]
        public void InitializeIsotopeDataTest() {
            mainViewModel.InitializeIsotopeData("H", 1);
            Assert.IsInstanceOfType(mainViewModel.CurrentIsotopeData, typeof(Models.IsotopeDataModel));
            Assert.IsTrue(mainViewModel.DecayChainViewModel.IsotopeDecayChain.Count > 0 && mainViewModel.DecayChainViewModel.IsArrowUpEnabled.HasValue && mainViewModel.DecayChainViewModel.IsArrowDownEnabled.HasValue);
        }

        [TestMethod()]
        public void TogglePropertyColorTest() {
            mainViewModel.TogglePropertyColor("instability");
            Assert.AreEqual((new Thickness(0, 100, 0, 0), Visibility.Visible), (mainViewModel.PeriodicTableAreaMargin, mainViewModel.ItemViewInstabilityColorVisibility));

            mainViewModel.TogglePropertyColor("energy");
            Assert.AreEqual((new Thickness(0, 100, 0, 0), Visibility.Visible), (mainViewModel.PeriodicTableAreaMargin, mainViewModel.ItemViewAERColorVisibility));
        }
    }
}