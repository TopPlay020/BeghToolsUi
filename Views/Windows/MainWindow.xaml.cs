using System.Diagnostics;
using System.Windows;
namespace BeghToolsUi.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window , ISingletonable , IAutoStartGUI
    {
        MainWindowViewModel ViewModel;
        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            ViewModel = vm;
            DataContext = vm;
            Show();
        }
    }
}
