using CommonControls;
using DevExpress.Mvvm;
using EasyScada.Core;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            object obj = SongETags.Instance;
            obj = SongBTags.Instance;
            obj = SongCTags.Instance;
            obj = CutterTags.Instance;
            obj = MayMenTags.Instance;

            obj = CutterController.Instance;
            obj = SongBController.Instance;
            obj = SongCController.Instance;
            obj = MayMenController.Instance;
            obj = SongEController.Instance;

            obj = UpdaterDonHang.Instance;


            try
            {
                CaiDat = Repository.Instance.GetCaiDat();
                DonHangDataSource.AddRange(Repository.Instance.GetDonHangs());

            }
            catch { }

            DriverConnector = EasyDriverConnectorProvider.GetEasyDriverConnector();
            DonHangHienThi = new NotifyCollection<DonHang>();
            danhSachDonHang.DonHangDataSource = DonHangHienThi;

            Messenger.Default.Register<MessageChuyenDonHang>(this, OnMessageChuyenDonHang);
            Messenger.Default.Register<MessageChuyenDonGiayMat>(this, OnMessageChuyenDonGiayMat);
            Messenger.Default.Register<MessageChuyenDonGiaySong>(this, OnMessageChuyenDonGiaySong);
            Messenger.Default.Register<MessageKiemTraChieuDai>(this, OnMessageKiemTraChieuDai);
            Messenger.Default.Register<MessageSTTDonHangThayDoi>(this, OnMessageSTTDonHangThayDoi);
            Messenger.Default.Register<MessageChuanBiGiay>(this, OnMessageChuanBiGiay);
            Messenger.Default.Register<MessageNapDonMaySong>(this, OnMessageNapDonMaySong);
            Messenger.Default.Register<MessageSLLoiMayCatThayDoi>(this, OnMessageSLLoiMayCatThayDoi);

            if (DriverConnector.IsStarted)
                OnDriverConnectorStarted(DriverConnector, EventArgs.Empty);
            else
                DriverConnector.Started += OnDriverConnectorStarted;

            Loaded += OnLoaded;
            danhSachDonHang.BeginEdit += DanhSachDonHang_BeginEdit;
            danhSachDonHang.EndEdit += DanhSachDonHang_EndEdit;
            matrixTask = Task.Factory.StartNew(HienThiMaTrix, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            DataContext = this;
        }

        #endregion

        #region Members
        private Task matrixTask;
        private Stopwatch sw = new Stopwatch();
        private long totalRefreshElapsed;
        private System.Timers.Timer refreshUITimer;
        private System.Timers.Timer thongBaoTimer;
        private Task taskKiemTra;
        #endregion

        #region Public properties
        public static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        public event PropertyChangedEventHandler PropertyChanged;
        public static CaiDat CaiDat { get; set; } = new CaiDat();
        public static NotifyCollection<DonHang> DonHangDataSource { get; set; } = new NotifyCollection<DonHang>();
        public NotifyCollection<DonHang> DonHangHienThi { get; set; }
        public IEasyDriverConnector DriverConnector { get; set; }
        public SolidColorBrush CheDoChuyenDonBrush { get; set; } = new SolidColorBrush(Color.FromRgb(0, 32, 96));
        SolidColorBrush manualBrush = new SolidColorBrush(Color.FromRgb(0, 32, 96));
        public string CheDoChuyenDonText { get; set; }

        public static ThongTinTram ThongTinTram { get; set; } = new ThongTinTram();
        public static TrangThaiDonHang TrangThaiDonHang { get; set; } = new TrangThaiDonHang();
        public static GiaySongGiayMatDangChay GiaySongGiayMatDangChayE { get; set; } = new GiaySongGiayMatDangChay();
        public static GiaySongGiayMatDangChay GiaySongGiayMatDangChayB { get; set; } = new GiaySongGiayMatDangChay();
        public static GiaySongGiayMatDangChay GiaySongGiayMatDangChayC { get; set; } = new GiaySongGiayMatDangChay();
        public static GiaySongGiayMatDangChay MayMenDangChay { get; set; } = new GiaySongGiayMatDangChay();
        public static WrapData WrapData { get; set; }
        #endregion

        #region Event handlers
        private void OnLoaded(object sender, RoutedEventArgs e)
        {

            btnDonHang.Click += BtnDonHang_Click;
            btnTangGiamLoi.Click += BtnTangGiamLoi_Click;
            btnCaiDat.Click += BtnCaiDat_Click;
            btnBaoCao.Click += BtnBaoCao_Click;
            KeyDown += MainWindow_KeyDown;
        }

        private void BtnBaoCao_Click(object sender, RoutedEventArgs e)
        {
            BaoCaoWindow window = new BaoCaoWindow();
            window.ShowDialog();
        }

        private void BtnCaiDat_Click(object sender, RoutedEventArgs e)
        {
            CaiDatWindow window = new CaiDatWindow();
            window.ShowDialog();
        }

        private void BtnTangGiamLoi_Click(object sender, RoutedEventArgs e)
        {
            TangGiamLoiWindow window = new TangGiamLoiWindow();
            window.Show();
        }

        private void BtnDonHang_Click(object sender, RoutedEventArgs e)
        {
            DonHangForm form = new DonHangForm(this);
            form.ShowDialog();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // Đơn hàng
            if (e.Key == Key.F1)
            {
                BtnDonHang_Click(null, null);
            }
            // +/- Lỗi
            else if (e.Key == Key.F2)
            {
                BtnTangGiamLoi_Click(null, null);
            }
            // Sản xuất
            else if (e.Key == Key.F3)
            {
                BtnBaoCao_Click(null, null);
            }
            // Cài đặt
            else if (e.Key == Key.F4)
            {
                BtnCaiDat_Click(null, null);
            }
        }

        private void DanhSachDonHang_EndEdit(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Row.DataContext is DonHang dh)
            {
                if (e.EditAction == DataGridEditAction.Commit)
                {
                    if (danhSachDonHang.DonHangDataSource != null)
                    {
                        DataGrid datagrid = sender as DataGrid;
                        var selectedCells = datagrid.SelectedCells;
                        if (selectedCells.Count > 0)
                        {
                            string selectedColumn = selectedCells[0].Column.Header.ToString();
                            var editElement = e.EditingElement as TextBox;
                            DonHang dhSource = null;
                            try
                            {
                                semaphore.Wait();
                                dhSource = DonHangDataSource.FirstOrDefault(x => x.Id == dh.Id);
                            }
                            catch { }
                            finally { semaphore.Release(); }

                            if (selectedColumn == "Dài" || selectedColumn == "S.L")
                            {
                                if (CutterTags.Instance.DoiDon != null && CutterTags.Instance.DoiDon.Quality == Quality.Good)
                                {
                                    if (double.TryParse(editElement.Text, out double number))
                                    {
                                        if (number > 0)
                                        {
                                            if (CutterTags.Instance.DoiDon != null && CutterTags.Instance.DoiDon.Quality == Quality.Good)
                                            {
                                                double.TryParse(SongETags.Instance.STTSong1.Value, out double sttSongE);
                                                double.TryParse(SongETags.Instance.STTMat1.Value, out double sttMatE);
                                                double.TryParse(SongETags.Instance.STT1.Value, out double sttE);
                                                double.TryParse(SongBTags.Instance.STTSong1.Value, out double sttSongB);
                                                double.TryParse(SongBTags.Instance.STTMat1.Value, out double sttMatB);
                                                double.TryParse(SongBTags.Instance.STT1.Value, out double sttB);
                                                double.TryParse(SongCTags.Instance.STTSong1.Value, out double sttSongC);
                                                double.TryParse(SongCTags.Instance.STTMat1.Value, out double sttMatC);
                                                double.TryParse(SongCTags.Instance.STT1.Value, out double sttC);
                                                double.TryParse(MayMenTags.Instance.STTMat1?.Value, out double sttMatMen);
                                                double.TryParse(MayMenTags.Instance.STT1?.Value, out double sttMen);
                                                double.TryParse(CutterTags.Instance.STT1.Value, out double sttCut);
                                                double.TryParse(CutterTags.Instance.DoiDon.Value, out double doiDon);
                                                double.TryParse(SongETags.Instance.DoiDon.Value, out double doiDonE);
                                                double.TryParse(SongBTags.Instance.DoiDon.Value, out double doiDonB);
                                                double.TryParse(SongCTags.Instance.DoiDon.Value, out double doiDonC);
                                                //double.TryParse(MayMenTags.Instance.DoiDon?.Value, out double doiDonMen);

                                                doiDon = doiDon / 1000.0f;
                                                doiDonE = doiDonE / 1000.0f;
                                                doiDonB = doiDonB / 1000.0f;
                                                doiDonC = doiDonC / 1000.0f;
                                                double doiDonMen = 0;
                                                doiDonMen = doiDon - CaiDat.DanMayMen;

                                                bool choPhepNap = true;

                                                // Nếu có giấy E
                                                if (!string.IsNullOrEmpty(dh.GiaySongE) || !string.IsNullOrEmpty(dh.GiayMatE))
                                                {
                                                    // Kiểm tra STT sửa phải bé hơn hoặc bằng stt đang chạy
                                                    if (dh.STT >= sttSongE && dh.STT >= sttMatE && dh.STT >= sttE)
                                                    {

                                                    }
                                                    else { choPhepNap = false; }

                                                }

                                                if (!string.IsNullOrEmpty(dh.GiaySongB) || !string.IsNullOrEmpty(dh.GiayMatB))
                                                {
                                                    // Kiểm tra STT sửa phải bé hơn hoặc bằng stt đang chạy
                                                    if (dh.STT >= sttSongB && dh.STT >= sttMatB && dh.STT >= sttB)
                                                    {

                                                    }
                                                    else { choPhepNap = false; }

                                                }

                                                if (!string.IsNullOrEmpty(dh.GiaySongC) || !string.IsNullOrEmpty(dh.GiayMatC))
                                                {
                                                    // Kiểm tra STT sửa phải bé hơn hoặc bằng stt đang chạy
                                                    if (dh.STT >= sttSongC && dh.STT >= sttMatC && dh.STT >= sttC)
                                                    {

                                                    }
                                                    else { choPhepNap = false; }

                                                }

                                                if (!string.IsNullOrEmpty(dh.Men))
                                                {
                                                    // Kiểm tra STT sửa phải bé hơn hoặc bằng stt đang chạy
                                                    if (dh.STT >= sttMatMen && dh.STT >= sttMen)
                                                    {

                                                    }
                                                    else { choPhepNap = false; }

                                                }

                                                if (choPhepNap &&
                                                    dh.STT >= sttCut)
                                                {
                                                    choPhepNap = true;
                                                    if (dh.STT == sttE ||
                                                        dh.STT == sttB ||
                                                        dh.STT == sttC ||
                                                        dh.STT == sttMen)
                                                    {
                                                        if (dh.STT == sttCut)
                                                        {
                                                            if (doiDon <= CaiDat.ChieuDaiToiThieuChoPhepSuaDon)
                                                                choPhepNap = false;
                                                        }
                                                        else if (dh.STT > sttCut)
                                                        {
                                                            if (!string.IsNullOrEmpty(dh.GiaySongE) || !string.IsNullOrEmpty(dh.GiayMatE))
                                                            {
                                                                if (doiDonE <= CaiDat.ChieuDaiToiThieuChoPhepSuaDon)
                                                                    choPhepNap = false;
                                                            }
                                                            if (!string.IsNullOrEmpty(dh.GiaySongB) || !string.IsNullOrEmpty(dh.GiayMatB))
                                                            {
                                                                if (doiDonB <= CaiDat.ChieuDaiToiThieuChoPhepSuaDon)
                                                                    choPhepNap = false;
                                                            }
                                                            if (!string.IsNullOrEmpty(dh.GiaySongC) || !string.IsNullOrEmpty(dh.GiayMatC))
                                                            {
                                                                if (doiDonC <= CaiDat.ChieuDaiToiThieuChoPhepSuaDon)
                                                                    choPhepNap = false;
                                                            }
                                                            if (!string.IsNullOrEmpty(dh.Men))
                                                            {
                                                                if (doiDonMen <= CaiDat.ChieuDaiToiThieuChoPhepSuaDon)
                                                                    choPhepNap = false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            choPhepNap = false;
                                                        }
                                                    }


                                                    if (choPhepNap)
                                                    {
                                                        string value = "";
                                                        if (selectedColumn == "Dài")
                                                        {
                                                            if (uint.TryParse(editElement.Text, out uint dai))
                                                                dh.Dai = (int)dai;
                                                            value = dh.Dai.ToString();
                                                            selectedColumn = "Dai";
                                                        }
                                                        else if (selectedColumn == "S.L")
                                                        {
                                                            if (uint.TryParse(editElement.Text, out uint sl))
                                                                dh.SL = (int)sl;
                                                            value = dh.SL.ToString();
                                                            selectedColumn = "SL";
                                                        }
                                                        string where = $" where Id = {dh.Id}";

                                                        Repository.Instance.UpdateColumn("dhdangchay", selectedColumn, value, where);

                                                        string where2 = $" where STT = {dh.STT}";
                                                        if (dhSource != null)
                                                        {
                                                            dhSource.Dai = dh.Dai;
                                                            dhSource.SL = dh.SL;
                                                            Repository.Instance.UpdateColumn("donhang", selectedColumn, value, where2);

                                                            semaphore.Wait();
                                                            try
                                                            {
                                                                var dhNap = DonHangDataSource.ToList();
                                                                CutterController.Instance.NapDanhSachDonHang(dhNap, true, true);
                                                                if (double.TryParse(SongETags.Instance.HeSoSong?.Value, out double heSoSong))
                                                                    SongEController.Instance.NapDanhSachDonHang(dhNap, heSoSong, true);
                                                                if (double.TryParse(SongBTags.Instance.HeSoSong?.Value, out double heSoSongb))
                                                                    SongBController.Instance.NapDanhSachDonHang(dhNap, heSoSongb, true);
                                                                if (double.TryParse(SongCTags.Instance.HeSoSong?.Value, out double heSoSongc))
                                                                    SongCController.Instance.NapDanhSachDonHang(dhNap, heSoSongc, true);
                                                                MayMenController.Instance.NapDanhSachDonHang(dhNap, 1, true);
                                                            }
                                                            catch { }
                                                            finally { semaphore.Release(); }
                                                        }

                                                        return;
                                                    }
                                                    else
                                                    {
                                                        Dispatcher.Invoke(() =>
                                                        {
                                                            MessageBox.Show("Không thể sửa đơn hàng vì đơn hàng sắp hoàn thành.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                        });
                                                    }
                                                }
                                                else
                                                {
                                                    Dispatcher.Invoke(() =>
                                                    {
                                                        MessageBox.Show("Không thể sửa đơn hàng đã hoàn thành.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                    });
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (selectedColumn == "Pallet")
                            {
                                if (CutterTags.Instance.DoiDon != null && CutterTags.Instance.DoiDon.Quality == Quality.Good)
                                {
                                    if (double.TryParse(editElement.Text, out double number))
                                    {
                                        if (number > 0)
                                        {
                                            if (CutterTags.Instance.DoiDon != null && CutterTags.Instance.DoiDon.Quality == Quality.Good)
                                            {
                                                double.TryParse(SongETags.Instance.STTSong1.Value, out double sttSongE);
                                                double.TryParse(SongETags.Instance.STTMat1.Value, out double sttMatE);
                                                double.TryParse(SongETags.Instance.STT1.Value, out double sttE);
                                                double.TryParse(SongBTags.Instance.STTSong1.Value, out double sttSongB);
                                                double.TryParse(SongBTags.Instance.STTMat1.Value, out double sttMatB);
                                                double.TryParse(SongBTags.Instance.STT1.Value, out double sttB);
                                                double.TryParse(SongCTags.Instance.STTSong1.Value, out double sttSongC);
                                                double.TryParse(SongCTags.Instance.STTMat1.Value, out double sttMatC);
                                                double.TryParse(SongCTags.Instance.STT1.Value, out double sttC);
                                                double.TryParse(MayMenTags.Instance.STTMat1.Value, out double sttMatMen);
                                                double.TryParse(MayMenTags.Instance.STT1.Value, out double sttMen);
                                                double.TryParse(CutterTags.Instance.STT1.Value, out double sttCut);
                                                double.TryParse(CutterTags.Instance.DoiDon.Value, out double doiDon);
                                                double.TryParse(SongETags.Instance.DoiDon.Value, out double doiDonE);
                                                double.TryParse(SongBTags.Instance.DoiDon.Value, out double doiDonB);
                                                double.TryParse(SongCTags.Instance.DoiDon.Value, out double doiDonC);

                                                doiDon = doiDon / 1000.0f;
                                                doiDonE = doiDonE / 1000.0f;
                                                doiDonB = doiDonB / 1000.0f;
                                                doiDonC = doiDonC / 1000.0f;
                                                double doiDonMen = 0;
                                                doiDonMen = doiDon - CaiDat.DanMayMen;

                                                if (dh.STT >= sttSongE &&
                                                    dh.STT >= sttMatE &&
                                                    dh.STT >= sttSongB &&
                                                    dh.STT >= sttMatB &&
                                                    dh.STT >= sttSongC &&
                                                    dh.STT >= sttMatC &&
                                                    dh.STT >= sttCut)
                                                {
                                                    bool choPhepNap = true;
                                                    //if (dh.STT == sttSongE ||
                                                    //    dh.STT == sttSongB ||
                                                    //    dh.STT == sttSongC ||
                                                    //    dh.STT == sttCut)
                                                    //{
                                                    //    // luôn luôn cho nạp pallet
                                                    //    //if (doiDon <= CaiDat.ChieuDaiToiThieuChoPhepSuaDon)
                                                    //    //    choPhepNap = false;
                                                    //}

                                                    if (choPhepNap)
                                                    {
                                                        string value = "";
                                                        if (selectedColumn == "Pallet")
                                                        {
                                                            if (uint.TryParse(editElement.Text, out uint pallet))
                                                                dh.Pallet = (int)pallet;
                                                            value = dh.Pallet.ToString();
                                                        }
                                                        string where = $" where Id = {dh.Id}";
                                                        Repository.Instance.UpdateColumn("dhdangchay", selectedColumn, value, where);
                                                        string where2 = $" where STT = {dh.STT}";
                                                        if (dhSource != null)
                                                        {
                                                            dhSource.Pallet = dh.Pallet;
                                                            Repository.Instance.UpdateColumn("donhang", selectedColumn, value, where2);

                                                            semaphore.Wait();
                                                            try
                                                            {
                                                                CutterController.Instance.NapDanhSachDonHang(DonHangDataSource.ToList(), true, true);
                                                            }
                                                            catch { }
                                                            finally { semaphore.Release(); }
                                                        }
                                                        return;
                                                    }
                                                    else
                                                    {
                                                        Dispatcher.Invoke(() =>
                                                        {
                                                            MessageBox.Show("Không thể sửa đơn hàng vì đơn hàng sắp hoàn thành.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                        });

                                                    }
                                                }
                                                else
                                                {
                                                    Dispatcher.Invoke(() =>
                                                    {
                                                        MessageBox.Show("Không thể sửa đơn hàng đã hoàn thành.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                    });
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (selectedColumn == "Ghi chú")
                            {
                                string where = $" where Id = {dh.Id}";
                                string where2 = $" where STT = {dh.STT}";
                                dh.GhiChu = editElement.Text?.Trim();
                                if (dhSource != null)
                                {
                                    dhSource.GhiChu = dh.GhiChu;
                                    Repository.Instance.UpdateColumn("donhang", "GhiChu", editElement.Text?.Trim(), where2);
                                }
                                Repository.Instance.UpdateColumn("dhdangchay", "GhiChu", editElement.Text?.Trim(), where);
                                return;
                            }
                            if (selectedColumn == "STT")
                            {
                                if (CutterTags.Instance.DoiDon != null && CutterTags.Instance.DoiDon.Quality == Quality.Good)
                                {
                                    if (double.TryParse(editElement.Text, out double number))
                                    {
                                        if (number > 0)
                                        {
                                            if (CutterTags.Instance.DoiDon != null && CutterTags.Instance.DoiDon.Quality == Quality.Good)
                                            {
                                                double.TryParse(SongETags.Instance.STTSong1.Value, out double sttSongE);
                                                double.TryParse(SongETags.Instance.STTMat1.Value, out double sttMatE);
                                                double.TryParse(SongETags.Instance.STT1.Value, out double sttE);
                                                double.TryParse(SongBTags.Instance.STTSong1.Value, out double sttSongB);
                                                double.TryParse(SongBTags.Instance.STTMat1.Value, out double sttMatB);
                                                double.TryParse(SongBTags.Instance.STT1.Value, out double sttB);
                                                double.TryParse(SongCTags.Instance.STTSong1.Value, out double sttSongC);
                                                double.TryParse(SongCTags.Instance.STTMat1.Value, out double sttMatC);
                                                double.TryParse(SongCTags.Instance.STT1.Value, out double sttC);
                                                double.TryParse(MayMenTags.Instance.STTMat1?.Value, out double sttMatMen);
                                                double.TryParse(MayMenTags.Instance.STT1?.Value, out double sttMen);
                                                double.TryParse(CutterTags.Instance.STT1.Value, out double sttCut);
                                                double.TryParse(CutterTags.Instance.DoiDon.Value, out double doiDon);
                                                double.TryParse(SongETags.Instance.DoiDon.Value, out double doiDonE);
                                                double.TryParse(SongBTags.Instance.DoiDon.Value, out double doiDonB);
                                                double.TryParse(SongCTags.Instance.DoiDon.Value, out double doiDonC);

                                                doiDon = doiDon / 1000.0f;
                                                doiDonE = doiDonE / 1000.0f;
                                                doiDonB = doiDonB / 1000.0f;
                                                doiDonC = doiDonC / 1000.0f;

                                                double doiDonMen = 0;
                                                doiDonMen = doiDon - CaiDat.DanMayMen;

                                                bool choPhepNap = true;

                                                // Nếu có giấy E
                                                if (!string.IsNullOrEmpty(dh.GiaySongE) || !string.IsNullOrEmpty(dh.GiayMatE))
                                                {
                                                    // Kiểm tra STT sửa phải bé hơn hoặc bằng stt đang chạy
                                                    if (dh.STT >= sttSongE && dh.STT >= sttMatE && number > sttE)
                                                    {

                                                    }
                                                    else { choPhepNap = false; }

                                                }

                                                if (!string.IsNullOrEmpty(dh.GiaySongB) || !string.IsNullOrEmpty(dh.GiayMatB))
                                                {
                                                    // Kiểm tra STT sửa phải bé hơn hoặc bằng stt đang chạy
                                                    if (dh.STT >= sttSongB && dh.STT >= sttMatB && number > sttB)
                                                    {

                                                    }
                                                    else { choPhepNap = false; }

                                                }

                                                if (!string.IsNullOrEmpty(dh.GiaySongC) || !string.IsNullOrEmpty(dh.GiayMatC))
                                                {
                                                    // Kiểm tra STT sửa phải bé hơn hoặc bằng stt đang chạy
                                                    if (dh.STT >= sttSongC && dh.STT >= sttMatC && number > sttC)
                                                    {

                                                    }
                                                    else { choPhepNap = false; }

                                                }

                                                if (!string.IsNullOrEmpty(dh.Men))
                                                {
                                                    // Kiểm tra STT sửa phải bé hơn hoặc bằng stt đang chạy
                                                    if (dh.STT >= sttMatMen && number > sttMen)
                                                    {

                                                    }
                                                    else { choPhepNap = false; }

                                                }

                                                if (choPhepNap &&
                                                    dh.STT >= sttCut)
                                                {
                                                    choPhepNap = true;
                                                    if (dh.STT == sttE ||
                                                        dh.STT == sttB ||
                                                        dh.STT == sttC ||
                                                        dh.STT == sttMen ||
                                                        dh.STT == sttCut)
                                                    {
                                                        if (doiDon <= CaiDat.ChieuDaiToiThieuChoPhepSuaDon)
                                                            choPhepNap = false;
                                                    }

                                                    if (choPhepNap)
                                                    {

                                                        if (CutterTags.Instance.STT1 != null && long.TryParse(CutterTags.Instance.STT1.Value, out long sttCutter))
                                                        {
                                                            if (long.TryParse(editElement.Text?.Trim(), out long newSTT))
                                                            {
                                                                long sttMax = DonHangDataSource.Max(x => x.STT);
                                                                if (DonHangDataSource != null && DonHangDataSource.Count > 0)
                                                                {
                                                                    using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default["constr"].ToString()))
                                                                    {
                                                                        conn.Open();
                                                                        using (MySqlCommand cmd = new MySqlCommand())
                                                                        {
                                                                            cmd.Connection = conn;
                                                                            cmd.CommandType = CommandType.Text;
                                                                            cmd.CommandText = "select lastId from common";
                                                                            using (MySqlDataAdapter adp = new MySqlDataAdapter(cmd))
                                                                            {
                                                                                DataTable dt = new DataTable();
                                                                                adp.Fill(dt);
                                                                                if (dt.Rows.Count > 0)
                                                                                {
                                                                                    if (long.TryParse(dt.Rows[0]["lastId"].ToString(), out long lastSTT))
                                                                                        sttMax = lastSTT + 9;
                                                                                }
                                                                            }
                                                                        }
                                                                    }

                                                                    if (newSTT > sttMax)
                                                                    {
                                                                        MessageBox.Show("STT mới không thể lớn hơn STT của đơn hàng cuối cùng trong danh sách đơn hàng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                    }
                                                                    else
                                                                    {
                                                                        foreach (var item in DonHangDataSource)
                                                                        {
                                                                            if (item.HoanTatCutter + item.HoanTatSongB + item.HoanTatSongC +
                                                                                item.HoanTatSongE + item.HoanTatSpliter + item.HoanTatMayMen > 0)
                                                                            {
                                                                                if (newSTT <= item.STT)
                                                                                {
                                                                                    MessageBox.Show("STT mới không thể nhỏ hơn STT của các đơn đã hoàn thành hoặc đang chạy!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                                                    choPhepNap = false;
                                                                                    break;
                                                                                }
                                                                            }
                                                                        }

                                                                        if (choPhepNap)
                                                                        {
                                                                            string where = $" where Id = {dh.Id}";
                                                                            string where2 = $" where STT = {dh.STT}";

                                                                            if (newSTT > sttCutter)
                                                                            {
                                                                                if (DonHangDataSource.FirstOrDefault(x => x != dh && x.STT == newSTT) == null)
                                                                                {
                                                                                    long sttCu = dh.STT;
                                                                                    dh.STT = newSTT;
                                                                                    if (dhSource != null)
                                                                                    {
                                                                                        dhSource.GhiChu = dh.GhiChu;
                                                                                        Repository.Instance.UpdateColumn("donhang", "STT", editElement.Text?.Trim(), where2);
                                                                                    }
                                                                                    Repository.Instance.UpdateColumn("dhdangchay", "STT", editElement.Text?.Trim(), where);

                                                                                    Dispatcher.Invoke(() =>
                                                                                    {
                                                                                        DonHangHienThi.DisableNotifyChanged = true;
                                                                                        DonHangHienThi.Clear();
                                                                                        DonHangHienThi.DisableNotifyChanged = false;
                                                                                        DonHangHienThi.AddRange(DonHangDataSource.Where(x => x.HoanTatCutter == 0 && x.TGKetThuc == DateTime.MinValue && x.STT >= sttCutter).OrderBy(x => x.STT));
                                                                                    });

                                                                                    semaphore.Wait();
                                                                                    try
                                                                                    {
                                                                                        var dhNaps = DonHangDataSource.ToList();
                                                                                        if (DonHangDataSource.FirstOrDefault(x => x.STT == sttCu) is DonHang dhNap)
                                                                                            dhNap.STT = newSTT;

                                                                                        CutterController.Instance.NapDanhSachDonHang(dhNaps, true, true);
                                                                                        if (double.TryParse(SongETags.Instance.HeSoSong?.Value, out double heSoSong))
                                                                                            SongEController.Instance.NapDanhSachDonHang(dhNaps, heSoSong, true);
                                                                                        if (double.TryParse(SongBTags.Instance.HeSoSong?.Value, out double heSoSongb))
                                                                                            SongBController.Instance.NapDanhSachDonHang(dhNaps, heSoSongb, true);
                                                                                        if (double.TryParse(SongCTags.Instance.HeSoSong?.Value, out double heSoSongc))
                                                                                            SongCController.Instance.NapDanhSachDonHang(dhNaps, heSoSongc, true);
                                                                                        MayMenController.Instance.NapDanhSachDonHang(dhNaps, 1, true);
                                                                                    }
                                                                                    catch (Exception ex)
                                                                                    {
                                                                                        MessageBox.Show(ex.ToString());
                                                                                    }
                                                                                    finally { semaphore.Release(); }
                                                                                    return;
                                                                                }
                                                                                else
                                                                                {
                                                                                    Dispatcher.Invoke(() =>
                                                                                    {
                                                                                        MessageBox.Show($"STT {dh.STT} đã tồn tại vui lòng nhập STT khác", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);

                                                                                    });
                                                                                }
                                                                            }

                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        Dispatcher.Invoke(() =>
                                                        {
                                                            MessageBox.Show("Không thể sửa đơn hàng vì đơn hàng sắp hoàn thành.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);

                                                        });

                                                    }
                                                }
                                                else
                                                {
                                                    Dispatcher.Invoke(() =>
                                                    {
                                                        MessageBox.Show("Không thể sửa đơn hàng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                    });
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            datagrid.CancelEdit();
                            e.Cancel = true;
                        }
                    }
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void DanhSachDonHang_BeginEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Row.DataContext is DonHang dh)
            {
                var dhDataSource = danhSachDonHang.DonHangDataSource;
                if (dhDataSource == null)
                {
                    e.Cancel = true;
                    return;
                }

                var dhDau = dhDataSource.FirstOrDefault();
                if (dhDau == null)
                {
                    e.Cancel = true;
                    return;
                }

                var col = e.Column.Header.ToString();
                if (col == "Mã" ||
                    col == "Sóng" ||
                    col == "Khổ" ||
                    col == "M" ||
                    col == "Sóng E" ||
                    col == "Mặt E" ||
                    col == "Sóng B" ||
                    col == "Mặt B" ||
                    col == "Sóng C" ||
                    col == "Mặt C" ||
                    col == "Xả" || col == "Rộng" || col == "Cánh" || col == "Cao" || col == "Lằng")
                {
                    e.Cancel = true;
                    return;
                }
                if (col == "STT")
                {
                    if (dhDau.STT == dh.STT)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void CheDoChuyenDon_ValueChanged(object sender, TagValueChangedEventArgs e)
        {
            if (e.NewValue == "100")
            {
                if (CheDoChuyenDonBrush != Brushes.Orange)
                {
                    Dispatcher.Invoke(() =>
                    {
                        CheDoChuyenDonBrush = Brushes.Orange;
                        CheDoChuyenDonText = "Chuyển đơn tự động";
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CheDoChuyenDonBrush"));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CheDoChuyenDonText"));

                    });
                }
            }
            else
            {
                if (CheDoChuyenDonBrush != manualBrush)
                {
                    Dispatcher.Invoke(() =>
                    {
                        CheDoChuyenDonBrush = manualBrush;
                        CheDoChuyenDonText = "Chuyển đơn tay";
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CheDoChuyenDonBrush"));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CheDoChuyenDonText"));
                    });

                }
            }
        }
        #endregion

        #region Public methods

        public void NapDon(bool napViTri1, bool napViTri2)
        {
            try
            {

                semaphore.Wait();
                List<DonHang> dhNap = DonHangDataSource.ToList();
                CutterController.Instance.NapDanhSachDonHang(dhNap, napViTri1, napViTri2);
                if (double.TryParse(SongETags.Instance.HeSoSong?.Value, out double heSoSong))
                    SongEController.Instance.NapDanhSachDonHang(dhNap, heSoSong, napViTri1);
                if (double.TryParse(SongBTags.Instance.HeSoSong?.Value, out double heSoSongB))
                    SongBController.Instance.NapDanhSachDonHang(dhNap, heSoSongB, napViTri1);
                if (double.TryParse(SongCTags.Instance.HeSoSong?.Value, out double heSoSongC))
                    SongCController.Instance.NapDanhSachDonHang(dhNap, heSoSongC, napViTri1);
                MayMenController.Instance.NapDanhSachDonHang(dhNap, 1, napViTri1);

                Dispatcher.Invoke(() => DonHangHienThi.NotifyResetCollection());

            }
            catch { }
            finally { semaphore.Release(); }
        }

        #endregion

        #region Messasge handle
        
        private void OnMessageSLLoiMayCatThayDoi(MessageSLLoiMayCatThayDoi message)
        {
            Dispatcher.Invoke(() =>
            {
                if (message != null)
                {
                    List<WriteCommand> commands = new List<WriteCommand>();
                    if (long.TryParse(message.STT, out long stt))
                    {
                        int dai = message.SLLoi * message.Dai;
                        if (SongETags.Instance.SoMetLoiChotGiayMat != null)
                        {
                            semaphore.Wait();
                            try
                            {
                                var dh = DonHangDataSource.FirstOrDefault(x => x.STT == stt);
                                if (dh != null)
                                {
                                    string giaydh = dh.Kho.ToString() + dh.GiaySongE + dh.GiayMatE;
                                    if (!string.IsNullOrEmpty(dh.GiaySongE) ||
                                        !string.IsNullOrEmpty(dh.GiayMatE))
                                    {
                                        if (SongETags.Instance.STT1 != null &&
                                            long.TryParse(SongETags.Instance.STT1.Value, out long sttMaySong))
                                        {
                                            var dhMaySong = DonHangDataSource.FirstOrDefault(x => x.STT == sttMaySong && x.HoanTatGiaySongE == 0 && x.HoanTatGiayMatE == 0);
                                            if (dhMaySong != null)
                                            {
                                                string giaySong = dhMaySong.Kho.ToString() + dhMaySong.GiaySongE + dhMaySong.GiayMatE;
                                                if (giaySong == giaydh)
                                                {
                                                    commands.Add(new WriteCommand() { PathToTag = SongETags.Instance.SoMetLoiChotGiayMat.Path, Value = dai.ToString() });
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch { }
                            finally { semaphore.Release(); }
                        }

                        if (SongBTags.Instance.SoMetLoiChotGiayMat != null)
                        {
                            semaphore.Wait();
                            try
                            {
                                var dh = DonHangDataSource.FirstOrDefault(x => x.STT == stt);
                                if (dh != null)
                                {
                                    string giaydh = dh.Kho.ToString() + dh.GiaySongB + dh.GiayMatB;
                                    if (!string.IsNullOrEmpty(dh.GiaySongB) ||
                                        !string.IsNullOrEmpty(dh.GiayMatB))
                                    {
                                        if (SongBTags.Instance.STT1 != null &&
                                            long.TryParse(SongBTags.Instance.STT1.Value, out long sttMaySong))
                                        {
                                            var dhMaySong = DonHangDataSource.FirstOrDefault(x => x.STT == sttMaySong && x.HoanTatGiaySongB == 0 && x.HoanTatGiayMatB == 0);
                                            if (dhMaySong != null)
                                            {
                                                string giaySong = dhMaySong.Kho.ToString() + dhMaySong.GiaySongB + dhMaySong.GiayMatB;
                                                if (giaySong == giaydh)
                                                {
                                                    commands.Add(new WriteCommand() { PathToTag = SongBTags.Instance.SoMetLoiChotGiayMat.Path, Value = dai.ToString() });
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch { }
                            finally { semaphore.Release(); }
                        }

                        if (SongCTags.Instance.SoMetLoiChotGiayMat != null)
                        {
                            semaphore.Wait();
                            try
                            {
                                var dh = DonHangDataSource.FirstOrDefault(x => x.STT == stt);
                                if (dh != null)
                                {
                                    string giaydh = dh.Kho.ToString() + dh.GiaySongC + dh.GiayMatC;
                                    if (!string.IsNullOrEmpty(dh.GiaySongC) ||
                                        !string.IsNullOrEmpty(dh.GiayMatC))
                                    {
                                        if (SongCTags.Instance.STT1 != null &&
                                            long.TryParse(SongCTags.Instance.STT1.Value, out long sttMaySong))
                                        {
                                            var dhMaySong = DonHangDataSource.FirstOrDefault(x => x.STT == sttMaySong && x.HoanTatGiaySongC == 0 && x.HoanTatGiayMatC == 0);
                                            if (dhMaySong != null)
                                            {
                                                string giaySong = dhMaySong.Kho.ToString() + dhMaySong.GiaySongC + dhMaySong.GiayMatC;
                                                if (giaySong == giaydh)
                                                {
                                                    commands.Add(new WriteCommand() { PathToTag = SongCTags.Instance.SoMetLoiChotGiayMat.Path, Value = dai.ToString() });
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch { }
                            finally { semaphore.Release(); }
                        }

                        if (MayMenTags.Instance.SoMetLoiChotGiayMat != null)
                        {
                            semaphore.Wait();
                            try
                            {
                                var dh = DonHangDataSource.FirstOrDefault(x => x.STT == stt);
                                if (dh != null)
                                {
                                    string giaydh = dh.Kho.ToString() + dh.Men;
                                    if (!string.IsNullOrEmpty(dh.Men))
                                    {
                                        if (MayMenTags.Instance.STT1 != null &&
                                            long.TryParse(MayMenTags.Instance.STT1.Value, out long sttMen))
                                        {
                                            var dhMaySong = DonHangDataSource.FirstOrDefault(x => x.STT == sttMen && x.HoanTatMayMen == 0);
                                            if (dhMaySong != null)
                                            {
                                                string giaySong = dhMaySong.Kho.ToString() + dhMaySong.Men;
                                                if (giaySong == giaydh)
                                                {
                                                    commands.Add(new WriteCommand() { PathToTag = MayMenTags.Instance.SoMetLoiChotGiayMat.Path, Value = dai.ToString() });
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch { }
                            finally { semaphore.Release(); }
                        }
                    }
                    if (commands.Count > 0)
                    {
                        Task.Factory.StartNew(async () =>
                        {
                            await DriverConnector.WriteMultiTagAsync(commands);
                        });
                    }
                }
            });
        }

        protected void OnMessageNapDonMaySong(MessageNapDonMaySong message)
        {
            Dispatcher.Invoke(() =>
            {
                if (message != null)
                {
                    if (message.DonHangMayCatKeTiep != null)
                    {
                        try
                        {
                            semaphore.Wait();
                            if (message.DeviceName == SongETags.Instance.DeviceName && SongETags.Instance.STT1 != null)
                            {
                                if (long.TryParse(SongETags.Instance.STT1.Value, out long stt))
                                {
                                    if (stt < message.DonHangMayCatKeTiep.STT)
                                    {
                                        List<DonHang> dhNap = DonHangDataSource.Where(x => x.STT >= message.DonHangMayCatHienTai.STT).OrderBy(x => x.STT).ToList();
                                        dhNap.Remove(message.DonHangMayCatHienTai);
                                        if (double.TryParse(SongETags.Instance.HeSoSong?.Value, out double heSoSong))
                                            SongEController.Instance.NapDanhSachDonHang(dhNap, heSoSong);
                                        Dispatcher.Invoke(() => DonHangHienThi.NotifyResetCollection());
                                        Task.Factory.StartNew(async () =>
                                        {
                                            Thread.Sleep(1000);
                                            await SongETags.Instance.CoBaoNapGiay.WriteAsync("100");
                                        });
                                    }
                                }
                            }
                            else if (message.DeviceName == SongBTags.Instance.DeviceName && SongBTags.Instance.STT1 != null)
                            {
                                if (long.TryParse(SongBTags.Instance.STT1.Value, out long stt))
                                {
                                    if (stt < message.DonHangMayCatKeTiep.STT)
                                    {
                                        List<DonHang> dhNap = DonHangDataSource.Where(x => x.STT >= message.DonHangMayCatHienTai.STT).OrderBy(x => x.STT).ToList();
                                        dhNap.Remove(message.DonHangMayCatHienTai);
                                        if (double.TryParse(SongBTags.Instance.HeSoSong?.Value, out double heSoSong))
                                            SongBController.Instance.NapDanhSachDonHang(dhNap, heSoSong);
                                        Dispatcher.Invoke(() => DonHangHienThi.NotifyResetCollection());

                                        Task.Factory.StartNew(async () =>
                                        {
                                            Thread.Sleep(1000);
                                            await SongBTags.Instance.CoBaoNapGiay.WriteAsync("100");
                                        });
                                    }
                                }
                            }
                            else if (message.DeviceName == SongCTags.Instance.DeviceName && SongCTags.Instance.STT1 != null)
                            {
                                if (long.TryParse(SongCTags.Instance.STT1.Value, out long stt))
                                {
                                    if (stt < message.DonHangMayCatKeTiep.STT)
                                    {
                                        List<DonHang> dhNap = DonHangDataSource.Where(x => x.STT >= message.DonHangMayCatHienTai.STT).OrderBy(x => x.STT).ToList();
                                        dhNap.Remove(message.DonHangMayCatHienTai);
                                        if (double.TryParse(SongCTags.Instance.HeSoSong?.Value, out double heSoSong))
                                            SongCController.Instance.NapDanhSachDonHang(dhNap, heSoSong);
                                        Dispatcher.Invoke(() => DonHangHienThi.NotifyResetCollection());

                                        Task.Factory.StartNew(async () =>
                                        {
                                            Thread.Sleep(1000);
                                            await SongCTags.Instance.CoBaoNapGiay.WriteAsync("100");
                                        });
                                    }
                                }
                            }
                        }
                        catch (Exception ex) { /*MessageBox.Show(ex.ToString());*/ }
                        finally { semaphore.Release(); }
                    }
                }
            });
        }

        protected void OnMessageChuanBiGiay(MessageChuanBiGiay message)
        {
            Dispatcher.Invoke(() =>
            {
                if (message != null && message.DonHangKeTiep != null)
                {
                    try
                    {
                        if (message.DeviceName == SongETags.Instance.DeviceName && SongETags.Instance.STT1 != null)
                        {
                            if (long.TryParse(SongETags.Instance.STT1.Value, out long stt))
                            {
                                if (stt < message.DonHangKeTiep.STT)
                                    Task.Factory.StartNew(async () => { await SongETags.Instance.CoBaoChuanBiGiay.WriteAsync("100"); });
                            }
                        }
                        else if (message.DeviceName == SongBTags.Instance.DeviceName && SongBTags.Instance.STT1 != null)
                        {
                            if (long.TryParse(SongBTags.Instance.STT1.Value, out long stt))
                            {
                                if (stt < message.DonHangKeTiep.STT)
                                    Task.Factory.StartNew(async () => { await SongBTags.Instance.CoBaoChuanBiGiay.WriteAsync("100"); });
                                
                            }
                        }
                        else if (message.DeviceName == SongCTags.Instance.DeviceName && SongCTags.Instance.STT1 != null)
                        {
                            if (long.TryParse(SongCTags.Instance.STT1.Value, out long stt))
                            {
                                if (stt < message.DonHangKeTiep.STT)
                                    Task.Factory.StartNew(async () => { await SongCTags.Instance.CoBaoChuanBiGiay.WriteAsync("100"); });
                                
                            }
                        }
                    }
                    catch { }
                }
            });
        }

        protected void OnMessageSTTDonHangThayDoi(MessageSTTDonHangThayDoi message)
        {
            //if (message != null)
            //{
            //    semaphore.Wait();
            //    try
            //    {
            //        if (DonHangDataSource.FirstOrDefault(x => x.STT.ToString() == message.NewSTT) is DonHang dh)
            //        {
            //            UpdaterDonHang.Instance.Start(dh);
            //        }
            //        else
            //        {
            //            UpdaterDonHang.Instance.Stop();
            //        }
            //    }
            //    catch { }
            //    finally { semaphore.Release(); }
            //}
        }

        protected void OnMessageKiemTraChieuDai(MessageKiemTraChieuDai message)
        {
            Dispatcher.Invoke(() =>
            {
                if (message != null &&
                CutterTags.Instance.STT1 != null && long.TryParse(CutterTags.Instance.STT1.Value, out long sttCut) &&
                long.TryParse(CutterTags.Instance.DoiDon.Value, out long doiDon))
                {
                    List<WriteCommand> writeCommands = new List<WriteCommand>();
                    if (message.DeviceName == SongETags.Instance.DeviceName &&
                        SongETags.Instance.STT1 != null && long.TryParse(SongETags.Instance.STT1.Value, out long sttDon))
                    {
                        if (sttCut != sttDon)
                            doiDon += 1000000000;

                        var donHangs = DonHangDataSource.Where(x => x.STT > sttCut && x.STT <= sttDon)?.ToList();
                        if (donHangs == null)
                            donHangs = new List<DonHang>();
                 
                        int totalSum = donHangs.Sum(x => x.Dai * x.SL);
                        doiDon += totalSum;

                        writeCommands.Add(new WriteCommand() { PathToTag = SongETags.Instance.ChieuDaiKiemTra.Path, Value = doiDon.ToString() });
                        writeCommands.Add(new WriteCommand() { PathToTag = SongETags.Instance.CoKiemTraChieuDai.Path, Value = doiDon.ToString() });
                    }
                    else if (message.DeviceName == SongBTags.Instance.DeviceName &&
                        SongBTags.Instance.STT1 != null && long.TryParse(SongBTags.Instance.STT1.Value, out sttDon))
                    {
                        if (sttCut != sttDon)
                            doiDon += 1000000000;

                        var donHangs = DonHangDataSource.Where(x => x.STT > sttCut && x.STT <= sttDon)?.ToList();
                        if (donHangs == null)
                            donHangs = new List<DonHang>();

                        int totalSum = donHangs.Sum(x => x.Dai * x.SL);
                        doiDon += totalSum;

                        writeCommands.Add(new WriteCommand() { PathToTag = SongBTags.Instance.ChieuDaiKiemTra.Path, Value = doiDon.ToString() });
                        writeCommands.Add(new WriteCommand() { PathToTag = SongBTags.Instance.CoKiemTraChieuDai.Path, Value = doiDon.ToString() });
                    }
                    else if (message.DeviceName == SongCTags.Instance.DeviceName &&
                        SongCTags.Instance.STT1 != null && long.TryParse(SongCTags.Instance.STT1.Value, out sttDon))
                    {
                        if (sttCut != sttDon)
                            doiDon += 1000000000;

                        var donHangs = DonHangDataSource.Where(x => x.STT > sttCut && x.STT <= sttDon)?.ToList();
                        if (donHangs == null)
                            donHangs = new List<DonHang>();

                        int totalSum = donHangs.Sum(x => x.Dai * x.SL);
                        doiDon += totalSum;

                        writeCommands.Add(new WriteCommand() { PathToTag = SongCTags.Instance.ChieuDaiKiemTra.Path, Value = doiDon.ToString() });
                        writeCommands.Add(new WriteCommand() { PathToTag = SongCTags.Instance.CoKiemTraChieuDai.Path, Value = doiDon.ToString() });
                    }
                    else if (message.DeviceName == MayMenTags.Instance.DeviceName &&
                        MayMenTags.Instance.STT1 != null && long.TryParse(MayMenTags.Instance.STT1.Value, out sttDon))
                    {
                        if (sttCut != sttDon)
                            doiDon += 1000000000;

                        var donHangs = DonHangDataSource.Where(x => x.STT > sttCut && x.STT <= sttDon)?.ToList();
                        if (donHangs == null)
                            donHangs = new List<DonHang>();

                        int totalSum = donHangs.Sum(x => x.Dai * x.SL);
                        doiDon += totalSum;

                        writeCommands.Add(new WriteCommand() { PathToTag = MayMenTags.Instance.ChieuDaiKiemTra.Path, Value = doiDon.ToString() });
                        writeCommands.Add(new WriteCommand() { PathToTag = MayMenTags.Instance.CoKiemTraChieuDai.Path, Value = doiDon.ToString() });
                    }

                    Task.Factory.StartNew(async () =>
                    {
                        await DriverConnector.WriteMultiTagAsync(writeCommands);
                    });
                }
            });
        }

        protected void OnMessageChuyenDonHang(MessageChuyenDonHang message)
        {
            Dispatcher.Invoke(() =>
            {
                semaphore.Wait();
                try
                {
                    if (message != null)
                    {
                        DonHang dh = DonHangDataSource.FirstOrDefault(x => x.STT.ToString() == message.STT);
                        DonHang dhHienThi = DonHangHienThi.FirstOrDefault(x => x.STT.ToString() == message.STT);
                        if (dh != null)
                        {
                            if (message.DeviceName == CutterTags.Instance.DeviceName)
                            {
                                dh.HoanTatCutter = 1;
                                dh.TGKetThuc = DateTime.Now;
                                CutterController.Instance.NapDanhSachDonHang(DonHangDataSource.ToList(), false, false);

                                //UpdaterDonHang.Instance.LockSTT = dh.STT;
                                Task.Factory.StartNew(() =>
                                {
                                    if (int.TryParse(CutterTags.Instance.SLDatChot.Value, out int slDat))
                                        dh.SLDat = slDat;
                                    if (int.TryParse(CutterTags.Instance.SLLoiChot.Value, out int slLoi))
                                        dh.SLLoi = slLoi;

                                    //Repository.Instance.LuuDonHangDaChay(dh);
                                });
                                Repository.Instance.UpdateColumns("dhdangchay", $"set HoanTatCutter = '1', TGKetThuc = '{dh.TGKetThuc.ToString("yyyy-MM-dd HH:mm:ss")}' where Id = {dh.Id}");
                                if (dhHienThi != null)
                                {
                                    dhHienThi.HoanTatCutter = 1;
                                    int indexHienTai = DonHangHienThi.IndexOf(dhHienThi);


                                    Dispatcher.Invoke(() =>
                                    {
                                        DonHangHienThi.Remove(dhHienThi);
                                        DonHangHienThi.NotifyResetCollection();
                                    });
                                }
                                Dispatcher.Invoke(() => DonHangHienThi.NotifyResetCollection());

                                var nextDH = DonHangDataSource.FirstOrDefault(x => x.STT > dh.STT);
                                if (nextDH != null)
                                {
                                    Task.Factory.StartNew(async () =>
                                    {
                                        if (SongETags.Instance != null && SongETags.Instance.Setting9 != null)
                                        {
                                            await SongETags.Instance.Setting9.WriteAsync(nextDH.STT.ToString());
                                        }
                                        if (SongBTags.Instance != null && SongBTags.Instance.Setting9 != null)
                                        {
                                            await SongBTags.Instance.Setting9.WriteAsync(nextDH.STT.ToString());
                                        }
                                        if (SongCTags.Instance != null && SongCTags.Instance.Setting9 != null)
                                        {
                                            await SongCTags.Instance.Setting9.WriteAsync(nextDH.STT.ToString());
                                        }
                                        if (MayMenTags.Instance != null && MayMenTags.Instance.Setting9 != null)
                                        {
                                            await MayMenTags.Instance.Setting9.WriteAsync(nextDH.STT.ToString());
                                        }
                                    });
                                }
                            }
                            else if (message.DeviceName == SongETags.Instance.DeviceName)
                            {
                                dh.HoanTatSongE = 1;
                                long.TryParse(message.STT, out long stt);
                                SongEController.Instance.NapDonHang(DonHangDataSource.ToList(), stt, false);
                                Repository.Instance.UpdateColumns("dhdangchay", $"set HoanTatSongE = '1' where Id = {dh.Id}");
                                Dispatcher.Invoke(() => DonHangHienThi.NotifyResetCollection());

                            }
                            else if (message.DeviceName == SongBTags.Instance.DeviceName)
                            {
                                dh.HoanTatSongB = 1;
                                long.TryParse(message.STT, out long stt);
                                SongBController.Instance.NapDonHang(DonHangDataSource.ToList(), stt, false);
                                Repository.Instance.UpdateColumns("dhdangchay", $"set HoanTatSongB = '1' where Id = {dh.Id}");
                                Dispatcher.Invoke(() => DonHangHienThi.NotifyResetCollection());

                            }
                            else if (message.DeviceName == SongCTags.Instance.DeviceName)
                            {
                                dh.HoanTatSongC = 1;
                                long.TryParse(message.STT, out long stt);
                                SongCController.Instance.NapDonHang(DonHangDataSource.ToList(), stt, false);
                                Repository.Instance.UpdateColumns("dhdangchay", $"set HoanTatSongC = '1' where Id = {dh.Id}");
                                Dispatcher.Invoke(() => DonHangHienThi.NotifyResetCollection());

                            }
                            else if (message.DeviceName == MayMenTags.Instance.DeviceName)
                            {
                                dh.HoanTatMayMen = 1;
                                long.TryParse(message.STT, out long stt);
                                MayMenController.Instance.NapDonHang(DonHangDataSource.ToList(), stt, false);
                                Repository.Instance.UpdateColumns("dhdangchay", $"set HoanTatMayMen = '1' where Id = {dh.Id}");
                                Dispatcher.Invoke(() => DonHangHienThi.NotifyResetCollection());
                            }
                        }
                    }
                }
                catch { }
                finally { semaphore.Release(); }
            });
        }

        protected void OnMessageChuyenDonGiayMat(MessageChuyenDonGiayMat message)
        {
            Dispatcher.Invoke(() =>
            {
                semaphore.Wait();
                try
                {
                    if (message != null)
                    {
                        List<DonHang> dhs = DonHangDataSource.OrderBy(x => x.STT).ToList();
                        DonHang dh = dhs.FirstOrDefault(x => x.STT.ToString() == message.STT);
                        if (dh != null)
                        {
                            if (message.DeviceName == SongETags.Instance.DeviceName)
                            {
                                dh.HoanTatGiayMatE = 1;
                                string giay = dh.Kho.ToString() + dh.GiayMatE;
                                int index = dhs.IndexOf(dh);
                                if (index > -1)
                                {
                                    for (int i = index; i < dhs.Count; i++)
                                    {
                                        string giayHienTai = dhs[i].Kho.ToString() + dhs[i].GiayMatE;

                                        if (giayHienTai == giay)
                                        {
                                            dhs[i].HoanTatGiayMatE = 1;
                                            Repository.Instance.UpdateColumns("dhdangchay", $"set HoanTatGiayMatE = '1' where Id = {dhs[i].Id}");
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                long.TryParse(message.STT, out long stt);
                                SongEController.Instance.NapGiayMat(dhs.ToList(), stt, false);
                                DonHangHienThi.NotifyResetCollection();
                            }
                            else if (message.DeviceName == SongBTags.Instance.DeviceName)
                            {
                                if (message.DeviceName == SongBTags.Instance.DeviceName)
                                {
                                    dh.HoanTatGiayMatB = 1;
                                    string giay = dh.Kho.ToString() + dh.GiayMatB;
                                    int index = dhs.IndexOf(dh);
                                    if (index > -1)
                                    {
                                        for (int i = index; i < dhs.Count; i++)
                                        {
                                            string giayHienTai = dhs[i].Kho.ToString() + dhs[i].GiayMatB;

                                            if (giayHienTai == giay)
                                            {
                                                dhs[i].HoanTatGiayMatB = 1;
                                                Repository.Instance.UpdateColumns("dhdangchay", $"set HoanTatGiayMatB = '1' where Id = {dhs[i].Id}");
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    long.TryParse(message.STT, out long stt);
                                    SongBController.Instance.NapGiayMat(dhs.ToList(), stt, false);
                                    DonHangHienThi.NotifyResetCollection();
                                }
                            }
                            else if (message.DeviceName == SongCTags.Instance.DeviceName)
                            {
                                if (message.DeviceName == SongCTags.Instance.DeviceName)
                                {
                                    dh.HoanTatGiayMatC = 1;
                                    string giay = dh.Kho.ToString() + dh.GiayMatC;
                                    int index = dhs.IndexOf(dh);
                                    if (index > -1)
                                    {
                                        for (int i = index; i < dhs.Count; i++)
                                        {
                                            string giayHienTai = dhs[i].Kho.ToString() + dhs[i].GiayMatC;

                                            if (giayHienTai == giay)
                                            {
                                                dhs[i].HoanTatGiayMatC = 1;
                                                Repository.Instance.UpdateColumns("dhdangchay", $"set HoanTatGiayMatC = '1' where Id = {dhs[i].Id}");
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    long.TryParse(message.STT, out long stt);
                                    SongCController.Instance.NapGiayMat(dhs.ToList(), stt, false);
                                    DonHangHienThi.NotifyResetCollection();
                                }
                            }
                            else if (message.DeviceName == MayMenTags.Instance.DeviceName)
                            {
                                if (message.DeviceName == MayMenTags.Instance.DeviceName)
                                {
                                    dh.HoanTatMayMen = 1;
                                    string giay = dh.Kho.ToString() + dh.Men;
                                    int index = dhs.IndexOf(dh);
                                    if (index > -1)
                                    {
                                        for (int i = index; i < dhs.Count; i++)
                                        {
                                            string giayHienTai = dhs[i].Kho.ToString() + dhs[i].Men;

                                            if (giayHienTai == giay)
                                            {
                                                dhs[i].HoanTatMayMen = 1;
                                                Repository.Instance.UpdateColumns("dhdangchay", $"set HoanTatMayMen = '1' where Id = {dhs[i].Id}");
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    long.TryParse(message.STT, out long stt);
                                    MayMenController.Instance.NapGiayMat(dhs.ToList(), stt, false);
                                    DonHangHienThi.NotifyResetCollection();
                                }
                            }
                        }
                    }
                }
                catch { }
                finally { semaphore.Release(); }
            });
        }

        protected void OnMessageChuyenDonGiaySong(MessageChuyenDonGiaySong message)
        {
            Dispatcher.Invoke(() =>
            {
                try
                {
                    if (message != null)
                    {
                        List<DonHang> dhs = DonHangDataSource.OrderBy(x => x.STT).ToList();
                        DonHang dh = dhs.FirstOrDefault(x => x.STT.ToString() == message.STT);

                        if (dh != null)
                        {
                            if (message.DeviceName == SongETags.Instance.DeviceName)
                            {
                                dh.HoanTatGiaySongE = 1;
                                string giay = dh.Kho.ToString() + dh.GiaySongE;
                                int index = dhs.IndexOf(dh);
                                if (index > -1)
                                {
                                    for (int i = index; i < dhs.Count; i++)
                                    {
                                        string giayHienTai = dhs[i].Kho.ToString() + dhs[i].GiaySongE;
                                        if (giayHienTai == giay)
                                        {
                                            dhs[i].HoanTatGiaySongE = 1;
                                            Repository.Instance.UpdateColumns("dhdangchay", $"set HoanTatGiaySongE = '1' where Id = {dhs[i].Id}");
                                        }
                                        else
                                            break;
                                    }
                                }

                                long.TryParse(message.STT, out long stt);
                                if (double.TryParse(SongETags.Instance.HeSoSong?.Value, out double heSoSong))
                                {
                                    SongEController.Instance.NapGiaySong(dhs.ToList(), stt, heSoSong, false);
                                }
                                Dispatcher.Invoke(() => DonHangHienThi.NotifyResetCollection());
                            }
                            else if (message.DeviceName == SongBTags.Instance.DeviceName)
                            {
                                dh.HoanTatGiaySongB = 1;
                                string giay = dh.Kho.ToString() + dh.GiaySongB;
                                int index = dhs.IndexOf(dh);
                                if (index > -1)
                                {
                                    for (int i = index; i < dhs.Count; i++)
                                    {
                                        string giayHienTai = dhs[i].Kho.ToString() + dhs[i].GiaySongB;
                                        if (giayHienTai == giay)
                                        {
                                            dhs[i].HoanTatGiaySongB = 1;
                                            Repository.Instance.UpdateColumns("dhdangchay", $"set HoanTatGiaySongB = '1' where Id = {dhs[i].Id}");
                                        }
                                        else
                                            break;
                                    }
                                }

                                long.TryParse(message.STT, out long stt);
                                if (double.TryParse(SongBTags.Instance.HeSoSong?.Value, out double heSoSong))
                                {
                                    SongBController.Instance.NapGiaySong(dhs.ToList(), stt, heSoSong, false);
                                }
                                Dispatcher.Invoke(() => DonHangHienThi.NotifyResetCollection());
                            }
                            else if (message.DeviceName == SongCTags.Instance.DeviceName)
                            {
                                dh.HoanTatGiaySongC = 1;
                                string giay = dh.Kho.ToString() + dh.GiaySongC;
                                int index = dhs.IndexOf(dh);
                                if (index > -1)
                                {
                                    for (int i = index; i < dhs.Count; i++)
                                    {
                                        string giayHienTai = dhs[i].Kho.ToString() + dhs[i].GiaySongC;
                                        if (giayHienTai == giay)
                                        {
                                            dhs[i].HoanTatGiaySongC = 1;
                                            Repository.Instance.UpdateColumns("dhdangchay", $"set HoanTatGiaySongC = '1' where Id = {dhs[i].Id}");
                                        }
                                        else
                                            break;
                                    }
                                }

                                long.TryParse(message.STT, out long stt);
                                if (double.TryParse(SongCTags.Instance.HeSoSong?.Value, out double heSoSong))
                                {
                                    SongCController.Instance.NapGiaySong(dhs.ToList(), stt, heSoSong, false);
                                }
                                Dispatcher.Invoke(() => DonHangHienThi.NotifyResetCollection());
                            }
                        }
                    }
                }
                catch { }
            });
        }

        #endregion

        #region Refresh UI
        int soMetLoi;
        private async void CapNhatSoMetLoi()
        {
            try
            {
                await DriverConnector.WriteTagAsync(CutterTags.Instance.LenhChuyenDon?.Path, "0");
                await DriverConnector.WriteTagAsync(SongETags.Instance.LenhChuyenDon?.Path, "0");
                await DriverConnector.WriteTagAsync(SongETags.Instance.LenhChuyenDonGiayMat.Path, "0");
                await DriverConnector.WriteTagAsync(SongETags.Instance.LenhChuyenDonGiaySong?.Path, "0");
            }
            catch { }

            while(true)
            {
                try
                {
                    if (CutterTags.Instance.DaiCat1 != null && CutterTags.Instance.SLLoi != null &&
                        CutterTags.Instance.STT1 != null)
                    {
                        if (int.TryParse(CutterTags.Instance.DaiCat1.Value, out int dai) &&
                            int.TryParse(CutterTags.Instance.SLLoi.Value, out int slLoi) &&
                            long.TryParse(CutterTags.Instance.STT1.Value, out long sttCutter))
                        {
                            if (dai * slLoi != soMetLoi)
                            {
                                soMetLoi = dai * slLoi;
                                List<WriteCommand> writeComamnds = new List<WriteCommand>();

                                if (SongETags.Instance.Dan != null && SongETags.Instance.STT1 != null &&
                                    long.TryParse(SongETags.Instance.STT1.Value, out long sttSongE))
                                {
                                    if (sttCutter == sttSongE)
                                        writeComamnds.Add(new WriteCommand() { PathToTag = SongETags.Instance.Dan.Path, Value = soMetLoi.ToString() });
                                }

                                if (SongBTags.Instance.Dan != null && SongBTags.Instance.STT1 != null &&
                                    long.TryParse(SongBTags.Instance.STT1.Value, out long sttSongB))
                                {
                                    if (sttCutter == sttSongB)
                                        writeComamnds.Add(new WriteCommand() { PathToTag = SongBTags.Instance.Dan.Path, Value = soMetLoi.ToString() });
                                }

                                if (SongCTags.Instance.Dan != null && SongCTags.Instance.STT1 != null &&
                                     long.TryParse(SongCTags.Instance.STT1.Value, out long sttSongC))
                                {
                                    if (sttCutter == sttSongC)
                                        writeComamnds.Add(new WriteCommand() { PathToTag = SongCTags.Instance.Dan.Path, Value = soMetLoi.ToString() });
                                }

                                if (writeComamnds.Count > 0)
                                {
                                    await DriverConnector.WriteMultiTagAsync(writeComamnds);
                                }
                            }
                        }
                    }
                }
                catch (Exception) { }
                finally { Thread.Sleep(50); }
            }
        }

        private void OnDriverConnectorStarted(object sender, EventArgs e)
        {
            try
            {
                CutterTags.Instance.GetTags();
                SongETags.Instance.GetTags();
                SongBTags.Instance.GetTags();
                SongCTags.Instance.GetTags();
                MayMenTags.Instance.GetTags();

                try
                {
                    CaiDat = Repository.Instance.GetCaiDat();
                }
                catch { }
                NapDon(true, true);

                Dispatcher.Invoke(() =>
                {
                    if (CutterTags.Instance.STT1 != null &&
                        long.TryParse(CutterTags.Instance.STT1.Value, out long sttCutter))
                    {
                        DonHangHienThi.AddRange(DonHangDataSource.Where(x => x.HoanTatCutter == 0 && x.TGKetThuc == DateTime.MinValue && x.STT >= sttCutter).OrderBy(x => x.STT));
                        foreach (var item in DonHangDataSource)
                        {
                            if (item.STT < sttCutter)
                            {
                                if (item.HoanTatCutter == 0)
                                    item.HoanTatCutter = 1;
                            }
                        }
                    }
                    else
                    {
                        DonHangHienThi.AddRange(DonHangDataSource.Where(x => x.HoanTatCutter == 0 && x.TGKetThuc == DateTime.MinValue).OrderBy(x => x.STT));
                    }

                    danhSachDonHang.DonHangDataSource = DonHangHienThi;
                    danhSachDonHang.DonHangDataSource.NotifyResetCollection();
                });

                refreshUITimer = new System.Timers.Timer();
                refreshUITimer.Elapsed += OnRefreshUI;
                refreshUITimer.Interval = 20;
                refreshUITimer.Start();

                thongBaoTimer = new System.Timers.Timer();
                thongBaoTimer.Elapsed += ThongBaoTimer_Elapsed;
                thongBaoTimer.Interval = 500;
                thongBaoTimer.Start();

                taskKiemTra = Task.Factory.StartNew(CapNhatSoMetLoi, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                if (CutterTags.Instance.CheDoChuyenDon != null)
                {
                    CutterTags.Instance.CheDoChuyenDon.ValueChanged += CheDoChuyenDon_ValueChanged;
                    CheDoChuyenDon_ValueChanged(null, new TagValueChangedEventArgs(CutterTags.Instance.CheDoChuyenDon, "0", CutterTags.Instance.CheDoChuyenDon.Value));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ThongBaoTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            thongBaoTimer.Stop();
            try
            {
                Dispatcher.Invoke(() =>
                {
                    HienThiTrangThaiMaySong();
                });
            }
            catch { }
            finally { thongBaoTimer.Start(); }
        }

        private void OnRefreshUI(object sender, System.Timers.ElapsedEventArgs e)
        {
            refreshUITimer.Stop();

            totalRefreshElapsed = sw.ElapsedMilliseconds;
            sw.Restart();

            try
            {

                Dispatcher.Invoke(() =>
                {
                    HienThi3DonHangDau();
                    HienThiDonHangDangChay();
                    HienThiTrangThaiDonHangCa();
                    HienThiThongTinDoiDonCacTram();
                });

                using (MySqlConnection conn = new MySqlConnection(Repository.Instance.ConnectionString))
                {
                    
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        if (WrapData == null)
                        {
                            WrapData = new WrapData();
                            WrapData.TrangThaiDonHang = TrangThaiDonHang;
                            WrapData.ThongTinTram = ThongTinTram;
                            WrapData.GiaySongGiayMatDangChayE = GiaySongGiayMatDangChayE;
                            WrapData.GiaySongGiayMatDangChayB = GiaySongGiayMatDangChayB;
                            WrapData.GiaySongGiayMatDangChayC = GiaySongGiayMatDangChayC;
                            WrapData.GiayMenDangChay = MayMenDangChay;
                            WrapData.DanhSachDonHang = new List<DonHang>();
                        }

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = $"update common set data = '{JsonConvert.SerializeObject(WrapData)}'";
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch { }
            finally { refreshUITimer.Start(); }
        }

        private void HienThiThongTinDoiDonCacTram()
        {
            try
            {
                if (DriverConnector.IsStarted)
                {
                    if (CutterTags.Instance.CheDoChuyenDon != null)
                    {
                        if (CutterTags.Instance.CheDoChuyenDon.Value == "100")
                        {
                            lbChuyenDon.Content = "Chuyển đơn Auto";
                            lbChuyenDon.Background = Brushes.Orange;

                        }
                        else
                        {
                            lbChuyenDon.Content = "Chuyển đơn tay";
                            lbChuyenDon.Background = manualBrush;

                        }
                    }

                    thongTinCacTram.TocDo1 = CutterTags.Instance.TocDo?.Value;
                    thongTinCacTram.TocDo2 = CutterTags.Instance.TocDo?.Value;
                    thongTinCacTram.TocDo3 = CutterTags.Instance.TocDo?.Value;
                    thongTinCacTram.TocDo4 = SongETags.Instance.TocDo?.Value;
                    thongTinCacTram.TocDo5 = SongBTags.Instance.TocDo?.Value;
                    thongTinCacTram.TocDo6 = SongCTags.Instance.TocDo?.Value;

                    ThongTinTram.TocDo1 = CutterTags.Instance.TocDo?.Value;
                    ThongTinTram.TocDo2 = CutterTags.Instance.TocDo?.Value;
                    ThongTinTram.TocDo3 = CutterTags.Instance.TocDo?.Value;
                    ThongTinTram.TocDo4 = SongETags.Instance.TocDo?.Value;
                    ThongTinTram.TocDo5 = SongBTags.Instance.TocDo?.Value;
                    ThongTinTram.TocDo6 = SongCTags.Instance.TocDo?.Value;

                    if (double.TryParse(CutterTags.Instance.DoiDon.Value, out double doiDonCutter))
                    {
                        // Đổi đơn máy cắt tấm / máy xả / máy mền
                        doiDonCutter = doiDonCutter / 1000.0f;
                        double doidon1 = doiDonCutter - CaiDat.DanCatTam;
                        if (doidon1 < 0.0)
                            doidon1 = 0.0;
                        double doidon2 = doiDonCutter - CaiDat.DanMayXa;
                        if (doidon2 < 0.0)
                            doidon2 = 0.0;
                        double doidon3 = doiDonCutter - CaiDat.DanMayMen;
                        if (doidon3 < 0.0)
                            doidon3 = 0.0;

                        thongTinCacTram.DoiDon1 = doidon1.ToString("f0");
                        thongTinCacTram.DoiDon2 = doidon2.ToString("f0");
                        thongTinCacTram.DoiDon3 = doidon3.ToString("f0");


                        ThongTinTram.DoiDon1 = doidon1.ToString("f0");
                        ThongTinTram.DoiDon2 = doidon2.ToString("f0");
                        ThongTinTram.DoiDon3 = doidon3.ToString("f0");

                        // Đổi đơn và dàn sóng E
                        if (double.TryParse(SongETags.Instance.DoiDon.Value, out double doiDonSongE))
                        {
                            doiDonSongE = doiDonSongE / 1000.0f;
                            thongTinCacTram.DoiDon4 = doiDonSongE.ToString("f0");
                            ThongTinTram.DoiDon4 = thongTinCacTram.DoiDon4;
                            double danSongE = doiDonCutter - doiDonSongE;
                            
                            if (danSongE < 0)
                                danSongE = 0;
                            thongTinCacTram.Dan4 = danSongE.ToString("f0");
                            ThongTinTram.Dan4 = thongTinCacTram.Dan4;
                        }

                        // Đổi đơn và dàn sóng B
                        if (double.TryParse(SongBTags.Instance.DoiDon.Value, out double doiDonSongB))
                        {
                            doiDonSongB = doiDonSongB / 1000.0f;
                            thongTinCacTram.DoiDon5 = doiDonSongB.ToString("f0");
                            ThongTinTram.DoiDon5 = thongTinCacTram.DoiDon5;
                            double danSongB = doiDonCutter - doiDonSongB;
                            if (danSongB < 0)
                                danSongB = 0;
                            thongTinCacTram.Dan5 = danSongB.ToString("f0");
                            ThongTinTram.Dan5 = thongTinCacTram.Dan5;
                        }

                        // Đổi đơn và dàn sóng C
                        if (double.TryParse(SongCTags.Instance.DoiDon.Value, out double doiDonSongC))
                        {
                            doiDonSongC = doiDonSongC / 1000.0f;
                            thongTinCacTram.DoiDon6 = doiDonSongC.ToString("f0");
                            ThongTinTram.DoiDon6 = thongTinCacTram.DoiDon6;
                            double danSongC = doiDonCutter - doiDonSongC;
                            if (danSongC < 0)
                                danSongC = 0;
                            thongTinCacTram.Dan6 = danSongC.ToString("f0");
                            ThongTinTram.Dan6 = thongTinCacTram.Dan6;
                        }
                    }
                }
            }
            catch { }
        }

        private void HienThi3DonHangDau()
        {
            try
            {
                if (DriverConnector.IsStarted)
                {
                    donHangChuanBi.CatTam1 = CutterTags.Instance.STT1.Value;
                    donHangChuanBi.CatTam2 = CutterTags.Instance.STT2.Value;
                    donHangChuanBi.CatTam3 = CutterTags.Instance.STT3.Value;

                    donHangChuanBi.DaiCat1 = CutterTags.Instance.DaiCat1.Value;
                    donHangChuanBi.DaiCat2 = CutterTags.Instance.DaiCat2.Value;
                    donHangChuanBi.DaiCat3 = CutterTags.Instance.DaiCat3.Value;

                    donHangChuanBi.SLCat1 = CutterTags.Instance.SLCat1.Value;
                    donHangChuanBi.SLCat2 = CutterTags.Instance.SLCat2.Value;
                    donHangChuanBi.SLCat3 = CutterTags.Instance.SLCat3.Value;

                    donHangChuanBi.Pallet1 = CutterTags.Instance.Pallet1.Value;
                    donHangChuanBi.Pallet2 = CutterTags.Instance.Pallet2.Value;
                    donHangChuanBi.Pallet3 = CutterTags.Instance.Pallet3.Value;

                    if (int.TryParse(CutterTags.Instance.SLCat1.Value, out int slCat) &&
                        int.TryParse(CutterTags.Instance.SLDat.Value, out int slDat) &&
                        int.TryParse(CutterTags.Instance.SLLoi.Value, out int slLoi))
                    {
                        if (slDat < 0)
                            slDat = 0;
                        donHangChuanBi.SLDat1 = slDat.ToString();
                        donHangChuanBi.SLConLai1 = (slCat - slDat).ToString();
                    }
                    else
                    {
                        donHangChuanBi.SLConLai1 = "0";
                    }

                    donHangChuanBi.SLLoi1 = CutterTags.Instance.SLLoi.Value;
                }
            }
            catch { }
        }

        private void HienThiDonHangDangChay()
        {
            semaphore.Wait();
            try
            {
                if (DriverConnector != null && DonHangDataSource != null)
                {
                    List<DonHang> dsdhTam = DonHangDataSource.OrderBy(x => x.STT).ToList();
                    DonHang donHang = null;
                    if (CutterTags.Instance.STT1 != null)
                        donHang = dsdhTam.FirstOrDefault(x => x.STT.ToString() == CutterTags.Instance.STT1.Value);

                    if (donHang != null)
                    {
                        if (UpdaterDonHang.Instance.DonHang != donHang)
                        {
                            UpdaterDonHang.Instance.Start(donHang);
                        }

                        if (SongETags.Instance.STT1 != null)
                        {
                            bool canTinhChieuDaiSong = false;
                            bool canTinhChieuDaiMat = false;
                            long.TryParse(SongETags.Instance.STT1.Value, out long sttDonE);

                            long.TryParse(SongETags.Instance.STTSong1.Value, out long sttSongE1);
                            long.TryParse(SongETags.Instance.STTMat1.Value, out long sttMatE1);
                            long.TryParse(SongETags.Instance.ChieuDaiSong1.Value, out long chieuDaiSong1);
                            long.TryParse(SongETags.Instance.ChieuDaiMat1.Value, out long chieuDaiMat1);

                            long.TryParse(SongETags.Instance.STTSong2.Value, out long sttSongE2);
                            long.TryParse(SongETags.Instance.STTMat2.Value, out long sttMatE2);
                            long.TryParse(SongETags.Instance.ChieuDaiSong2.Value, out long chieuDaiSong2);
                            long.TryParse(SongETags.Instance.ChieuDaiMat2.Value, out long chieuDaiMat2);

                            double.TryParse(SongETags.Instance.DoiGiaySong.Value, out double doiGiaySong);
                            double.TryParse(SongETags.Instance.DoiGiayMat.Value, out double doiGiayMat);

                            DonHang dhSongE = dsdhTam.FirstOrDefault(x => x.STT == sttDonE);
                            if (dhSongE != null)
                            {
                                if (dhSongE.Id > 0 && dhSongE.TGBatDau == DateTime.MinValue)
                                {
                                    dhSongE.TGBatDau = DateTime.Now;
                                    Repository.Instance.UpdateColumn("dhdangchay", "TGBatDau", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), $"where Id = {dhSongE.Id}");
                                    danhSachDonHang.DonHangDataSource.NotifyResetCollection();
                                }
                            }

                            DonHang dhSong1 = dsdhTam.FirstOrDefault(x => x.STT == sttSongE1);
                            if (dhSong1 == null)
                                dhSong1 = DonHang.Empty;

                            DonHang dhMat1 = dsdhTam.FirstOrDefault(x => x.STT == sttMatE1);
                            if (dhMat1 == null)
                                dhMat1 = DonHang.Empty;

                            DonHang dhSong2 = dsdhTam.FirstOrDefault(x => x.STT == sttSongE2);
                            if (dhSong2 == null)
                            {
                                int index1 = dsdhTam.IndexOf(dhSong1);
                                if (index1 >= 0 && index1 + 1 < dsdhTam.Count)
                                {
   
                                }
                                else
                                {
                                    index1 = -1;
                                }

                                string giayTruocDo = dhSong1.GiaySongE;
                                for (int i = index1 + 1; i < dsdhTam.Count; i++)
                                {

                                    if (!string.IsNullOrEmpty(dsdhTam[i].GiaySongE))
                                    {
                                        if (string.IsNullOrEmpty(dhSong1.GiaySongE))
                                        {
                                            canTinhChieuDaiSong = true;
                                            dhSong2 = dsdhTam[i];
                                            break;
                                        }
                                        else
                                        {
                                            if (dhSong1.GiaySongE != dsdhTam[i].GiaySongE)
                                            {
                                                canTinhChieuDaiSong = true;
                                                dhSong2 = dsdhTam[i];
                                                break;
                                            }
                                            else
                                            {
                                                if (giayTruocDo != dhSong1.GiaySongE)
                                                {
                                                    canTinhChieuDaiSong = true;
                                                    dhSong2 = dsdhTam[i];
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        giayTruocDo = dsdhTam[i].GiaySongE;
                                    }
                                }
                            }

                            if (dhSong2 == null)
                                dhSong2 = DonHang.Empty;

                            if (dhSong2.STT <= dhSong1.STT)
                                dhSong2 = DonHang.Empty;

                            DonHang dhMat2 = DonHangDataSource.FirstOrDefault(x => x.STT == sttMatE2);
                            if (dhMat2 == null)
                            {
                                int index1 = dsdhTam.IndexOf(dhMat1);
                                if (index1 >= 0 && index1 + 1 < dsdhTam.Count)
                                {
                                }
                                else
                                {
                                    index1 = -1;
                                }

                                string giayTruocDo = dhMat1.GiayMatE;
                                for (int i = index1 + 1; i < dsdhTam.Count; i++)
                                {

                                    if (!string.IsNullOrEmpty(dsdhTam[i].GiayMatE))
                                    {
                                        if (string.IsNullOrEmpty(dhMat1.GiayMatE))
                                        {
                                            canTinhChieuDaiMat = true;
                                            dhMat2 = dsdhTam[i];
                                            break;
                                        }
                                        else
                                        {
                                            if (dhMat1.GiayMatE != dsdhTam[i].GiayMatE)
                                            {
                                                canTinhChieuDaiMat = true;
                                                dhMat2 = dsdhTam[i];
                                                break;
                                            }
                                            else
                                            {
                                                if (giayTruocDo != dhMat1.GiayMatE)
                                                {
                                                    canTinhChieuDaiMat = true;
                                                    dhMat2 = dsdhTam[i];
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        giayTruocDo = dsdhTam[i].GiayMatE;
                                    }
                                }
                            }
                            if (dhMat2 == null)
                                dhMat2 = DonHang.Empty;
                            if (dhMat2.STT <= dhMat1.STT)
                                dhMat2 = DonHang.Empty;

                            //if (donHang.STT > sttDonE)
                            //    sttDonE = donHang.STT;

                            GiaySongGiayMatDangChayE.STTDon = sttDonE.ToString();
                            GiaySongGiayMatDangChayE.STTSong = sttSongE1.ToString();
                            GiaySongGiayMatDangChayE.STTMat = sttMatE1.ToString();
                            GiaySongGiayMatDangChayE.LoaiGiaySong1 = dhSong1.GiaySongE;
                            GiaySongGiayMatDangChayE.LoaiGiayMat1 = dhMat1.GiayMatE;
                            GiaySongGiayMatDangChayE.LoaiGiaySong2 = dhSong2.GiaySongE;
                            GiaySongGiayMatDangChayE.LoaiGiayMat2 = dhMat2.GiayMatE;
                            GiaySongGiayMatDangChayE.DoiGiayMat = (doiGiayMat / 1000).ToString("f0");
                            GiaySongGiayMatDangChayE.DoiGiaySong = (doiGiaySong / 1000).ToString("f0");
                            GiaySongGiayMatDangChayE.ChieuDaiSong1 = (chieuDaiSong1 / 1000).ToString();
                            GiaySongGiayMatDangChayE.ChieuDaiMat1 = (chieuDaiMat1 / 1000).ToString();
                            GiaySongGiayMatDangChayE.KhoMat1 = dhMat1.Kho.ToString();
                            GiaySongGiayMatDangChayE.KhoMat2 = dhMat2.Kho.ToString();
                            GiaySongGiayMatDangChayE.KhoSong1 = dhSong1.Kho.ToString();
                            GiaySongGiayMatDangChayE.KhoSong2 = dhSong2.Kho.ToString();
                            GiaySongGiayMatDangChayE.STTMat2 = dhMat2.STT.ToString();
                            GiaySongGiayMatDangChayE.STTSong2 = dhSong2.STT.ToString();

                            if (canTinhChieuDaiSong)
                            {
                                Repository.Instance.TinhTongChieuDaiGiay(dsdhTam, dhSong2, "e", out long daiSong, out long daiMat);

                                if (double.TryParse(SongETags.Instance.HeSoSong?.Value, out double heSoSong))
                                {
                                    GiaySongGiayMatDangChayE.ChieuDaiSong2 = Convert.ToInt64((daiSong * heSoSong / 1000)).ToString();
                                }
                                else
                                {
                                    GiaySongGiayMatDangChayE.ChieuDaiSong2 = "0";
                                }

                            }
                            else
                            {
                                GiaySongGiayMatDangChayE.ChieuDaiSong2 = (chieuDaiSong2 / 1000).ToString();
                            }

                            if (canTinhChieuDaiMat)
                            {
                                Repository.Instance.TinhTongChieuDaiGiay(dsdhTam, dhMat2, "e", out long daiSong, out long daiMat);
                                GiaySongGiayMatDangChayE.ChieuDaiMat2 = (daiMat / 1000).ToString();
                            }
                            else
                            {
                                GiaySongGiayMatDangChayE.ChieuDaiMat2 = (chieuDaiMat2 / 1000).ToString();
                            }
                        }

                        if (SongBTags.Instance.STT1 != null)
                        {
                            bool canTinhChieuDaiSong = false;
                            bool canTinhChieuDaiMat = false;

                            long.TryParse(SongBTags.Instance.STT1.Value, out long sttDonB);

                            long.TryParse(SongBTags.Instance.STTSong1.Value, out long sttSongB1);
                            long.TryParse(SongBTags.Instance.STTMat1.Value, out long sttMatB1);
                            long.TryParse(SongBTags.Instance.ChieuDaiSong1.Value, out long chieuDaiSong1);
                            long.TryParse(SongBTags.Instance.ChieuDaiMat1.Value, out long chieuDaiMat1);

                            long.TryParse(SongBTags.Instance.STTSong2.Value, out long sttSongB2);
                            long.TryParse(SongBTags.Instance.STTMat2.Value, out long sttMatB2);
                            long.TryParse(SongBTags.Instance.ChieuDaiSong2.Value, out long chieuDaiSong2);
                            long.TryParse(SongBTags.Instance.ChieuDaiMat2.Value, out long chieuDaiMat2);

                            double.TryParse(SongBTags.Instance.DoiGiaySong.Value, out double doiGiaySong);
                            double.TryParse(SongBTags.Instance.DoiGiayMat.Value, out double doiGiayMat);

                            DonHang dhSongB = dsdhTam.FirstOrDefault(x => x.STT == sttDonB);
                            if (dhSongB != null)
                            {
                                if (dhSongB.Id > 0 && dhSongB.TGBatDau == DateTime.MinValue)
                                {
                                    dhSongB.TGBatDau = DateTime.Now;
                                    Repository.Instance.UpdateColumn("dhdangchay", "TGBatDau", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), $"where Id = {dhSongB.Id}");
                                    danhSachDonHang.DonHangDataSource.NotifyResetCollection();
                                }
                            }

                            DonHang dhSong1 = DonHangDataSource.FirstOrDefault(x => x.STT == sttSongB1);
                            if (dhSong1 == null)
                                dhSong1 = DonHang.Empty;

                            DonHang dhMat1 = DonHangDataSource.FirstOrDefault(x => x.STT == sttMatB1);
                            if (dhMat1 == null)
                                dhMat1 = DonHang.Empty;

                            DonHang dhSong2 = dsdhTam.FirstOrDefault(x => x.STT == sttSongB2);
                            if (dhSong2 == null)
                            {
                                int index1 = dsdhTam.IndexOf(dhSong1);
                                if (index1 >= 0 && index1 + 1 < dsdhTam.Count)
                                {

                                }
                                else
                                {
                                    index1 = -1;
                                }

                                string giayTruocDo = dhSong1.GiaySongB;
                                for (int i = index1 + 1; i < dsdhTam.Count; i++)
                                {

                                    if (!string.IsNullOrEmpty(dsdhTam[i].GiaySongB))
                                    {
                                        if (string.IsNullOrEmpty(dhSong1.GiaySongB))
                                        {
                                            canTinhChieuDaiSong = true;
                                            dhSong2 = dsdhTam[i];
                                            break;
                                        }
                                        else
                                        {
                                            if (dhSong1.GiaySongB != dsdhTam[i].GiaySongB)
                                            {
                                                canTinhChieuDaiSong = true;
                                                dhSong2 = dsdhTam[i];
                                                break;
                                            }
                                            else
                                            {
                                                if (giayTruocDo != dhSong1.GiaySongB)
                                                {
                                                    canTinhChieuDaiSong = true;
                                                    dhSong2 = dsdhTam[i];
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        giayTruocDo = dsdhTam[i].GiaySongB;
                                    }
                                }
                            }
                            if (dhSong2 == null)
                                dhSong2 = DonHang.Empty;
                            if (dhSong2.STT <= dhSong1.STT)
                                dhSong2 = DonHang.Empty;

                            DonHang dhMat2 = DonHangDataSource.FirstOrDefault(x => x.STT == sttMatB2);
                            if (dhMat2 == null)
                            {
                                int index1 = dsdhTam.IndexOf(dhMat1);
                                if (index1 >= 0 && index1 + 1 < dsdhTam.Count)
                                {
  
                                }
                                else
                                {
                                    index1 = -1;
                                }

                                string giayTruocDo = dhMat1.GiayMatB;
                                for (int i = index1 + 1; i < dsdhTam.Count; i++)
                                {

                                    if (!string.IsNullOrEmpty(dsdhTam[i].GiayMatB))
                                    {
                                        if (string.IsNullOrEmpty(dhMat1.GiayMatB))
                                        {
                                            canTinhChieuDaiMat = true;
                                            dhMat2 = dsdhTam[i];
                                            break;
                                        }
                                        else
                                        {
                                            if (dhMat1.GiayMatB != dsdhTam[i].GiayMatB)
                                            {
                                                canTinhChieuDaiMat = true;
                                                dhMat2 = dsdhTam[i];
                                                break;
                                            }
                                            else
                                            {
                                                if (giayTruocDo != dhMat1.GiayMatB)
                                                {
                                                    canTinhChieuDaiMat = true;
                                                    dhMat2 = dsdhTam[i];
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        giayTruocDo = dsdhTam[i].GiayMatB;
                                    }
                                }
                            }
                            if (dhMat2 == null)
                                dhMat2 = DonHang.Empty;
                            if (dhMat2.STT <= dhMat1.STT)
                                dhMat2 = DonHang.Empty;

                            //if (donHang.STT > sttDonB)
                            //    sttDonB = donHang.STT;

                            GiaySongGiayMatDangChayB.STTDon = sttDonB.ToString();
                            GiaySongGiayMatDangChayB.STTSong = sttSongB1.ToString();
                            GiaySongGiayMatDangChayB.STTMat = sttMatB1.ToString();
                            GiaySongGiayMatDangChayB.LoaiGiaySong1 = dhSong1.GiaySongB;
                            GiaySongGiayMatDangChayB.LoaiGiayMat1 = dhMat1.GiayMatB;
                            GiaySongGiayMatDangChayB.LoaiGiaySong2 = dhSong2.GiaySongB;
                            GiaySongGiayMatDangChayB.LoaiGiayMat2 = dhMat2.GiayMatB;
                            GiaySongGiayMatDangChayB.DoiGiayMat = (doiGiayMat / 1000).ToString("f0");
                            GiaySongGiayMatDangChayB.DoiGiaySong = (doiGiaySong / 1000).ToString("f0");

                            GiaySongGiayMatDangChayB.ChieuDaiSong1 = (chieuDaiSong1 / 1000).ToString();
                            GiaySongGiayMatDangChayB.ChieuDaiMat1 = (chieuDaiMat1 / 1000).ToString();

                            GiaySongGiayMatDangChayB.KhoMat1 = dhMat1.Kho.ToString();
                            GiaySongGiayMatDangChayB.KhoMat2 = dhMat2.Kho.ToString();
                            GiaySongGiayMatDangChayB.KhoSong1 = dhSong1.Kho.ToString();
                            GiaySongGiayMatDangChayB.KhoSong2 = dhSong2.Kho.ToString();

                            GiaySongGiayMatDangChayB.STTMat2 = dhMat2.STT.ToString();
                            GiaySongGiayMatDangChayB.STTSong2 = dhSong2.STT.ToString();

                            if (canTinhChieuDaiSong)
                            {
                                Repository.Instance.TinhTongChieuDaiGiay(dsdhTam, dhSong2, "b", out long daiSong, out long daiMat);

                                if (double.TryParse(SongBTags.Instance.HeSoSong?.Value, out double heSoSong))
                                {
                                    GiaySongGiayMatDangChayB.ChieuDaiSong2 = Convert.ToInt64((daiSong * heSoSong / 1000)).ToString();
                                }
                                else
                                {
                                    GiaySongGiayMatDangChayB.ChieuDaiSong2 = "0";
                                }
                            }
                            else
                            {
                                GiaySongGiayMatDangChayB.ChieuDaiSong2 = (chieuDaiSong2 / 1000).ToString();
                            }

                            if (canTinhChieuDaiMat)
                            {
                                Repository.Instance.TinhTongChieuDaiGiay(dsdhTam, dhMat2, "b", out long daiSong, out long daiMat);
                                GiaySongGiayMatDangChayB.ChieuDaiMat2 = (daiMat / 1000).ToString();
                            }
                            else
                            {
                                GiaySongGiayMatDangChayB.ChieuDaiMat2 = (chieuDaiMat2 / 1000).ToString();

                            }
                        }

                        if (SongCTags.Instance.STT1 != null)
                        {
                            bool canTinhChieuDaiSong = false;
                            bool canTinhChieuDaiMat = false;

                            long.TryParse(SongCTags.Instance.STT1.Value, out long sttDonC);

                            long.TryParse(SongCTags.Instance.STTSong1.Value, out long sttSongC1);
                            long.TryParse(SongCTags.Instance.STTMat1.Value, out long sttMatC1);
                            long.TryParse(SongCTags.Instance.ChieuDaiSong1.Value, out long chieuDaiSong1);
                            long.TryParse(SongCTags.Instance.ChieuDaiMat1.Value, out long chieuDaiMat1);

                            long.TryParse(SongCTags.Instance.STTSong2.Value, out long sttSongC2);
                            long.TryParse(SongCTags.Instance.STTMat2.Value, out long sttMatC2);
                            long.TryParse(SongCTags.Instance.ChieuDaiSong2.Value, out long chieuDaiSong2);
                            long.TryParse(SongCTags.Instance.ChieuDaiMat2.Value, out long chieuDaiMat2);

                            double.TryParse(SongCTags.Instance.DoiGiaySong.Value, out double doiGiaySong);
                            double.TryParse(SongCTags.Instance.DoiGiayMat.Value, out double doiGiayMat);

                            DonHang dhSongC = dsdhTam.FirstOrDefault(x => x.STT == sttDonC);
                            if (dhSongC != null)
                            {
                                if (dhSongC.Id > 0 && dhSongC.TGBatDau == DateTime.MinValue)
                                {
                                    dhSongC.TGBatDau = DateTime.Now;
                                    Repository.Instance.UpdateColumn("dhdangchay", "TGBatDau", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), $"where Id = {dhSongC.Id}");
                                    danhSachDonHang.DonHangDataSource.NotifyResetCollection();
                                }
                            }

                            DonHang dhSong1 = DonHangDataSource.FirstOrDefault(x => x.STT == sttSongC1);
                            if (dhSong1 == null)
                                dhSong1 = DonHang.Empty;

                            DonHang dhMat1 = DonHangDataSource.FirstOrDefault(x => x.STT == sttMatC1);
                            if (dhMat1 == null)
                                dhMat1 = DonHang.Empty;

                            DonHang dhSong2 = dsdhTam.FirstOrDefault(x => x.STT == sttSongC2);
                            if (dhSong2 == null)
                            {
                                int index1 = dsdhTam.IndexOf(dhSong1);
                                if (index1 >= 0 && index1 + 1 < dsdhTam.Count)
                                {
                                }
                                else
                                {
                                    index1 = -1;
                                }
                                string giayTruocDo = dhSong1.GiaySongC;
                                for (int i = index1 + 1; i < dsdhTam.Count; i++)
                                {

                                    if (!string.IsNullOrEmpty(dsdhTam[i].GiaySongC))
                                    {
                                        if (string.IsNullOrEmpty(dhSong1.GiaySongC))
                                        {
                                            canTinhChieuDaiSong = true;
                                            dhSong2 = dsdhTam[i];
                                            break;
                                        }
                                        else
                                        {
                                            if (dhSong1.GiaySongC != dsdhTam[i].GiaySongC)
                                            {
                                                canTinhChieuDaiSong = true;
                                                dhSong2 = dsdhTam[i];
                                                break;
                                            }
                                            else
                                            {
                                                if (giayTruocDo != dhSong1.GiaySongC)
                                                {
                                                    canTinhChieuDaiSong = true;
                                                    dhSong2 = dsdhTam[i];
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        giayTruocDo = dsdhTam[i].GiaySongC;
                                    }
                                }
                            }
                            if (dhSong2 == null)
                                dhSong2 = DonHang.Empty;
                            if (dhSong2.STT <= dhSong1.STT)
                                dhSong2 = DonHang.Empty;


                            DonHang dhMat2 = DonHangDataSource.FirstOrDefault(x => x.STT == sttMatC2);
                            if (dhMat2 == null)
                            {
                                int index1 = dsdhTam.IndexOf(dhMat1);
                                if (index1 >= 0 && index1 + 1 < dsdhTam.Count)
                                {
                                }
                                else
                                {
                                    index1 = -1;
                                }

                                string giayTruocDo = dhMat1.GiayMatC;
                                for (int i = index1 + 1; i < dsdhTam.Count; i++)
                                {

                                    if (!string.IsNullOrEmpty(dsdhTam[i].GiayMatC))
                                    {
                                        if (string.IsNullOrEmpty(dhMat1.GiayMatC))
                                        {
                                            canTinhChieuDaiMat = true;
                                            dhMat2 = dsdhTam[i];
                                            break;
                                        }
                                        else
                                        {
                                            if (dhMat1.GiayMatC != dsdhTam[i].GiayMatC)
                                            {
                                                canTinhChieuDaiMat = true;
                                                dhMat2 = dsdhTam[i];
                                                break;
                                            }
                                            else
                                            {
                                                if (giayTruocDo != dhMat1.GiayMatC)
                                                {
                                                    canTinhChieuDaiMat = true;
                                                    dhMat2 = dsdhTam[i];
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        giayTruocDo = dsdhTam[i].GiayMatC;
                                    }
                                }

                            }
                            if (dhMat2 == null)
                                dhMat2 = DonHang.Empty;
                            if (dhMat2.STT <= dhMat1.STT)
                                dhMat2 = DonHang.Empty;


                            //if (donHang.STT > sttDonC)
                            //    sttDonC = donHang.STT;

                            GiaySongGiayMatDangChayC.STTDon = sttDonC.ToString();
                            GiaySongGiayMatDangChayC.STTSong = sttSongC1.ToString();
                            GiaySongGiayMatDangChayC.STTMat = sttMatC1.ToString();
                            GiaySongGiayMatDangChayC.LoaiGiaySong1 = dhSong1.GiaySongC;
                            GiaySongGiayMatDangChayC.LoaiGiayMat1 = dhMat1.GiayMatC;
                            GiaySongGiayMatDangChayC.LoaiGiaySong2 = dhSong2.GiaySongC;
                            GiaySongGiayMatDangChayC.LoaiGiayMat2 = dhMat2.GiayMatC;
                            GiaySongGiayMatDangChayC.DoiGiayMat = (doiGiayMat / 1000).ToString("f0");
                            GiaySongGiayMatDangChayC.DoiGiaySong = (doiGiaySong / 1000).ToString("f0");
                            GiaySongGiayMatDangChayC.ChieuDaiSong1 = (chieuDaiSong1 / 1000).ToString();
                            GiaySongGiayMatDangChayC.ChieuDaiMat1 = (chieuDaiMat1 / 1000).ToString();
                            GiaySongGiayMatDangChayC.KhoMat1 = dhMat1.Kho.ToString();
                            GiaySongGiayMatDangChayC.KhoMat2 = dhMat2.Kho.ToString();
                            GiaySongGiayMatDangChayC.KhoSong1 = dhSong1.Kho.ToString();
                            GiaySongGiayMatDangChayC.KhoSong2 = dhSong2.Kho.ToString();
                            GiaySongGiayMatDangChayC.STTMat2 = dhMat2.STT.ToString();
                            GiaySongGiayMatDangChayC.STTSong2 = dhSong2.STT.ToString();
                            if (canTinhChieuDaiSong)
                            {
                                Repository.Instance.TinhTongChieuDaiGiay(dsdhTam, dhSong2, "c", out long daiSong, out long daiMat);

                                if (double.TryParse(SongCTags.Instance.HeSoSong?.Value, out double heSoSong))
                                {
                                    GiaySongGiayMatDangChayC.ChieuDaiSong2 = Convert.ToInt64((daiSong * heSoSong / 1000)).ToString();
                                }
                                else
                                {
                                    GiaySongGiayMatDangChayC.ChieuDaiSong2 = "0";
                                }
                            }
                            else
                            {
                                GiaySongGiayMatDangChayC.ChieuDaiSong2 = (chieuDaiSong2 / 1000).ToString();
                            }

                            if (canTinhChieuDaiMat)
                            {
                                Repository.Instance.TinhTongChieuDaiGiay(dsdhTam, dhMat2, "c", out long daiSong, out long daiMat);
                                GiaySongGiayMatDangChayC.ChieuDaiMat2 = (daiMat / 1000).ToString();
                            }
                            else
                            {
                                GiaySongGiayMatDangChayC.ChieuDaiMat2 = (chieuDaiMat2 / 1000).ToString();

                            }
                        }

                        if (MayMenTags.Instance.STT1 != null)
                        {
                            bool canTinhChieuDaiSong = false;
                            bool canTinhChieuDaiMat = false;

                            long.TryParse(MayMenTags.Instance.STT1.Value, out long sttDonC);

                            long.TryParse(MayMenTags.Instance.STTSong1.Value, out long sttSongC1);
                            long.TryParse(MayMenTags.Instance.STTMat1.Value, out long sttMatC1);
                            long.TryParse(MayMenTags.Instance.ChieuDaiSong1.Value, out long chieuDaiSong1);
                            long.TryParse(MayMenTags.Instance.ChieuDaiMat1.Value, out long chieuDaiMat1);

                            long.TryParse(MayMenTags.Instance.STTSong2.Value, out long sttSongC2);
                            long.TryParse(MayMenTags.Instance.STTMat2.Value, out long sttMatC2);
                            long.TryParse(MayMenTags.Instance.ChieuDaiSong2.Value, out long chieuDaiSong2);
                            long.TryParse(MayMenTags.Instance.ChieuDaiMat2.Value, out long chieuDaiMat2);

                            double.TryParse(MayMenTags.Instance.DoiGiaySong.Value, out double doiGiaySong);
                            double.TryParse(MayMenTags.Instance.DoiGiayMat.Value, out double doiGiayMat);

                            DonHang dhMat1 = DonHangDataSource.FirstOrDefault(x => x.STT == sttMatC1);
                            if (dhMat1 == null)
                                dhMat1 = DonHang.Empty;


                            DonHang dhMat2 = DonHangDataSource.FirstOrDefault(x => x.STT == sttMatC2);
                            if (dhMat2 == null)
                            {
                                int index1 = dsdhTam.IndexOf(dhMat1);
                                if (index1 >= 0 && index1 + 1 < dsdhTam.Count)
                                {
                                }
                                else
                                {
                                    index1 = -1;
                                }

                                string giayTruocDo = dhMat1.Men;
                                for (int i = index1 + 1; i < dsdhTam.Count; i++)
                                {

                                    if (!string.IsNullOrEmpty(dsdhTam[i].Men))
                                    {
                                        if (string.IsNullOrEmpty(dhMat1.Men))
                                        {
                                            canTinhChieuDaiMat = true;
                                            dhMat2 = dsdhTam[i];
                                            break;
                                        }
                                        else
                                        {
                                            if (dhMat1.Men != dsdhTam[i].Men)
                                            {
                                                canTinhChieuDaiMat = true;
                                                dhMat2 = dsdhTam[i];
                                                break;
                                            }
                                            else
                                            {
                                                if (giayTruocDo != dhMat1.Men)
                                                {
                                                    canTinhChieuDaiMat = true;
                                                    dhMat2 = dsdhTam[i];
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        giayTruocDo = dsdhTam[i].Men;
                                    }
                                }

                            }
                            if (dhMat2 == null)
                                dhMat2 = DonHang.Empty;
                            if (dhMat2.STT <= dhMat1.STT)
                                dhMat2 = DonHang.Empty;


                            if (donHang.STT > sttDonC)
                                sttDonC = donHang.STT;

                            MayMenDangChay.STTDon = sttDonC.ToString();
                            MayMenDangChay.STTSong = sttSongC1.ToString();
                            MayMenDangChay.STTMat = sttMatC1.ToString();
                            MayMenDangChay.LoaiGiaySong1 = "";
                            MayMenDangChay.LoaiGiayMat1 = dhMat1.Men;
                            MayMenDangChay.LoaiGiaySong2 = "";
                            MayMenDangChay.LoaiGiayMat2 = dhMat2.Men;
                            MayMenDangChay.DoiGiayMat = (doiGiayMat / 1000).ToString("f0");
                            MayMenDangChay.DoiGiaySong = (doiGiaySong / 1000).ToString("f0");
                            MayMenDangChay.ChieuDaiSong1 = (chieuDaiSong1 / 1000).ToString();
                            MayMenDangChay.ChieuDaiMat1 = (chieuDaiMat1 / 1000).ToString();
                            MayMenDangChay.KhoMat1 = dhMat1.Kho.ToString();
                            MayMenDangChay.KhoMat2 = dhMat2.Kho.ToString();
                            MayMenDangChay.KhoSong1 = "";
                            MayMenDangChay.KhoSong2 = "";
                            MayMenDangChay.STTMat2 = dhMat2.STT.ToString();
                            MayMenDangChay.STTSong2 = "";

                            MayMenDangChay.ChieuDaiSong2 = "0";

                            if (canTinhChieuDaiMat)
                            {
                                Repository.Instance.TinhTongChieuDaiGiay(dsdhTam, dhMat2, "m", out long daiSong, out long daiMat);
                                MayMenDangChay.ChieuDaiMat2 = (daiMat / 1000).ToString();
                            }
                            else
                            {
                                MayMenDangChay.ChieuDaiMat2 = (chieuDaiMat2 / 1000).ToString();
                            }
                        }


                        if (int.TryParse(CutterTags.Instance.SLDat.Value, out int slDat))
                            donHang.SLDat = slDat;
                        if (int.TryParse(CutterTags.Instance.SLLoi.Value, out int slLoi))
                            donHang.SLLoi = slLoi;

                        if (!string.IsNullOrEmpty(donHang.TrangThaiTruocDo))
                        {
                            if (CutterTags.Instance.Run.Value == "100")
                            {
                                donHang.Chay = donHang.Chay.Add(TimeSpan.FromMilliseconds(totalRefreshElapsed));
                            }
                            else
                            {
                                if (donHang.TrangThaiTruocDo == "100")
                                {
                                    donHang.SoDung++;
                                }
                                donHang.Dung = donHang.Dung.Add(TimeSpan.FromMilliseconds(totalRefreshElapsed));
                            }
                        }
                        donHang.TrangThaiTruocDo = CutterTags.Instance.Run.Value;
                    }
                    else
                    {
                        donHang = new DonHang();
                        UpdaterDonHang.Instance.Stop();
                    }

                    if (WrapData != null && danhSachDonHang.DonHangDataSource != null)
                        WrapData.DanhSachDonHang = danhSachDonHang.DonHangDataSource.ToList();

                    donHangDangChay.TK1 = donHang.STT.ToString();
                    donHangDangChay.SoMet1 = donHang.Tong.ToString("f0");
                    donHangDangChay.SoMetDat1 = donHang.SoMetDat.ToString("f0");
                    donHangDangChay.SoMetLoi1 = donHang.SoMetLoi.ToString("f0");
                    donHangDangChay.ConLai1 = donHang.ConLai.ToString("f0");
                    donHangDangChay.PhanTramLoi1 = donHang.PhanTramLoi;
                    donHangDangChay.TocDoTrungBinh1 = donHang.TocDoTB.ToString();
                    donHangDangChay.Chay1 = $"{donHang.Chay.Hours}:{donHang.Chay.Minutes.ToString("00")}:{donHang.Chay.Seconds.ToString("00")}";
                    donHangDangChay.Dung1 = $"{donHang.Dung.Hours}:{donHang.Dung.Minutes.ToString("00")}:{donHang.Dung.Seconds.ToString("00")}";
                    donHangDangChay.SoDung1 = donHang.SoDung.ToString();
                    donHangDangChay.M2Dat1 = donHang.M2Dat.ToString("f0");
                    donHangDangChay.M2Loi1 = donHang.M2Loi.ToString("f0");

                    donHangDangChay.TK2 = $"Ca {Repository.Instance.Ca}";
                    donHangDangChay.SoMet2 = TrangThaiDonHang.SoMet2;
                    donHangDangChay.SoMetDat2 = TrangThaiDonHang.SoMetDat2;
                    donHangDangChay.SoMetLoi2 = TrangThaiDonHang.SoMetLoi2;
                    donHangDangChay.ConLai2 = TrangThaiDonHang.ConLai2;
                    donHangDangChay.PhanTramLoi2 = TrangThaiDonHang.PhanTramLoi2;
                    donHangDangChay.TocDoTrungBinh2 = TrangThaiDonHang.TocDoTB2;
                    donHangDangChay.Chay2 = TrangThaiDonHang.Chay2;
                    donHangDangChay.Dung2 = TrangThaiDonHang.Dung2;
                    donHangDangChay.SoDung2 = TrangThaiDonHang.SoDung2;
                    donHangDangChay.M2Dat2 = TrangThaiDonHang.M2Dat2;
                    donHangDangChay.M2Loi2 = TrangThaiDonHang.M2Loi2;

                    TrangThaiDonHang.STT1 = donHang.STT.ToString();
                    TrangThaiDonHang.SoMet1 = donHang.Tong.ToString("f0");
                    TrangThaiDonHang.SoMetDat1 = donHang.SoMetDat.ToString("f0");
                    TrangThaiDonHang.SoMetLoi1 = donHang.SoMetLoi.ToString("f0");
                    TrangThaiDonHang.ConLai1 = donHang.ConLai.ToString("f0");
                    TrangThaiDonHang.PhanTramLoi1 = donHang.PhanTramLoi;
                    TrangThaiDonHang.TocDoTB1 = donHang.TocDoTB.ToString();
                    TrangThaiDonHang.Chay1 = $"{donHang.Chay.Hours}:{donHang.Chay.Minutes.ToString("00")}:{donHang.Chay.Seconds.ToString("00")}";
                    TrangThaiDonHang.Dung1 = $"{donHang.Dung.Hours}:{donHang.Dung.Minutes.ToString("00")}:{donHang.Dung.Seconds.ToString("00")}";
                    TrangThaiDonHang.SoDung1 = donHang.SoDung.ToString();
                    TrangThaiDonHang.M2Dat1 = donHang.M2Dat.ToString("f0");
                    TrangThaiDonHang.M2Loi1 = donHang.M2Loi.ToString("f0");

                }
            }
            catch { }
            finally { semaphore.Release(); }
        }

        private void HienThiTrangThaiMaySong()
        {
            semaphore.Wait();
            try
            {
                SolidColorBrush mauTrangThaiSongE = Brushes.Red;
                SolidColorBrush mauTrangThaiSongB = Brushes.Red;
                SolidColorBrush mauTrangThaiSongC = Brushes.Red;

                if (CutterTags.Instance.STT1 != null)
                {
                    string sttMayCat = CutterTags.Instance.STT1.Value;
                    DonHang dhHienTai = DonHangDataSource.FirstOrDefault(x => x.STT.ToString() == sttMayCat);

                    if (SongETags.Instance.STT1 != null)
                    {
                        if (SongETags.Instance.STT1.Quality == Quality.Good)
                        {
                            if (CutterController.Instance.ThongBaoChuanBiGiaySongE)
                            {
                                if (thongTinCacTram.TrangThai4 != Brushes.Orange)
                                    mauTrangThaiSongE = Brushes.Orange;
                                else
                                    mauTrangThaiSongE = Brushes.Transparent;
                            }
                            else
                            {
                                if (CutterController.Instance.ThongBaoNapDonSongE)
                                {
                                    mauTrangThaiSongE = Brushes.Lime;
                                }
                                else
                                {
                                    string giayHienTai = dhHienTai?.GiaySongE + dhHienTai?.GiayMatE;
                                    if (string.IsNullOrWhiteSpace(giayHienTai))
                                    {
                                        mauTrangThaiSongE = Brushes.Transparent;
                                    }
                                    else
                                    {
                                        mauTrangThaiSongE = Brushes.Lime;
                                    }
                                }
                            }
                        }
                        else
                        {
                            mauTrangThaiSongE = Brushes.Red;
                        }
                    }

                    if (SongBTags.Instance.STT1 != null)
                    {
                        if (SongBTags.Instance.STT1.Quality == Quality.Good)
                        {
                            if (CutterController.Instance.ThongBaoChuanBiGiaySongB)
                            {
                                if (thongTinCacTram.TrangThai4 != Brushes.Orange)
                                    mauTrangThaiSongB = Brushes.Orange;
                                else
                                    mauTrangThaiSongB = Brushes.Transparent;
                            }
                            else
                            {
                                if (CutterController.Instance.ThongBaoNapDonSongB)
                                {
                                    mauTrangThaiSongB = Brushes.Lime;
                                }
                                else
                                {
                                    string giayHienTai = dhHienTai?.GiaySongB + dhHienTai?.GiayMatB;
                                    if (string.IsNullOrWhiteSpace(giayHienTai))
                                    {
                                        mauTrangThaiSongB = Brushes.Transparent;
                                    }
                                    else
                                    {
                                        mauTrangThaiSongB = Brushes.Lime;
                                    }
                                }
                            }
                        }
                        else
                        {
                            mauTrangThaiSongB = Brushes.Red;
                        }
                    }

                    if (SongCTags.Instance.STT1 != null)
                    {
                        if (SongCTags.Instance.STT1.Quality == Quality.Good)
                        {
                            if (CutterController.Instance.ThongBaoChuanBiGiaySongC)
                            {
                                if (thongTinCacTram.TrangThai4 != Brushes.Orange)
                                    mauTrangThaiSongC = Brushes.Orange;
                                else
                                    mauTrangThaiSongC = Brushes.Transparent;
                            }
                            else
                            {
                                if (CutterController.Instance.ThongBaoNapDonSongC)
                                {
                                    mauTrangThaiSongC = Brushes.Lime;
                                }
                                else
                                {
                                    string giayHienTai = dhHienTai?.GiaySongC + dhHienTai?.GiayMatC;
                                    if (string.IsNullOrWhiteSpace(giayHienTai))
                                    {
                                        mauTrangThaiSongC = Brushes.Transparent;
                                    }
                                    else
                                    {
                                        mauTrangThaiSongC = Brushes.Lime;
                                    }
                                }
                            }
                        }
                        else
                        {
                            mauTrangThaiSongC = Brushes.Red;
                        }
                    }
                }

                thongTinCacTram.TrangThai4 = mauTrangThaiSongE;
                thongTinCacTram.TrangThai5 = mauTrangThaiSongB;
                thongTinCacTram.TrangThai6 = mauTrangThaiSongC;

                ThongTinTram.TrangThai4 = mauTrangThaiSongE.ToString();
                ThongTinTram.TrangThai5 = mauTrangThaiSongB.ToString();
                ThongTinTram.TrangThai6 = mauTrangThaiSongC.ToString();
            }
            catch { }
            finally { semaphore.Release(); }
        }

        DateTime lastUpdateTime;
        DonHang lastUpdateDH;
        private void HienThiMaTrix()
        {
            //while (true)
            //{
            //    try
            //    {
            //        DonHang donHang = null;
            //        semaphore.Wait();
            //        try
            //        {
            //            if (CutterTags.Instance.STT1 != null)
            //                donHang = DonHangDataSource.FirstOrDefault(x => x.STT.ToString() == CutterTags.Instance.STT1.Value);
            //        }
            //        catch { }
            //        finally { semaphore.Release(); }

            //        if (lastUpdateDH != donHang)
            //        {
            //            lastUpdateDH = donHang;
            //            Repository.Instance.HienThiMatrix(lastUpdateDH, TrangThaiDonHang);
            //            lastUpdateTime = DateTime.Now;
            //        }

            //        if ((DateTime.Now - lastUpdateTime).TotalSeconds >= 2)
            //        {
            //            Repository.Instance.HienThiMatrix(lastUpdateDH, TrangThaiDonHang);
            //            lastUpdateTime = DateTime.Now;
            //        }

            //    }
            //    catch { }
            //    finally { Thread.Sleep(1000); }
            //}
        }

        private void HienThiTrangThaiDonHangCa()
        {
            try
            {
                string time = DateTime.Now.ToString("yyyy-MM-dd 00:00:01");
                List<DonHang> dhs = Repository.Instance.GetDonHangDataChay($"select * from donhang where Ca = {Repository.Instance.Ca} and " +
                    $"NgayTao >= '{time}' and SLDat > 0");
                if (dhs == null)
                    dhs = new List<DonHang>();
                var result = dhs.Where(x => x.TGBatDau != DateTime.MinValue && x.SLDat > 0).ToList();

                TrangThaiDonHang.SoMet2 = dhs.Select(x => x.Tong).Sum().ToString("f0");
                TrangThaiDonHang.SoMetDat2 = dhs.Select(x => x.SoMetDat).Sum().ToString("f0");
                TrangThaiDonHang.SoMetLoi2 = dhs.Select(x => x.SoMetLoi).Sum().ToString("f0");
                TrangThaiDonHang.ConLai2 = dhs.Select(x => x.ConLai).Sum().ToString("f0");
                TrangThaiDonHang.PhanTramLoi2 = dhs.Select(x => {
                    double.TryParse(x.PhanTramLoi, out double phanTram);
                    return phanTram;
                }).Sum().ToString("f0");
                TrangThaiDonHang.TocDoTB2 = ((double)dhs.Select(x => x.TocDoTB).Sum() / (double)dhs.Count).ToString("f0");

                TimeSpan tgChay = TimeSpan.Zero;
                TimeSpan tgDung = TimeSpan.Zero;
                int soDung = 0;
                double m2Dat = 0.0;
                double m2Loi = 0.0;
                foreach (var item in dhs)
                {
                    tgChay += item.Chay;
                    tgDung += item.Dung;

                    soDung += item.SoDung;

                    m2Dat += item.M2Dat;
                    m2Loi += item.M2Loi;
                }
                TrangThaiDonHang.Chay2 = $"{tgChay.Hours}:{tgChay.Minutes.ToString("00")}:{tgChay.Seconds.ToString("00")}";
                TrangThaiDonHang.Dung2 = $"{tgDung.Hours}:{tgDung.Minutes.ToString("00")}:{tgDung.Seconds.ToString("00")}";
                TrangThaiDonHang.SoDung2 = soDung.ToString();
                TrangThaiDonHang.M2Dat2 = m2Dat.ToString("f0");
                TrangThaiDonHang.M2Loi2 = m2Loi.ToString("f0");
            }
            catch { }
        }

        #endregion
    }
}
