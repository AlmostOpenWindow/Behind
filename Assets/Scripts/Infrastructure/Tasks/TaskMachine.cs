using System.Collections.Generic;
using Infrastructure.GameEntryPoint;

namespace Infrastructure.Tasks
{
    public class TaskMachine
    {
        private readonly List<ITask> _tasks = new List<ITask>();
        private int _activeTask;
        
        public TaskMachine(
            ApplicationContainer applicationContainer)
        {
            _tasks.Add(new InitializedApplicationTask(applicationContainer));
            _tasks.Add(new LoadLevelTask(applicationContainer));
        }

        public void Start()
        {
            _activeTask = 0;
            StartActiveTask();
        }

        private void NextTask()
        {
            StopActiveTask();
            _activeTask++;
            if(_activeTask >= _tasks.Count)
                return;
            StartActiveTask();
        }

        private void StartActiveTask()
        {
            _tasks[_activeTask].FinishedTask += NextTask;
            _tasks[_activeTask].Init();
        }

        private void StopActiveTask()
        {
            _tasks[_activeTask].FinishedTask -= NextTask;
            _tasks[_activeTask].Destroy();
        }
    }
}