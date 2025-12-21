using System.Diagnostics;
using System.Net.NetworkInformation;

namespace BeghToolsUi.Services
{
    public class MonitoringService : ISingletonable
    {
        public List<NetworkInterfaceViewModel> NetworkInterfaces { get; set; } = default!;
        public MonitoringService()
        {
            InitGetNetworkInterface();
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
                     n.GetIPProperties().UnicastAddresses.Count > 0);
        }

        public void InitGetNetworkInterface()
        {
            var networkInterfaces = GetNetworkInterfaces();
            foreach (var networkInterface in networkInterfaces)
            {
                Debug.WriteLine($"Name: {networkInterface.Name}, Status: {networkInterface.OperationalStatus}");
            }
            NetworkInterfaces =
                networkInterfaces.Select(ni => new NetworkInterfaceViewModel
                {
                    Name = ni.Name,
                    IsUp = ni.OperationalStatus == OperationalStatus.Up,
                }).ToList();
        }

        public void UpdateNetworkStatus()
        {
            var networkInterfaces = GetNetworkInterfaces();

            // Update existing and add new
            foreach (var ni in networkInterfaces)
            {
                var existing = NetworkInterfaces.FirstOrDefault(x => x.Name == ni.Name);
                if (existing != null)
                {
                    existing.IsUp = ni.OperationalStatus == OperationalStatus.Up;
                }
                else
                {
                    NetworkInterfaces.Add(new NetworkInterfaceViewModel
                    {
                        Name = ni.Name,
                        IsUp = ni.OperationalStatus == OperationalStatus.Up
                    });
                }
            }

        }
    }
}
