using System.Collections.Generic;
using Components.Common;

namespace Infrastructure.Mechanics.Routines
{
    public class RoutineMechanics : IRoutineMechanics, IUpdater
    {
        private List<Routine> _routines = new();
        private Stack<Routine> _routinesAdditional = new();
        private Stack<Routine> _routinesRemovable = new();
        
        public bool IsEnable { get; private set; }
        public void Enable()
        {
            IsEnable = true;
        }

        public void Disable()
        {
            IsEnable = false;
        }

        public void OnUpdate()
        {
            if (!IsEnable)
                return;

            while (_routinesAdditional.Count > 0)
            {
                _routines.Add(_routinesAdditional.Pop());
            }

            while (_routinesRemovable.Count > 0)
            {
                _routines.Remove(_routinesRemovable.Pop());
            }

            foreach (var routine in _routines)
            {
                var completed = routine.OnUpdate();
                if (completed)
                    RemoveRoutine(routine);
            }

        }

        public void AddRoutine(Routine routine)
        {
            _routinesAdditional.Push(routine);
            
        }

        public void RemoveRoutine(Routine routine)
        {
            _routinesRemovable.Push(routine);
        }
    }
}