using AppConfigurator.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigurator.Models
{
    public class SettingSection
    {
        public string TabHeader { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        private ObservableCollection<AppSetting> settings;
        public ObservableCollection<AppSetting> Settings
        {
            get { return settings; }
            set
            {
                if (value != settings)
                {
                    settings = value;
                    OnPropertyChanged("Settings");
                }
            }
        }

        public SettingSection(string sectionName, List<AppSetting> appSettings, string sectionPath)
        {
            TabHeader = SettingsHelper.BeautifySettingName(sectionName);
            Name = sectionName;
            Settings = new ObservableCollection<AppSetting>(appSettings);
            Path = sectionPath;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
