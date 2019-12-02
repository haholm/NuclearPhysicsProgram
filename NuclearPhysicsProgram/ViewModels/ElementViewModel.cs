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

        public ObservableCollection<ElementDataModel> Elements { get; private set; }
        public ObservableCollection<IsotopeDataModel> Isotopes { get; private set; }
        public static Dictionary<string, ElementDataModel> ElementDataDictionary { get; private set; }
        public static Dictionary<string, IsotopeDataModel> IsotopeDataDictionary { get; private set; }

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
        public ObservableCollection<PeriodicTableElementModel > SeventhRightElements { get; private set; }

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
            ///
            if (isotopeData == null)
                return new IsotopeModel(symbol, massNumber, "?", new DecayModel[0]);
            ///
            IEnumerable<IsotopeModel> isotopeList = isotopeData.Isotopes.Where(i => i.MassNumber == massNumber);
            if (isotopeList.Count() == 0)
                return new IsotopeModel(symbol, massNumber, "", new DecayModel[0]);

            return isotopeList.First();
        }
        public static IsotopeModel GetIsotope(IsotopeDataModel isotopeData, int massNumber) {
            ///
            if (isotopeData == null)
                return new IsotopeModel(isotopeData.Symbol, massNumber, "?", new DecayModel[0]);
            ///
            IEnumerable<IsotopeModel> isotopeList = isotopeData.Isotopes.Where(i => i.MassNumber == massNumber);
            if (isotopeList.Count() == 0)
                return new IsotopeModel(isotopeData.Symbol, massNumber, "", new DecayModel[0]);

            return isotopeList.First();
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
        }

        private void InitializePeriodicTableElements() {
            for (int i = 0; i < 1; i++)
                AddPeriodicTableElement(Elements[i], FirstLeftElements);

            for (int i = 1; i < 2; i++)
                AddPeriodicTableElement(Elements[i], FirstRightElements);

            for (int i = 2; i < 4; i++)
                AddPeriodicTableElement(Elements[i], SecondLeftElements);

            for (int i = 4; i < 10; i++)
                AddPeriodicTableElement(Elements[i], SecondRightElements);

            for (int i = 10; i < 12; i++)
                AddPeriodicTableElement(Elements[i], ThirdLeftElements);

            for (int i = 12; i < 18; i++)
                AddPeriodicTableElement(Elements[i], ThirdRightElements);

            for (int i = 18; i < 36; i++)
                AddPeriodicTableElement(Elements[i], FourthElements);

            for (int i = 36; i < 54; i++)
                AddPeriodicTableElement(Elements[i], FifthElements);

            for (int i = 54; i < 56; i++)
                AddPeriodicTableElement(Elements[i], SixthLeftElements);

            for (int i = 56; i < 71; i++)
                AddPeriodicTableElement(Elements[i], SixthHiddenElements);

            for (int i = 71; i < 86; i++)
                AddPeriodicTableElement(Elements[i], SixthRightElements);

            for (int i = 86; i < 88; i++)
                AddPeriodicTableElement(Elements[i], SeventhLeftElements);

            for (int i = 88; i < 103; i++)
                AddPeriodicTableElement(Elements[i], SeventhHiddenElements);

            for (int i = 103; i < 118; i++)
                AddPeriodicTableElement(Elements[i], SeventhRightElements);
        }

        private void AddPeriodicTableElement(ElementDataModel element, ObservableCollection<PeriodicTableElementModel> to) {
            double avarageEnergyReleased = mainViewModel.DecayChainViewModel.SetupDecayChains(element);
            double normalizedAER = avarageEnergyReleased / 6.9246087974211306E-09;
            var aerColor = System.Windows.Media.Color.FromArgb(255, 255, (byte)(255 - (normalizedAER/* hm */ * 255)), (byte)(255 - (normalizedAER/* hm */ * 255)));
            var aerSolidColorBrush = new System.Windows.Media.SolidColorBrush(aerColor);
            IsotopeDataDictionary.TryGetValue(element.Symbol, out var isotopeData);
            double halfLife = GetAvarageHalfLife(isotopeData);
            double normalizedHalfLife = halfLife / 6.311390375E+28;
            var instabilityColor = System.Windows.Media.Color.FromArgb(255, 255, (byte)(0 + (normalizedHalfLife * 255)), (byte)(0 + (normalizedHalfLife * 255)));
            var instabilitySolidColorBrush = new System.Windows.Media.SolidColorBrush(instabilityColor);
            to.Add(new PeriodicTableElementModel(element, aerSolidColorBrush, instabilitySolidColorBrush));
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
                    sw.Write($"\n\n{e.Message}\n{e.StackTrace}");
                }

                Environment.Exit(1);
            }

            return new ObservableCollection<T>();
        }

        private ObservableCollection<T> DeserializeJson<T>(string json) {
            ObservableCollection<T> collection = new ObservableCollection<T>();

            T[] models = JsonConvert.DeserializeObject<T[]>(json);
            foreach (var model in models) {
                collection.Add(model);
            }

            return collection;
        }

        private void FillDictionaries() {
            foreach (var element in Elements)
                ElementDataDictionary.Add(element.Symbol, element);

            foreach (var isotope in Isotopes)
                IsotopeDataDictionary.Add(isotope.Symbol, isotope);
        }
    }
}
