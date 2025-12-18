using Microsoft.Win32;
using System.IO;
namespace BeghToolsUi.ViewModel.Pages
{
    [PageInfo(false, "Image Converter", "/Assets/Icons/ImageEditor.png")]
    public partial class ImageConverterViewModel : ObservableObject, IPageMenu, ITransientable
    {
        private const string DefaultImagePath = "/Assets/Icons/Image.png";
        [ObservableProperty]
        private List<string> imageTypes;
        [ObservableProperty]
        private string outputImageType;
        [ObservableProperty]
        private string sourceImagePath = DefaultImagePath;
        [ObservableProperty]
        private string? unsupportedImageInput = null;
        [ObservableProperty]
        private string outputImagePath = DefaultImagePath;
        [ObservableProperty]
        private string? unsupportedImageOutput = null;
        [ObservableProperty]
        private string selectedColorHex = "#FFFFFF";
        [ObservableProperty]
        private int outputWidth = 100;
        [ObservableProperty]
        private int outputHeight = 100;
        [ObservableProperty]
        private bool maintainAspectRatio = true;

        [ObservableProperty]
        private bool canConvert = false;
        [ObservableProperty]
        private bool isConverting = false;
        [ObservableProperty]
        private bool isJPEG = false;
        [ObservableProperty]
        private bool readyToExport = false;

        public bool IsConvertMenuEnabled => CanConvert && !IsConverting;

        partial void OnCanConvertChanged(bool oldValue, bool newValue) { OnPropertyChanged(nameof(IsConvertMenuEnabled)); }
        partial void OnIsConvertingChanged(bool oldValue, bool newValue) { OnPropertyChanged(nameof(IsConvertMenuEnabled)); }

        private ImageConversionService _imageConversionService;
        public ImageConverterViewModel(ImageConversionService imageConversionService)
        {
            imageTypes = ImageConversionService.SupportedFormats;
            outputImageType = imageTypes[0];
            _imageConversionService = imageConversionService;
        }

        partial void OnOutputImageTypeChanged(string value)
        {
            IsJPEG = value == "jpg";
            ReadyToExport = false;
        }

        private void SelectFilePath(string SelectedFilePath)
        {
            SourceImagePath = SelectedFilePath;
            ReadyToExport = false;
            CanConvert = true;
            OutputImagePath = DefaultImagePath;
        }

        public void DropSourceImagePathCommand(string SelectedFilePath)
        {
            var ext = Path.GetExtension(SelectedFilePath).TrimStart('.');
            if (ImageTypes.Contains(ext.ToLower()))
                SelectFilePath(SelectedFilePath);
        }

        [RelayCommand]
        private void SelectSourceImagePath()
        {
            var OldCanConvert = CanConvert;
            CanConvert = false;
            var dialog = new OpenFileDialog
            {
                Title = "Select Source Image",
                Filter = "Image Files|" + string.Join(";", ImageTypes.Select(f => "*." + f))

            };
            if (dialog.ShowDialog() == true)
            {
                var selectedFilePath = dialog.FileName;
                var selectedFileExtension = Path.GetExtension(selectedFilePath).ToLower().TrimStart('.');
                if (WPFImageUnsupportedFormats.Contains(selectedFileExtension))
                    UnsupportedImageInput = selectedFileExtension;
                else
                    UnsupportedImageInput = null;
                SelectFilePath(dialog.FileName);
            }
            else
            {
                CanConvert = OldCanConvert;
            }
        }

        [RelayCommand]
        private void SelectColor()
        {
            
        }

        [RelayCommand]
        private async Task ConvertImage()
        {
            string TempDir = GetRequiredService<TempService>().TempDir;
            string tempFile = Path.Combine(TempDir, $"{Guid.NewGuid()}.{OutputImageType}");
            IsConverting = true;
            CanConvert = false;
            ReadyToExport = false;
            await Task.Run(() =>
            {
                _imageConversionService.ConvertImage(SourceImagePath, tempFile, OutputImageType);
            });
            IsConverting = false;
            CanConvert = true;
            ReadyToExport = true;
            if (WPFImageUnsupportedFormats.Contains(OutputImageType))
                UnsupportedImageOutput = OutputImageType;
            else
                UnsupportedImageOutput = null;
            OutputImagePath = tempFile;
        }

        [RelayCommand]
        private void ExportImage()
        {
            var suggestedName = $"{Path.GetFileNameWithoutExtension(SourceImagePath)}.{OutputImageType!}";
            var dialog = new SaveFileDialog
            {
                Title = "Export Converted Image",
                FileName = suggestedName,
                Filter = $"{OutputImageType.ToUpper()} Files|*.{OutputImageType.ToLower()}"
            };
            if (dialog.ShowDialog() == true)
            {
                File.Copy(OutputImagePath, dialog.FileName, true);
            }
        }
    }
}