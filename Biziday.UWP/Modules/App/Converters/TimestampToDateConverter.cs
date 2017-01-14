using System;
using Windows.UI.Xaml.Data;

namespace Biziday.UWP.Modules.App.Converters
{
    public class TimestampToDateConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            long timestamp = (int)value;
            var date = timestamp.GetDate();
            return date.ToString("ddd, MMM dd, HH:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}