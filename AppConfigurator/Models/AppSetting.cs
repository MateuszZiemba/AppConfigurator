﻿using AppConfigurator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigurator.Models
{
    public class AppSetting 
    {
        public string ConfigurationName { get; set; }
        public string LabelName { get; set; }
        public string Value { get; set; }
        public string OriginalValue { get; private set; }
        public SettingEditorType SettingType { get; set; }

        public AppSetting(string labelName, string configurationName, string value, SettingEditorType settingType = SettingEditorType.String)
        {
            ConfigurationName = configurationName;
            LabelName = String.Concat(labelName, ": ");
            Value = value;
            OriginalValue = value;
            SettingType = settingType;
        }
    }
}