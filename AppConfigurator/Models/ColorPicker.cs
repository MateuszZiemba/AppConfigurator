using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AppConfigurator.Models
{
    public class ColorPicker
    {
        public string ConfigurationName { get; set; }
        public string LabelName { get; set; }
        public Color SelectedColor { get; set; }
        public ColorPicker(string labelName, string configurationName, string hexColorCode)
        {
            ConfigurationName = configurationName;
            LabelName = String.Concat(labelName, ": ");
            try
            {
                SelectedColor = (Color)ColorConverter.ConvertFromString(hexColorCode);
            }
            catch (Exception)
            {
                SelectedColor = Color.FromRgb(255, 255, 255);
            }
        }
    }
}