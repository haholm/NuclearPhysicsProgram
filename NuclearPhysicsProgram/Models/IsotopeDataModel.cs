namespace NuclearPhysicsProgram.Models {
    public class IsotopeDataModel {
        public IsotopeDataModel(string symbol, IsotopeModel[] isotopes) {
            Symbol = symbol;
            Isotopes = isotopes;
        }

        public string Symbol { get; private set; }
        public IsotopeModel[] Isotopes { get; private set; }
    }
}
