using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace BeghToolsUi.Views.Pages
{
    public partial class Settings : UserControl , ITransientable
    {
        SettingsViewModel ViewModel;
        public Settings(SettingsViewModel vm)
        {
            InitializeComponent();
            ViewModel = vm;
            DataContext = vm;
        }
    }
}
