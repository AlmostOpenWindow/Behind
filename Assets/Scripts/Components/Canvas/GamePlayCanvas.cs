using UnityEngine;

namespace Components.Canvas
{
    public class GamePlayCanvas : MonoBehaviour
    {
        [SerializeField] private RectTransform _windowsContainer;

        public RectTransform WindowsContainer => _windowsContainer;
    }
}