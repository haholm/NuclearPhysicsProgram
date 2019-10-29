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
        public string Symbol { // DO I REALLY NEED THIS?
            get => GetValue(SymbolProperty) as string;
            set => SetValue(SymbolProperty, value);
        }
        public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register(
            "Symbol", typeof(string), typeof(ItemView), new PropertyMetadata("A"));

        public int? AtomicNumber { // DO I REALLY NEED THIS?
            get => GetValue(AtomicNumberProperty) as int?;
            set => SetValue(AtomicNumberProperty, value);
        }
        public static readonly DependencyProperty AtomicNumberProperty = DependencyProperty.Register(
            "AtomicNumber", typeof(int?), typeof(ItemView), new PropertyMetadata(0));
        #endregion

        public ItemView() {
            InitializeComponent();
        }
    }
}
