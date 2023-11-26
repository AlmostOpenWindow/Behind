using System;
using Windows.WindowViews.Base;

namespace Windows.WindowControllers.Base
{
    public interface IWindowController
    {
        IWindowView View { get; }
    }
    public interface IWindowController<in TView> : IWindowController where TView : IWindowView
    {
        void Close();
        void Open(TView view, Action closeCallback);
    }
}