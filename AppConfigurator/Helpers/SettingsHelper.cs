using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AppConfigurator.Helpers
{
    public static class SettingsHelper //I think that this method is a bit of a code smell, but it is the easiest way - KISS
    {
        public static string GetLabelFromSettingName(string settingName)
        {
            var camelCaseLabel = Regex.Replace(settingName, @"\d( *\d*)*\d", (MatchEvaluator)(match => { return match.Value.Replace(" ", ""); }));
            return Regex.Replace(Regex.Replace(settingName, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

        public static bool IsSettingColor(string settingValue) 
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
        public static bool IsSettingDateTime(string settingValue)
        {
            try
            {
                DateTime.Parse(settingValue);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool IsSettingNumeric(string settingValue)
        {
            double result;
            if (Double.TryParse(settingValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
                return true;
            else 
                return false;
        }
    }
}
