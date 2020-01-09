using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuclearPhysicsProgram.ViewModels.ElementInfoViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuclearPhysicsProgram.ViewModels.ElementInfoViewModels.Tests {
    [TestClass()]
    public class ElementInfoViewModelTests {
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
        public void SwitchInfoTest() {
            mainViewModel.ElementInfoViewModel.SwitchInfo("r");
            Assert.AreEqual((false, true),
                            (mainViewModel.ElementInfoViewModel.IsArrowRightEnabled, mainViewModel.ElementInfoViewModel.IsArrowLeftEnabled));

            mainViewModel.ElementInfoViewModel.SwitchInfo("l");
            Assert.AreEqual((true, false),
                            (mainViewModel.ElementInfoViewModel.IsArrowRightEnabled, mainViewModel.ElementInfoViewModel.IsArrowLeftEnabled));
        }
    }
}