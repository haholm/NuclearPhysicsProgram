using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuclearPhysicsProgram.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuclearPhysicsProgram.ViewModels.Tests {
    [TestClass()]
    public class ElementViewModelTests {
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
        public void ElementViewModelTest() {
            Assert.IsTrue(mainViewModel.ElementViewModel.Elements.Count > 0 &&
                          mainViewModel.ElementViewModel.Isotopes.Count > 0 &&
                          ElementViewModel.ElementDataDictionary.Count > 0 &&
                          ElementViewModel.IsotopeDataDictionary.Count > 0 &&
                          ElementViewModel.ElementsLoaded == true &&
                          mainViewModel.ElementViewModel.FirstLeftElements.Count > 0 &&
                          mainViewModel.ElementViewModel.FirstRightElements.Count > 0 &&
                          mainViewModel.ElementViewModel.SecondLeftElements.Count > 0 &&
                          mainViewModel.ElementViewModel.SecondRightElements.Count > 0 &&
                          mainViewModel.ElementViewModel.ThirdLeftElements.Count > 0 &&
                          mainViewModel.ElementViewModel.ThirdRightElements.Count > 0 &&
                          mainViewModel.ElementViewModel.FourthElements.Count > 0 &&
                          mainViewModel.ElementViewModel.FifthElements.Count > 0 &&
                          mainViewModel.ElementViewModel.SixthLeftElements.Count > 0 &&
                          mainViewModel.ElementViewModel.SixthHiddenElements.Count > 0 &&
                          mainViewModel.ElementViewModel.SixthRightElements.Count > 0 &&
                          mainViewModel.ElementViewModel.SeventhLeftElements.Count > 0 &&
                          mainViewModel.ElementViewModel.SeventhHiddenElements.Count > 0 &&
                          mainViewModel.ElementViewModel.SeventhRightElements.Count > 0);
        }

        [TestMethod()]
        public void GetIsotopeTest() {
            foreach (var isotopeData in ElementViewModel.IsotopeDataDictionary.Values) {
                foreach (var isotope in isotopeData.Isotopes) {
                    var retrievedIsotope = ElementViewModel.GetIsotope(isotope.Symbol, isotope.MassNumber);
                    Assert.AreEqual(isotope, retrievedIsotope);
                }
            }
        }

        [TestMethod()]
        public void GetIsotopeTest1() {
            foreach (var isotopeData in ElementViewModel.IsotopeDataDictionary.Values) {
                foreach (var isotope in isotopeData.Isotopes) {
                    var retrievedIsotope = ElementViewModel.GetIsotope(isotopeData, isotope.MassNumber);
                    Assert.AreEqual(isotope, retrievedIsotope);
                }
            }
        }

        [TestMethod()]
        public void GetHalfLifeTest() {
            foreach (var isotopeData in ElementViewModel.IsotopeDataDictionary.Values) {
                foreach (var isotope in isotopeData.Isotopes) {
                    ElementViewModel.GetHalfLife(isotope);
                }
            }
        }

        [TestMethod()]
        public void GetAvarageHalfLifeTest() {
            foreach (var isotopeData in ElementViewModel.IsotopeDataDictionary.Values) {
                ElementViewModel.GetAvarageHalfLife(isotopeData);
            }
        }

        [TestMethod()]
        public void GetMassInAMUTest() {
            foreach (var elementData in ElementViewModel.ElementDataDictionary.Values) {
                Assert.AreEqual((elementData.AtomicNumber * Constants.Proton.Mass.AtomicMassUnits) + ((elementData.MassNumber - elementData.AtomicNumber) * Constants.Neutron.Mass.AtomicMassUnits), 
                                ElementViewModel.GetMassInAMU(elementData));
            }
        }
    }
}