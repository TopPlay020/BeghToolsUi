#pragma warning disable CS8601
namespace BeghToolsUi.ViewModel.Pages
{
    [PageInfo(false, "Monitoring", "/Assets/Icons/Monitoring.png")]
    public partial class MonitoringViewModel : ObservableObject, IPageMenu, ITransientable
    {
        private MonitoringService _monitoringService;
        [ObservableProperty]
        private List<NetworkInterfaceViewModel> networkInterface;
        [ObservableProperty]
        private NetworkInterfaceViewModel? selectedNetworkInterface;

        [ObservableProperty]
        private string downloadSpeed = "0 kb/s";
        [ObservableProperty]
        private string uploadSpeed = "0 kb/s";
        public MonitoringViewModel(MonitoringService monitoringService)
        {
            _monitoringService = monitoringService;
            NetworkInterface = _monitoringService.GetCurrentNetworkInterfaces().Select(nis => new NetworkInterfaceViewModel
            {
                Id = nis.Id,
                Name = nis.Name,
                IsUp = nis.IsUp
            }).ToList();
            SelectedNetworkInterface = NetworkInterface.FirstOrDefault(ni => ni.IsUp == true);

            _monitoringService.OnNetworkStatusChanged += OnNetworkInterfaceStatusChanged;
            _monitoringService.OnBandwidthChanged += OnBandwidthChanged;
        }
        public void OnUnload()
        {
            _monitoringService.OnNetworkStatusChanged -= OnNetworkInterfaceStatusChanged;
            _monitoringService.OnBandwidthChanged -= OnBandwidthChanged;

        }
        partial void OnSelectedNetworkInterfaceChanged(NetworkInterfaceViewModel? value)
        {
            OnBandwidthChanged(0, 0);
            _monitoringService.SetMonitoredInterface(value?.Id);
        }
        public void OnNetworkInterfaceStatusChanged(string id, bool isUp)
        {
            var ni = NetworkInterface.FirstOrDefault(n => n.Id == id);
            if (ni == null) return;
            ni.IsUp = isUp;
            if (ni != SelectedNetworkInterface && SelectedNetworkInterface != null) return;
            SelectedNetworkInterface = NetworkInterface.FirstOrDefault(ni => ni.IsUp == true);
        }

        public void OnBandwidthChanged(int downloadKbps, int uploadKbps)
        {
            string FormatSpeed(int kbps)
            {
                if (kbps >= 1024)
                    return $"{kbps / 1024.0:F2} mb/s";
                else
                    return $"{kbps} kb/s";
            }
            DownloadSpeed = FormatSpeed(downloadKbps);
            UploadSpeed = FormatSpeed(uploadKbps);
        }
    }
}
