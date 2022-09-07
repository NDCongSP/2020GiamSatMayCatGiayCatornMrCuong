using CommonControls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace MayCatClient
{
    /// <summary>
    /// Interaction logic for BaoCaoWindow.xaml
    /// </summary>
    public partial class BaoCaoWindow : Window
    {
        List<DonHang> DataSourceCore { get; set; }
        public BaoCaoWindow()
        {
            InitializeComponent();
            DataSourceCore = MainWindow.donHangServer;
            if (DataSourceCore == null)
                DataSourceCore = new List<DonHang>();
            txbTuNgay.Value = Convert.ToDateTime($"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day} 00:00:00");
            txbTuNgay.DefaultValue = Convert.ToDateTime($"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day} 00:00:00");

            txbDenNgay.Value = Convert.ToDateTime($"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day} 23:59:00");
            txbDenNgay.DefaultValue = Convert.ToDateTime($"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day} 23:59:00");
            btnTruyVan.Click += BtnTruyVan_Click;
            dataGrid.EnableRowVirtualization = true;
            btnXoaLoc.Click += BtnXoaLoc_Click;
            btnXuatExcel.Click += BtnXuatExcel_Click;
        }

        private void BtnXuatExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbTuNgay.Value.HasValue && txbDenNgay.Value.HasValue)
                {
                    List<DonHang> dataSource = null;
                    if (dataGrid.ItemsSource is NotifyCollection<DonHang> dongHangs)
                        dataSource = dongHangs.ToList();
                    else
                        dataSource = new List<DonHang>();

                    string tuNgay = $"{txbTuNgay.Value.Value.ToString("dd/MM/yyyy HH:mm:ss")}";
                    string denNgay = $"{txbDenNgay.Value.Value.ToString("dd/MM/yyyy HH:mm:ss")}";
                    Repository.Instance.ExportToExcel(dataSource, "THỐNG KÊ SẢN XUẤT", $"Từ ngày: {tuNgay} - Đến ngày: {denNgay}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnXoaLoc_Click(object sender, RoutedEventArgs e)
        {

            foreach (var item in grid.Children)
            {
                if (item is ComboBox cob)
                    cob.Text = "";
            }
        }

        private void BtnTruyVan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbTuNgay.Value.HasValue && txbDenNgay.Value.HasValue && txbTuNgay.Value >= txbDenNgay.Value)
                {
                    MessageBox.Show("Thời gian 'Từ ngày' phải bé hơn 'Đến ngày'", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string query = $"select * from donhang where " +
                            $"NgayTao >= '{txbTuNgay.Value.Value.ToString("yyyy-MM-dd HH:mm:ss")}' " +
                            $"and NgayTao <= '{txbDenNgay.Value.Value.ToString("yyyy-MM-dd HH:mm:ss")}'";
                IEnumerable<DonHang> donHangs = Repository.Instance.GetDonHangDataChay(query).AsEnumerable<DonHang>();

                DataSourceCore = new List<DonHang>(donHangs);

                if (!string.IsNullOrWhiteSpace(cobSong.Text?.ToString()))
                {
                    donHangs = donHangs.Where(x => x.Song.Contains(cobSong.Text?.ToString()));
                }

                if (!string.IsNullOrWhiteSpace(cobSongE.Text?.ToString()))
                {
                    donHangs = donHangs.Where(x => x.GiaySongE.Contains(cobSongE.Text?.ToString()));
                }

                if (!string.IsNullOrWhiteSpace(cobMatE.Text?.ToString()))
                {
                    donHangs = donHangs.Where(x => x.GiayMatE.Contains(cobMatE.Text?.ToString()));
                }

                if (!string.IsNullOrWhiteSpace(cobSongB.Text?.ToString()))
                {
                    donHangs = donHangs.Where(x => x.GiaySongB.Contains(cobSongB.Text?.ToString()));
                }

                if (!string.IsNullOrWhiteSpace(cobMatB.Text?.ToString()))
                {
                    donHangs = donHangs.Where(x => x.GiayMatB.Contains(cobMatB.Text?.ToString()));
                }

                if (!string.IsNullOrWhiteSpace(cobSongC.Text?.ToString()))
                {
                    donHangs = donHangs.Where(x => x.GiaySongC.Contains(cobSongC.Text?.ToString()));
                }

                if (!string.IsNullOrWhiteSpace(cobMatC.Text?.ToString()))
                {
                    donHangs = donHangs.Where(x => x.GiayMatC.Contains(cobMatC.Text?.ToString()));
                }

                if (!string.IsNullOrWhiteSpace(cobTrangThai.Text?.ToString()))
                {
                    // "Tất cả", "Hoàn thành", "Chưa hoàn thành"

                    switch (cobTrangThai.Text?.ToString())
                    {
                        case "Tất cả":
                            break;
                        case "Hoàn thành":
                            donHangs = donHangs.Where(x => x.TGKetThuc != DateTime.MinValue && x.SLDat >= x.SL);
                            break;
                        case "Chưa hoàn thành":
                            donHangs = donHangs.Where(x => x.TGKetThuc == DateTime.MinValue || x.SLDat < x.SL);
                            break;
                        default:
                            break;
                    }

                }

                if (!string.IsNullOrWhiteSpace(cobMa.Text?.ToString()))
                {
                    donHangs = donHangs.Where(x => x.Ma.ToString().Contains(cobMa.Text?.ToString()));
                }

                if (!string.IsNullOrWhiteSpace(cobKho.Text?.ToString()))
                {
                    donHangs = donHangs.Where(x => x.Kho.ToString().Contains(cobKho.Text?.ToString()));
                }

                if (!string.IsNullOrWhiteSpace(cobMen.Text?.ToString()))
                {
                    donHangs = donHangs.Where(x => x.Men.ToString().Contains(cobMen.Text?.ToString()));
                }

                if (!string.IsNullOrWhiteSpace(cobSTT.Text?.ToString()))
                {
                    donHangs = donHangs.Where(x => x.STT.ToString().Contains(cobSTT.Text?.ToString()));
                }

                if (!string.IsNullOrWhiteSpace(cobCa.Text?.ToString()))
                {
                    //"Tất cả", "1", "2"
                    switch (cobCa.Text?.ToString())
                    {
                        case "1":
                            donHangs = donHangs.Where(x => x.Ca == 1);
                            break;
                        case "2":
                            donHangs = donHangs.Where(x => x.Ca == 2);
                            break;
                        case "Tất cả":
                        default:
                            break;
                    }
                }

                dataGrid.ItemsSource = new NotifyCollection<DonHang>(donHangs);
                ReloadFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReloadFilter()
        {
            cobSong.ItemsSource = DataSourceCore?.Select(x => x.Song)?.Distinct();
            cobSongE.ItemsSource = DataSourceCore?.Select(x => x.GiaySongE)?.Distinct();
            cobSongB.ItemsSource = DataSourceCore?.Select(x => x.GiaySongB)?.Distinct();
            cobSongC.ItemsSource = DataSourceCore?.Select(x => x.GiaySongC)?.Distinct();
            cobMatE.ItemsSource = DataSourceCore?.Select(x => x.GiayMatE)?.Distinct();
            cobMatB.ItemsSource = DataSourceCore?.Select(x => x.GiayMatB)?.Distinct();
            cobMatC.ItemsSource = DataSourceCore?.Select(x => x.GiayMatC)?.Distinct();
            cobMa.ItemsSource = DataSourceCore?.Select(x => x.Ma.ToString())?.Distinct();
            cobKho.ItemsSource = DataSourceCore?.Select(x => x.Kho.ToString())?.Distinct();
            cobMen.ItemsSource = DataSourceCore?.Select(x => x.Men.ToString())?.Distinct();

            cobSTT.ItemsSource = DataSourceCore?.Select(x => x.STT.ToString())?.Distinct();

            cobTrangThai.ItemsSource = new List<string>() { "Tất cả", "Hoàn thành", "Chưa hoàn thành" };
            cobCa.ItemsSource = new List<string>() { "Tất cả", "1", "2" };
        }

        private void BtnXuatExcel_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
