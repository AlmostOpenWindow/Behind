using Object = UnityEngine.Object;

namespace Components.Common
{
    public interface ILateUpdater
    {
        void OnLateUpdate();
    }
    
    public interface ILateUpdaterLinker : ILateUpdater
    {
    }
    
    public class MonoLateUpdaterLinkBase<T> : ILateUpdaterLinker where T : Object, ILateUpdater
    {
        public T Value;

        public void OnLateUpdate()
            => Value.OnLateUpdate();
    }
}