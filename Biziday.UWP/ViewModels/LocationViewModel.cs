using Biziday.UWP.Repositories;

namespace Biziday.UWP.ViewModels
{
    public class LocationViewModel: ViewModelBase
    {
        private readonly ISettingsRepository _settingsRepository;

        public LocationViewModel(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }
    }
}
