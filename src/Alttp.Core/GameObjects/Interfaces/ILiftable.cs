namespace Alttp.Core.GameObjects.Interfaces
{
    public interface ILiftable
    {
        float Weight { get; }

        bool Lift();
        void Throw();
    }
}
