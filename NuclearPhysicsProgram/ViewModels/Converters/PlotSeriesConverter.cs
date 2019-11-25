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
            string strippedResult = (value as OxyPlot.TrackerHitResult).Text.Replace($"\n{PeriodicTablePlotViewModel.BottomTitle}: ", "");
            string nucleonAmountString = "";
            foreach (char c in strippedResult) {
                if (int.TryParse(c.ToString(), out int digit))
                    nucleonAmountString += digit;
                else if (c == ',')
                    nucleonAmountString += c;
                else
                    break;
            }

            if (!int.TryParse(nucleonAmountString, out int nucleonAmount))
                return "-";

            var dataPoint = PeriodicTablePlotViewModel.OpenDataPoints.Find(dataPoint => dataPoint.nucleons == nucleonAmount);
            return $"{ConvertToSuperScript(dataPoint.atomicNumber.ToString())} {dataPoint.elementName}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;

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
