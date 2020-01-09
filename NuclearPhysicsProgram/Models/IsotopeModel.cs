using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuclearPhysicsProgram.Models {
    public class IsotopeModel {
        public IsotopeModel(string symbol, int massNumber, string halfLife, DecayModel[] decays) {
            Symbol = symbol;
            MassNumber = massNumber;
            HalfLife = halfLife;
            Decays = decays;
        }

        public string Symbol { get; private set; }
        public int MassNumber { get; private set; }
        public string HalfLife { get; private set; }
        public DecayModel[] Decays { get; private set; }
    }
}
