using UnityEngine;

namespace Doom.Infrastructure.Containers.UnityApi
{
    public class UnityApiBehavior : MonoBehaviour
    {
        private IFrameUpdater _frameUpdater;
        
        public void Construct(IFrameUpdater frameUpdater)
        {
            _frameUpdater = frameUpdater;
        }

        private void Update()
        {
            _frameUpdater.Update();
        }

        private void FixedUpdate()
        {
            _frameUpdater.FixedUpdate();
        }
    }
}