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

namespace MayCatServer
{
    /// <summary>
    /// Interaction logic for DonHangDangChay.xaml
    /// </summary>
    public partial class DonHangDangChay : UserControl
    {
        #region Properties

        public string CatTam1
        {
            get { return (string)GetValue(CatTam1Property); }
            set { SetValue(CatTam1Property, value); }
        }
        public static readonly DependencyProperty CatTam1Property =
            DependencyProperty.Register("CatTam1", typeof(string), typeof(DonHangDangChay), new PropertyMetadata(""));

        public string CatTam2
        {
            get { return (string)GetValue(CatTam2Property); }
            set { SetValue(CatTam2Property, value); }
        }
        public static readonly DependencyProperty CatTam2Property =
            DependencyProperty.Register("CatTam2", typeof(string), typeof(DonHangDangChay), new PropertyMetadata(""));

        public string CatTam3
        {
            get { return (string)GetValue(CatTam3Property); }
            set { SetValue(CatTam3Property, value); }
        }
        public static readonly DependencyProperty CatTam3Property =
            DependencyProperty.Register("CatTam3", typeof(string), typeof(DonHangDangChay), new PropertyMetadata(""));

        public string DaiCat1
        {
            get { return (string)GetValue(DaiCat1Property); }
            set { SetValue(DaiCat1Property, value); }
        }
        public static readonly DependencyProperty DaiCat1Property =
            DependencyProperty.Register("DaiCat1", typeof(string), typeof(DonHangDangChay), new PropertyMetadata(""));

        public string DaiCat2
        {
            get { return (string)GetValue(DaiCat2Property); }
            set { SetValue(DaiCat2Property, value); }
        }
        public static readonly DependencyProperty DaiCat2Property =
            DependencyProperty.Register("DaiCat2", typeof(string), typeof(DonHangDangChay), new PropertyMetadata(""));

        public string DaiCat3
        {
            get { return (string)GetValue(DaiCat3Property); }
            set { SetValue(DaiCat3Property, value); }
        }
        public static readonly DependencyProperty DaiCat3Property =
            DependencyProperty.Register("DaiCat3", typeof(string), typeof(DonHangDangChay), new PropertyMetadata(""));

        public string SLCat1
        {
            get { return (string)GetValue(SLCat1Property); }
            set { SetValue(SLCat1Property, value); }
        }
        public static readonly DependencyProperty SLCat1Property =
            DependencyProperty.Register("SLCat1", typeof(string), typeof(DonHangDangChay), new PropertyMetadata(""));

        public string SLCat2
        {
            get { return (string)GetValue(SLCat2Property); }
            set { SetValue(SLCat2Property, value); }
        }
        public static readonly DependencyProperty SLCat2Property =
            DependencyProperty.Register("SLCat2", typeof(string), typeof(DonHangDangChay), new PropertyMetadata(""));

        public string SLCat3
        {
            get { return (string)GetValue(SLCat3Property); }
            set { SetValue(SLCat3Property, value); }
        }
        public static readonly DependencyProperty SLCat3Property =
            DependencyProperty.Register("SLCat3", typeof(string), typeof(DonHangDangChay), new PropertyMetadata(""));

        public string SLDat1
        {
            get { return (string)GetValue(SLDat1Property); }
            set { SetValue(SLDat1Property, value); }
        }
        public static readonly DependencyProperty SLDat1Property =
            DependencyProperty.Register("SLDat1", typeof(string), typeof(DonHangDangChay), new PropertyMetadata(""));

        public string SLDat2
        {
            get { return (string)GetValue(SLDat2Property); }
            set { SetValue(SLDat2Property, value); }
        }
        public static readonly DependencyProperty SLDat2Property =
            DependencyProperty.Register("SLDat2", typeof(string), typeof(DonHangDangChay), new PropertyMetadata(""));

        public string SLDat3
        {
            get { return (string)GetValue(SLDat3Property); }
            set { SetValue(SLDat3Property, value); }
        }
        public static readonly DependencyProperty SLDat3Property =
            DependencyProperty.Register("SLDat3", typeof(string), typeof(DonHangDangChay), new PropertyMetadata(""));

        public string SLLoi1
        {
            get { return (string)GetValue(SLLoi1Property); }
            set { SetValue(SLLoi1Property, value); }
        }
        public static readonly DependencyProperty SLLoi1Property =
            DependencyProperty.Register("SLLoi1", typeof(string), typeof(DonHangDangChay), new PropertyMetadata(""));

        public string Pallet1
        {
            get { return (string)GetValue(Pallet1Property); }
            set { SetValue(Pallet1Property, value); }
        }
        public static readonly DependencyProperty Pallet1Property =
            DependencyProperty.Register("Pallet1", typeof(string), typeof(DonHangDangChay), new PropertyMetadata(""));

        public string Pallet2
        {
            get { return (string)GetValue(Pallet2Property); }
            set { SetValue(Pallet2Property, value); }
        }
        public static readonly DependencyProperty Pallet2Property =
            DependencyProperty.Register("Pallet2", typeof(string), typeof(DonHangDangChay), new PropertyMetadata(""));


        public string Pallet3
        {
            get { return (string)GetValue(Pallet3Property); }
            set { SetValue(Pallet3Property, value); }
        }
        public static readonly DependencyProperty Pallet3Property =
            DependencyProperty.Register("Pallet3", typeof(string), typeof(DonHangDangChay), new PropertyMetadata(""));


        public string SLConLai1
        {
            get { return (string)GetValue(SLConLai1Property); }
            set { SetValue(SLConLai1Property, value); }
        }
        public static readonly DependencyProperty SLConLai1Property =
            DependencyProperty.Register("SLConLai1", typeof(string), typeof(DonHangDangChay), new PropertyMetadata(""));

        public string SLConLai2
        {
            get { return (string)GetValue(SLConLai2Property); }
            set { SetValue(SLConLai2Property, value); }
        }
        public static readonly DependencyProperty SLConLai2Property =
            DependencyProperty.Register("SLConLai2", typeof(string), typeof(DonHangDangChay), new PropertyMetadata(""));

        public string SLConLai3
        {
            get { return (string)GetValue(SLConLai3Property); }
            set { SetValue(SLConLai3Property, value); }
        }
        public static readonly DependencyProperty SLConLai3Property =
            DependencyProperty.Register("SLConLai3", typeof(string), typeof(DonHangDangChay), new PropertyMetadata(""));

        #endregion

        public DonHangDangChay()
        {
            InitializeComponent();
        }
    }
}
