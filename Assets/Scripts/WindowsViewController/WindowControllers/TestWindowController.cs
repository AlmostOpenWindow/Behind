using Windows.WindowControllers.Base;
using Services.Debug;
using WindowsViewController.WindowViews;

namespace WindowsViewController.WindowControllers
{
    public class TestWindowController : WindowController<TestWindowView>
    {
        private readonly ILogService _logService;

        public TestWindowController(ILogService logService)
        {
            _logService = logService;
        }
        
        protected override void DoOpen(TestWindowView view)
        {
            _logService.Log("Test window OPENED");
            view.SetTestData("TEST DATA");
        }

        protected override void DoClose()
        {
            _logService.Log("Test window CLOSED");
        }
    }
}