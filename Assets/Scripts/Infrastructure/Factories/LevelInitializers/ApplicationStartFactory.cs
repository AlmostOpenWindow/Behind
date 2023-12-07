using Infrastructure.Containers.UnityApi;
using Infrastructure.Services.Input;
using Services.Debug;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure.Factories.LevelInitializers
{
    public class ApplicationStartFactory : BaseFactory, ILevelInitializedFactory, IFactory
    {
        private readonly IFrameUpdater _frameUpdater;
        
        private string UnityApiContainerPrefabPath 
            => "Prefabs/Common/UnityApiContainer";

        private string PlayerInputPrefabPath 
            => "Prefabs/Common/PlayerInput";
        
        public ApplicationStartFactory(IFrameUpdater frameUpdater)
        {
            _frameUpdater = frameUpdater;
        }
        
        public void SpawnInfrastructurePrefabs()
        {
            SpawnUnityApiBehavior();
        }

        public IInputService SpawnPlayerInput()
        {
            var playerInputPrefab = Load(PlayerInputPrefabPath);

            var playerInputObject = GameObject.FindGameObjectWithTag(playerInputPrefab.tag);
            if (playerInputObject == null)
            {
                playerInputObject = Object.Instantiate(playerInputPrefab);
                Object.DontDestroyOnLoad(playerInputObject);
            }

            var input = playerInputObject.GetComponent<StarterAssetsInputs>();
            PlayerInput playerInput = null;
#if ENABLE_INPUT_SYSTEM
            playerInput = playerInputObject.GetComponent<PlayerInput>();
#endif
            var inputService = new UnityInputService(input, playerInput);
            return inputService;
        }
        
        private void SpawnUnityApiBehavior()
        {
            var unityApiPrefab = Load(UnityApiContainerPrefabPath);
            if (GameObject.FindGameObjectWithTag(unityApiPrefab.tag) != null)
                return;
            
            var unityApiObject = Object.Instantiate(unityApiPrefab);
            Object.DontDestroyOnLoad(unityApiObject);

            var unityApiBehavior = unityApiObject.GetComponent<UnityApiBehavior>();
            unityApiBehavior.Construct(_frameUpdater);
        }
    }
}