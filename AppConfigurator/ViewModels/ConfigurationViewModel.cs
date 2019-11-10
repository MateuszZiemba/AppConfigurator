using AppConfigurator.Models;
using AppConfigurator.Repositories;
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
    public class ConfigurationViewModel : INotifyPropertyChanged
    {
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

        public ConfigurationViewModel(ISettingsRepository settingsRepository)
        {
            Settings = new ObservableCollection<AppSetting>(settingsRepository.GetApplicationSettings());
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
