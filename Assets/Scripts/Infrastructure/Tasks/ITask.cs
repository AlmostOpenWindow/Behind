using System;

namespace Infrastructure.Tasks
{
    public interface ITask
    {
        event Action FinishedTask;
        void Init();
        void Update();
        void Destroy();
    }
}