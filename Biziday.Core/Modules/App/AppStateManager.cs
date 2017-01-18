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

            //
            // Must be the same entry point that is specified in the manifest.
            //
            string taskEntryPoint = "Biziday.NewsTask.NewsBackgroundTask";

            //
            // A time trigger that repeats at 15-minute intervals.
            //
            IBackgroundTrigger trigger = new TimeTrigger(15, false);

            //
            // Builds the background task.
            //
            BackgroundTaskBuilder builder = new BackgroundTaskBuilder
            {
                Name = name,
                TaskEntryPoint = taskEntryPoint
            };

            builder.SetTrigger(trigger);

            //
            // Registers the background task, and get back a BackgroundTaskRegistration object representing the registered task.
            //
            var access = await BackgroundExecutionManager.RequestAccessAsync();

            //abort if access isn't granted
            if (access == BackgroundAccessStatus.DeniedByUser || access == BackgroundAccessStatus.DeniedBySystemPolicy)
                return;
            BackgroundTaskRegistration task = builder.Register();
            task.Completed += Task_Completed;            
        }

        private void Task_Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
        }
    }
}
