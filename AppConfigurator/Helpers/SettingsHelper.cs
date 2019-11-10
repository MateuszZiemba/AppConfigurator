using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AppConfigurator.Helpers
{
    public static class SettingsHelper
    {
        public static string GetLabelFromSettingName(string settingName) //todo bug - check if number, not a digit
        {
            return string.Concat(settingName.Select(x => Char.IsUpper(x) || Char.IsDigit(x) ? " " + x : x.ToString())).TrimStart(' ');
        }

        public static bool IsSettingColor(string settingValue) //bit of a code smell, but it is the easiest way - KISS ;) 
        {
            try
            {
                ColorConverter.ConvertFromString(settingValue);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
