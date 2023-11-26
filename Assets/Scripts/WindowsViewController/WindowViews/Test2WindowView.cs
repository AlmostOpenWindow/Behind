using Windows.WindowViews.Base;
using UnityEngine;
using UnityEngine.UI;

namespace WindowsViewController.WindowViews
{
    public class Test2WindowView : WindowView, IWindowView
    {
        [SerializeField] private Image _image;

        public void SetColor(Color color)
        {
            _image.color = color;
        }
    }
}
