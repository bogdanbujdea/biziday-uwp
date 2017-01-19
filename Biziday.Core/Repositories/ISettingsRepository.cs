namespace Biziday.Core.Repositories
{
    public interface ISettingsRepository
    {
        T GetLocalData<T>(SettingsKey key);

        void SetLocalData(SettingsKey key, object value);

        T GetRoamningData<T>(SettingsKey key);

        void SetRoamningData(SettingsKey key, object value);
    }
}