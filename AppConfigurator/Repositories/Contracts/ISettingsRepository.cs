﻿using AppConfigurator.Models;
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
        List<SettingSection> GetSettingSections();
        bool SaveSettings(SettingsViewModel viewModel);
    }
}
