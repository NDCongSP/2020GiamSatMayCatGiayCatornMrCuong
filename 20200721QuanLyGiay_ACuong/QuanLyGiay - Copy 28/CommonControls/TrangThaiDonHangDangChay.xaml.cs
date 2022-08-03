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

namespace CommonControls
{
    /// <summary>
    /// Interaction logic for TrangThaiDonHangDangChay.xaml
    /// </summary>
    public partial class TrangThaiDonHangDangChay : UserControl
    {
        #region Properties

        public string TK1
        {
            get { return (string)GetValue(TK1Property); }
            set { SetValue(TK1Property, value); }
        }
        public static readonly DependencyProperty TK1Property =
            DependencyProperty.Register("TK1", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        public string TK2
        {
            get { return (string)GetValue(TK2Property); }
            set { SetValue(TK2Property, value); }
        }
        public static readonly DependencyProperty TK2Property =
            DependencyProperty.Register("TK2", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata("CA"));

        public string SoMet1
        {
            get { return (string)GetValue(SoMet1Property); }
            set { SetValue(SoMet1Property, value); }
        }
        public static readonly DependencyProperty SoMet1Property =
            DependencyProperty.Register("SoMet1", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        public string SoMet2
        {
            get { return (string)GetValue(SoMet2Property); }
            set { SetValue(SoMet2Property, value); }
        }
        public static readonly DependencyProperty SoMet2Property =
            DependencyProperty.Register("SoMet2", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));


        public string SoMetDat1
        {
            get { return (string)GetValue(SoMetDat1Property); }
            set { SetValue(SoMetDat1Property, value); }
        }
        public static readonly DependencyProperty SoMetDat1Property =
            DependencyProperty.Register("SoMetDat1", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        public string SoMetDat2
        {
            get { return (string)GetValue(SoMetDat2Property); }
            set { SetValue(SoMetDat2Property, value); }
        }
        public static readonly DependencyProperty SoMetDat2Property =
            DependencyProperty.Register("SoMetDat2", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        public string SoMetLoi1
        {
            get { return (string)GetValue(SoMetLoi1Property); }
            set { SetValue(SoMetLoi1Property, value); }
        }
        public static readonly DependencyProperty SoMetLoi1Property =
            DependencyProperty.Register("SoMetLoi1", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        public string SoMetLoi2
        {
            get { return (string)GetValue(SoMetLoi2Property); }
            set { SetValue(SoMetLoi2Property, value); }
        }
        public static readonly DependencyProperty SoMetLoi2Property =
            DependencyProperty.Register("SoMetLoi2", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        public string ConLai1
        {
            get { return (string)GetValue(ConLai1Property); }
            set { SetValue(ConLai1Property, value); }
        }
        public static readonly DependencyProperty ConLai1Property =
            DependencyProperty.Register("ConLai1", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        public string ConLai2
        {
            get { return (string)GetValue(ConLai2Property); }
            set { SetValue(ConLai2Property, value); }
        }
        public static readonly DependencyProperty ConLai2Property =
            DependencyProperty.Register("ConLai2", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        public string PhanTramLoi1
        {
            get { return (string)GetValue(PhanTramLoi1Property); }
            set { SetValue(PhanTramLoi1Property, value); }
        }
        public static readonly DependencyProperty PhanTramLoi1Property =
            DependencyProperty.Register("PhanTramLoi1", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        public string PhanTramLoi2
        {
            get { return (string)GetValue(PhanTramLoi2Property); }
            set { SetValue(PhanTramLoi2Property, value); }
        }
        public static readonly DependencyProperty PhanTramLoi2Property =
            DependencyProperty.Register("PhanTramLoi2", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        public string TocDoTrungBinh1
        {
            get { return (string)GetValue(TocDoTrungBinh1Property); }
            set { SetValue(TocDoTrungBinh1Property, value); }
        }
        public static readonly DependencyProperty TocDoTrungBinh1Property =
            DependencyProperty.Register("TocDoTrungBinh1", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        public string TocDoTrungBinh2
        {
            get { return (string)GetValue(TocDoTrungBinh2Property); }
            set { SetValue(TocDoTrungBinh2Property, value); }
        }
        public static readonly DependencyProperty TocDoTrungBinh2Property =
            DependencyProperty.Register("TocDoTrungBinh2", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        public string Chay1
        {
            get { return (string)GetValue(Chay1Property); }
            set { SetValue(Chay1Property, value); }
        }
        public static readonly DependencyProperty Chay1Property =
            DependencyProperty.Register("Chay1", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        public string Chay2
        {
            get { return (string)GetValue(Chay2Property); }
            set { SetValue(Chay2Property, value); }
        }
        public static readonly DependencyProperty Chay2Property =
            DependencyProperty.Register("Chay2", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        public string Dung1
        {
            get { return (string)GetValue(Dung1Property); }
            set { SetValue(Dung1Property, value); }
        }
        public static readonly DependencyProperty Dung1Property =
            DependencyProperty.Register("Dung1", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        public string Dung2
        {
            get { return (string)GetValue(Dung2Property); }
            set { SetValue(Dung2Property, value); }
        }
        public static readonly DependencyProperty Dung2Property =
            DependencyProperty.Register("Dung2", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        public string SoDung1
        {
            get { return (string)GetValue(SoDung1Property); }
            set { SetValue(SoDung1Property, value); }
        }
        public static readonly DependencyProperty SoDung1Property =
            DependencyProperty.Register("SoDung1", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        public string SoDung2
        {
            get { return (string)GetValue(SoDung2Property); }
            set { SetValue(SoDung2Property, value); }
        }
        public static readonly DependencyProperty SoDung2Property =
            DependencyProperty.Register("SoDung2", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        public string M2Dat1
        {
            get { return (string)GetValue(M2Dat1Property); }
            set { SetValue(M2Dat1Property, value); }
        }
        public static readonly DependencyProperty M2Dat1Property =
            DependencyProperty.Register("M2Dat1", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        public string M2Dat2
        {
            get { return (string)GetValue(M2Dat2Property); }
            set { SetValue(M2Dat2Property, value); }
        }
        public static readonly DependencyProperty M2Dat2Property =
            DependencyProperty.Register("M2Dat2", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        public string M2Loi1
        {
            get { return (string)GetValue(M2Loi1Property); }
            set { SetValue(M2Loi1Property, value); }
        }
        public static readonly DependencyProperty M2Loi1Property =
            DependencyProperty.Register("M2Loi1", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        public string M2Loi2
        {
            get { return (string)GetValue(M2Loi2Property); }
            set { SetValue(M2Loi2Property, value); }
        }
        public static readonly DependencyProperty M2Loi2Property =
            DependencyProperty.Register("M2Loi2", typeof(string), typeof(TrangThaiDonHangDangChay), new PropertyMetadata(""));

        #endregion

        public TrangThaiDonHangDangChay()
        {
            InitializeComponent();
        }
    }
}
