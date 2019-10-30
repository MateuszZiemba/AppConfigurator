﻿using AppConfigurator.Enums;
using AppConfigurator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AppConfigurator.ItemTemplateSelectors
{
    public class BooleanSettingDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            AppSetting AppSetting = item as AppSetting;
            
            if (AppSetting.SettingType.Equals(SettingType.Boolean))
                return element.FindResource("BooleanSettingTemplate") as DataTemplate;
            else
                return element.FindResource("StringSettingTemplate") as DataTemplate;
        }
    }
}