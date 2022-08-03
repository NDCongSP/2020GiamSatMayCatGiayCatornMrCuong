using CommonControls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;

namespace MaySong
{
    /// <summary>
    /// Interaction logic for DonHangForm.xaml
    /// </summary>
    public partial class DonHangForm : Window
    {
        ObservableCollection<DonHang> DonHangDataSource = new ObservableCollection<DonHang>();
        public DonHangForm()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            PreviewKeyDown += DonHangForm_PreviewKeyDown;
            Loaded += DonHangForm_Loaded; ;
        }

        private void DonHangForm_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
                this.Close();
        }

        private void DonHangForm_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshTable();
        }

        public void RefreshTable()
        {
            DonHangDataSource = new ObservableCollection<DonHang>(GetDonHangs().OrderByDescending(x => x.STT));
            dataGrid.ItemsSource = DonHangDataSource;
        }

        public List<DonHang> GetDonHangs(string query = "")
        {
            var donHangs = new List<DonHang>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default["constr"].ToString()))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        if (query == "")
                            cmd.CommandText = $"select * from dhdangchay";
                        else
                            cmd.CommandText = query;
                        using (MySqlDataAdapter adp = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adp.Fill(dt);
                            foreach (DataRow dtRow in dt.Rows)
                            {
                                DonHang dh = new DonHang();
                                if (long.TryParse(dtRow["Id"]?.ToString(), out long id))
                                    dh.Id = id;
                                if (long.TryParse(dtRow["STT"]?.ToString(), out long stt))
                                    dh.STT = stt;
                                if (DateTime.TryParse(dtRow["NgayTao"]?.ToString(), out DateTime ngayTao))
                                    dh.NgayTao = ngayTao;
                                dh.Ma = dtRow["Ma"]?.ToString();
                                dh.Song = dtRow["Song"]?.ToString();
                                if (int.TryParse(dtRow["Kho"]?.ToString(), out int kho))
                                    dh.Kho = kho;
                                if (int.TryParse(dtRow["Dai"]?.ToString(), out int dai))
                                    dh.Dai = dai;
                                if (int.TryParse(dtRow["SL"]?.ToString(), out int sl))
                                    dh.SL = sl;
                                if (int.TryParse(dtRow["SLDat"]?.ToString(), out int slDat))
                                    dh.SLDat = slDat;
                                if (int.TryParse(dtRow["SLLoi"]?.ToString(), out int slLoi))
                                    dh.SLLoi = slLoi;
                                if (TimeSpan.TryParse(dtRow["TGChay"]?.ToString(), out TimeSpan chay))
                                    dh.Chay = chay;
                                if (TimeSpan.TryParse(dtRow["TGDung"]?.ToString(), out TimeSpan dung))
                                    dh.Dung = dung;
                                if (DateTime.TryParse(dtRow["TGBatDau"]?.ToString(), out DateTime tgBatDau))
                                    dh.TGBatDau = tgBatDau;
                                if (DateTime.TryParse(dtRow["TGKetThuc"]?.ToString(), out DateTime tgKetThuc))
                                    dh.TGKetThuc = tgKetThuc;
                                if (int.TryParse(dtRow["SoLanDung"]?.ToString(), out int sld))
                                    dh.SoDung = sld;
                                if (int.TryParse(dtRow["Pallet"]?.ToString(), out int pallet))
                                    dh.Pallet = pallet;
                                if (int.TryParse(dtRow["Xa"]?.ToString(), out int xa))
                                    dh.Xa = xa;
                                if (int.TryParse(dtRow["Rong"]?.ToString(), out int rong))
                                    dh.Rong = rong;
                                if (int.TryParse(dtRow["Canh"]?.ToString(), out int canh))
                                    dh.Canh = canh;
                                if (int.TryParse(dtRow["Cao"]?.ToString(), out int cao))
                                    dh.Cao = cao;
                                if (int.TryParse(dtRow["Lang"]?.ToString(), out int lang))
                                    dh.Lang = lang;
                                if (int.TryParse(dtRow["Ca"]?.ToString(), out int ca))
                                    dh.Ca = ca;
                                dh.GhiChu = dtRow["GhiChu"]?.ToString();
                                dh.GiaySongE = dtRow["GiaySongE"]?.ToString();
                                dh.GiaySongB = dtRow["GiaySongB"]?.ToString();
                                dh.GiaySongC = dtRow["GiaySongC"]?.ToString();
                                dh.GiayMatE = dtRow["GiayMatE"]?.ToString();
                                dh.GiayMatB = dtRow["GiayMatB"]?.ToString();
                                dh.GiayMatC = dtRow["GiayMatC"]?.ToString();
                                dh.Men = dtRow["GiayMen"]?.ToString();

                                if (int.TryParse(dtRow["HoanTatCutter"]?.ToString(), out int htCutter))
                                    dh.HoanTatCutter = htCutter;
                                if (int.TryParse(dtRow["HoanTatMayMen"]?.ToString(), out int htMen))
                                    dh.HoanTatMayMen = htMen;
                                if (int.TryParse(dtRow["HoanTatSongB"]?.ToString(), out int htB))
                                    dh.HoanTatSongB = htB;
                                if (int.TryParse(dtRow["HoanTatGiaySongB"]?.ToString(), out int htsB))
                                    dh.HoanTatGiaySongB = htsB;
                                if (int.TryParse(dtRow["HoanTatGiayMatB"]?.ToString(), out int htmB))
                                    dh.HoanTatGiayMatB = htmB;
                                if (int.TryParse(dtRow["HoanTatSongC"]?.ToString(), out int htC))
                                    dh.HoanTatSongC = htC;
                                if (int.TryParse(dtRow["HoanTatGiaySongC"]?.ToString(), out int htsC))
                                    dh.HoanTatGiaySongC = htsC;
                                if (int.TryParse(dtRow["HoanTatGiayMatC"]?.ToString(), out int htmC))
                                    dh.HoanTatGiayMatC = htmC;
                                if (int.TryParse(dtRow["HoanTatSongE"]?.ToString(), out int htE))
                                    dh.HoanTatSongE = htE;
                                if (int.TryParse(dtRow["HoanTatGiaySongE"]?.ToString(), out int htsE))
                                    dh.HoanTatGiaySongE = htsE;
                                if (int.TryParse(dtRow["HoanTatGiayMatE"]?.ToString(), out int htmE))
                                    dh.HoanTatGiayMatE = htmE;

                                if (int.TryParse(dtRow["HoanTatSpliter"]?.ToString(), out int htSpliter))
                                    dh.HoanTatSpliter = htSpliter;
                                donHangs.Add(dh);
                            }
                        }
                        return new List<DonHang>(donHangs.OrderBy(x => x.STT));
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return donHangs;
        }
    }
}
