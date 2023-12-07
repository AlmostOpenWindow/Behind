using Components.Common;
using Configs.WindowConfigs;
using Infrastructure.Tasks;
using UnityEngine;

namespace Infrastructure.GameEntryPoint
{
    public class EntryPointMonoBehaviour : MonoBehaviour
    {
        [SerializeField]
        private SceneData _sceneData;

        [SerializeField] private BaseWindowConfig _test2WindowConfig;
        
        
        private ApplicationContainer _applicationContainer;
        private TaskMachine _taskMachine;
        
        private void Awake()
        {
            _applicationContainer = new ApplicationContainer(_sceneData);
            _taskMachine = new TaskMachine(_applicationContainer);
            _taskMachine.Start();

            if (_test2WindowConfig == null)
                return;
            
            var opened = _test2WindowConfig.TryOpen(_applicationContainer);
            Debug.Log("Window opened from EntryPoint");
        }
    }
}