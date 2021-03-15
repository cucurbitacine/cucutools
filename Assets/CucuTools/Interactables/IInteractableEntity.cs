namespace CucuTools.Interactables
{
    public interface IInteractableEntity
    {
        bool IsEnabled { get; set; }

        void Normal();
        void Hover();
        void Click();
    }
}