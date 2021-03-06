﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FakeIMDB_DesktopClient.Converters
{
    /// <summary>
    /// Converter for binding to substract 40
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    class WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double) value - 40;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
