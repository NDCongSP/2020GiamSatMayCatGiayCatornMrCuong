using CommonControls;
using Microsoft.AspNet.SignalR.Client;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MayCatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int DaiOffset = 30;

        private BrushConverter brushConverter = new BrushConverter();
        public HubConnection hubConnection;
        public IHubProxy hubProxy;
        private Task task;
        DispatcherFacade DispatcherFacade { get; set; }
        MySqlConnection conn;
        MySqlCommand cmd;

        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            DispatcherFacade = new DispatcherFacade(Application.Current.Dispatcher);
            Loaded += MainWindow_Loaded;
            danhSachDonHang.BeginEdit += DanhSachDonHang_BeginEdit;
            PreviewKeyDown += MainWindow_PreviewKeyDown;

            if (File.Exists("Dai.txt"))
            {
                if (int.TryParse(File.ReadAllText("Dai.txt"), out int result))
                {
                    DaiOffset = result;
                }
            }

            try
            {
                conn = new MySqlConnection(Properties.Settings.Default["constr"].ToString());
                conn.Open();
                cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select data from common";
            }
            catch { }

            task = Task.Factory.StartNew(UpdateUI, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                BtnDonHang_Click(null, null);
            }
            if (e.Key == Key.F3)
            {
                BtnBaoCao_Click(null, null);
            }
        }

        public static List<DonHang> donHangServer;

        private void UpdateUI()
        {
            while (true)
            {
                try
                {
                    DispatcherFacade.AddToDispatcherQueue(new Action(() =>
                    {
                        mainGrid.Background = NormalBackground;
                    }));

                    using (MySqlDataAdapter adp = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            string wrapDataString = dt.Rows[0][0].ToString();
                            WrapData wrapData = JsonConvert.DeserializeObject<WrapData>(wrapDataString);
                            if (wrapData != null)
                            {
                                long sttDH = 0;
                                if (wrapData.DonHangChuanBi != null)
                                {
                                    long.TryParse(wrapData.DonHangChuanBi.CatTam1, out sttDH);
                                }

                                HienThiDonHangDangChay(wrapData.TrangThaiDonHang);
                                HienThiThongTinTram(wrapData.ThongTinTram);
                                HienThiDonHangChuanBi(wrapData.DonHangChuanBi);
                                HienThiDanhSachDonHang(wrapData.DanhSachDonHang, sttDH);

                                Dispatcher.Invoke(() =>
                                {
                                    lbChuyenDon.Content = wrapData.CheDoChuyenDon;
                                });
                                donHangServer = wrapData.DanhSachDonHang;
                                Debug.WriteLine($"Tg cập nhật: {DateTime.Now.ToString("HH:mm:ss")}");
                            }
                        }
                    }
                }
                catch
                {

                    try
                    {
                        DispatcherFacade.AddToDispatcherQueue(new Action(() =>
                        {
                            mainGrid.Background = Brushes.Red;
                        }));

                        conn = new MySqlConnection(Properties.Settings.Default["constr"].ToString());
                        conn.Open();
                        cmd = new MySqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "select data from common";
                    }
                    catch { }
                }
                finally { Thread.Sleep(100); }
            }
        }

        private void DanhSachDonHang_BeginEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        Brush NormalBackground;

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            NormalBackground = mainGrid.Background;
            hubConnection = new HubConnection(Properties.Settings.Default["serverUrl"].ToString());
            hubConnection.Closed += HubConnection_Closed;
            hubConnection.StateChanged += HubConnection_StateChanged;
            hubConnection.Error += HubConnection_Error;
            hubProxy = hubConnection.CreateHubProxy("cutterHub");

            try
            {
                await hubConnection.Start();
            }
            catch { }


            btnDonHang.Click += BtnDonHang_Click;
            btnBaoCao.Click += BtnBaoCao_Click;
        }

        private void BtnBaoCao_Click(object sender, RoutedEventArgs e)
        {

            BaoCaoWindow form = new BaoCaoWindow();
            form.ShowDialog();
        }

        private void BtnDonHang_Click(object sender, RoutedEventArgs e)
        {
            DonHangForm form = new DonHangForm(this);
            form.ShowDialog();
        }

        private void HubConnection_Error(Exception obj)
        {
        }

        private void HubConnection_StateChanged(StateChange obj)
        {
            if (obj.NewState == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
            {
                DispatcherFacade.AddToDispatcherQueue(new Action(() =>
                {
                    mainGrid.Background = NormalBackground;
                }));
            }
            else
            {
                DispatcherFacade.AddToDispatcherQueue(new Action(() =>
                {
                    mainGrid.Background = Brushes.Red;
                }));
            }
        }

        private void HubConnection_Closed()
        {
            Thread.Sleep(2000);
            StartHub();
        }

        private async void StartHub()
        {
            try
            {
                if (hubConnection != null)
                {
                    try
                    {
                        hubConnection.StateChanged -= HubConnection_StateChanged;
                        hubConnection.Closed -= HubConnection_Closed;
                        if (hubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                            await Task.Factory.StartNew(() => hubConnection.Stop());
                        await Task.Factory.StartNew(() => hubConnection.Dispose());
                    }
                    catch { }
                }

                hubConnection = new HubConnection(Properties.Settings.Default["serverUrl"].ToString());
                hubConnection.Closed += HubConnection_Closed;
                hubConnection.StateChanged += HubConnection_StateChanged;
                hubProxy = hubConnection.CreateHubProxy("cutterHub");

                await hubConnection.Start();
            }
            catch { }
        }

        public void HienThiDonHangDangChay(TrangThaiDonHang trangThaiDonHang)
        {
            try
            {
                if (trangThaiDonHang == null)
                    trangThaiDonHang = new TrangThaiDonHang();

                DispatcherFacade.AddToDispatcherQueue(new Action(() =>
                {
                    try
                    {
                        donHangDangChay.TK1 = trangThaiDonHang.STT1;
                        donHangDangChay.SoMet1 = trangThaiDonHang.SoMet1;
                        donHangDangChay.SoMetDat1 = trangThaiDonHang.SoMetDat1;
                        donHangDangChay.SoMetLoi1 = trangThaiDonHang.SoMetLoi1;
                        donHangDangChay.ConLai1 = trangThaiDonHang.ConLai1;
                        donHangDangChay.PhanTramLoi1 = trangThaiDonHang.PhanTramLoi1;
                        donHangDangChay.TocDoTrungBinh1 = trangThaiDonHang.TocDoTB1;
                        donHangDangChay.Chay1 = trangThaiDonHang.Chay1;
                        donHangDangChay.Dung1 = trangThaiDonHang.Dung1;
                        donHangDangChay.SoDung1 = trangThaiDonHang.SoDung1;
                        donHangDangChay.M2Dat1 = trangThaiDonHang.M2Dat1;
                        donHangDangChay.M2Loi1 = trangThaiDonHang.M2Loi1;

                        donHangDangChay.TK2 = trangThaiDonHang.STT2;
                        donHangDangChay.SoMet2 = trangThaiDonHang.SoMet2;
                        donHangDangChay.SoMetDat2 = trangThaiDonHang.SoMetDat2;
                        donHangDangChay.SoMetLoi2 = trangThaiDonHang.SoMetLoi2;
                        donHangDangChay.ConLai2 = trangThaiDonHang.ConLai2;
                        donHangDangChay.PhanTramLoi2 = trangThaiDonHang.PhanTramLoi2;
                        donHangDangChay.TocDoTrungBinh2 = trangThaiDonHang.TocDoTB2;
                        donHangDangChay.Chay2 = trangThaiDonHang.Chay2;
                        donHangDangChay.Dung2 = trangThaiDonHang.Dung2;
                        donHangDangChay.SoDung2 = trangThaiDonHang.SoDung2;
                        donHangDangChay.M2Dat2 = trangThaiDonHang.M2Dat2;
                        donHangDangChay.M2Loi2 = trangThaiDonHang.M2Loi2;
                    }
                    catch { }
                }));
            }
            catch { }
        }

        public void HienThiThongTinTram(ThongTinTram thongTinTram)
        {
            if (thongTinTram == null)
                thongTinTram = new ThongTinTram();
           
            DispatcherFacade.AddToDispatcherQueue(new Action(() =>
            {
                try
                {
                    thongTinCacTram.TocDo1 = thongTinTram.TocDo1;
                    thongTinCacTram.TocDo2 = thongTinTram.TocDo2;
                    thongTinCacTram.TocDo3 = thongTinTram.TocDo3;
                    thongTinCacTram.TocDo4 = thongTinTram.TocDo4;
                    thongTinCacTram.TocDo5 = thongTinTram.TocDo5;
                    thongTinCacTram.TocDo6 = thongTinTram.TocDo6;

                    thongTinCacTram.DoiDon1 = thongTinTram.DoiDon1;
                    thongTinCacTram.DoiDon2 = thongTinTram.DoiDon2;
                    thongTinCacTram.DoiDon3 = thongTinTram.DoiDon3;
                    thongTinCacTram.DoiDon4 = thongTinTram.DoiDon4;
                    thongTinCacTram.DoiDon5 = thongTinTram.DoiDon5;
                    thongTinCacTram.DoiDon6 = thongTinTram.DoiDon6;

                    thongTinCacTram.Dan1 = thongTinTram.Dan1;
                    thongTinCacTram.Dan2 = thongTinTram.Dan2;
                    thongTinCacTram.Dan3 = thongTinTram.Dan3;
                    thongTinCacTram.Dan4 = thongTinTram.Dan4;
                    thongTinCacTram.Dan5 = thongTinTram.Dan5;
                    thongTinCacTram.Dan6 = thongTinTram.Dan6;

                    var tt4 = brushConverter.ConvertFromString(thongTinTram.TrangThai4);
                    if (tt4 is Brush tt4Brush)
                        thongTinCacTram.TrangThai4 = tt4Brush;

                    var tt5 = brushConverter.ConvertFromString(thongTinTram.TrangThai5);
                    if (tt5 is Brush tt5Brush)
                        thongTinCacTram.TrangThai5 = tt5Brush;

                    var tt6 = brushConverter.ConvertFromString(thongTinTram.TrangThai6);
                    if (tt6 is Brush tt6Brush)
                        thongTinCacTram.TrangThai6 = tt6Brush;
                }
                catch { }
            }));
        }

        public void HienThiDonHangChuanBi(DonHangChuanBi dhChuanBi)
        {
            if (dhChuanBi == null)
                dhChuanBi = new DonHangChuanBi();

            DispatcherFacade.AddToDispatcherQueue(new Action(() =>
            {
                try
                {
                    donHangChuanBi.CatTam1 = dhChuanBi.CatTam1;
                    donHangChuanBi.CatTam2 = dhChuanBi.CatTam2;
                    donHangChuanBi.CatTam3 = dhChuanBi.CatTam3;

                    donHangChuanBi.DaiCat1 = dhChuanBi.DaiCat1;
                    donHangChuanBi.DaiCat2 = dhChuanBi.DaiCat2;
                    donHangChuanBi.DaiCat3 = dhChuanBi.DaiCat3;

                    donHangChuanBi.SLCat1 = dhChuanBi.SLCat1;
                    donHangChuanBi.SLCat2 = dhChuanBi.SLCat2;
                    donHangChuanBi.SLCat3 = dhChuanBi.SLCat3;

                    donHangChuanBi.Pallet1 = dhChuanBi.Pallet1;
                    donHangChuanBi.Pallet2 = dhChuanBi.Pallet2;
                    donHangChuanBi.Pallet3 = dhChuanBi.Pallet3;

                    donHangChuanBi.SLDat1 = dhChuanBi.SLDat1;
                    donHangChuanBi.SLConLai1 = dhChuanBi.SLConLai1;
                    donHangChuanBi.SLLoi1 = dhChuanBi.SLLoi1;
                }
                catch { }
            }));
        }

        private DateTime LastUpdate = DateTime.MinValue;
        public void HienThiDanhSachDonHang(List<DonHang> danhSachDonHang, long sttDH)
        {
            try
            {
                if ((DateTime.Now - LastUpdate).TotalSeconds >= 1.5)
                {
                    NotifyCollection<DonHang> donhangs = new NotifyCollection<DonHang>();
                    if (danhSachDonHang == null)
                    {

                    }
                    else
                    {
                        donhangs = new NotifyCollection<DonHang>(danhSachDonHang.Where(x => x.HoanTatCutter == 0 && x.STT >= sttDH).OrderBy(x => x.STT));
                    }
                    DispatcherFacade.AddToDispatcherQueue(new Action(() =>
                    {
                        try
                        {
                            this.danhSachDonHang.DonHangDataSource = donhangs;
                        }
                        catch { }
                    }));
                    LastUpdate = DateTime.Now;
                }
            }
            catch { }
        }
    }
}
