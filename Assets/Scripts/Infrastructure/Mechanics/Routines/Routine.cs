using System;

namespace Infrastructure.Mechanics.Routines
{
    public abstract class Routine
    {
        protected readonly Action _onComplete;
        
        public Routine(Action onComplete)
        {
            _onComplete = onComplete;
        }
        public abstract bool OnUpdate();
    }
}