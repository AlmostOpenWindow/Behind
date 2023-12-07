using Components.CharacterControllers;
using Components.Common;
using Configs;
using Configs.Units;
using Infrastructure.Services.Input;
using UnityEngine;

namespace Components.Units
{
    public class Ship : PausableMonoController, IUnit
    {
        [SerializeField] private ShipController _shipController;
        [SerializeField] private FlashlightController _flashlightController;
        [SerializeField] private Transform _cameraRoot;
        public Vector3 Position => transform.position;
        public Transform CameraRoot => _cameraRoot;

        private ConfigID<ShipConfig> _shipConfig;
        
        public void Construct(ConfigID<ShipConfig> shipConfig, IInputService input, GameplayCamera.GameplayCamera gameplayCamera)
        {
            _shipConfig = shipConfig;
            _shipController.Construct(input, gameplayCamera);
            _flashlightController.Construct(input);
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