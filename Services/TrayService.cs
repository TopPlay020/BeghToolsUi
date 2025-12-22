using Hardcodet.Wpf.TaskbarNotification;
using System.Windows.Media.Imaging;

namespace BeghToolsUi.Services
{
    public class TrayService : ISingletonable , IAutoStartGUI
    {
        private TaskbarIcon _trayIcon;
        public TrayService()
        {
            _trayIcon = new TaskbarIcon
            {
                IconSource = new BitmapImage(GetUri("App.ico")),
                ToolTipText = "BeghToolsUi"
            };
        }
    }
}
