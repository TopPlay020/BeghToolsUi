namespace BeghToolsUi.ViewModel.Shared
{
    public partial class ContextMenuItemModel : ObservableObject
    {
        [ObservableProperty]
        public string description = default!;
        [ObservableProperty]
        public string iconPath = default!;
        required public IContextMenuAddable ContextMenuAddable;
        [ObservableProperty]
        public bool isInstalled;
        [ObservableProperty]
        public bool isInstalling;
    }
}
