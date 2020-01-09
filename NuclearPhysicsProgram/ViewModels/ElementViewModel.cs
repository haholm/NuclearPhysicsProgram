using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using NuclearPhysicsProgram.Models;

namespace NuclearPhysicsProgram.ViewModels {
    public class ElementViewModel : PropertyHandler.NotifyPropertyChanged {
        private MainViewModel mainViewModel;

        public static bool ElementsLoaded { get; private set; }
        public static Dictionary<string, ElementDataModel> ElementDataDictionary { get; private set; }
        public static Dictionary<string, IsotopeDataModel> IsotopeDataDictionary { get; private set; }
        public static double[] PeriodicTableMasses { get; } = {
            1.008, 4.0026,
            6.94, 9.0122, 10.81, 12.011, 14.007, 15.999, 18.998, 20.180,
            22.990, 24.305, 26.982, 28.085, 30.974, 32.06, 35.45, 39.948,
            39.098, 40.078, 44.956, 47.867, 50.942, 51.996, 54.938, 55.845, 58.933, 58.693, 63.546, 65.38, 69.723, 72.630, 74.922, 78.971, 79.904, 83.798,
            85.468, 87.62, 88.906, 91.224, 92.906, 95.95, 98, 101.07, 102.91, 106.42, 107.87, 112.41, 114.82, 118.71, 121.76, 127.60, 126.90, 131.29,
            132.91, 137.33, 138.91, 140.12, 140.91, 144.24, 145, 150.36, 151.96, 157.25, 158.93, 162.50, 164.93, 167.26, 168.93, 173.04, 174.97, 178.49, 180.95, 183.84, 186.21, 190.23, 192.22, 195.08, 196.97, 200.59, 204.38, 207.2, 208.98, 209, 210, 222,
            223, 226, 227, 232.04, 231.04, 238.03, 237, 244, 243, 247, 247, 251, 252, 257, 258, 259, 266, 267, 268, 269, 270, 270, 278, 281, 282, 285, 286, 289, 290, 293, 294, 294
        };

        public ObservableCollection<ElementDataModel> Elements { get; private set; }
        public ObservableCollection<IsotopeDataModel> Isotopes { get; private set; }
        public ObservableCollection<PeriodicTableElementModel> FirstLeftElements { get; private set; }
        public ObservableCollection<PeriodicTableElementModel> FirstRightElements { get; private set; }
        public ObservableCollection<PeriodicTableElementModel> SecondLeftElements { get; private set; }
        public ObservableCollection<PeriodicTableElementModel> SecondRightElements { get; private set; }
        public ObservableCollection<PeriodicTableElementModel> ThirdLeftElements { get; private set; }
        public ObservableCollection<PeriodicTableElementModel> ThirdRightElements { get; private set; }
        public ObservableCollection<PeriodicTableElementModel> FourthElements { get; private set; }
        public ObservableCollection<PeriodicTableElementModel> FifthElements { get; private set; }
        public ObservableCollection<PeriodicTableElementModel> SixthLeftElements { get; private set; }
        public ObservableCollection<PeriodicTableElementModel> SixthHiddenElements { get; private set; }
        public ObservableCollection<PeriodicTableElementModel> SixthRightElements { get; private set; }
        public ObservableCollection<PeriodicTableElementModel> SeventhLeftElements { get; private set; }
        public ObservableCollection<PeriodicTableElementModel> SeventhHiddenElements { get; private set; }
        public ObservableCollection<PeriodicTableElementModel> SeventhRightElements { get; private set; }

