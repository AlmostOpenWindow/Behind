using JetBrains.Annotations;
using Services.Debug;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure.Services.Input
{
    public class UnityInputService : IInputService
    {
        private readonly StarterAssetsInputs _assetsInputs;
#if ENABLE_INPUT_SYSTEM
        public PlayerInput PlayerInput { get; }
#endif
        public InputData Data { get
        {
            if (_assetsInputs == null)
            {
                Debug.LogWarning("StarterAssetsInput missing data");
                return new InputData();
            }

            return _assetsInputs.Data;
        }}
        
        public UnityInputService(StarterAssetsInputs starterAssetsInputs, [CanBeNull]PlayerInput playerInput)
        {
            _assetsInputs = starterAssetsInputs;
#if ENABLE_INPUT_SYSTEM
            PlayerInput = playerInput;
#endif
        }
    }
}