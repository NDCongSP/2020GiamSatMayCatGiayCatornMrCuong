using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MayCat
{
    /// <summary>
    /// Interaction logic for DonHangDangChayView.xaml
    /// </summary>
    public partial class DonHangDangChayView : UserControl
    {
        public DonHangDangChayView()
        {
            InitializeComponent();

            btnUp1.Click += BtnUp1_Click;
            btnDown1.Click += BtnDown1_Click;
            btnUp2.Click += BtnUp2_Click;
            btnDown2.Click += BtnDown2_Click;
        }

        private void BtnDown2_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new GiamKheHoLangMessage() { May = 2 });
        }

        private void BtnUp2_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new TangKheHoLangMessaage() { May = 2 });
        }

        private void BtnDown1_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new GiamKheHoLangMessage() { May = 1 });
        }

        private void BtnUp1_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new TangKheHoLangMessaage() { May = 1 });
        }

        public string Song1
        {
            get { return (string)GetValue(Song1Property); }
            set { SetValue(Song1Property, value); }
        }
        public static readonly DependencyProperty Song1Property =
            DependencyProperty.Register("Song1", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string Song2
        {
            get { return (string)GetValue(Song2Property); }
            set { SetValue(Song2Property, value); }
        }
        public static readonly DependencyProperty Song2Property =
            DependencyProperty.Register("Song2", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string Song3
        {
            get { return (string)GetValue(Song3Property); }
            set { SetValue(Song3Property, value); }
        }
        public static readonly DependencyProperty Song3Property =
            DependencyProperty.Register("Song3", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string Song4
        {
            get { return (string)GetValue(Song4Property); }
            set { SetValue(Song4Property, value); }
        }
        public static readonly DependencyProperty Song4Property =
            DependencyProperty.Register("Song4", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string Xa1
        {
            get { return (string)GetValue(Xa1Property); }
            set { SetValue(Xa1Property, value); }
        }
        public static readonly DependencyProperty Xa1Property =
            DependencyProperty.Register("Xa1", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string Xa2
        {
            get { return (string)GetValue(Xa2Property); }
            set { SetValue(Xa2Property, value); }
        }
        public static readonly DependencyProperty Xa2Property =
            DependencyProperty.Register("Xa2", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string Xa3
        {
            get { return (string)GetValue(Xa3Property); }
            set { SetValue(Xa3Property, value); }
        }
        public static readonly DependencyProperty Xa3Property =
            DependencyProperty.Register("Xa3", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string Xa4
        {
            get { return (string)GetValue(Xa4Property); }
            set { SetValue(Xa4Property, value); }
        }
        public static readonly DependencyProperty Xa4Property =
            DependencyProperty.Register("Xa4", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string Nap11
        {
            get { return (string)GetValue(Nap11Property); }
            set { SetValue(Nap11Property, value); }
        }
        public static readonly DependencyProperty Nap11Property =
            DependencyProperty.Register("Nap11", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata("", UpdateRong1));

        public string Nap12
        {
            get { return (string)GetValue(Nap12Property); }
            set { SetValue(Nap12Property, value); }
        }
        public static readonly DependencyProperty Nap12Property =
            DependencyProperty.Register("Nap12", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata("", UpdateRong2));

        public string Nap13
        {
            get { return (string)GetValue(Nap13Property); }
            set { SetValue(Nap13Property, value); }
        }
        public static readonly DependencyProperty Nap13Property =
            DependencyProperty.Register("Nap13", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata("", UpdateRong3));

        public string Nap14
        {
            get { return (string)GetValue(Nap14Property); }
            set { SetValue(Nap14Property, value); }
        }
        public static readonly DependencyProperty Nap14Property =
            DependencyProperty.Register("Nap14", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata("", UpdateRong4));

        public string Cao1
        {
            get { return (string)GetValue(Cao1Property); }
            set { SetValue(Cao1Property, value); }
        }
        public static readonly DependencyProperty Cao1Property =
            DependencyProperty.Register("Cao1", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata("", UpdateRong1));

        public string Cao2
        {
            get { return (string)GetValue(Cao2Property); }
            set { SetValue(Cao2Property, value); }
        }
        public static readonly DependencyProperty Cao2Property =
            DependencyProperty.Register("Cao2", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata("", UpdateRong2));

        public string Cao3
        {
            get { return (string)GetValue(Cao3Property); }
            set { SetValue(Cao3Property, value); }
        }
        public static readonly DependencyProperty Cao3Property =
            DependencyProperty.Register("Cao3", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata("", UpdateRong3));

        public string Cao4
        {
            get { return (string)GetValue(Cao4Property); }
            set { SetValue(Cao4Property, value); }
        }
        public static readonly DependencyProperty Cao4Property =
            DependencyProperty.Register("Cao4", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata("", UpdateRong4));

        public string Nap21
        {
            get { return (string)GetValue(Nap21Property); }
            set { SetValue(Nap21Property, value); }
        }
        public static readonly DependencyProperty Nap21Property =
            DependencyProperty.Register("Nap21", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata("", UpdateRong1));

        public string Nap22
        {
            get { return (string)GetValue(Nap22Property); }
            set { SetValue(Nap22Property, value); }
        }
        public static readonly DependencyProperty Nap22Property =
            DependencyProperty.Register("Nap22", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata("", UpdateRong2));

        public string Nap23
        {
            get { return (string)GetValue(Nap23Property); }
            set { SetValue(Nap23Property, value); }
        }
        public static readonly DependencyProperty Nap23Property =
            DependencyProperty.Register("Nap23", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata("", UpdateRong3));

        public string Nap24
        {
            get { return (string)GetValue(Nap24Property); }
            set { SetValue(Nap24Property, value); }
        }
        public static readonly DependencyProperty Nap24Property =
            DependencyProperty.Register("Nap24", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata("", UpdateRong4));

        public string Rong1
        {
            get { return (string)GetValue(Rong1Property); }
            set { SetValue(Rong1Property, value); }
        }
        public static readonly DependencyProperty Rong1Property =
            DependencyProperty.Register("Rong1", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string Rong2
        {
            get { return (string)GetValue(Rong2Property); }
            set { SetValue(Rong2Property, value); }
        }
        public static readonly DependencyProperty Rong2Property =
            DependencyProperty.Register("Rong2", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string Rong3
        {
            get { return (string)GetValue(Rong3Property); }
            set { SetValue(Rong3Property, value); }
        }
        public static readonly DependencyProperty Rong3Property =
            DependencyProperty.Register("Rong3", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string Rong4
        {
            get { return (string)GetValue(Rong4Property); }
            set { SetValue(Rong4Property, value); }
        }
        public static readonly DependencyProperty Rong4Property =
            DependencyProperty.Register("Rong4", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string Lang1
        {
            get { return (string)GetValue(Lang1Property); }
            set { SetValue(Lang1Property, value); }
        }
        public static readonly DependencyProperty Lang1Property =
            DependencyProperty.Register("Lang1", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string Lang2
        {
            get { return (string)GetValue(Lang2Property); }
            set { SetValue(Lang2Property, value); }
        }
        public static readonly DependencyProperty Lang2Property =
            DependencyProperty.Register("Lang2", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string Lang3
        {
            get { return (string)GetValue(Lang3Property); }
            set { SetValue(Lang3Property, value); }
        }
        public static readonly DependencyProperty Lang3Property =
            DependencyProperty.Register("Lang3", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string Lang4
        {
            get { return (string)GetValue(Lang4Property); }
            set { SetValue(Lang4Property, value); }
        }
        public static readonly DependencyProperty Lang4Property =
            DependencyProperty.Register("Lang4", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string KheHoLang1
        {
            get { return (string)GetValue(KheHoLang1Property); }
            set { SetValue(KheHoLang1Property, value); }
        }
        public static readonly DependencyProperty KheHoLang1Property =
            DependencyProperty.Register("KheHoLang1", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string KheHoLang2
        {
            get { return (string)GetValue(KheHoLang2Property); }
            set { SetValue(KheHoLang2Property, value); }
        }
        public static readonly DependencyProperty KheHoLang2Property =
            DependencyProperty.Register("KheHoLang2", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string KheHoLang3
        {
            get { return (string)GetValue(KheHoLang3Property); }
            set { SetValue(KheHoLang3Property, value); }
        }
        public static readonly DependencyProperty KheHoLang3Property =
            DependencyProperty.Register("KheHoLang3", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string KheHoLang4
        {
            get { return (string)GetValue(KheHoLang4Property); }
            set { SetValue(KheHoLang4Property, value); }
        }
        public static readonly DependencyProperty KheHoLang4Property =
            DependencyProperty.Register("KheHoLang4", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string STT1
        {
            get { return (string)GetValue(STT1Property); }
            set { SetValue(STT1Property, value); }
        }
        public static readonly DependencyProperty STT1Property =
            DependencyProperty.Register("STT1", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string STT2
        {
            get { return (string)GetValue(STT2Property); }
            set { SetValue(STT2Property, value); }
        }
        public static readonly DependencyProperty STT2Property =
            DependencyProperty.Register("STT2", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string STT3
        {
            get { return (string)GetValue(STT3Property); }
            set { SetValue(STT3Property, value); }
        }
        public static readonly DependencyProperty STT3Property =
            DependencyProperty.Register("STT3", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string STT4
        {
            get { return (string)GetValue(STT4Property); }
            set { SetValue(STT4Property, value); }
        }
        public static readonly DependencyProperty STT4Property =
            DependencyProperty.Register("STT4", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string KheHoLangThucTe1
        {
            get { return (string)GetValue(KheHoLangThucTe1Property); }
            set { SetValue(KheHoLangThucTe1Property, value); }
        }
        public static readonly DependencyProperty KheHoLangThucTe1Property =
            DependencyProperty.Register("KheHoLangThucTe1", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string KheHoLangThucTe2
        {
            get { return (string)GetValue(KheHoLangThucTe2Property); }
            set { SetValue(KheHoLangThucTe2Property, value); }
        }
        public static readonly DependencyProperty KheHoLangThucTe2Property =
            DependencyProperty.Register("KheHoLangThucTe2", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string KheHoLangThucTe3
        {
            get { return (string)GetValue(KheHoLangThucTe3Property); }
            set { SetValue(KheHoLangThucTe3Property, value); }
        }
        public static readonly DependencyProperty KheHoLangThucTe3Property =
            DependencyProperty.Register("KheHoLangThucTe3", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));

        public string KheHoLangThucTe4
        {
            get { return (string)GetValue(KheHoLangThucTe4Property); }
            set { SetValue(KheHoLangThucTe4Property, value); }
        }
        public static readonly DependencyProperty KheHoLangThucTe4Property =
            DependencyProperty.Register("KheHoLangThucTe4", typeof(string), typeof(DonHangDangChayView), new PropertyMetadata(""));



        public Brush BackgroundMay1
        {
            get { return (Brush)GetValue(BackgroundMay1Property); }
            set { SetValue(BackgroundMay1Property, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundMay1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundMay1Property =
            DependencyProperty.Register("BackgroundMay1", typeof(Brush), typeof(DonHangDangChayView), new PropertyMetadata(Brushes.White));

        public Brush BackgroundMay2
        {
            get { return (Brush)GetValue(BackgroundMay2Property); }
            set { SetValue(BackgroundMay2Property, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundMay2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundMay2Property =
            DependencyProperty.Register("BackgroundMay2", typeof(Brush), typeof(DonHangDangChayView), new PropertyMetadata(Brushes.White));


        private static void UpdateRong1(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DonHangDangChayView control)
            {
                if (double.TryParse(control.Nap11, out double nap11) &&
                    double.TryParse(control.Nap21, out double nap12) &&
                    double.TryParse(control.Cao1, out double cao1))
                {
                    control.Rong1 = (nap11 + nap12 + cao1).ToString();
                }
                else
                {
                    control.Rong1 = "0";
                }
            }
        }

        private static void UpdateRong2(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DonHangDangChayView control)
            {
                if (double.TryParse(control.Nap12, out double nap11) &&
                    double.TryParse(control.Nap22, out double nap12) &&
                    double.TryParse(control.Cao2, out double cao1))
                {
                    control.Rong2 = (nap11 + nap12 + cao1).ToString();
                }
                else
                {
                    control.Rong2 = "0";
                }
            }
        }

        private static void UpdateRong3(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DonHangDangChayView control)
            {
                if (double.TryParse(control.Nap13, out double nap11) &&
                    double.TryParse(control.Nap23, out double nap12) &&
                    double.TryParse(control.Cao3, out double cao1))
                {
                    control.Rong3 = (nap11 + nap12 + cao1).ToString();
                }
                else
                {
                    control.Rong3 = "0";
                }
            }
        }

        private static void UpdateRong4(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DonHangDangChayView control)
            {
                if (double.TryParse(control.Nap14, out double nap11) &&
                    double.TryParse(control.Nap24, out double nap12) &&
                    double.TryParse(control.Cao4, out double cao1))
                {
                    control.Rong4 = (nap11 + nap12 + cao1).ToString();
                }
                else
                {
                    control.Rong4 = "0";
                }
            }
        }
    }
}
