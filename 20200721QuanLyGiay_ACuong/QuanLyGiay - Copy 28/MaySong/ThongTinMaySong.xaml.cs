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

namespace MaySong
{
    /// <summary>
    /// Interaction logic for ThongTinMaySong.xaml
    /// </summary>
    public partial class ThongTinMaySong : UserControl
    {
        public ThongTinMaySong()
        {
            InitializeComponent();
        }

        public string KhoSong1
        {
            get { return (string)GetValue(KhoSong1Property); }
            set { SetValue(KhoSong1Property, value); }
        }

        public static readonly DependencyProperty KhoSong1Property =
            DependencyProperty.Register("KhoSong1", typeof(string), typeof(ThongTinMaySong), new PropertyMetadata(""));

        public string KhoSong2
        {
            get { return (string)GetValue(KhoSong2Property); }
            set { SetValue(KhoSong2Property, value); }
        }

        public static readonly DependencyProperty KhoSong2Property =
            DependencyProperty.Register("KhoSong2", typeof(string), typeof(ThongTinMaySong), new PropertyMetadata(""));


        public string KhoMat1
        {
            get { return (string)GetValue(KhoMat1Property); }
            set { SetValue(KhoMat1Property, value); }
        }

        public static readonly DependencyProperty KhoMat1Property =
            DependencyProperty.Register("KhoMat1", typeof(string), typeof(ThongTinMaySong), new PropertyMetadata(""));

        public string KhoMat2
        {
            get { return (string)GetValue(KhoMat2Property); }
            set { SetValue(KhoMat2Property, value); }
        }

        public static readonly DependencyProperty KhoMat2Property =
            DependencyProperty.Register("KhoMat2", typeof(string), typeof(ThongTinMaySong), new PropertyMetadata(""));

        public string LoaiGiaySong1
        {
            get { return (string)GetValue(LoaiGiaySong1Property); }
            set { SetValue(LoaiGiaySong1Property, value); }
        }

        public static readonly DependencyProperty LoaiGiaySong1Property =
            DependencyProperty.Register("LoaiGiaySong1", typeof(string), typeof(ThongTinMaySong), new PropertyMetadata(""));

        public string LoaiGiaySong2
        {
            get { return (string)GetValue(LoaiGiaySong2Property); }
            set { SetValue(LoaiGiaySong2Property, value); }
        }

        public static readonly DependencyProperty LoaiGiaySong2Property =
            DependencyProperty.Register("LoaiGiaySong2", typeof(string), typeof(ThongTinMaySong), new PropertyMetadata(""));

        public string LoaiGiayMat1
        {
            get { return (string)GetValue(LoaiGiayMat1Property); }
            set { SetValue(LoaiGiayMat1Property, value); }
        }

        public static readonly DependencyProperty LoaiGiayMat1Property =
            DependencyProperty.Register("LoaiGiayMat1", typeof(string), typeof(ThongTinMaySong), new PropertyMetadata(""));

        public string LoaiGiayMat2
        {
            get { return (string)GetValue(LoaiGiayMat2Property); }
            set { SetValue(LoaiGiayMat2Property, value); }
        }

        public static readonly DependencyProperty LoaiGiayMat2Property =
            DependencyProperty.Register("LoaiGiayMat2", typeof(string), typeof(ThongTinMaySong), new PropertyMetadata(""));

        public string ChieuDaiSong1
        {
            get { return (string)GetValue(ChieuDaiSong1Property); }
            set { SetValue(ChieuDaiSong1Property, value); }
        }

        public static readonly DependencyProperty ChieuDaiSong1Property =
            DependencyProperty.Register("ChieuDaiSong1", typeof(string), typeof(ThongTinMaySong), new PropertyMetadata(""));

        public string ChieuDaiSong2
        {
            get { return (string)GetValue(ChieuDaiSong2Property); }
            set { SetValue(ChieuDaiSong2Property, value); }
        }

        public static readonly DependencyProperty ChieuDaiSong2Property =
            DependencyProperty.Register("ChieuDaiSong2", typeof(string), typeof(ThongTinMaySong), new PropertyMetadata(""));

        public string ChieuDaiMat1
        {
            get { return (string)GetValue(ChieuDaiMat1Property); }
            set { SetValue(ChieuDaiMat1Property, value); }
        }

        public static readonly DependencyProperty ChieuDaiMat1Property =
            DependencyProperty.Register("ChieuDaiMat1", typeof(string), typeof(ThongTinMaySong), new PropertyMetadata(""));

        public string ChieuDaiMat2
        {
            get { return (string)GetValue(ChieuDaiMat2Property); }
            set { SetValue(ChieuDaiMat2Property, value); }
        }

        public static readonly DependencyProperty ChieuDaiMat2Property =
            DependencyProperty.Register("ChieuDaiMat2", typeof(string), typeof(ThongTinMaySong), new PropertyMetadata(""));

        public string DoiGiaySong
        {
            get { return (string)GetValue(DoiGiaySongProperty); }
            set { SetValue(DoiGiaySongProperty, value); }
        }

        public static readonly DependencyProperty DoiGiaySongProperty =
            DependencyProperty.Register("DoiGiaySong", typeof(string), typeof(ThongTinMaySong), new PropertyMetadata(""));

        public string DoiGiayMat
        {
            get { return (string)GetValue(DoiGiayMatProperty); }
            set { SetValue(DoiGiayMatProperty, value); }
        }


        public static readonly DependencyProperty DoiGiayMatProperty =
            DependencyProperty.Register("DoiGiayMat", typeof(string), typeof(ThongTinMaySong), new PropertyMetadata(""));


        public string STTSong
        {
            get { return (string)GetValue(STTSongProperty); }
            set { SetValue(STTSongProperty, value); }
        }


        public static readonly DependencyProperty STTSongProperty =
            DependencyProperty.Register("STTSong", typeof(string), typeof(ThongTinMaySong), new PropertyMetadata(""));


        public string STTMat
        {
            get { return (string)GetValue(STTMatProperty); }
            set { SetValue(STTMatProperty, value); }
        }


        public static readonly DependencyProperty STTMatProperty =
            DependencyProperty.Register("STTMat", typeof(string), typeof(ThongTinMaySong), new PropertyMetadata(""));

        public string STTSong2
        {
            get { return (string)GetValue(STTSong2Property); }
            set { SetValue(STTSong2Property, value); }
        }


        public static readonly DependencyProperty STTSong2Property =
            DependencyProperty.Register("STTSong2", typeof(string), typeof(ThongTinMaySong), new PropertyMetadata(""));


        public string STTMat2
        {
            get { return (string)GetValue(STTMat2Property); }
            set { SetValue(STTMat2Property, value); }
        }


        public static readonly DependencyProperty STTMat2Property =
            DependencyProperty.Register("STTMat2", typeof(string), typeof(ThongTinMaySong), new PropertyMetadata(""));



        public string TenSong
        {
            get { return (string)GetValue(TenSongProperty); }
            set { SetValue(TenSongProperty, value); }
        }


        public static readonly DependencyProperty TenSongProperty =
            DependencyProperty.Register("TenSong", typeof(string), typeof(ThongTinMaySong), new PropertyMetadata(""));

    }
}
