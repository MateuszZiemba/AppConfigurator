using AppConfigurator.Models;
using AppConfigurator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigurator.Repositories.Contract
{
    public interface ISettingsRepository
    {
        List<AppSetting> GetApplicationSettings();
        List<string> GetSectionNames();
        bool SaveSettings(SettingsViewModel viewModel);
    }
}
