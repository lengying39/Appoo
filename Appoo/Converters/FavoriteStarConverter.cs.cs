using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Appoo.Converters
{
    public class FavoriteStarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is bool isFavorite)
                    return isFavorite ? "★" : "☆";
                return "☆";
            }
            catch
            {
                return "☆";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}