using System;
using Windows.Storage;

namespace Biziday.UWP.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        public T GetData<T>(SettingsKey key)
        {
            try
            {
                return (T)ApplicationData.Current.LocalSettings.Values[key.ToString()];
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public void SetData(SettingsKey key, object value)
        {
            ApplicationData.Current.LocalSettings.Values[key.ToString()] = value;
        }
    }
}