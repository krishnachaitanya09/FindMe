using Microsoft.Phone.Shell;
using System;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GPSTracker.Helpers
{
    public class UIHelper
    {
        //shell--frame--phone application page
        private ProgressIndicator ProgressIndicator = null;

        public void ShowProgressIndicator(String message, DependencyObject element)
        {
            if (ProgressIndicator == null)
            {
                ProgressIndicator = new ProgressIndicator();
                ProgressIndicator.IsIndeterminate = true;
            }
            ProgressIndicator.Text = message;
            ProgressIndicator.IsVisible = true;
            SystemTray.SetProgressIndicator(element, ProgressIndicator);
        }

        public void HideProgressIndicator(DependencyObject element)
        {
            ProgressIndicator.IsVisible = false;
            SystemTray.SetProgressIndicator(element, ProgressIndicator);
        }
    }

    public class BytesToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is byte[])
            {
                byte[] bytes = value as byte[];
                MemoryStream stream = new MemoryStream(bytes);
                BitmapImage image = new BitmapImage();
                image.CreateOptions = BitmapCreateOptions.BackgroundCreation;
                image.SetSource(stream);
                stream.Close();
                GC.Collect();
                return image;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
