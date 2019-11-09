using AppConfigurator.Enums;
using AppConfigurator.Helpers;
using AppConfigurator.Models;
using AppConfigurator.Repositories.Contract;
using AppConfigurator.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppConfigurator.Repositories
{
    public class ClientSettingsRepository : ISettingsRepository
    {
        private Configuration config;
        private ClientSettingsSection userSettingsSection;
        private string connectionStringName;
        private const string userSettingsName = "userSettings";

        private List<ColorPicker> ColorPickers { get; set; }
        private List<AppSetting> AppSettings { get; set; }

        public ClientSettingsRepository(string configFilePath)
        {
            string userSettingsPath = GetFullUserSettingsSectionPath(configFilePath);

            //connectionStringName = "connstringName"; //todo to implement

            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = configFilePath;
            config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            userSettingsSection = (ClientSettingsSection)config.GetSection(userSettingsPath);
            
            

            ColorPickers = new List<ColorPicker>();
            AppSettings = new List<AppSetting>();
            MethodToGetData();
        }

        private string GetFullUserSettingsSectionPath(string configFilePath)
        {
            string userSettingsNodeName = null;
            XDocument configXml = XDocument.Load(configFilePath);
            var userSettingsNode = (XElement)configXml.Descendants().Where(n => n.Name.LocalName.Equals(userSettingsName)).FirstOrDefault();
            if (userSettingsNode != null)
                userSettingsNodeName = ((XElement)userSettingsNode.FirstNode).Name.LocalName;
            return String.Concat(userSettingsName, "/", userSettingsNodeName);
        }

        private void MethodToGetData()
        {
            foreach (SettingElement setting in userSettingsSection.Settings)
            {
                var name = setting.Name;
                var value = ((setting.Value.ValueXml).LastChild).InnerText.ToString();
                var labelName = LabelHelper.GetLabelFromSettingName(name);
                //var settingType = GetSettingType();

                AppSettings.Add(new AppSetting(labelName, name, value, SettingType.String));
            }
        }

        private SettingType GetSettingType(string settingElementType)
        {
            return SettingType.String;
        }

        public List<ColorPicker> GetAppearanceSettings()
        {
            //List<ColorPicker> colorPickers = new List<ColorPicker>();

            //colorPickers.Add(GetColorFromConfig("ExampleOfProperty", "Example of label"));

            return ColorPickers;
        }

        public List<AppSetting> GetApplicationSettings()
        {
            //List<AppSetting> AppSettings = new List<AppSetting>();

            //AppSettings.Add(GetAppSettingFromConfig("ExampleOfProperty", "Example of label"));

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
