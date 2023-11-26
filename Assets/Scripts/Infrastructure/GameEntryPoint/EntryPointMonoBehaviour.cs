using Components.Common;
using GameEntryPoint;
using Infrastructure.Tasks;
using UnityEngine;

namespace Infrastructure.GameEntryPoint
{
    public class EntryPointMonoBehaviour : MonoBehaviour
    {
        [SerializeField]
        private SceneData _sceneData;
        
        private ApplicationContainer _applicationContainer;
        private TaskMachine _taskMachine;
        
        private void Awake()
        {
            _applicationContainer = new ApplicationContainer(_sceneData);
            _taskMachine = new TaskMachine(_applicationContainer);
            _taskMachine.Start();
        }
    }
}