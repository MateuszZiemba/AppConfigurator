using AppConfigurator.Models;
using AppConfigurator.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigurator.ViewModels
{
    public class SettingsViewModel
    {
        public ObservableCollection<TabItem> Tabs { get; set; }

        public SettingsViewModel(ISettingsRepository settingsRepository)
        {
            Tabs = new ObservableCollection<TabItem>();

            List<string> sectionNames = settingsRepository.GetSectionNames();

            foreach (var sectionName in sectionNames)
            {
                Tabs.Add(new TabItem { Header = sectionName, Settings = new ObservableCollection<AppSetting>(settingsRepository.GetApplicationSettings())});
            }
        }
    }
    public class TabItem
    {
        public string Header { get; set; }

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
