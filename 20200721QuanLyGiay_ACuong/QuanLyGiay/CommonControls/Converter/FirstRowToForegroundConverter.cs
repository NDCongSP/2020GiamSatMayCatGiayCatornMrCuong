using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CommonControls
{
    class FirstRowToForegroundConverter : IValueConverter
    {
        public Brush DefaultColor { get; set; } = Brushes.Black;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DonHang dh)
            {
                if (dh.FirstRow != 0)
                    return DefaultColor;
            }
            return DefaultColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
