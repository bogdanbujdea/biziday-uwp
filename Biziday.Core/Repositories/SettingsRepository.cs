using System;
using Windows.Storage;

namespace Biziday.Core.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        public T GetLocalData<T>(SettingsKey key)
        {
            try
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key.ToString()))
                    return (T)ApplicationData.Current.LocalSettings.Values[key.ToString()];
                return default(T);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public void SetLocalData(SettingsKey key, object value)
        {
            ApplicationData.Current.LocalSettings.Values[key.ToString()] = value;
        }

        public T GetRoamningData<T>(SettingsKey key)
        {
            try
            {
                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(key.ToString()))
                {
                    var value = (T)ApplicationData.Current.RoamingSettings.Values[key.ToString()];
                    return value;
                }
                return default(T);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public void SetRoamningData(SettingsKey key, object value)
        {
            ApplicationData.Current.RoamingSettings.Values[key.ToString()] = value;
        }
    }
}