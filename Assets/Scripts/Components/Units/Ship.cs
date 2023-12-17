using Components.CharacterControllers;
using Configs;
using Configs.Units;
using DG.Tweening;
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

        [SerializeField] 
        private float _initRotationSmoothTime = 0.5f;
        
        public Vector3 Position => transform.position;
        
        private ConfigID<ShipConfig> _shipConfig;
        private Quaternion _rotationVelocity;
        private float _currentRotationTime;

        private Sequence _initRotationSeq;
        
        public void Construct(ConfigID<ShipConfig> shipConfig, IInputService input, GameplayCamera.GameplayCamera gameplayCamera)
        {
            _shipConfig = shipConfig;
            _shipController.Construct(input, gameplayCamera);
            _flashlightController.Construct(input);
        }

        protected override void OnMountStateChanged(bool mounted)
        {
            if (!mounted)
            {
                SetInitialRotation();
            }
            else
            {
                if (_initRotationSeq != null) 
                    _initRotationSeq.Kill();
            }
        }

        private void SetInitialRotation()
        {
            var newEulerRotation = new Vector3(0.0f, transform.rotation.eulerAngles.y, 0.0f);

            _initRotationSeq = DOTween.Sequence()
                .Append(transform.DORotate(newEulerRotation, _initRotationSmoothTime));

            _initRotationSeq.Play();
        }
    }
}