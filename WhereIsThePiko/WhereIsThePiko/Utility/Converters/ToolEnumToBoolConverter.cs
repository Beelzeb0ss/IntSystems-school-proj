using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WhereIsThePiko.Utility.Converters
{
    class ToolEnumToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return false;
            }
            string enumVal = value.ToString();
            string RBVal = parameter.ToString();
            return enumVal.Equals(RBVal, StringComparison.InvariantCultureIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return null;
            }

            bool useValue = (bool)value;
            string targetValue = parameter.ToString();
            if (useValue)
            {
                return Enum.Parse(targetType, targetValue);
            }

            return null;
        }
    }
}
