using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ex04_wpf_bikeshop
{
    internal class TwiceConverter : IValueConverter // 인터페이스를 상속하고 구현하지 않으면 빨간줄이 그어짐
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return double.Parse(value.ToString())*3;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null; 
                //int.Parse(value.ToString())*3;
        }
    }
}