        public ElementViewModel(MainViewModel mainViewModel) {
            this.mainViewModel = mainViewModel;

            ElementDataDictionary = new Dictionary<string, ElementDataModel>();
            IsotopeDataDictionary = new Dictionary<string, IsotopeDataModel>();

            FirstLeftElements = new ObservableCollection<PeriodicTableElementModel>();
            FirstRightElements = new ObservableCollection<PeriodicTableElementModel>();
            SecondLeftElements = new ObservableCollection<PeriodicTableElementModel>();
            SecondRightElements = new ObservableCollection<PeriodicTableElementModel>();
            ThirdLeftElements = new ObservableCollection<PeriodicTableElementModel>();
            ThirdRightElements = new ObservableCollection<PeriodicTableElementModel>();
            FourthElements = new ObservableCollection<PeriodicTableElementModel>();
            FifthElements = new ObservableCollection<PeriodicTableElementModel>();
            SixthLeftElements = new ObservableCollection<PeriodicTableElementModel>();
            SixthHiddenElements = new ObservableCollection<PeriodicTableElementModel>();
            SixthRightElements = new ObservableCollection<PeriodicTableElementModel>();
            SeventhLeftElements = new ObservableCollection<PeriodicTableElementModel>();
            SeventhHiddenElements = new ObservableCollection<PeriodicTableElementModel>();
            SeventhRightElements = new ObservableCollection<PeriodicTableElementModel>();

            InitializeElements();
        }

        public static IsotopeModel GetIsotope(string symbol, int massNumber) {
            IsotopeDataDictionary.TryGetValue(symbol, out var isotopeData);
            return GetIsotope(isotopeData, massNumber);
        }
        public static IsotopeModel GetIsotope(IsotopeDataModel isotopeData, int massNumber) {
            if (isotopeData == null)
                return new IsotopeModel(isotopeData.Symbol, massNumber, "?", new DecayModel[0]);

            var isotopeEnumerable = isotopeData.Isotopes.Where(i => i.MassNumber == massNumber);
            if (isotopeEnumerable.Count() == 0)
                return new IsotopeModel(isotopeData.Symbol, massNumber, "", new DecayModel[0]);

            return isotopeEnumerable.First();
        }

        public static double GetHalfLife(IsotopeModel isotope, double unknownValue = -1) {
            string halfLifeString = isotope.HalfLife.Replace(",", "").Replace(".", ",");
            if (halfLifeString == string.Empty)
                return 0;
            else if (halfLifeString == "?")
                return unknownValue;

            if (halfLifeString.Contains("e")) {
                double.TryParse(halfLifeString.Substring(0, halfLifeString.IndexOf("e")), out double firstNumber);
                int.TryParse(halfLifeString.Substring(halfLifeString.IndexOf("e") + 1), out int power);
                return firstNumber * Math.Pow(10, power);
            }

            return double.Parse(halfLifeString);
        }

        public static double GetAvarageHalfLife(IsotopeDataModel isotopeData) {
            double avarageHalfLife = 0;
            foreach (var isotope in isotopeData.Isotopes) {
                avarageHalfLife += GetHalfLife(isotope, 0);
            }

            avarageHalfLife /= isotopeData.Isotopes.Length;
            return avarageHalfLife;
        }

        public static double GetMassInAMU(ElementDataModel element) =>
                (element.AtomicNumber * Constants.Proton.Mass.AtomicMassUnits) +
                ((element.MassNumber - element.AtomicNumber) * Constants.Neutron.Mass.AtomicMassUnits);

        private void InitializeElements() {
            (Elements, Isotopes) = LoadElementData();
            FillDictionaries();
            InitializePeriodicTableElements();
            ElementsLoaded = true;
        }

