using Object = UnityEngine.Object;

namespace Components.Common
{
    public interface IFixedUpdater
    {
        void OnFixedUpdate();
    }
    
    public interface IFixedUpdaterLinker : IFixedUpdater
    {
    }

    public class MonoFixedUpdaterLinkBase<T> : IFixedUpdaterLinker where T : Object, IFixedUpdater
    {
        public T Value;

        public void OnFixedUpdate()
            => Value.OnFixedUpdate();
    }
    
    //[Serializable] public class VisualStrategyLink : MonoFixedUpdaterLinkBase<VisualUnitRotationStrategy> { }
}