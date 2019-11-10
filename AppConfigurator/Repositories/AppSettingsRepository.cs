using AppConfigurator.Enums;
using AppConfigurator.Helpers;
using AppConfigurator.Models;
using AppConfigurator.Repositories.Contract;
using AppConfigurator.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppConfigurator.Repositories
{
    public class AppSettingsRepository : ISettingsRepository
    {
        private Configuration config;
        private ClientSettingsSection userSettingsSection;
        private string connectionStringName;
        private const string userSettingsName = "userSettings";

        private List<AppSetting> AppSettings { get; set; }

        public AppSettingsRepository(string configFilePath)
        {
            string userSettingsPath = GetFullUserSettingsSectionPath(configFilePath);

            //connectionStringName = "connstringName"; //todo to implement

            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = configFilePath;
            config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            userSettingsSection = (ClientSettingsSection)config.GetSection(userSettingsPath);
            
            AppSettings = new List<AppSetting>();

            GetAppSettingsFromSettingsFile();
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

        private void GetAppSettingsFromSettingsFile() 
        {
            foreach (SettingElement setting in userSettingsSection.Settings)
            {
                var name = setting.Name;
                var value = ((setting.Value.ValueXml).LastChild).InnerText.ToString();
                var labelName = SettingsHelper.GetLabelFromSettingName(name);
                var settingEditorType = GetSettingEditorType(value);

                AppSettings.Add(new AppSetting(labelName, name, value, settingEditorType));
            }
        }

        private SettingEditorType GetSettingEditorType(string settingValue)
        {
            string lowercaseValue = settingValue.ToLower();

            if (lowercaseValue.Equals("true") || lowercaseValue.Equals("false"))
                return SettingEditorType.Bool;
            else if (SettingsHelper.IsSettingNumeric(settingValue))
                return SettingEditorType.Numeric;
            else if (SettingsHelper.IsSettingDateTime(settingValue))
                return SettingEditorType.DateTime;
            else if (SettingsHelper.IsSettingColor(settingValue))
                return SettingEditorType.ColorPicker;
            else
                return SettingEditorType.String;
        }

        public List<AppSetting> GetApplicationSettings()
        {
            return AppSettings;
        }
        public bool SaveSettings(ConfigurationViewModel viewModel)
        {
            try
            {
                FormatSettingsDateTimes(viewModel.Settings);
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

        private void FormatSettingsDateTimes(IList<AppSetting> appSettings)
        {
            DateTimeFormatInfo formatInfo = CultureInfo.CurrentCulture.DateTimeFormat;
            foreach (var setting in appSettings)
            {
                if (setting.SettingType.Equals(SettingEditorType.DateTime))
                {
                    if (setting.OriginalValue.Length > 10)
                        setting.Value = Convert.ToDateTime(setting.Value, formatInfo).ToString();
                    else
                        setting.Value = Convert.ToDateTime(setting.Value, formatInfo).ToString(formatInfo.ShortDatePattern);
                }
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
