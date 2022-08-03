using CommonControls;
using Microsoft.AspNet.SignalR.Client;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
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

namespace MaySong
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BrushConverter brushConverter = new BrushConverter();
        public HubConnection hubConnection;
        public IHubProxy hubProxy;
        private Task task;
        DispatcherFacade DispatcherFacade { get; set; }
        MySqlConnection conn;
        MySqlCommand cmd;
        public string MaySong = Properties.Settings.Default["May"].ToString().ToUpper();

        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            DispatcherFacade = new DispatcherFacade(Application.Current.Dispatcher);
            Loaded += MainWindow_Loaded;
            danhSachDonHang.BeginEdit += DanhSachDonHang_BeginEdit;
            PreviewKeyDown += MainWindow_PreviewKeyDown;

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
            else if (e.Key == Key.F2)   
            {
                BtnLoi_Click(null, null);
            }
            else if (e.Key == Key.F12)
            {
                ChonSongWindow window = new ChonSongWindow();
                window.ShowDialog();
            }
            else if (e.Key == Key.F3)
            {
                BtnCapNhat_Click(null, null);
            }
        }

        private void UpdateUI()
        {
            while(true)
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
                                if (MaySong.ToLower() == "e")
                                {
                                    HienThiDonSong(wrapData.GiaySongGiayMatDangChayE);
                                }
                                else if (MaySong.ToLower() == "b")
                                {
                                    HienThiDonSong(wrapData.GiaySongGiayMatDangChayB);
                                }
                                else if (MaySong.ToLower() == "c")
                                {
                                    HienThiDonSong(wrapData.GiaySongGiayMatDangChayC);
                                }
                                else if (MaySong.ToLower() == "m")
                                {
                                    HienThiDonSong(wrapData.GiayMenDangChay);
                                }

                                HienThiDonHangDangChay(wrapData.TrangThaiDonHang);
                                HienThiThongTinTram(wrapData.ThongTinTram);
                                HienThiDanhSachDonHang(wrapData.DanhSachDonHang);

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

            //switch (Properties.Settings.Default["May"].ToString().ToUpper())
            //{
            //    case "E":
            //        hubProxy.On<string>("broadcastGiayE", HienThiDonSong);
            //        break;
            //    case "B":
            //        hubProxy.On<string>("broadcastGiayB", HienThiDonSong);
            //        break;
            //    case "C":
            //        hubProxy.On<string>("broadcastGiayC", HienThiDonSong);
            //        break;
            //    default:
            //        break;
            //}

            //hubProxy.On<string>("broadcastTrangThaiDonHang", HienThiDonHangDangChay);
            //hubProxy.On<string>("broadcastThongTinTram", HienThiThongTinTram);
            //hubProxy.On<string>("broadcastDanhSachDonHang", HienThiDanhSachDonHang);

            try
            {
                await hubConnection.Start();
            }
            catch { }
            btnDonHang.Click += BtnDonHang_Click;
            btnLoi.Click += BtnLoi_Click;
            btnCapNhat.Click += BtnCapNhat_Click;
        }

        private void BtnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (hubProxy != null && hubConnection != null && hubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                {
                    hubProxy.Invoke("capNhat", MaySong);
                }
            }
            catch { }
        }

        private void BtnLoi_Click(object sender, RoutedEventArgs e)
        {
            ChinhSuaMetLoi form = new ChinhSuaMetLoi(this);
            form.ShowDialog();
        }

        private void BtnDonHang_Click(object sender, RoutedEventArgs e)
        {
            DonHangForm form = new DonHangForm();
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

                //switch (Properties.Settings.Default["May"].ToString().ToUpper())
                //{
                //    case "E":
                //        hubProxy.On<string>("broadcastGiayE", HienThiDonSong);
                //        break;
                //    case "B":
                //        hubProxy.On<string>("broadcastGiayB", HienThiDonSong);
                //        break;
                //    case "C":
                //        hubProxy.On<string>("broadcastGiayC", HienThiDonSong);
                //        break;
                //    default:
                //        break;
                //}

                //hubProxy.On<string>("broadcastTrangThaiDonHang", HienThiDonHangDangChay);
                //hubProxy.On<string>("broadcastThongTinTram", HienThiThongTinTram);
                //hubProxy.On<string>("broadcastDanhSachDonHang", HienThiDanhSachDonHang);

                await hubConnection.Start();
            }
            catch { }
        }

        public void HienThiDonSong(GiaySongGiayMatDangChay x)
        {
            try
            {
                
                //Dispatcher.Invoke(() =>
                //{
                //    thongTinMaySong.DoiGiaySong = x.DoiGiaySong;
                //    thongTinMaySong.DoiGiayMat = x.DoiGiayMat;

                //    thongTinMaySong.KhoSong1 = x.KhoSong1;
                //    thongTinMaySong.LoaiGiaySong1 = x.LoaiGiaySong1;
                //    thongTinMaySong.ChieuDaiSong1 = x.ChieuDaiSong1;
                //    thongTinMaySong.KhoSong2 = x.KhoSong2;
                //    thongTinMaySong.LoaiGiaySong2 = x.LoaiGiaySong2;
                //    thongTinMaySong.ChieuDaiSong2 = x.ChieuDaiSong2;

                //    thongTinMaySong.KhoMat1 = x.KhoMat1;
                //    thongTinMaySong.LoaiGiayMat1 = x.LoaiGiayMat1;
                //    thongTinMaySong.ChieuDaiMat1 = x.ChieuDaiMat1;
                //    thongTinMaySong.KhoMat2 = x.KhoMat2;
                //    thongTinMaySong.LoaiGiayMat2 = x.LoaiGiayMat2;
                //    thongTinMaySong.ChieuDaiMat2 = x.ChieuDaiMat2;

                //    thongTinMaySong.STTSong = x.STTSong;
                //    thongTinMaySong.STTMat = x.STTMat;
                //    thongTinMaySong.TenSong = $"MÁY SÓNG {Properties.Settings.Default["May"].ToString().ToUpper()} - {x.STTDon}";
                //});

                DispatcherFacade.AddToDispatcherQueue(new Action(() =>
                {
                    try
                    {
                        thongTinMaySong.DoiGiaySong = x.DoiGiaySong;
                        thongTinMaySong.DoiGiayMat = x.DoiGiayMat;

                        thongTinMaySong.KhoSong1 = x.KhoSong1;
                        thongTinMaySong.LoaiGiaySong1 = x.LoaiGiaySong1;
                        thongTinMaySong.ChieuDaiSong1 = x.ChieuDaiSong1;
                        thongTinMaySong.KhoSong2 = x.KhoSong2;
                        thongTinMaySong.LoaiGiaySong2 = x.LoaiGiaySong2;
                        thongTinMaySong.ChieuDaiSong2 = x.ChieuDaiSong2;

                        thongTinMaySong.KhoMat1 = x.KhoMat1;
                        thongTinMaySong.LoaiGiayMat1 = x.LoaiGiayMat1;
                        thongTinMaySong.ChieuDaiMat1 = x.ChieuDaiMat1;
                        thongTinMaySong.KhoMat2 = x.KhoMat2;
                        thongTinMaySong.LoaiGiayMat2 = x.LoaiGiayMat2;
                        thongTinMaySong.ChieuDaiMat2 = x.ChieuDaiMat2;

                        thongTinMaySong.STTSong = x.STTSong;
                        thongTinMaySong.STTMat = x.STTMat;

                        string tenSong = "";
                        switch (Properties.Settings.Default["May"].ToString().ToUpper())
                        {
                            case "E":
                                tenSong = "1"; break;
                            case "B":
                                tenSong = "2"; break;
                            case "C":
                                tenSong = "3"; break;
                            case "M":
                                tenSong = "";break;
                            default:
                                break;
                        }


                        if (tenSong == "")
                        {
                            thongTinMaySong.TenSong = $"MÁY MỀN - {x.STTDon}";
                        }
                        else
                        {
                            thongTinMaySong.TenSong = $"MÁY SÓNG {tenSong} - {x.STTDon}";
                        }

                        thongTinMaySong.STTMat2 = x.STTMat2;
                        thongTinMaySong.STTSong2 = x.STTSong2;
                    }
                    catch { }
                }));
            }
            catch { }
        }

        public void HienThiDonHangDangChay(TrangThaiDonHang trangThaiDonHang)
        {
            try
            {
                if (trangThaiDonHang == null)
                    trangThaiDonHang = new TrangThaiDonHang();
                //Dispatcher.Invoke(() =>
                //{
                //    donHangDangChay.TK1 = trangThaiDonHang.STT1;
                //    donHangDangChay.SoMet1 = trangThaiDonHang.SoMet1;
                //    donHangDangChay.SoMetDat1 = trangThaiDonHang.SoMetDat1;
                //    donHangDangChay.SoMetLoi1 = trangThaiDonHang.SoMetLoi1;
                //    donHangDangChay.ConLai1 = trangThaiDonHang.ConLai1;
                //    donHangDangChay.PhanTramLoi1 = trangThaiDonHang.PhanTramLoi1;
                //    donHangDangChay.TocDoTrungBinh1 = trangThaiDonHang.TocDoTB1;
                //    donHangDangChay.Chay1 = trangThaiDonHang.Chay1;
                //    donHangDangChay.Dung1 = trangThaiDonHang.Dung1;
                //    donHangDangChay.SoDung1 = trangThaiDonHang.SoDung1;
                //    donHangDangChay.M2Dat1 = trangThaiDonHang.M2Dat1;
                //    donHangDangChay.M2Loi1 = trangThaiDonHang.M2Loi1;

                //    donHangDangChay.TK2 = trangThaiDonHang.STT2;
                //    donHangDangChay.SoMet2 = trangThaiDonHang.SoMet2;
                //    donHangDangChay.SoMetDat2 = trangThaiDonHang.SoMetDat2;
                //    donHangDangChay.SoMetLoi2 = trangThaiDonHang.SoMetLoi2;
                //    donHangDangChay.ConLai2 = trangThaiDonHang.ConLai2;
                //    donHangDangChay.PhanTramLoi2 = trangThaiDonHang.PhanTramLoi2;
                //    donHangDangChay.TocDoTrungBinh2 = trangThaiDonHang.TocDoTB2;
                //    donHangDangChay.Chay2 = trangThaiDonHang.Chay2;
                //    donHangDangChay.Dung2 = trangThaiDonHang.Dung2;
                //    donHangDangChay.SoDung2 = trangThaiDonHang.SoDung2;
                //    donHangDangChay.M2Dat2 = trangThaiDonHang.M2Dat2;
                //    donHangDangChay.M2Loi2 = trangThaiDonHang.M2Loi2;
                //});

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
            //Dispatcher.Invoke(() =>
            //{
            //    thongTinCacTram.TocDo1 = thongTinTram.TocDo1;
            //    thongTinCacTram.TocDo2 = thongTinTram.TocDo2;
            //    thongTinCacTram.TocDo3 = thongTinTram.TocDo3;
            //    thongTinCacTram.TocDo4 = thongTinTram.TocDo4;
            //    thongTinCacTram.TocDo5 = thongTinTram.TocDo5;
            //    thongTinCacTram.TocDo6 = thongTinTram.TocDo6;

            //    thongTinCacTram.DoiDon1 = thongTinTram.DoiDon1;
            //    thongTinCacTram.DoiDon2 = thongTinTram.DoiDon2;
            //    thongTinCacTram.DoiDon3 = thongTinTram.DoiDon3;
            //    thongTinCacTram.DoiDon4 = thongTinTram.DoiDon4;
            //    thongTinCacTram.DoiDon5 = thongTinTram.DoiDon5;
            //    thongTinCacTram.DoiDon6 = thongTinTram.DoiDon6;

            //    thongTinCacTram.Dan1 = thongTinTram.Dan1;
            //    thongTinCacTram.Dan2 = thongTinTram.Dan2;
            //    thongTinCacTram.Dan3 = thongTinTram.Dan3;
            //    thongTinCacTram.Dan4 = thongTinTram.Dan4;
            //    thongTinCacTram.Dan5 = thongTinTram.Dan5;
            //    thongTinCacTram.Dan6 = thongTinTram.Dan6;

            //    Chuẩn bị đơn
            //    if (thongTinTram.TrangThai4 == "1")
            //    {
            //        thongTinCacTram.TrangThai4 = Brushes.Orange;
            //    }
            //    Đang chạy
            //    else if (thongTinTram.TrangThai4 == "2")
            //    {
            //        thongTinCacTram.TrangThai4 = Brushes.LimeGreen;
            //    }
            //    Stop
            //    else
            //    {
            //        thongTinCacTram.TrangThai4 = Brushes.Red;
            //    }

            //    Chuẩn bị đơn
            //    if (thongTinTram.TrangThai5 == "1")
            //    {
            //        thongTinCacTram.TrangThai5 = Brushes.Orange;
            //    }
            //    Đang chạy
            //    else if (thongTinTram.TrangThai5 == "2")
            //    {
            //        thongTinCacTram.TrangThai5 = Brushes.LimeGreen;
            //    }
            //    Stop
            //    else
            //    {
            //        thongTinCacTram.TrangThai5 = Brushes.Red;
            //    }

            //    Chuẩn bị đơn
            //    if (thongTinTram.TrangThai6 == "1")
            //    {
            //        thongTinCacTram.TrangThai6 = Brushes.Orange;
            //    }
            //    Đang chạy
            //    else if (thongTinTram.TrangThai6 == "2")
            //    {
            //        thongTinCacTram.TrangThai6 = Brushes.LimeGreen;
            //    }
            //    Stop
            //    else
            //    {
            //        thongTinCacTram.TrangThai6 = Brushes.Red;
            //    }
            //});

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

        private DateTime LastUpdate = DateTime.MinValue;
        public void HienThiDanhSachDonHang(List<DonHang> danhSachDonHang)
        {
            try
            {           
                if ((DateTime.Now - LastUpdate).TotalSeconds >= 2)
                {
                    NotifyCollection<DonHang> donhangs = new NotifyCollection<DonHang>();
                    if (danhSachDonHang == null)
                    {

                    }
                    else
                    {
                        if (MaySong.ToLower() == "e")
                        {
                            donhangs = new NotifyCollection<DonHang>(danhSachDonHang.Where(x => x.HoanTatCutter == 0 && x.HoanTatSongE == 0).OrderBy(x => x.STT));
                        }
                        else if (MaySong.ToLower() == "b")
                        {
                            donhangs = new NotifyCollection<DonHang>(danhSachDonHang.Where(x => x.HoanTatCutter == 0 && x.HoanTatSongB == 0).OrderBy(x => x.STT));
                        }
                        else if (MaySong.ToLower() == "c")
                        {
                            donhangs = new NotifyCollection<DonHang>(danhSachDonHang.Where(x => x.HoanTatCutter == 0 && x.HoanTatSongC == 0).OrderBy(x => x.STT));
                        }
                        else if (MaySong.ToLower() == "m")
                        {
                            donhangs = new NotifyCollection<DonHang>(danhSachDonHang.Where(x => x.HoanTatMayMen == 0));
                        }
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

        //public void HienThiDonSong(string message)
        //{
        //    try
        //    {
        //        GiaySongGiayMatDangChay x = JsonConvert.DeserializeObject<GiaySongGiayMatDangChay>(message);
        //        if (x == null)
        //            x = new GiaySongGiayMatDangChay();

        //        DispatcherFacade.AddToDispatcherQueue(new Action(() =>
        //        {
        //            thongTinMaySong.DoiGiaySong = x.DoiGiaySong;
        //            thongTinMaySong.DoiGiayMat = x.DoiGiayMat;

        //            thongTinMaySong.KhoSong1 = x.KhoSong1;
        //            thongTinMaySong.LoaiGiaySong1 = x.LoaiGiaySong1;
        //            thongTinMaySong.ChieuDaiSong1 = x.ChieuDaiSong1;
        //            thongTinMaySong.KhoSong2 = x.KhoSong2;
        //            thongTinMaySong.LoaiGiaySong2 = x.LoaiGiaySong2;
        //            thongTinMaySong.ChieuDaiSong2 = x.ChieuDaiSong2;

        //            thongTinMaySong.KhoMat1 = x.KhoMat1;
        //            thongTinMaySong.LoaiGiayMat1 = x.LoaiGiayMat1;
        //            thongTinMaySong.ChieuDaiMat1 = x.ChieuDaiMat1;
        //            thongTinMaySong.KhoMat2 = x.KhoMat2;
        //            thongTinMaySong.LoaiGiayMat2 = x.LoaiGiayMat2;
        //            thongTinMaySong.ChieuDaiMat2 = x.ChieuDaiMat2;

        //            thongTinMaySong.STTSong = x.STTSong;
        //            thongTinMaySong.STTMat = x.STTMat;
        //            thongTinMaySong.TenSong = $"MÁY SÓNG {Properties.Settings.Default["May"].ToString().ToUpper()} - {x.STTDon}";
        //        }));
        //    }
        //    catch { }
        //}

        //public void HienThiDonHangDangChay(string message)
        //{
        //    try
        //    {
        //        TrangThaiDonHang trangThaiDonHang = JsonConvert.DeserializeObject<TrangThaiDonHang>(message);
        //        if (trangThaiDonHang == null)
        //            trangThaiDonHang = new TrangThaiDonHang();
        //        DispatcherFacade.AddToDispatcherQueue(new Action(() =>
        //        {
        //            donHangDangChay.TK1 = trangThaiDonHang.STT1;
        //            donHangDangChay.SoMet1 = trangThaiDonHang.SoMet1;
        //            donHangDangChay.SoMetDat1 = trangThaiDonHang.SoMetDat1;
        //            donHangDangChay.SoMetLoi1 = trangThaiDonHang.SoMetLoi1;
        //            donHangDangChay.ConLai1 = trangThaiDonHang.ConLai1;
        //            donHangDangChay.PhanTramLoi1 = trangThaiDonHang.PhanTramLoi1;
        //            donHangDangChay.TocDoTrungBinh1 = trangThaiDonHang.TocDoTB1;
        //            donHangDangChay.Chay1 = trangThaiDonHang.Chay1;
        //            donHangDangChay.Dung1 = trangThaiDonHang.Dung1;
        //            donHangDangChay.SoDung1 = trangThaiDonHang.SoDung1;
        //            donHangDangChay.M2Dat1 = trangThaiDonHang.M2Dat1;
        //            donHangDangChay.M2Loi1 = trangThaiDonHang.M2Loi1;

        //            donHangDangChay.TK2 = trangThaiDonHang.STT2;
        //            donHangDangChay.SoMet2 = trangThaiDonHang.SoMet2;
        //            donHangDangChay.SoMetDat2 = trangThaiDonHang.SoMetDat2;
        //            donHangDangChay.SoMetLoi2 = trangThaiDonHang.SoMetLoi2;
        //            donHangDangChay.ConLai2 = trangThaiDonHang.ConLai2;
        //            donHangDangChay.PhanTramLoi2 = trangThaiDonHang.PhanTramLoi2;
        //            donHangDangChay.TocDoTrungBinh2 = trangThaiDonHang.TocDoTB2;
        //            donHangDangChay.Chay2 = trangThaiDonHang.Chay2;
        //            donHangDangChay.Dung2 = trangThaiDonHang.Dung2;
        //            donHangDangChay.SoDung2 = trangThaiDonHang.SoDung2;
        //            donHangDangChay.M2Dat2 = trangThaiDonHang.M2Dat2;
        //            donHangDangChay.M2Loi2 = trangThaiDonHang.M2Loi2;
        //        }));
        //    }
        //    catch { }
        //}

        //public void HienThiThongTinTram(string message)
        //{
        //    ThongTinTram thongTinTram = JsonConvert.DeserializeObject<ThongTinTram>(message);
        //    if (thongTinTram == null)
        //        thongTinTram = new ThongTinTram();
        //    DispatcherFacade.AddToDispatcherQueue(new Action(() =>
        //    {
        //        thongTinCacTram.TocDo1 = thongTinTram.TocDo1;
        //        thongTinCacTram.TocDo2 = thongTinTram.TocDo2;
        //        thongTinCacTram.TocDo3 = thongTinTram.TocDo3;
        //        thongTinCacTram.TocDo4 = thongTinTram.TocDo4;
        //        thongTinCacTram.TocDo5 = thongTinTram.TocDo5;
        //        thongTinCacTram.TocDo6 = thongTinTram.TocDo6;

        //        thongTinCacTram.DoiDon1 = thongTinTram.DoiDon1;
        //        thongTinCacTram.DoiDon2 = thongTinTram.DoiDon2;
        //        thongTinCacTram.DoiDon3 = thongTinTram.DoiDon3;
        //        thongTinCacTram.DoiDon4 = thongTinTram.DoiDon4;
        //        thongTinCacTram.DoiDon5 = thongTinTram.DoiDon5;
        //        thongTinCacTram.DoiDon6 = thongTinTram.DoiDon6;

        //        thongTinCacTram.Dan1 = thongTinTram.Dan1;
        //        thongTinCacTram.Dan2 = thongTinTram.Dan2;
        //        thongTinCacTram.Dan3 = thongTinTram.Dan3;
        //        thongTinCacTram.Dan4 = thongTinTram.Dan4;
        //        thongTinCacTram.Dan5 = thongTinTram.Dan5;
        //        thongTinCacTram.Dan6 = thongTinTram.Dan6;

        //        // Chuẩn bị đơn
        //        if (thongTinTram.TrangThai4 == "1")
        //        {
        //            thongTinCacTram.TrangThai4 = Brushes.Orange;
        //        }
        //        // Đang chạy
        //        else if (thongTinTram.TrangThai4 == "2")
        //        {
        //            thongTinCacTram.TrangThai4 = Brushes.LimeGreen;
        //        }
        //        // Stop
        //        else
        //        {
        //            thongTinCacTram.TrangThai4 = Brushes.Red;
        //        }

        //        // Chuẩn bị đơn
        //        if (thongTinTram.TrangThai5 == "1")
        //        {
        //            thongTinCacTram.TrangThai5 = Brushes.Orange;
        //        }
        //        // Đang chạy
        //        else if (thongTinTram.TrangThai5 == "2")
        //        {
        //            thongTinCacTram.TrangThai5 = Brushes.LimeGreen;
        //        }
        //        // Stop
        //        else
        //        {
        //            thongTinCacTram.TrangThai5 = Brushes.Red;
        //        }

        //        // Chuẩn bị đơn
        //        if (thongTinTram.TrangThai6 == "1")
        //        {
        //            thongTinCacTram.TrangThai6 = Brushes.Orange;
        //        }
        //        // Đang chạy
        //        else if (thongTinTram.TrangThai6 == "2")
        //        {
        //            thongTinCacTram.TrangThai6 = Brushes.LimeGreen;
        //        }
        //        // Stop
        //        else
        //        {
        //            thongTinCacTram.TrangThai6 = Brushes.Red;
        //        }

        //    }));
        //}

        //public void HienThiDanhSachDonHang(string message)
        //{
        //    try
        //    {
        //        List<DonHang> danhSachDonHang = JsonConvert.DeserializeObject<List<DonHang>>(message);
        //        NotifyCollection<DonHang> donhangs = new NotifyCollection<DonHang>();
        //        if (danhSachDonHang == null)
        //        {

        //        }
        //        else
        //        {
        //            if (MaySong.ToLower() == "e")
        //            {
        //                donhangs = new NotifyCollection<DonHang>(danhSachDonHang.Where(x => x.HoanTatCutter == 0 && x.HoanTatSongE == 0).OrderBy(x => x.STT));
        //            }
        //            else if (MaySong.ToLower() == "b")
        //            {
        //                donhangs = new NotifyCollection<DonHang>(danhSachDonHang.Where(x => x.HoanTatCutter == 0 && x.HoanTatSongB == 0).OrderBy(x => x.STT));
        //            }
        //            else if (MaySong.ToLower() == "c")
        //            {
        //                donhangs = new NotifyCollection<DonHang>(danhSachDonHang.Where(x => x.HoanTatCutter == 0 && x.HoanTatSongC == 0).OrderBy(x => x.STT));
        //            }
        //        }

        //        DispatcherFacade.AddToDispatcherQueue(new Action(() =>
        //        {
        //            if (this.danhSachDonHang.DonHangDataSource == null)
        //                this.danhSachDonHang.DonHangDataSource = donhangs;
        //            else
        //            {
        //                this.danhSachDonHang.DonHangDataSource.DisableNotifyChanged = true;
        //                this.danhSachDonHang.DonHangDataSource.Clear();
        //                this.danhSachDonHang.DonHangDataSource.AddRange(donhangs);
        //                this.danhSachDonHang.DonHangDataSource.DisableNotifyChanged = false;
        //                this.danhSachDonHang.DonHangDataSource.NotifyResetCollection();
        //            }
        //        }));
        //    }
        //    catch { }
        //}
    }
}
