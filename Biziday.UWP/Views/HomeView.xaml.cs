using Biziday.UWP.ViewModels;

namespace Biziday.UWP.Views
{
    public sealed partial class HomeView
    {
        public HomeView()
        {
            InitializeComponent();
        }

        public HomeViewModel ViewModel => DataContext as HomeViewModel;
    }
}