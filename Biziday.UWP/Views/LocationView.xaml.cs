using Windows.UI.Core;

namespace Biziday.UWP.Views
{
    public sealed partial class LocationView
    {
        public LocationView()
        {
            InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }
    }
}