using UnityEngine;

namespace Infrastructure.Containers.UnityApi
{
    public class UnityApiBehavior : MonoBehaviour
    {
        private IFrameUpdater _frameUpdater;

        private bool _constructed;
        
        public void Construct(IFrameUpdater frameUpdater)
        {
            _frameUpdater = frameUpdater;
            _constructed = true;
        }

        private void Update()
        {
            if (!_constructed)
                return;
            
            _frameUpdater.Update();
        }

        private void FixedUpdate()
        {
            if (!_constructed)
                return;
            
            _frameUpdater.FixedUpdate();
        }

        private void LateUpdate()
        {
            if (!_constructed)
                return;
            
            _frameUpdater.LateUpdate();
        }
    }
}