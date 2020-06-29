using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WhereIsThePiko.Utility.Converters
{
    class TwoBoolsToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool wasVisited = (bool)values[0];
            bool isFinalPath = (bool)values[1];

            if(wasVisited && isFinalPath)
            {
                return new SolidColorBrush(Color.FromRgb(0, 180, 180));
            }
            else if (wasVisited)
            {
                return new SolidColorBrush(Color.FromRgb(139, 0, 139));
            }

            return new SolidColorBrush(Color.FromRgb(211, 211, 0));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
