using System;
using Windows.WindowViews.Base;
using UnityEngine;
using UnityEngine.UI;

namespace WindowsViewController.WindowViews
{
    public class TestWindowView : WindowView, IWindowView
    {
        [SerializeField]
        private Text _text;
        
        protected override void DoOpen()
        {
            base.DoOpen();
            
        }

        protected override void DoClose()
        {
            base.DoClose();
            
        }

        public void SetTestData(string testData)
        {
            _text.text = testData;
        }
    }
}
