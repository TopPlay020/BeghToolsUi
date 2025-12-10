using BeghToolsUi.Attributes;

namespace BeghToolsUi.ViewModel.Pages
{
    [PageInfo(false, "Home", "/Assets/Icons/Home.png")]
    public partial class HomeViewModel : ObservableObject, IPageMenu , ITransientable
    {
    }
}
