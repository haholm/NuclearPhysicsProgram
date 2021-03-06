﻿using System;
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

        public SolidColorBrush InstabilityColor {
            get => GetValue(InstabilityColorProperty) as SolidColorBrush;
            set => SetValue(InstabilityColorProperty, value);
        }
        public static readonly DependencyProperty InstabilityColorProperty = DependencyProperty.Register(
            "InstabilityColor", typeof(SolidColorBrush), typeof(ItemView), new PropertyMetadata(new SolidColorBrush()));

        public SolidColorBrush AERColor {
            get => GetValue(AERColorProperty) as SolidColorBrush;
            set => SetValue(AERColorProperty, value);
        }
        public static readonly DependencyProperty AERColorProperty = DependencyProperty.Register(
            "AERColor", typeof(SolidColorBrush), typeof(ItemView), new PropertyMetadata(new SolidColorBrush()));

        public Visibility? PropertiesColorVisibility {
            get => GetValue(PropertiesColorVisibilityProperty) as Visibility?;
            set => SetValue(PropertiesColorVisibilityProperty, value);
        }
        public static readonly DependencyProperty PropertiesColorVisibilityProperty = DependencyProperty.Register(
            "PropertiesColorVisibility", typeof(Visibility), typeof(ItemView), new PropertyMetadata(Visibility.Visible));

        public string EName {
            get => GetValue(ENameProperty) as string;
            set => SetValue(ENameProperty, value);
        }
        public static readonly DependencyProperty ENameProperty = DependencyProperty.Register(
            "EName", typeof(string), typeof(ItemView), new PropertyMetadata(""));

        public double? Mass {
            get => GetValue(MassProperty) as double?;
            set => SetValue(MassProperty, value);
        }
        public static readonly DependencyProperty MassProperty = DependencyProperty.Register(
            "Mass", typeof(double), typeof(ItemView), new PropertyMetadata(0.0));
        #endregion
        private bool MassExists { get => Mass.GetValueOrDefault() != 0.0; }

        public Thickness AtomicNumberPadding { get => MassExists ? new Thickness(1, 3, 1, 3) : new Thickness(1, 10, 1, 10); }
        public Thickness SymbolPadding { get => MassExists ? new Thickness(1, 0, 0, 16) : new Thickness(1, 0, 0, 0); }

        public Thickness MassMargin { get => MassExists ? new Thickness(0) : new Thickness(0, -12, 0, -12); }

        public ItemView() {
            InitializeComponent();
        }
    }
}
