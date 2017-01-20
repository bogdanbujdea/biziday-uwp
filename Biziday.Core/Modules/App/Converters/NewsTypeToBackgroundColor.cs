using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Biziday.Core.Modules.News.Models;

namespace Biziday.Core.Modules.App.Converters
{
    public class NewsTypeToBackgroundColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            NewsType newsType = (NewsType)value;
            switch (newsType)
            {
                case NewsType.Normal:
                    return new SolidColorBrush(Colors.White);
                case NewsType.Warning:
                    return new SolidColorBrush(Color.FromArgb(255, 241, 219, 141));
                case NewsType.Alert:
                    return new SolidColorBrush(Color.FromArgb(255, 255, 59, 47));
                default:
                    return new SolidColorBrush(Colors.White);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}