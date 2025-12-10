using System.Windows.Controls;
namespace BeghToolsUi.Views.Pages
{
    public partial class ImageConverter : UserControl , ITransientable
    {
        ImageConverterViewModel ViewModel;
        public ImageConverter(ImageConverterViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;
        }
    }
}
