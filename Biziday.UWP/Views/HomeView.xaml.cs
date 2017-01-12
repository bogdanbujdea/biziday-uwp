using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Biziday.UWP.ViewModels;

namespace Biziday.UWP.Views
{
    public sealed partial class HomeView
    {
        public HomeView()
        {
            InitializeComponent();
            Loaded += HomeView_Loaded;            
        }

        private void HomeView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
            if (ViewModel.LocationIsSelected == true)
            {
                Grid.SetRowSpan(DetailsView, 2);
                Grid.SetRow(DetailsView, 0);
            }
        }

        public HomeViewModel ViewModel => DataContext as HomeViewModel;
    }
}