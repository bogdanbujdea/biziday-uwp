using System;
using Windows.UI.Xaml.Data;

namespace Biziday.Core.Modules.App.Converters
{
    public class UrlToWebsiteNameConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var link = new Uri(value as string);
            return link.Host.Replace("www.", "");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
