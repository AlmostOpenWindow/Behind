namespace Components.Common
{
    public class PausableMonoController : MonoController
    {
        private bool _paused;
        
        public void Pause()
        {
            _paused = true;
        }

        public void Resume()
        {
            _paused = false;
        }

        public override void OnUpdate()
        {
            if (_paused)
                return;
            
            base.OnUpdate();
        }

        public override void OnFixedUpdate()
        {
            if (_paused)
                return;
            
            base.OnFixedUpdate();
        }

        public override void OnLateUpdate()
        {
            if (_paused)
                return;
            
            base.OnLateUpdate();
        }
    }
}