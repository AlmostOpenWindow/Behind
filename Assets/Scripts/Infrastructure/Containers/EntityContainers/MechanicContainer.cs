using Infrastructure.Containers.UnityApi;
using Services.Mechanics;

namespace Infrastructure.Containers.EntityContainers
{
    public class MechanicContainer : UpdaterEntityContainer<IMechanic>, ISwitchEnableState
    {
        protected override string ContainerName
            => "MechanicContainer";

        protected override string EntityName
            => "mechanic";

        public bool IsEnable { get; private set; }
        
        public MechanicContainer(
            IFrameUpdaterSubscribe frameUpdaterSubscribe, 
            IFrameFixedUpdaterSubscribe frameFixedUpdaterSubscribe,
            IFrameLateUpdaterSubscribe frameLateUpdaterSubscribe) 
            : base(frameUpdaterSubscribe, frameFixedUpdaterSubscribe, frameLateUpdaterSubscribe)
        {
        }
        
        public void Enable()
        {
            IsEnable = true;
            foreach (var key in _entities.Keys)
            {
                _entities[key].Enable();
            }
        }

        public void Disable()
        {
            IsEnable = false;
            foreach (var key in _entities.Keys)
            {
                _entities[key].Disable();
            }
        }
    }
}