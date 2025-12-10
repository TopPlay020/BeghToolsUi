namespace BeghToolsUi.Interfaces
{
    public interface IContextMenuAddable
    {
        void AddToContextMenu();
        void RemoveFromContextMenu();
        bool ExistsInContextMenu();
    }
}
