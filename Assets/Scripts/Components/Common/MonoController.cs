using System.Collections.Generic;
using Doom.GamePlay.Components.Common;
using SerializeReferenceEditor;
using UnityEngine;

namespace Components.Common
{
    public class MonoController : EnabledMonoBehavior, IUpdater, IFixedUpdater
    {
        public List<IUpdater> _updaters = new List<IUpdater>();
        public List<IFixedUpdater> _fixedUpdaters = new List<IFixedUpdater>();

        [SR(typeof(IUpdaterLinker))]
        [SerializeReference]
        public List<IUpdaterLinker> _updatersLinks = new List<IUpdaterLinker>();
        
        [SR(typeof(IFixedUpdaterLinker))]
        [SerializeReference]
        public List<IFixedUpdaterLinker> _fixedUpdatersLinks = new List<IFixedUpdaterLinker>();
        
        public void InitializedUnityApi()
        {
            FindIUpdater();
            FindIFixedUpdater();
            AttachSerializableUpdaters();
        }

        private void AttachSerializableUpdaters()
        {
            foreach (var updater in _updatersLinks)
            {
                if (!_updaters.Contains(updater))
                    _updaters.Add(updater);
            }
            foreach (var fixedUpdater in _fixedUpdatersLinks)
            {
                if (!_fixedUpdaters.Contains(fixedUpdater))
                    _fixedUpdaters.Add(fixedUpdater);
            }
        }

        public void OnUpdate()
        {
            foreach (var updater in _updaters)
                updater.OnUpdate();
        }

        public void OnFixedUpdate()
        {
            foreach (var fixedUpdater in _fixedUpdaters) 
                fixedUpdater.OnFixedUpdate();
        }

        private void FindIUpdater()
        {
            var updaters = GetComponents<IUpdater>();
            foreach (var updater in updaters)
            {
                if (updater is MonoController monoController 
                    && monoController == this)
                    continue;
                
                if (!_updaters.Contains(updater))
                    _updaters.Add(updater);
            }
        }

        private void FindIFixedUpdater()
        {
            var fixedUpdaters = GetComponents<IFixedUpdater>();
            foreach (var fixedUpdater in fixedUpdaters)
            {
                if (fixedUpdater is MonoController monoController 
                    && monoController == this)
                    continue;
                
                if (!_fixedUpdaters.Contains(fixedUpdater))
                    _fixedUpdaters.Add(fixedUpdater);
            }
        }
        
        protected void Reset()
        {
            _fixedUpdaters.Clear();
            _updaters.Clear();
        }

#if UNITY_EDITOR

        protected virtual void OnValidate()
        {
            
        }

#endif
    }
}