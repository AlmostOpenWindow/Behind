using Cinemachine;
using Components.Canvas;
using UnityEngine;

namespace Components.Common
{
    public class SceneData : MonoBehaviour
    {
        [SerializeField]
        private GamePlayCanvas _gamePlayCanvas;

        [SerializeField] 
        private Transform _shipInitialPoint;
        
        [SerializeField] 
        private Transform _heroInitialPoint;

        [SerializeField] 
        private GameplayCamera.GameplayCamera _gameplayCamera;

        [SerializeField] 
        private CinemachineVirtualCamera _shipFollowCamera;
        
        [SerializeField] 
        private CinemachineVirtualCamera _heroFollowCamera;

        public GameplayCamera.GameplayCamera GameplayCamera => _gameplayCamera;
        public GamePlayCanvas GamePlayCanvas => _gamePlayCanvas;
        public Transform ShipInitialPoint => _shipInitialPoint;
        public Transform HeroInitialPoint => _heroInitialPoint;
        public CinemachineVirtualCamera ShipFollowCamera => _shipFollowCamera;
        public CinemachineVirtualCamera HeroFollowCamera => _heroFollowCamera;
        
    }
}