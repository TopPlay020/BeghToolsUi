using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BeghToolsUi.Views.Pages
{
    /// <summary>
    /// Interaction logic for Monitoring.xaml
    /// </summary>
    public partial class Monitoring : UserControl, ITransientable
    {
        MonitoringViewModel ViewModel;
        public Monitoring(MonitoringViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnUnload();
        }
    }
}
