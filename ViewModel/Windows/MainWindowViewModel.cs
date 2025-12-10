using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Documents;

namespace BeghToolsUi.ViewModel.Windows
{
    public partial class MainWindowViewModel : ObservableObject, ITransientable
    {
#pragma warning disable CS8618
        [ObservableProperty]
        private object mainView;
        [ObservableProperty]
        private bool isAnimated;
        [ObservableProperty]
        private ObservableCollection<MenuItemModel> mainMenuItems;
        [ObservableProperty]
        private ObservableCollection<MenuItemModel> footerMenuItems;
        [ObservableProperty]
        private MenuItemModel? mainSelectedItem;
        [ObservableProperty]
        private MenuItemModel? footerSelectedItem;
        public MainWindowViewModel()
        {
            MainMenuItems = new();
            FooterMenuItems = new();
            foreach (var type in GetTypesImplementing<IPageMenu>())
            {
                var attr = type.GetCustomAttribute<PageInfoAttribute>()!;
                var PageType = GetTypeByName(type.Name.Substring(0, type.Name.Length - "ViewModel".Length))!;

                if (attr.IsFooterPage)
                    FooterMenuItems.Add(new MenuItemModel { Title = attr.PageTitle, Icon = attr.PageIcon, PageType = PageType });
                else
                    MainMenuItems.Add(new MenuItemModel { Title = attr.PageTitle, Icon = attr.PageIcon, PageType = PageType });
            }

            MainSelectedItem = mainMenuItems.First();
        }

        void ChangeView(MenuItemModel? value, bool isMain)
        {
            if (value == null) return;
            if (isMain) FooterSelectedItem = null;
            else MainSelectedItem = null;
            IsAnimated = false;
            MainView = App.Services.GetRequiredService(value.PageType);
            IsAnimated = true;
        }
        partial void OnMainSelectedItemChanged(MenuItemModel? value) => ChangeView(value, true);
        partial void OnFooterSelectedItemChanged(MenuItemModel? value) => ChangeView(value, false);

    }
}
