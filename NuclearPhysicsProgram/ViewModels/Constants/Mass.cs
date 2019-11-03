using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuclearPhysicsProgram.ViewModels.Constants {
    public struct Mass {
        public readonly double Kilograms;
        public readonly double MegaElectronVolts;
        public readonly double AtomicMassUnits;

        public Mass(double kilograms, double megaElectronVolts, double atomicMassUnits) {
            Kilograms = kilograms;
            MegaElectronVolts = megaElectronVolts;
            AtomicMassUnits = atomicMassUnits;
        }
    }
}
