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
        public ObservableCollection<ElementDataModel> Elements { get; private set; }
        public ObservableCollection<IsotopeDataModel> Isotopes { get; private set; }
        public static Dictionary<string, ElementDataModel> ElementDataDictionary { get; private set; }
        public static Dictionary<string, IsotopeDataModel> IsotopeDataDictionary { get; private set; }

        public ObservableCollection<ElementDataModel> FirstLeftElements { get; private set; }
        public ObservableCollection<ElementDataModel> FirstRightElements { get; private set; }
        public ObservableCollection<ElementDataModel> SecondLeftElements { get; private set; }
        public ObservableCollection<ElementDataModel> SecondRightElements { get; private set; }
        public ObservableCollection<ElementDataModel> ThirdLeftElements { get; private set; }
        public ObservableCollection<ElementDataModel> ThirdRightElements { get; private set; }
        public ObservableCollection<ElementDataModel> FourthElements { get; private set; }
        public ObservableCollection<ElementDataModel> FifthElements { get; private set; }
        public ObservableCollection<ElementDataModel> SixthLeftElements { get; private set; }
        public ObservableCollection<ElementDataModel> SixthHiddenElements { get; private set; }
        public ObservableCollection<ElementDataModel> SixthRightElements { get; private set; }
        public ObservableCollection<ElementDataModel> SeventhLeftElements { get; private set; }
        public ObservableCollection<ElementDataModel> SeventhHiddenElements { get; private set; }
        public ObservableCollection<ElementDataModel> SeventhRightElements { get; private set; }

        public ElementViewModel() {
            ElementDataDictionary = new Dictionary<string, ElementDataModel>();
            IsotopeDataDictionary = new Dictionary<string, IsotopeDataModel>();

            FirstLeftElements = new ObservableCollection<ElementDataModel>();
            FirstRightElements = new ObservableCollection<ElementDataModel>();
            SecondLeftElements = new ObservableCollection<ElementDataModel>();
            SecondRightElements = new ObservableCollection<ElementDataModel>();
            ThirdLeftElements = new ObservableCollection<ElementDataModel>();
            ThirdRightElements = new ObservableCollection<ElementDataModel>();
            FourthElements = new ObservableCollection<ElementDataModel>();
            FifthElements = new ObservableCollection<ElementDataModel>();
            SixthLeftElements = new ObservableCollection<ElementDataModel>();
            SixthHiddenElements = new ObservableCollection<ElementDataModel>();
            SixthRightElements = new ObservableCollection<ElementDataModel>();
            SeventhLeftElements = new ObservableCollection<ElementDataModel>();
            SeventhHiddenElements = new ObservableCollection<ElementDataModel>();
            SeventhRightElements = new ObservableCollection<ElementDataModel>();

            InitializeElements();
        }

        public static IsotopeModel GetIsotope(string symbol, int massNumber) {
            IsotopeDataDictionary.TryGetValue(symbol, out var isotopeData);
            IEnumerable<IsotopeModel> isotopeList = isotopeData.Isotopes.Where(i => i.MassNumber == massNumber);
            if (isotopeList.Count() == 0)
                return new IsotopeModel(symbol, massNumber, "", Array.Empty<DecayModel>());

            return isotopeList.First();
        }

        private void InitializeElements() {
            (Elements, Isotopes) = LoadElementData();
            FillDictionaries();
            
            for (int i = 0; i < 1; i++) 
                FirstLeftElements.Add(Elements[i]);

            for (int i = 1; i < 2; i++)
                FirstRightElements.Add(Elements[i]);

            for (int i = 2; i < 4; i++) 
                SecondLeftElements.Add(Elements[i]);

            for (int i = 4; i < 10; i++)
                SecondRightElements.Add(Elements[i]);

            for (int i = 10; i < 12; i++) 
                ThirdLeftElements.Add(Elements[i]);

            for (int i = 12; i < 18; i++)
                ThirdRightElements.Add(Elements[i]);

            for (int i = 18; i < 36; i++) 
                FourthElements.Add(Elements[i]);

            for (int i = 36; i < 54; i++)
                FifthElements.Add(Elements[i]);

            for (int i = 54; i < 56; i++) 
                SixthLeftElements.Add(Elements[i]);

            for (int i = 56; i < 71; i++) 
                SixthHiddenElements.Add(Elements[i]);

            for (int i = 71; i < 86; i++) 
                SixthRightElements.Add(Elements[i]);

            for (int i = 86; i < 88; i++) 
                SeventhLeftElements.Add(Elements[i]);

            for (int i = 88; i < 103; i++) 
                SeventhHiddenElements.Add(Elements[i]);

            for (int i = 103; i < 118; i++) 
                SeventhRightElements.Add(Elements[i]);
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
