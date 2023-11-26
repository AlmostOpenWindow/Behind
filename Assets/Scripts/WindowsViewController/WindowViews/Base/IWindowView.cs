using System;
using UnityEngine;

namespace Windows.WindowViews.Base
{
    public interface IWindowView
    {
        GameObject WindowInstance { get; }
        event Action CloseEvent;
        void Close();
    }
}