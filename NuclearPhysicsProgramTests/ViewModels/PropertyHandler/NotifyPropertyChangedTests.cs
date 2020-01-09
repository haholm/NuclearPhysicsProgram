using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuclearPhysicsProgram.ViewModels.PropertyHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuclearPhysicsProgram.ViewModels.PropertyHandler.Tests {
    [TestClass()]
    public class NotifyPropertyChangedTests {
        public class PropertyClass : NotifyPropertyChanged {
            private bool? variable;

            public bool? Property {
                get => variable;
                set {
                    variable = value;
                    SetPropertyChanged(this, nameof(Property));
                }
            }
        }

        PropertyClass propertyClass;
        bool propertyChanged;

        [TestInitialize()]
        public void TestInitialize() {
            propertyClass = new PropertyClass();
            propertyClass.PropertyChanged += (o, args) => propertyChanged = true;
        }

        [TestCleanup()]
        public void TestCleanup() {
            propertyClass.PropertyChanged -= (o, args) => propertyChanged = true;
            propertyClass = null;
        }

        [TestMethod()]
        public void SetPropertyChangedTest() {
            propertyClass.Property = true;
            Assert.IsTrue(propertyChanged && (propertyClass.Property == true));

            propertyClass.Property = false;
            Assert.IsTrue(propertyChanged && (propertyClass.Property == false));

            propertyClass.Property = null;
            Assert.IsTrue(propertyChanged && (propertyClass.Property == null));
        }
    }
}