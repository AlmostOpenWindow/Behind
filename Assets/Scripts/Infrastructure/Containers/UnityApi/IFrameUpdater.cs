namespace Infrastructure.Containers.UnityApi
{
    public interface IFrameUpdater
    {
        void Update();
        void FixedUpdate();
        void LateUpdate();
    }
}