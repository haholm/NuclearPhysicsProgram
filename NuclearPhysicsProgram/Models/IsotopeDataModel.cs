namespace NuclearPhysicsProgram.Models {
    public class IsotopeDataModel {
        public IsotopeDataModel(string symbol, IsotopeModel[] isotopes) {
            Symbol = symbol;
            Isotopes = isotopes;
        }

        public string Symbol { get; private set; }
        public IsotopeModel[] Isotopes { get; private set; }
    }

    public class IsotopeModel {
        public IsotopeModel(string symbol, int massNumber, string halfLife, DecayModel[] decays) {
            Symbol = symbol;
            MassNumber = massNumber;
            HalfLife = halfLife;
            Decays = decays;
        }

        public string Symbol { get; private set; }
        public int MassNumber { get; private set; }   //ÄR DETTA ÄNDRAT TILL MASSNUMBER I ISOTOPES.JSON? JA?
        public string HalfLife { get; private set; }
        public DecayModel[] Decays { get; private set; }
    }
}
