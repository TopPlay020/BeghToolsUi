using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace BeghToolsUi.Views.Pages
{
    public partial class Home : UserControl , ITransientable
    {
        HomeViewModel ViewModel;
        public Home(HomeViewModel vm)
        {
            InitializeComponent();
            ViewModel = vm;
            DataContext = vm;
        }
    }
}
