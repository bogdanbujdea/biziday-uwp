namespace Biziday.UWP.Repositories
{
    public interface ISettingsRepository
    {
        T GetData<T>(SettingsKey key);
        void SetData(SettingsKey key, object value);
    }
}