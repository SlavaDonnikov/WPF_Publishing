using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Publishing
{
    public class BinaryImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                byte[] byteArray = value as byte[];
                if (byteArray == null)
                    return null;

                BitmapImage image = new BitmapImage();
                using (System.IO.MemoryStream imageStream = new System.IO.MemoryStream())
                {
                    imageStream.Write(byteArray, 0, byteArray.Length);

                    imageStream.Seek(0, System.IO.SeekOrigin.Begin);

                    image.BeginInit();

                    image.CacheOption = BitmapCacheOption.OnLoad;

                    image.StreamSource = imageStream;

                    image.EndInit();

                    image.Freeze();
                }
                return image;
            }
            return null;
        }        

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }        
    }
}
