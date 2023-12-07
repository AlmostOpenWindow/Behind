using Components.CharacterControllers;
using Components.Common;
using Configs;
using Configs.Units;
using Infrastructure.Services.Input;
using StarterAssets;
using UnityEngine;

namespace Components.Units
{
    public class Hero : PausableMonoController, IUnit
    {
        [SerializeField] private UniversalPersonController _universalPersonController;
        [SerializeField] private Transform _cameraRoot;
        public Vector3 Position => transform.position;
        public Transform CameraRoot => _cameraRoot;

        private ConfigID<HeroConfig> _heroConfig;
        
        public void Construct(ConfigID<HeroConfig> heroConfig, IInputService input, GameplayCamera.GameplayCamera gameplayCamera)
        {
            _heroConfig = heroConfig;
            _universalPersonController.Construct(input, gameplayCamera);
        }
        
        public void DoPause()
        {
            Pause();
        }

        public void DoResume()
        {
            Resume();
        }
    }
}