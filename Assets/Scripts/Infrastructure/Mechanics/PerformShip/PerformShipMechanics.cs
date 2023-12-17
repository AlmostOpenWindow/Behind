using Components.Common;
using Components.Interactions;
using Configs.Mechanics;
using Infrastructure.Factories.Units;
using Infrastructure.Mechanics.Interact;
using Infrastructure.Mechanics.Routines;
using Infrastructure.Services.Input;

namespace Infrastructure.Mechanics.PerformShip
{
    public class PerformShipMechanics : IPerformShipMechanics
    {
        private readonly SceneData _sceneData;
        private readonly PerformShipMechanicsConfig _config;
        private readonly IInteractMechanics _interactMechanics;
        private readonly IRoutineMechanics _routineMechanics;
        private readonly IUnitFactory _unitFactory;
        private readonly IInputService _inputService;

        private PerformShipButton _performShipButton;
        
        public PerformShipMechanics(
            PerformShipMechanicsConfig config,
            IInteractMechanics interactMechanics,
            IRoutineMechanics routineMechanics, 
            IUnitFactory unitFactory, 
            IInputService inputService,
            SceneData sceneData)
        {
            _sceneData = sceneData;
            _config = config;
            _interactMechanics = interactMechanics;
            _routineMechanics = routineMechanics;
            _unitFactory = unitFactory;
            _inputService = inputService;
        }
        
        public bool IsEnable { get; private set; }
        
        public void Enable()
        {
            IsEnable = true;
            _inputService.Data.InteractionClickEvent += OnInteractClicked;
        }

        public void Disable()
        {
            IsEnable = false;
            _inputService.Data.InteractionClickEvent -= OnInteractClicked;
        }

        private void OnInteractClicked()
        {
            var cameraT = _sceneData.GameplayCamera.transform;
            if (!_interactMechanics.TryInteract(
                    cameraT.position, 
                    cameraT.forward, 
                    out var interacted))
                return;

            _performShipButton = interacted as PerformShipButton;
            if (_performShipButton != null)
            {
                SpawnShip();
                StartLightsRoutine();
            }
        }
        
        private void StartLightsRoutine()
        {
            var routine = new ActivateGoRoutine(
                _performShipButton.Spotlights.ToArray(), 
                _config.EnableDelay, 
                _config.EnableTimeGap, 
                true, 
                OnLightsRoutineCompleted);
            
            _routineMechanics.AddRoutine(routine);
        }

        private void OnLightsRoutineCompleted()
        {
            _performShipButton.ButtonSpotlight.SetActive(false);
            _performShipButton.gameObject.SetActive(false);
        }
        
        private void SpawnShip()
        {
            _unitFactory.SpawnShip(false);
        }
    }
}