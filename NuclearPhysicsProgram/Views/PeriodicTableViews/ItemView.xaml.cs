using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NuclearPhysicsProgram.Views.PeriodicTableViews {
    /// <summary>
    /// Interaction logic for ItemView.xaml
    /// </summary>
    public partial class ItemView : UserControl {
        #region DependencyProperties
        public string Symbol {
            get => GetValue(SymbolProperty) as string;
            set => SetValue(SymbolProperty, value);
        }
        public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register(
            "Symbol", typeof(string), typeof(ItemView), new PropertyMetadata("A"));

        public int? AtomicNumber {
            get => GetValue(AtomicNumberProperty) as int?;
            set => SetValue(AtomicNumberProperty, value);
        }
        public static readonly DependencyProperty AtomicNumberProperty = DependencyProperty.Register(
            "AtomicNumber", typeof(int?), typeof(ItemView), new PropertyMetadata(0));

        public double? SymbolFontSize {
            get => GetValue(SymbolFontSizeProperty) as double?;
            set => SetValue(SymbolFontSizeProperty, value);
        }
        public static readonly DependencyProperty SymbolFontSizeProperty = DependencyProperty.Register(
            "SymbolFontSize", typeof(double?), typeof(ItemView), new PropertyMetadata(0.0));

        public SolidColorBrush PropertiesColor {
            get => GetValue(PropertiesColorProperty) as SolidColorBrush;
            set => SetValue(PropertiesColorProperty, value);
        }
        public static readonly DependencyProperty PropertiesColorProperty = DependencyProperty.Register(
            "PropertiesColor", typeof(SolidColorBrush), typeof(ItemView), new PropertyMetadata(new SolidColorBrush()));

        public Visibility? PropertiesColorVisibility {
            get => GetValue(PropertiesColorVisibilityProperty) as Visibility?;
            set => SetValue(PropertiesColorVisibilityProperty, value);
        }
        public static readonly DependencyProperty PropertiesColorVisibilityProperty = DependencyProperty.Register(
            "PropertiesColorVisibility", typeof(Visibility), typeof(ItemView), new PropertyMetadata(Visibility.Visible));

        public string EName { // DO I REALLY NEED THIS?
            get => GetValue(ENameProperty) as string;
            set => SetValue(ENameProperty, value);
        }
        public static readonly DependencyProperty ENameProperty = DependencyProperty.Register(
            "EName", typeof(string), typeof(ItemView), new PropertyMetadata(""));
        #endregion

        public ItemView() {
            InitializeComponent();
        }
    }
}
