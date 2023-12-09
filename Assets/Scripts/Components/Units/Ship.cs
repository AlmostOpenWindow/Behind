using Components.CharacterControllers;
using Configs;
using Configs.Units;
using Infrastructure.Services.Input;
using UnityEngine;

namespace Components.Units
{
    public class Ship : Mountable, IUnit
    {
        [SerializeField]
        private ShipController _shipController;
        
        [SerializeField] 
        private FlashlightController _flashlightController;
        
        public Vector3 Position => transform.position;
        
        private ConfigID<ShipConfig> _shipConfig;

        public void Construct(ConfigID<ShipConfig> shipConfig, IInputService input, GameplayCamera.GameplayCamera gameplayCamera)
        {
            _shipConfig = shipConfig;
            _shipController.Construct(input, gameplayCamera);
            _flashlightController.Construct(input);
        }
    }
}