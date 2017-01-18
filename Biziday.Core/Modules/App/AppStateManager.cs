using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Biziday.Core.Repositories;

namespace Biziday.Core.Modules.App
{
    public interface IAppStateManager
    {
        bool FirstTimeUse();
        bool LocationIsSelected();
        Task EnableBackgroundTask();
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
            return _settingsRepository.GetData<int>(SettingsKey.AreaId) != 0;
        }

        public async Task EnableBackgroundTask()
        {
            string name = "NewsTask";
            foreach (var backgroundTask in BackgroundTaskRegistration.AllTasks)
            {
                if (backgroundTask.Value.Name == name)
                {
                    return;
                }
            }
            string taskEntryPoint = "Biziday.NewsTask.NewsBackgroundTask";

            IBackgroundTrigger trigger = new TimeTrigger(15, false);

            BackgroundTaskBuilder builder = new BackgroundTaskBuilder
            {
                Name = name,
                TaskEntryPoint = taskEntryPoint
            };

            builder.SetTrigger(trigger);

            var access = await BackgroundExecutionManager.RequestAccessAsync();

            if (access == BackgroundAccessStatus.DeniedByUser || access == BackgroundAccessStatus.DeniedBySystemPolicy)
                return;
            builder.Register();            
        }
    }
}
