using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NuclearPhysicsProgram.ViewModels.Converters {
    class PlotSeriesConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null)
                return "";

            var hitResult = value as OxyPlot.TrackerHitResult;
            if (PeriodicTablePlotViewModel.CurrentPlotType == "instability")
                return ConvertInstabilityPlot(hitResult);
            else
                return ConvertBindingEnergyPlot(hitResult);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;

        public object ConvertInstabilityPlot(OxyPlot.TrackerHitResult value) {
            string strippedResult = value.Text.Replace($"\n{PeriodicTablePlotViewModel.BottomInstabilityTitle}: ", "");
            double neutronAmount = FindFirstNumber(strippedResult);
            string secondStrippedResult = strippedResult.Replace($"{neutronAmount}\n{PeriodicTablePlotViewModel.LeftInstabilityTitle}: ", "");
            double protonAmount = FindFirstNumber(secondStrippedResult);
            var scatterPoint = PeriodicTablePlotViewModel.OpenScatterPoints.Find(scatterPoint => scatterPoint.massNumber - scatterPoint.atomicNumber == neutronAmount && scatterPoint.atomicNumber == protonAmount);
            var isotopeModel = ElementViewModel.GetIsotope(scatterPoint.symbol, scatterPoint.massNumber);
            string decayTypeText = "";
            if (isotopeModel.Decays.Length > 0)
                decayTypeText = $"\nDecay type: {ElementInfoViewModels.DecayChainViewModel.GetDecaySymbol(isotopeModel.Decays[0].Type)}";
            return $"{ConvertToSuperScript(scatterPoint.massNumber.ToString())} {scatterPoint.elementName}\nHalf life (s): {scatterPoint.halfLife}{decayTypeText}";
        }

        public object ConvertBindingEnergyPlot(OxyPlot.TrackerHitResult value) {
            string strippedResult = value.Text.Replace($"\n{PeriodicTablePlotViewModel.BottomBindingEnergyTitle}: ", "");
            double nucleonAmount = FindFirstNumber(strippedResult);

            var dataPoint = PeriodicTablePlotViewModel.OpenDataPoints.Find(dataPoint => dataPoint.massNumber == nucleonAmount);
            return $"{ConvertToSuperScript(dataPoint.massNumber.ToString())} {dataPoint.elementName}";
        }

        private double FindFirstNumber(string str) {
            string numberString = "";
            foreach (char c in str) {
                if (int.TryParse(c.ToString(), out int digit))
                    numberString += digit;
                else if (c == ',')
                    numberString += c;
                else
                    break;
            }

            if (!int.TryParse(numberString, out int number))
                return 0;

            return number;
        }

        private string ConvertToSuperScript(string toConvert) {
            string superScript = "";
            foreach (var c in toConvert) {
                superScript += GetSuperScriptChar(c);
            }

            return superScript;
        }

        private char GetSuperScriptChar(char c) {
            switch (c) {
                case '0':
                    return '\x2070';
                case '1':
                    return '\xB9';
                case '2':
                    return '\xB2';
                case '3':
                    return '\xB3';
                case '4':
                    return '\x2074';
                case '5':
                    return '\x2075';
                case '6':
                    return '\x2076';
                case '7':
                    return '\x2077';
                case '8':
                    return '\x2078';
                case '9':
                    return '\x2079';
                default:
                    return '\x2070';
            }
        }
    }
}
