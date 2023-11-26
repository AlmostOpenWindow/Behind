using Windows.WindowControllers.Base;
using UnityEngine;
using WindowsViewController.WindowViews;

namespace WindowsViewController.WindowControllers
{
    public class Test2WindowController : WindowController<Test2WindowView>, IWindowController
    {
        protected override void DoOpen(Test2WindowView view)
        {
            view.SetColor(Color.black);
        }

        protected override void DoClose()
        {
        
        }
    }
}
