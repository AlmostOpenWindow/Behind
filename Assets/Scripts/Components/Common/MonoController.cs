using System.Collections.Generic;
using Doom.GamePlay.Components.Common;
using SerializeReferenceEditor;
using UnityEngine;

namespace Components.Common
{
    public class MonoController : EnabledMonoBehavior, IUpdater, IFixedUpdater, ILateUpdater
    {
        public List<IUpdater> _updaters = new List<IUpdater>();
        public List<IFixedUpdater> _fixedUpdaters = new List<IFixedUpdater>();
        public List<ILateUpdater> _lateUpdaters = new List<ILateUpdater>();
        
        [SR(typeof(IUpdaterLinker))]
        [SerializeReference]
        public List<IUpdaterLinker> _updatersLinks = new List<IUpdaterLinker>();
        
        [SR(typeof(IFixedUpdaterLinker))]
        [SerializeReference]
        public List<IFixedUpdaterLinker> _fixedUpdatersLinks = new List<IFixedUpdaterLinker>();
        
        [SR(typeof(ILateUpdaterLinker))]
        [SerializeReference]
        public List<ILateUpdaterLinker> _lateUpdatersLinks = new List<ILateUpdaterLinker>();
        
        public void InitializedUnityApi()
        {
            FindIUpdater();
            FindIFixedUpdater();
            FindILateUpdater();
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
            foreach (var lateUpdater in _lateUpdatersLinks)
            {
                if (!_lateUpdaters.Contains(lateUpdater))
                    _lateUpdaters.Add(lateUpdater);
            }
        }

        public virtual void OnUpdate()
        {
            foreach (var updater in _updaters)
                updater.OnUpdate();
        }

        public virtual void OnFixedUpdate()
        {
            foreach (var fixedUpdater in _fixedUpdaters) 
                fixedUpdater.OnFixedUpdate();
        }

        public virtual void OnLateUpdate()
        {
            foreach (var lateUpdater in _lateUpdaters)
                lateUpdater.OnLateUpdate();
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

        private void FindILateUpdater()
        {
            var lateUpdaters = GetComponents<ILateUpdater>();
            foreach (var lateUpdater in lateUpdaters)
            {
                if (lateUpdater is MonoController monoController 
                    && monoController == this)
                    continue;
                
                if (!_lateUpdaters.Contains(lateUpdater))
                    _lateUpdaters.Add(lateUpdater);
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