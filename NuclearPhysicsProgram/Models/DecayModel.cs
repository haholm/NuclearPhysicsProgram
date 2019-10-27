namespace NuclearPhysicsProgram.Models {
    public class DecayModel {
        public DecayModel(string type, string product, int atomicNumber, int massNumber) {
            Type = type;
            ProductSymbol = product;
            AtomicNumber = atomicNumber;
            MassNumber = massNumber;
        }

        public string Type { get; private set; }
        public string ProductSymbol { get; private set; }
        public int AtomicNumber { get; private set; }
        public int MassNumber { get; private set; }
    }

}
