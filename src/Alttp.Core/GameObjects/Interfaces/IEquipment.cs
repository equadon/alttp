namespace Alttp.Core.GameObjects.Interfaces
{
    public interface IEquipment : IGameObject
    {
        IGameObject Parent { get; set; }

        bool IsEquipped { get; }

        void EquippedBy(IGameObject parent);
        void UnequippedBy(IGameObject parent);
    }
}