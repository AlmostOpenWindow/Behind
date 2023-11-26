using System;
using GameEntryPoint;

namespace Infrastructure.Tasks
{
    public class LoadLevelTask : ITask
    {
        public event Action FinishedTask;
        private readonly ApplicationContainer _applicationContainer;

        public LoadLevelTask(ApplicationContainer applicationContainer)
        {
            _applicationContainer = applicationContainer;
        }

        public void Init()
        {
            //don't forget enable mechanics after init _applicationContainer.MechanicContainer.Enable();
            FinishedTask?.Invoke();
        }

        public void Update()
        {
            
        }

        public void Destroy()
        {
            
        }
    }
}