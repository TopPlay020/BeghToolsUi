using System.Net.NetworkInformation;
using System.Threading;

namespace BeghToolsUi.Services
{
    public class MonitoringService : ISingletonable
    {
        private List<NetworkInterface> NetworkInterfaces;
        public int DownloadKbps;
        public int UploadKbps;

        public Action<string, bool> OnNetworkStatusChanged = default!;
        public Action<int, int> OnBandwidthChanged = default!;

        private CancellationTokenSource? CancellationTokenSource;
        private NetworkInterface? SelectedInterface;
        private Task? MonitoringTask;

        public MonitoringService()
        {
            NetworkInterfaces = GetNetworkInterfaces().ToList();

            NetworkChange.NetworkAddressChanged += (s, e) => UpdateNetworkStatus();
            NetworkChange.NetworkAvailabilityChanged += (s, e) => UpdateNetworkStatus();
        }

        public List<NetworkInterfaceStatus> GetCurrentNetworkInterfaces()
        {
            return NetworkInterfaces.Select(ni => new NetworkInterfaceStatus
            {
                Name = ni.Name,
                Id = ni.Id,
                IsUp = ni.OperationalStatus == OperationalStatus.Up
            }).ToList();
        }

        public void SetMonitoredInterface(string? interfaceId)
        {
            if (interfaceId == null || NetworkInterfaces.FirstOrDefault(n => n.Id == interfaceId) is null)
            {
                StopMonitoring();
                SelectedInterface = null;
                return;
            }

            var ni = NetworkInterfaces.First(n => n.Id == interfaceId);
            if (SelectedInterface == ni)
                return;
            SelectedInterface = ni;
            StopMonitoring();
            StartMonitoring();
        }
        private IEnumerable<NetworkInterface> GetNetworkInterfaces()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
            .Where(n => (n.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                         n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) &&
                        !n.Description.Contains("Virtual") &&
                        !n.Description.Contains("Npcap") &&
                        !n.Description.Contains("WFP") &&
                        !n.Description.Contains("QoS") &&
                        n.GetIPProperties().UnicastAddresses.Count > 0)
            .OrderByDescending(n => n.OperationalStatus == OperationalStatus.Up);
        }
        private void UpdateNetworkStatus()
        {
            var updatedInterfaces = GetNetworkInterfaces().ToList();

            foreach (var ni in updatedInterfaces)
            {
                var existing = NetworkInterfaces.FirstOrDefault(n => n.Id == ni.Id);
                if (existing != null)
                {
                    // Update status
                    bool wasUp = existing.OperationalStatus == OperationalStatus.Up;
                    bool isUpNow = ni.OperationalStatus == OperationalStatus.Up;
                    if (wasUp != isUpNow)
                        OnNetworkStatusChanged?.Invoke(ni.Id, isUpNow);
                }
            }
            NetworkInterfaces = updatedInterfaces;
        }

        private void StartMonitoring(int intervalMs = 1000)
        {
            if (MonitoringTask != null && !MonitoringTask.IsCompleted)
                return;

            CancellationTokenSource = new CancellationTokenSource();
            MonitoringTask = MonitorNetworkSpeedAsync(intervalMs, CancellationTokenSource.Token);
        }

        private async Task MonitorNetworkSpeedAsync(int intervalMs, CancellationToken cancellationToken)
        {
            var stats1 = SelectedInterface!.GetIPv4Statistics();
            long bytesReceived1 = stats1.BytesReceived;
            long bytesSent1 = stats1.BytesSent;

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(intervalMs, cancellationToken);

                    var stats2 = SelectedInterface!.GetIPv4Statistics();
                    long bytesReceived2 = stats2.BytesReceived;
                    long bytesSent2 = stats2.BytesSent;

                    int downloadKbps = (int)((bytesReceived2 - bytesReceived1) / 1024.0);
                    int uploadKbps = (int)((bytesSent2 - bytesSent1) / 1024.0);

                    OnBandwidthChanged?.Invoke(downloadKbps, uploadKbps);

                    bytesReceived1 = bytesReceived2;
                    bytesSent1 = bytesSent2;
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        private void StopMonitoring()
        {
            CancellationTokenSource?.Cancel();
        }

    }
}
