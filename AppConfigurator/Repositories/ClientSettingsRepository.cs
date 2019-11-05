using AppConfigurator.Enums;
using AppConfigurator.Models;
using AppConfigurator.Repositories.Contract;
using AppConfigurator.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigurator.Repositories
{
    public class ClientSettingsRepository : ISettingsRepository
    {
        private Configuration config;
        private ClientSettingsSection userSettingsSection;
        private string connectionStringName;

        public ClientSettingsRepository(string configFilePath)
        {
            string configName = String.Concat("\\", ""); //todo add modal with path selection and checkbox to save selection for next time 
            string userSettingsPath = "userSettings/projectName";
            connectionStringName = "connstringName"; //todo to implement

            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = String.Concat(Environment.CurrentDirectory, configName);
            config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            userSettingsSection = (ClientSettingsSection)config.GetSection(userSettingsPath);
        }
        public List<ColorPicker> GetAppearanceSettings()
        {
            List<ColorPicker> colorPickers = new List<ColorPicker>();

            colorPickers.Add(GetColorFromConfig("ExampleOfProperty", "Example of label"));

            return colorPickers;
        }

        public List<AppSetting> GetApplicationSettings()
        {
            List<AppSetting> AppSettings = new List<AppSetting>();

            AppSettings.Add(GetAppSettingFromConfig("ExampleOfProperty", "Example of label"));

            return AppSettings;
        }
        public bool SaveSettings(ConfigurationViewModel viewModel)
        {
            try
            {
                UpdateAppearanceUserSettings(viewModel.Colors);
                UpdateAppSettingUserSettings(viewModel.Settings);
                //UpdateConnectionSettingUserSettings(viewModel.ConnectionSettings); //todo implement
                //UpdateConnectionString(viewModel.ConnectionSettings.Where(x=>x.ConfigurationName.Equals(connectionStringName)).FirstOrDefault());
                userSettingsSection.SectionInformation.ForceSave = true;
                config.Save(ConfigurationSaveMode.Full);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private ColorPicker GetColorFromConfig(string settingName, string labelName)
        {
            var setting = userSettingsSection.Settings.Get(settingName);
            var value = ((setting.Value.ValueXml).LastChild).InnerText.ToString();
            return new ColorPicker(labelName, settingName, value);
        }

        private AppSetting GetAppSettingFromConfig(string settingName, string labelName, SettingType settingType = SettingType.String)
        {
            var setting = userSettingsSection.Settings.Get(settingName);
            var value = ((setting.Value.ValueXml).LastChild).InnerText.ToString();
            return new AppSetting(labelName, settingName, value, settingType);
        }

        private void UpdateAppearanceUserSettings(IList<ColorPicker> colors)
        {
            foreach (var color in colors)
            { 
                var setting = userSettingsSection.Settings.Get(color.ConfigurationName);
                setting.Value.ValueXml.InnerText = color.SelectedColor.ToString();
            }
        }

        private void UpdateAppSettingUserSettings(IList<AppSetting> AppSettings)
        {
            foreach (var AppSetting in AppSettings)
            {
                var setting = userSettingsSection.Settings.Get(AppSetting.ConfigurationName);
                setting.Value.ValueXml.InnerText = AppSetting.Value;
            }
        }

        //private void UpdateConnectionString(ConnectionSetting connectionString) //todo implement
        //{
        //    config.ConnectionStrings.ConnectionStrings[connectionStringName].ConnectionString = connectionString.Value;
        //}
    }
}
