using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigurator.Helpers
{
    public static class LabelHelper
    {
        public static string GetLabelFromSettingName(string settingName) //todo bug - check if number, not a digit
        {
            return string.Concat(settingName.Select(x => Char.IsUpper(x) || Char.IsDigit(x) ? " " + x : x.ToString())).TrimStart(' ');
        }
    }
}
