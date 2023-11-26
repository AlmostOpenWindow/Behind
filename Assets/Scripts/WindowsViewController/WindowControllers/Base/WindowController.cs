using System;
using Windows.WindowViews.Base;

namespace Windows.WindowControllers.Base
{
    public abstract class WindowController<TView> : IWindowController<TView> where TView : IWindowView
    {
        public IWindowView View => _view;
        
        protected TView _view { get; private set; }
        
        private Action _closeCallback;
        
        public void Close()
        {
            if (View == null)
                return;

            View.CloseEvent -= Close;
            DoClose();
            _closeCallback?.Invoke();
            _closeCallback = null;
            _view = default;
        }

        public void Open(TView view, Action closeCallback)
        {
            if (view == null)
                return;
            
            _view = view;
            View.CloseEvent += Close;
            _closeCallback = closeCallback;
            DoOpen(view);
        }

        protected abstract void DoOpen(TView view);
        protected abstract void DoClose();
    }
}