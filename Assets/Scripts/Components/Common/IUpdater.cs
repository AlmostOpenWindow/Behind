using Object = UnityEngine.Object;

namespace Components.Common
{
    public interface IUpdater
    {
        void OnUpdate();
    }
    
    public interface IUpdaterLinker : IUpdater { }
    
    public abstract class MonoUpdaterLinkBase<T> : IUpdaterLinker where T : Object, IUpdater
    {
        public T Value;
        
        public void OnUpdate()
            => Value.OnUpdate();
    }
}