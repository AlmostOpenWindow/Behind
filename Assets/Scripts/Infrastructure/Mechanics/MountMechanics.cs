using Components.Common;
using Components.Units;
using Configs.Mechanics;
using Infrastructure.Containers.PoolObjectsContainer;
using Infrastructure.Mechanics.Interact;
using Infrastructure.Services.Input;
using UnityEngine;

namespace Infrastructure.Mechanics
{
    public class MountMechanics : IMountMechanics
    {
        private readonly MountMechanicsConfig _config;
        private readonly IInteractMechanics _interactMechanics;
        private readonly IInputService _inputService;
        private readonly IUnitsContainer _unitsContainer;
        private readonly SceneData _sceneData;

        private Mountable _currentMount;
        
        public bool IsEnable { get; private set; }

        public MountMechanics(
            MountMechanicsConfig config, 
            IInteractMechanics interactMechanics,
            IInputService inputService, 
            IUnitsContainer unitsContainer, 
            SceneData sceneData)
        {
            _config = config;
            _interactMechanics = interactMechanics;
            _inputService = inputService;
            _unitsContainer = unitsContainer;
            _sceneData = sceneData;
        }
        
        public void Enable()
        {
            IsEnable = true;
            _inputService.Data.InteractionClickEvent += OnInteractionClicked;
        }

        public void Disable()
        {
            IsEnable = false;
            _inputService.Data.InteractionClickEvent -= OnInteractionClicked;
        }
        
        private void OnInteractionClicked()
        {
            if (_currentMount == null)
                PerformMount();
            else
                PerformDismount();
        }

        private void PerformDismount()
        {
            if (_currentMount == null)
                return;
            
            var hero = _unitsContainer.Hero;
            
            var hTrans = hero.transform;
            hTrans.SetParent(null);
            hTrans.rotation = Quaternion.Euler(0.0f, hTrans.rotation.eulerAngles.y, 0.0f);
            hero.gameObject.SetActive(true);
            hero.DoResume();
            _currentMount.SetMountState(false);
            _currentMount.Pause();

            SetMountCameraState(false, _currentMount);

            _currentMount = null;
        }
        
        private void PerformMount()
        {
            if (TryGetMountable(out var mountable))
                Mount(mountable);
        }
        
        private bool TryGetMountable(out Mountable mountable)
        {
            mountable = null;
            var cameraT = _sceneData.GameplayCamera.transform;
            
            if (!_interactMechanics.TryInteract(
                    cameraT.position, 
                    cameraT.forward, 
                    out var interacted)) 
                return false;
            
            mountable = interacted as Mountable;
            return mountable != null;
        }
        
        private void Mount(Mountable mount)
        {
            if (mount.Mounted)
                return;
            
            _currentMount = mount;
            
            mount.SetMountState(true);
            
            var hero = _unitsContainer.Hero;
            var heroParent = mount.DismountPoint;
            
            hero.Pause();
            
            var heroT = hero.transform;
            heroT.SetParent(heroParent);
            heroT.localPosition = Vector3.zero;
            hero.gameObject.SetActive(false);
            
            mount.Resume();
            
            SetMountCameraState(true, mount);
        }
        
        private void SetMountCameraState(bool state, Mountable mount)
        {
            _sceneData.ShipFollowCamera.Follow = mount.CameraRoot;
            _sceneData.ShipFollowCamera.gameObject.SetActive(state);
        }
    }
}