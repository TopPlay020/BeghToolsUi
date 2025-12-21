using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeghToolsUi.ViewModel.Pages
{
    [PageInfo(false, "Monitoring", "/Assets/Icons/Monitoring.png")]
    public partial class MonitoringViewModel : ObservableObject, IPageMenu, ITransientable
    {
        private MonitoringService _monitoringService;
        [ObservableProperty]
        private ObservableCollection<NetworkInterfaceViewModel> networkInterface;
        [ObservableProperty]
        private NetworkInterfaceViewModel selectedNetworkInterface;
        public MonitoringViewModel(MonitoringService monitoringService)
        {
            _monitoringService = monitoringService;
            NetworkInterface = new(_monitoringService.NetworkInterfaces.OrderByDescending(ni => ni.isUp));
            SelectedNetworkInterface = NetworkInterface.First();
        }
    }
}
