namespace Doom.Infrastructure.Containers.UnityApi
{
    public interface IFrameUpdater
    {
        void Update();
        void FixedUpdate(); 
    }
}