using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuclearPhysicsProgram.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuclearPhysicsProgram.ViewModels.Tests {
    [TestClass()]
    public class ApplicationViewModelTests {
        private ApplicationViewModel applicationViewModel;

        [TestInitialize]
        public void TestInitialize() {
            if (File.Exists(ApplicationViewModel.ConfigPath))
                File.Delete(ApplicationViewModel.ConfigPath);

            applicationViewModel = new ApplicationViewModel();
        }

        [TestCleanup]
        public void TestCleanup() {
            applicationViewModel = null;
        }

        [TestMethod()]
        public void ApplicationViewModelTest() {
            Assert.AreEqual("1", File.ReadAllText(ApplicationViewModel.ConfigPath));

            applicationViewModel.Begin();
            Assert.AreEqual("0", File.ReadAllText(ApplicationViewModel.ConfigPath));

            // reinit and confirm reinit
            TestInitialize();
            Assert.AreEqual("1", File.ReadAllText(ApplicationViewModel.ConfigPath));
        }

        [TestMethod()]
        public void BeginTest() {
            applicationViewModel.Begin();
            Assert.AreEqual("0", File.ReadAllText(ApplicationViewModel.ConfigPath));
        }
    }
}