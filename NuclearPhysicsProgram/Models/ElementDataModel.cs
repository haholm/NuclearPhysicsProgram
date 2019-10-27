namespace NuclearPhysicsProgram.Models {
    public class ElementDataModel {
        public ElementDataModel(string name, string symbol, int atomicNumber, int massNumber) {
            Name = name;
            Symbol = symbol;
            AtomicNumber = atomicNumber;
            MassNumber = massNumber;
        }

        public string Name { get; private set; }
        public string Symbol { get; private set; }
        public int AtomicNumber { get; private set; }
        public int MassNumber { get; private set; }
    }
}
