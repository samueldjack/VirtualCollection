using System;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VirtualCollection.Demo
{
    public class StringToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var url = value as string;
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            try
            {
                var uri = new Uri(url);
                var bitmap = new BitmapImage(uri) { CreateOptions = BitmapCreateOptions.DelayCreation};
                return bitmap;
            }
            catch (UriFormatException)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