        private void InitializePeriodicTableElements() {
            for (int i = 0; i < 1; i++)
                AddPeriodicTableElement(Elements[i], FirstLeftElements, i);

            for (int i = 1; i < 2; i++)
                AddPeriodicTableElement(Elements[i], FirstRightElements, i);

            for (int i = 2; i < 4; i++)
                AddPeriodicTableElement(Elements[i], SecondLeftElements, i);

            for (int i = 4; i < 10; i++)
                AddPeriodicTableElement(Elements[i], SecondRightElements, i);

            for (int i = 10; i < 12; i++)
                AddPeriodicTableElement(Elements[i], ThirdLeftElements, i);

            for (int i = 12; i < 18; i++)
                AddPeriodicTableElement(Elements[i], ThirdRightElements, i);

            for (int i = 18; i < 36; i++)
                AddPeriodicTableElement(Elements[i], FourthElements, i);

            for (int i = 36; i < 54; i++)
                AddPeriodicTableElement(Elements[i], FifthElements, i);

            for (int i = 54; i < 56; i++)
                AddPeriodicTableElement(Elements[i], SixthLeftElements, i);

            for (int i = 56; i < 71; i++)
                AddPeriodicTableElement(Elements[i], SixthHiddenElements, i);

            for (int i = 71; i < 86; i++)
                AddPeriodicTableElement(Elements[i], SixthRightElements, i);

            for (int i = 86; i < 88; i++)
                AddPeriodicTableElement(Elements[i], SeventhLeftElements, i);

            for (int i = 88; i < 103; i++)
                AddPeriodicTableElement(Elements[i], SeventhHiddenElements, i);

            for (int i = 103; i < 118; i++)
                AddPeriodicTableElement(Elements[i], SeventhRightElements, i);
        }

        private void AddPeriodicTableElement(ElementDataModel element, ObservableCollection<PeriodicTableElementModel> to, int index) {
            double avarageEnergyReleased = mainViewModel.DecayChainViewModel.SetupDecayChains(element);
            double normalizedAER = avarageEnergyReleased / 6.9246087974211306E-09;
            var aerColor = System.Windows.Media.Color.FromArgb(255, 255, (byte)(255 - (normalizedAER/* hm */ * 255)), (byte)(255 - (normalizedAER/* hm */ * 255)));
            var aerSolidColorBrush = new System.Windows.Media.SolidColorBrush(aerColor);
            IsotopeDataDictionary.TryGetValue(element.Symbol, out var isotopeData);
            double halfLife = GetAvarageHalfLife(isotopeData);
            double normalizedHalfLife = halfLife / 6.311390375E+28;
            var instabilityColor = System.Windows.Media.Color.FromArgb(255, 255, (byte)(0 + (normalizedHalfLife * 255)), (byte)(0 + (normalizedHalfLife * 255)));
            var instabilitySolidColorBrush = new System.Windows.Media.SolidColorBrush(instabilityColor);
            to.Add(new PeriodicTableElementModel(element, aerSolidColorBrush, instabilitySolidColorBrush, PeriodicTableMasses[index]));
        }

        private ValueTuple<ObservableCollection<ElementDataModel>, ObservableCollection<IsotopeDataModel>> LoadElementData() =>
                (TryDeserializeJson<ElementDataModel>(Properties.Resources.Elements),
                TryDeserializeJson<IsotopeDataModel>(Properties.Resources.Isotopes));

        /// <summary>
        /// Deserializes JSON formatted text according to the structure of a specified model.
        /// </summary>
        private ObservableCollection<T> TryDeserializeJson<T>(string json) {
            try {
                return DeserializeJson<T>(json);
            }
            catch (Exception e) {
                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "error_log.txt", true)) {
                    sw.Write($"{e.Message}\n\n{e.StackTrace}");
                }

                Environment.Exit(1);
            }

            return new ObservableCollection<T>();
        }

        private ObservableCollection<T> DeserializeJson<T>(string json) {
            var collection = new ObservableCollection<T>();
            T[] models = JsonConvert.DeserializeObject<T[]>(json);
            foreach (var model in models) {
                collection.Add(model);
            }

            return collection;
        }

        private void FillDictionaries() {
            foreach (var element in Elements)
                ElementDataDictionary.Add(element.Symbol, element);

            foreach (var isotope in Isotopes) {
                IsotopeDataDictionary.Add(isotope.Symbol, isotope);
            }
        }
    }
}
