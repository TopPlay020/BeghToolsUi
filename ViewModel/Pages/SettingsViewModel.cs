using BeghToolsUi.CommandReplays;
using BeghToolsUi.Interfaces;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows;

namespace BeghToolsUi.ViewModel.Pages
{
    [PageInfo(true, "Settings", "/Assets/Icons/Settings.png")]
    public partial class SettingsViewModel : ObservableObject, IPageMenu, ITransientable
    {
        [ObservableProperty]
        private ObservableCollection<ContextMenuItemModel> contextMenuItems;

        //[ObservableProperty]
        //private ObservableCollection<IContextMenuAddable> contextMenuAddables;

        public SettingsViewModel()
        {
            contextMenuItems = new ObservableCollection<ContextMenuItemModel>();
            var contextMenuAddables = GetTypesImplementing<IContextMenuAddable>();
            foreach (var addable in contextMenuAddables)
            {
                var attrubute = addable.GetCustomAttribute<ArgumentPlayableAttribute>()!;
                var contextMenuAddable = (IContextMenuAddable)App.Services.GetRequiredService(addable);
                contextMenuItems.Add(new ContextMenuItemModel
                {
                    Description = attrubute.ArgumentDescription,
                    IconPath = attrubute.ArgumentIcon,
                    ContextMenuAddable = contextMenuAddable,
                    IsInstalled = contextMenuAddable.ExistsInContextMenu(),
                    IsInstalling = false
                });
            }
        }

        [RelayCommand]
        private void ContextMenuAddable(ContextMenuItemModel item)
        {
            item.IsInstalling = true;
            if(item.ContextMenuAddable.ExistsInContextMenu())
            {
                item.ContextMenuAddable.RemoveFromContextMenu();
                item.IsInstalled = false;
            }
            else
            {
                item.ContextMenuAddable.AddToContextMenu();
                item.IsInstalled = true;
            }
            item.IsInstalling = false;
        }
    }
}
