using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeghToolsUi.ViewModel.Shared
{
    public partial class NetworkInterfaceViewModel : ObservableObject
    {
        public required string Id;
        [ObservableProperty]
        public string name = default!;
        [ObservableProperty]
        public bool isUp = default!;
    }
}
