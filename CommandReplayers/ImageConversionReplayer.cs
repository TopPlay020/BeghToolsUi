using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace BeghToolsUi.CommandReplays
{
    [ArgumentPlayable("ImageConversion", "Add Context Menu Making converting Image Type easier.", "/Assets/Icons/ImageEditor.png")]

    internal class ImageConversionReplayer : IArgumentPlayable, ITransientable, IContextMenuAddable
    {
        private ImageConversionService _imageConversionService;
        public ImageConversionReplayer(ImageConversionService imageConversionService)
        {
            _imageConversionService = imageConversionService;
        }

        public void PlayWithArgument(StartupEventArgs argument)
        {
            var inputImagePath = argument.Args[1];
            var outputImageType = argument.Args[2];

            if (File.Exists(inputImagePath))
            {
                var outputImagePath = Path.ChangeExtension(inputImagePath, outputImageType);
                if (File.Exists(outputImagePath))
                    MessageBox.Show($"Output image file already exists: {outputImagePath}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    _imageConversionService.ConvertImage(inputImagePath, outputImagePath, outputImageType);
            }
            else
            {
                MessageBox.Show($"Input image file does not exist: {inputImagePath}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public void AddToContextMenu()
        {
            var supportedTypes = ImageConversionService.SupportedFormats;
            string execPath = GetCurrentExecutablePath();
            using var baseKey = Registry.ClassesRoot.OpenSubKey(@"SystemFileAssociations", true);

            foreach (var ext in supportedTypes)
            {
                string mainKeyPath = $@".{ext}\shell\ConvertImageType";
                using var mainKey = baseKey!.CreateSubKey(mainKeyPath);
                mainKey.SetValue("MUIVerb", "Convert Image Type");
                mainKey.SetValue("Icon", execPath);
                mainKey.SetValue("SubCommands", ""); // Empty string enables submenu!

                // Create shell subfolder for subcommands
                string shellPath = $@"{mainKeyPath}\shell";
                using var shellKey = baseKey.CreateSubKey(shellPath);

                // Create each conversion option
                foreach (var type in supportedTypes)
                {
                    if (type.Equals(ext)) continue;

                    string subKeyName = $"ConvertTo{type.ToUpper()}";

                    using var subKey = shellKey.CreateSubKey(subKeyName);
                    subKey.SetValue("", $"Convert to {type.ToUpper()}");
                    subKey.SetValue("Icon", execPath);

                    using var cmdKey = subKey.CreateSubKey("command");
                    cmdKey.SetValue("", $"\"{execPath}\" ImageConversion \"%1\" {type}");
                }
            }
        }

        public bool ExistsInContextMenu()
        {
            using var key = Registry.ClassesRoot.OpenSubKey(@"SystemFileAssociations\.png\shell\ConvertImageType");
            return key != null;
        }

        public void RemoveFromContextMenu()
        {
            string[] types = { ".png", ".jpg", ".jpeg", ".bmp", ".ico", ".gif" };
            using var baseKey = Registry.ClassesRoot.OpenSubKey(@"SystemFileAssociations", true);

            foreach (var ext in types)
            {
                try
                {
                    baseKey!.DeleteSubKeyTree($@"{ext}\shell\ConvertImageType", false);
                }
                catch { }
            }
        }


    }
}
