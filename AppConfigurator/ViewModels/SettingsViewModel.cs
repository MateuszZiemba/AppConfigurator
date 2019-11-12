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
        public ObservableCollection<SettingSection> SettingSections { get; set; }

        public SettingsViewModel(ISettingsRepository settingsRepository)
        {
            SettingSections = new ObservableCollection<SettingSection>(settingsRepository.GetSettingSections());
        }
    }
}
