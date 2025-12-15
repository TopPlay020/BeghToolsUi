using System.Windows;
using System.Windows.Controls;
namespace BeghToolsUi.Views.Pages
{
    public partial class ImageConverter : UserControl, ITransientable
    {
        ImageConverterViewModel ViewModel;
        public ImageConverter(ImageConverterViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;
        }

        private void Button_Drop(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string firstFile = files.FirstOrDefault(); // first dropped file path
                if (firstFile != null)
                {
                    ViewModel.DropSourceImagePathCommand(firstFile);
                }
            }
        }
    }
}
