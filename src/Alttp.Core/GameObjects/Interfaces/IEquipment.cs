namespace Alttp.Core.GameObjects.Interfaces
{
    public interface IEquipment : IGameObject
    {
        IGameObject Parent { get; set; }

        void EquippedBy(IGameObject parent);
        void UnequippedBy(IGameObject parent);
    }
}