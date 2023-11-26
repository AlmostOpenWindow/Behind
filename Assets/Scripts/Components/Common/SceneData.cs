using Components.Canvas;
using UnityEngine;

namespace Components.Common
{
    public class SceneData : MonoBehaviour
    {
        [SerializeField]
        private GamePlayCanvas _gamePlayCanvas;

        public GamePlayCanvas GamePlayCanvas => _gamePlayCanvas;
    }
}