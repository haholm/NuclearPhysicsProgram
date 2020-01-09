using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuclearPhysicsProgram.ViewModels.ElementInfoViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuclearPhysicsProgram.ViewModels.ElementInfoViewModels.Tests {
    [TestClass()]
    public class DecayChainViewModelTests {
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
        public void GetDecaySymbolTest() {
            foreach (var isotopeData in ElementViewModel.IsotopeDataDictionary.Values) {
                foreach (var isotope in isotopeData.Isotopes) {
                    foreach (var decay in isotope.Decays) {
                        GetDecaySymbolVerify(DecayChainViewModel.GetDecaySymbol(decay.Type));
                    }
                }
            }
        }

        private void GetDecaySymbolVerify(string decayType) {
            switch (decayType) {
                case "α":
                    return;
                case "β+":
                    return;
                case "β-":
                    return;
                case "EC":
                    return;
                case "β-,β-":
                    return ;
                case "γ":
                    return;
                case "β-,γ":
                    return;
                case "EC,γ":
                    return;
                case "EC,EC":
                    return;
                default:
                    Assert.Fail();
                    break;
            }
        }

        [TestMethod()]
        public void SwitchDecayChainIsotopeTest() {
            mainViewModel.DecayChainViewModel.SwitchDecayChainIsotope("next");
            mainViewModel.DecayChainViewModel.SwitchDecayChainIsotope("previous");
        }

        [TestMethod()]
        public void SetupDecayChainsTest() {
            foreach (var elementData in ElementViewModel.ElementDataDictionary.Values) {
                double avarageEnergyReleased = mainViewModel.DecayChainViewModel.SetupDecayChains(elementData, null, true);
                VerifyDecayChainsTest(elementData, avarageEnergyReleased);
            }
        }

        private void VerifyDecayChainsTest(Models.ElementDataModel elementData, double avarageEnergyReleased) {

            Assert.AreEqual(DecayChainViewModel.GetAvarageEnergyReleased(elementData), avarageEnergyReleased);
        }

        [TestMethod()]
        public void SetupDecayChainsTest1() {

        }
    }
}