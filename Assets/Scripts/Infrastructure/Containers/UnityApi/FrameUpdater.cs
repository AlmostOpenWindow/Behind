using System.Collections.Generic;
using Components.Common;
using Doom.GamePlay.Components.Common;
using Doom.Infrastructure.Containers.UnityApi;

namespace Infrastructure.Containers.UnityApi
{
    public class FrameUpdater : IFrameUpdaterSubscribe, IFrameFixedUpdaterSubscribe, IFrameMonoControllerSubscribe, IFrameUpdater
    {
        private readonly List<IUpdater> _updaters = new List<IUpdater>();
        private readonly List<IFixedUpdater> _fixedUpdaters = new List<IFixedUpdater>();
        
        private readonly Stack<IUpdater> _updatersAdditionalStack = new Stack<IUpdater>();
        private readonly Stack<IFixedUpdater> _fixedUpdatersAdditionalStack = new Stack<IFixedUpdater>();
        
        private readonly Stack<IUpdater> _updatersRemovableStack = new Stack<IUpdater>();
        private readonly Stack<IFixedUpdater> _fixedUpdatersRemovableStack = new Stack<IFixedUpdater>();
        
        private readonly Stack<MonoController> _updatersMonoControllersAdditionalStack = new Stack<MonoController>();
        private readonly Stack<MonoController> _fixedUpdatersMonoControllersAdditionalStack = new Stack<MonoController>();
        
        private readonly Stack<MonoController> _updatersMonoControllersRemovableStack = new Stack<MonoController>();
        private readonly Stack<MonoController> _fixedUpdatersMonoControllersRemovableStack = new Stack<MonoController>();

        public void AddUpdater(IUpdater updater)
        {
            _updatersAdditionalStack.Push(updater);
        }

        public void RemoveUpdater(IUpdater updater)
        {
            _updatersRemovableStack.Push(updater);
        }

        public void AddFixedUpdater(IFixedUpdater fixedUpdater)
        {
            _fixedUpdatersAdditionalStack.Push(fixedUpdater);
        }

        public void RemoveFixedUpdater(IFixedUpdater fixedUpdater)
        {
            _fixedUpdatersRemovableStack.Push(fixedUpdater);
        }
        
        public void AddMonoController(MonoController controller)
        {
            controller.InitializedUnityApi();
            _updatersMonoControllersAdditionalStack.Push(controller);
            _fixedUpdatersMonoControllersAdditionalStack.Push(controller);
        }
        
        private void OnControllerDisable(EnabledMonoBehavior obj)
        {
            obj.DisableEvent -= OnControllerDisable;
            if(!(obj is MonoController controller))
                return;
            
            if (_updaters.Contains(controller))
                _updatersMonoControllersRemovableStack.Push(controller);
            if (_fixedUpdaters.Contains(controller))
                _fixedUpdatersMonoControllersRemovableStack.Push(controller);
        }

        public void Update()
        {
            while (_updatersMonoControllersAdditionalStack.Count > 0)
            {
                var controller = _updatersMonoControllersAdditionalStack.Pop();
                _updaters.Add(controller);
                controller.DisableEvent += OnControllerDisable;
            }
            
            while (_updatersAdditionalStack.Count > 0)
            {
                var controller = _updatersAdditionalStack.Pop();
                _updaters.Add(controller);
            }

            while (_updatersMonoControllersRemovableStack.Count > 0)
            {
                var controller = _updatersMonoControllersRemovableStack.Pop();
                _updaters.Remove(controller);
            }
            
            while (_updatersRemovableStack.Count > 0)
            {
                var controller = _updatersRemovableStack.Pop();
                _updaters.Remove(controller);
            }
            
            foreach (var updater in _updaters)
            {
                updater.OnUpdate();
            }
        }

        public void FixedUpdate()
        {
            while (_fixedUpdatersMonoControllersAdditionalStack.Count > 0)
            {
                var controller = _fixedUpdatersMonoControllersAdditionalStack.Pop();
                _fixedUpdaters.Add(controller);
                controller.DisableEvent += OnControllerDisable;
            }

            while (_fixedUpdatersAdditionalStack.Count > 0)
            {
                var controller = _fixedUpdatersAdditionalStack.Pop();
                _fixedUpdaters.Add(controller);
            }
            
            while (_fixedUpdatersMonoControllersRemovableStack.Count > 0)
            {
                var controller = _fixedUpdatersMonoControllersRemovableStack.Pop();
                _fixedUpdaters.Remove(controller);
            }
            
            while (_fixedUpdatersRemovableStack.Count > 0)
            {
                var controller = _fixedUpdatersRemovableStack.Pop();
                _fixedUpdaters.Remove(controller);
            }
            
            foreach (var fixedUpdater in _fixedUpdaters)
            {
                fixedUpdater.OnFixedUpdate();
            }
        }
    }
}