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
        Configuration config;
        private List<SettingSection> SettingSections { get; set; }


        public AppSettingsRepository(string configFilePath)
        {
            SettingSections = new List<SettingSection>();
            LoadSettingsFromFile(configFilePath);
        }

        public List<SettingSection> GetSettingSections()
        {
            return SettingSections;
        }

        private void LoadSettingsFromFile(string configFilePath)
        {
            List<string> sectionNames;

            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = configFilePath;
            config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            sectionNames = GetSettingsSectionNames(configFilePath);

            foreach (var sectionName in sectionNames)
            {
                string settingsPath = GetFullSettingsSectionPath(configFilePath, sectionName);
                ClientSettingsSection clientSettingsSection = (ClientSettingsSection)config.GetSection(settingsPath);
                var appSettings = GetAppSettingsFromSection(clientSettingsSection);
                SettingSections.Add(new SettingSection(sectionName, appSettings, settingsPath));
            }
        }

        private string GetFullSettingsSectionPath(string configFilePath, string sectionName)
        {
            string userSettingsNodeName = null;
            XDocument configXml = XDocument.Load(configFilePath);
            var userSettingsNode = (XElement)configXml.Descendants().Where(n => n.Name.LocalName.Equals(sectionName)).FirstOrDefault();
            if (userSettingsNode != null)
                userSettingsNodeName = ((XElement)userSettingsNode.FirstNode).Name.LocalName;

            return String.Concat(sectionName, "/", userSettingsNodeName);
        }

        private List<string> GetSettingsSectionNames(string configFilePath)
        {
            const string sectionGroupName = "sectionGroup";
            List<string> sectionNames = new List<string>();

            XDocument configXml = XDocument.Load(configFilePath);
            var sectionGroupNames = configXml.Descendants().Where(x => x.Name.LocalName.Equals(sectionGroupName)).ToList();

            foreach (var section in sectionGroupNames)
            {
                var sectionName = section.FirstAttribute.Value.ToString();
                sectionNames.Add(sectionName);
            }
            return sectionNames;
        }

        private List<AppSetting> GetAppSettingsFromSection(ClientSettingsSection clientSettingsSection) 
        {
            List<AppSetting> appSettings = new List<AppSetting>();

            foreach (SettingElement setting in clientSettingsSection.Settings)
            {
                string value = String.Empty;
                var name = setting.Name;
                if((setting.Value.ValueXml).LastChild != null)
                    value = ((setting.Value.ValueXml).LastChild).InnerText?.ToString();
                var labelName = SettingsHelper.BeautifySettingName(name);
                var settingEditorType = GetSettingEditorType(value);

                appSettings.Add(new AppSetting(labelName, name, value, settingEditorType));
            }
            return appSettings;
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

        public bool SaveSettings(SettingsViewModel viewModel)
        {
            try
            {
                foreach (var section in viewModel.SettingSections)
                {
                    FormatSettingsDateTimes(section.Settings);
                    UpdateAppSettingUserSettings(section);
                }
                ////UpdateConnectionSettingUserSettings(viewModel.ConnectionSettings); //todo implement
                ////UpdateConnectionString(viewModel.ConnectionSettings.Where(x=>x.ConfigurationName.Equals(connectionStringName)).FirstOrDefault());
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

        private void UpdateAppSettingUserSettings(SettingSection section)
        {
            ClientSettingsSection clientSettingsSection = (ClientSettingsSection)config.GetSection(section.Path);
            foreach (var appSetting in section.Settings)
            {
                var setting = clientSettingsSection.Settings.Get(appSetting.ConfigurationName);
                setting.Value.ValueXml.InnerText = appSetting.Value;
            }
            clientSettingsSection.SectionInformation.ForceSave = true;
        }

        //private void UpdateConnectionString(ConnectionSetting connectionString) //todo implement
        //{
        //    config.ConnectionStrings.ConnectionStrings[connectionStringName].ConnectionString = connectionString.Value;
        //}
    }
}
