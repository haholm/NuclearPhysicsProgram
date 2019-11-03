using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        public ObservableCollection<(ElementDataModel element, double avarageEnergyReleased)> FirstLeftElements { get; private set; }
        public ObservableCollection<(ElementDataModel element, double avarageEnergyReleased)> FirstRightElements { get; private set; }
        public ObservableCollection<(ElementDataModel element, double avarageEnergyReleased)> SecondLeftElements { get; private set; }
        public ObservableCollection<(ElementDataModel element, double avarageEnergyReleased)> SecondRightElements { get; private set; }
        public ObservableCollection<(ElementDataModel element, double avarageEnergyReleased)> ThirdLeftElements { get; private set; }
        public ObservableCollection<(ElementDataModel element, double avarageEnergyReleased)> ThirdRightElements { get; private set; }
        public ObservableCollection<(ElementDataModel element, double avarageEnergyReleased)> FourthElements { get; private set; }
        public ObservableCollection<(ElementDataModel element, double avarageEnergyReleased)> FifthElements { get; private set; }
        public ObservableCollection<(ElementDataModel element, double avarageEnergyReleased)> SixthLeftElements { get; private set; }
        public ObservableCollection<(ElementDataModel element, double avarageEnergyReleased)> SixthHiddenElements { get; private set; }
        public ObservableCollection<(ElementDataModel element, double avarageEnergyReleased)> SixthRightElements { get; private set; }
        public ObservableCollection<(ElementDataModel element, double avarageEnergyReleased)> SeventhLeftElements { get; private set; }
        public ObservableCollection<(ElementDataModel element, double avarageEnergyReleased)> SeventhHiddenElements { get; private set; }
        public ObservableCollection<(ElementDataModel element, double avarageEnergyReleased)> SeventhRightElements { get; private set; }

        public ElementViewModel(MainViewModel mainViewModel) {
            this.mainViewModel = mainViewModel;

            ElementDataDictionary = new Dictionary<string, ElementDataModel>();
            IsotopeDataDictionary = new Dictionary<string, IsotopeDataModel>();

            FirstLeftElements = new ObservableCollection<(ElementDataModel, double)>();
            FirstRightElements = new ObservableCollection<(ElementDataModel, double)>();
            SecondLeftElements = new ObservableCollection<(ElementDataModel, double)>();
            SecondRightElements = new ObservableCollection<(ElementDataModel, double)>();
            ThirdLeftElements = new ObservableCollection<(ElementDataModel, double)>();
            ThirdRightElements = new ObservableCollection<(ElementDataModel, double)>();
            FourthElements = new ObservableCollection<(ElementDataModel, double)>();
            FifthElements = new ObservableCollection<(ElementDataModel, double)>();
            SixthLeftElements = new ObservableCollection<(ElementDataModel, double)>();
            SixthHiddenElements = new ObservableCollection<(ElementDataModel, double)>();
            SixthRightElements = new ObservableCollection<(ElementDataModel, double)>();
            SeventhLeftElements = new ObservableCollection<(ElementDataModel, double)>();
            SeventhHiddenElements = new ObservableCollection<(ElementDataModel, double)>();
            SeventhRightElements = new ObservableCollection<(ElementDataModel, double)>();

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

        private void AddPeriodicTableElement(ElementDataModel element, ObservableCollection<(ElementDataModel, double)> to) {
            double avarageEnergyReleased = mainViewModel.DecayChainViewModel.SetupDecayChains(element);
            to.Add((element, avarageEnergyReleased));
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
