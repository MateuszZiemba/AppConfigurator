using AppConfigurator.Models;
using AppConfigurator.Repositories.Contract;
using AppConfigurator.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigurator.Repositories //todo decide if should be implemented
{
    //public class UserSettingsRepository : ISettingsRepository
    //{
    //    public List<ColorPicker> GetAppearanceSettings()
    //    {
    //        List<ColorPicker> colorPickers = new List<ColorPicker>();

    //        colorPickers.Add(new ColorPicker("LabelName", nameof(Properties.Settings.Default.DefaultSetting), Properties.Settings.Default.DefaultSetting));

    //        return colorPickers;
    //    }

    //    public List<AppSetting> GetApplicationSettings()
    //    {
    //        List<AppSetting> applicationSettings = new List<AppSetting>();

    //        applicationSettings.Add(new AppSetting("LabelName", nameof(Properties.Settings.Default.DefaultSetting), Properties.Settings.Default.DefaultSetting));

    //        return applicationSettings;
    //    }


    //    public bool SaveSettings(ConfigurationViewModel viewModel)
    //    {
    //        // works for user settings saved in appdata 
    //        //ApplyColorChangesToConfig(viewModel);
    //        //ApplyApplicationSettingsToConfig(viewModel);
    //        //ApplyConnectionSettingsToConfig(viewModel);

    //        //try 
    //        //{ 
    //        //    Properties.Settings.Default.Save();
    //        //    return true;
    //        //}
    //        //catch (Exception ex)
    //        //{
    //        //    return false;
    //        //}
    //        return false;
    //    }

    //    private void ApplyColorChangesToConfig(ConfigurationViewModel viewModel)
    //    {
    //        foreach (var modifiedColors in viewModel.Colors)
    //        {
    //            Properties.Settings.Default[modifiedColors.ConfigurationName] = modifiedColors.SelectedColor.ToString();
    //        }
    //    }

    //    private void ApplyApplicationSettingsToConfig(ConfigurationViewModel viewModel)
    //    {
    //        foreach (var modifiedSettings in viewModel.Settings)
    //        {
    //            Properties.Settings.Default[modifiedSettings.ConfigurationName] = modifiedSettings.Value;
    //        }
    //    }
    //}
}
