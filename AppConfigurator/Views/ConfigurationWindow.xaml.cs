﻿using AppConfigurator.Helpers;
using AppConfigurator.Models;
using AppConfigurator.Repositories;
using AppConfigurator.Repositories.Contract;
using AppConfigurator.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppConfigurator
{
    public partial class ConfigurationWindow : Window
    {
        public SettingsViewModel ViewModel { get; set; }
        private ISettingsRepository settingsRepository;

        public ConfigurationWindow()
        {
            InitializeComponent();
            settingsRepository = new AppSettingsRepository(GetSelectedConfigFile());
            ViewModel = new SettingsViewModel(settingsRepository);
            this.DataContext = ViewModel;
        }

        private void SaveSettings(object sender, RoutedEventArgs e) //todo add to resources & check for changes in config 
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save changes?", "Save changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes && settingsRepository.SaveSettings(ViewModel))
                MessageBox.Show("Save successful!", "Saved!", MessageBoxButton.OK, MessageBoxImage.Information);
            else if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
            { 
                //do nothing
            }
            else
                MessageBox.Show("Save was not successful!", "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private string GetSelectedConfigFile() //todo add checkbox with remember selection setting, center openfiledialog 
        {
            string selection = null;
            var openFileDialog = UIHelper.GetConfigOpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
                selection = openFileDialog.FileName;
            else
                Environment.Exit(0);

            return selection;
        }
    }
}
