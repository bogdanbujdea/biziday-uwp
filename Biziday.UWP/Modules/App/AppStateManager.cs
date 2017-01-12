using System;
using Biziday.UWP.Repositories;

namespace Biziday.UWP.Modules.App
{
    public interface IAppStateManager
    {
        bool FirstTimeUse();
        bool LocationIsSelected();
    }

    public class AppStateManager : IAppStateManager
    {
        private readonly ISettingsRepository _settingsRepository;

        public AppStateManager(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public bool FirstTimeUse()
        {
            var userId = _settingsRepository.GetData<int>(SettingsKey.UserId);
            return userId == 0;
        }

        public bool LocationIsSelected()
        {
            return _settingsRepository.GetData<bool>(SettingsKey.LocationIsSelected);
        }
    }
}
