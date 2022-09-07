using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace CommonControls
{
    public class FirstRowToRedForegroundConverter : IValueConverter
    {
        public Brush DefaultColor { get; set; } = Brushes.Black;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DonHang dh)
            {
                if (dh.TGBatDau != DateTime.MinValue ||
                    dh.HoanTatCutter != 0 ||
                    dh.HoanTatMayMen != 0 ||
                    dh.HoanTatSongB != 0 ||
                    dh.HoanTatGiayMatB != 0 ||
                    dh.HoanTatGiayMatE != 0 ||
                    dh.HoanTatGiayMatC != 0 ||
                    dh.HoanTatGiaySongB != 0 ||
                    dh.HoanTatGiaySongE != 0 ||
                    dh.HoanTatGiaySongC != 0 ||
                    dh.HoanTatSongC != 0 ||
                    dh.HoanTatSongE != 0 ||
                    dh.HoanTatSpliter != 0)
                    return Brushes.Red;
            }
            return DefaultColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
