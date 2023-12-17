using Cinemachine;
using Components.Canvas;
using UnityEngine;

namespace Components.Common
{
    public enum StartSceneType
    {
        StartFromPlayer,
        StartFromShip
    }
    
    public class SceneData : MonoBehaviour
    {
        [SerializeField] 
        private StartSceneType _startSceneType;
        
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
        public StartSceneType StartSceneType => _startSceneType;
    }
}