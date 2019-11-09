using AppConfigurator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace AppConfigurator.UserControls
{
    /// <summary>
    /// Interaction logic for AppearanceUserControl.xaml
    /// </summary>
    public partial class AppearanceUserControl : UserControl
    {
        public ObservableCollection<ColorPicker> ColorPickers
        {
            get { return (ObservableCollection<ColorPicker>)GetValue(ColorPickersProperty); }
            set { SetValue(ColorPickersProperty, value); }
        }

        public static readonly DependencyProperty ColorPickersProperty =
            DependencyProperty.Register("ColorPickers", typeof(ObservableCollection<ColorPicker>), typeof(AppearanceUserControl));


        public AppearanceUserControl()
        {
            InitializeComponent();
        }
    }
}
