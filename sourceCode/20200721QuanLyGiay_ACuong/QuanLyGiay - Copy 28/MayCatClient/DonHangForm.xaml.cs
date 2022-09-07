using ClosedXML.Excel;
using CommonControls;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MayCatClient
{
    /// <summary>
    /// Interaction logic for DonHangForm.xaml
    /// </summary>
    public partial class DonHangForm : Window
    {
        MainWindow mainWindow;
        public CaiDat CaiDat { get; set; }
        public int State { get; set; }
        public static ObservableCollection<DonHang> DonHangDataSource { get; set; }
        public static List<string> MaDataSource { get; set; }
        public static List<string> SongDataSource { get; set; }
        public static List<string> GiayMenDataSource { get; set; }
        public static List<string> GiaySongEDataSource { get; set; }
        public static List<string> GiayMatEDataSource { get; set; }
        public static List<string> GiaySongBDataSource { get; set; }
        public static List<string> GiayMatBDataSource { get; set; }
        public static List<string> GiaySongCDataSource { get; set; }
        public static List<string> GiayMatCDataSource { get; set; }
        public static int IsChanged { get; set; }
        public bool ChoPhepSuaGiay { get; set; }
        public bool STTChanged { get; set; }

        static DonHangForm()
        {
            MaDataSource = GetList("Ma", "donhang");
            SongDataSource = GetList("Song", "donhang");
            GiayMenDataSource = GetList("GiayMen", "donhang");
            GiaySongEDataSource = GetList("GiaySongE", "donhang");
            GiayMatEDataSource = GetList("GiayMatE", "donhang");
            GiaySongBDataSource = GetList("GiaySongB", "donhang");
            GiayMatBDataSource = GetList("GiayMatB", "donhang");
            GiaySongCDataSource = GetList("GiaySongC", "donhang");
            GiayMatCDataSource = GetList("GiayMatC", "donhang");
        }

        public DonHangForm(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            CaiDat = Repository.Instance.GetCaiDat();

            Loaded += DonHangForm_Loaded;
            KeyDown += DonHangForm_KeyDown;
            dataGrid.PreviewKeyUp += DataGrid_PreviewKeyUp;
            txbSTT.TextChanged += TxbSTT_TextChanged;
        }

        private void TxbSTT_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (State == 1)
            {
                if (!string.IsNullOrEmpty(txbSTT.Text))
                    STTChanged = true;
            }
        }

        private void DataGrid_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            DonHangForm_KeyDown(sender, e);
        }

        private void DonHangForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                if (btnThem.IsEnabled)
                    BtnThem_Click(null, null);
            }
            else if (e.Key == Key.Enter)
            {
                if (btnLuu.IsEnabled)
                    BtnLuu_Click(null, null);

            }
            else if (e.Key == Key.F2)
            {
                if (btnSua.IsEnabled)
                {
                    BtnSua_Click(null, null);
                }
            }
            else if (e.Key == Key.Escape)
            {
                if (btnHuy.IsEnabled)
                    BtnHuy_Click(null, null);
                else
                    this.Close();
            }
            else if (e.Key == Key.Delete)
            {
                if (btnXoa.IsEnabled)
                    BtnXoa_Click(null, null);
            }
            else if (e.Key == Key.F5)
            {
                if (btnLamMoi.IsEnabled)
                {
                    BtnLamMoi_Click(null, null);
                }
            }
        }

        private void DonHangForm_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateButton();
            RefreshTable();
            if (DonHangDataSource.Count > 0)
            {
                dataGrid.SelectedItem = DonHangDataSource[0];
                dataGrid.SelectedIndex = 0;
            }
            btnThem.Click += BtnThem_Click;
            btnHuy.Click += BtnHuy_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLuu.Click += BtnLuu_Click;
            btnClear.Click += BtnClear_Click;
            btnLamMoi.Click += BtnLamMoi_Click;
            btnNhapExcel.Click += BtnNhapExcel_Click;
            foreach (var item in grid.Children)
            {
                if (item is ComboBox cob)
                {
                    cob.StaysOpenOnEdit = true;
                    cob.GotFocus += (s, args) =>
                    {
                        cob.IsDropDownOpen = true;
                    };
                }
                if (item is TextBox tb)
                {
                    tb.GotFocus += (s, args) =>
                    {
                        tb.SelectAll();
                    };
                }
            }
        }

        private async void BtnNhapExcel_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Import";
                ofd.Filter = "Excel file *.xlsx|*.xlsx";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == true)
                {
                    string filePath = ofd.FileName;
                    using (var wb = new XLWorkbook(filePath))
                    {
                        var ws = wb.Worksheets.FirstOrDefault();
                        if (ws == null)
                        {
                            MessageBox.Show($"Không tìm thấy Sheet", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            int rowIndex = 4;
                            int colIndex = 2;

                            List<DonHang> donHangs = new List<DonHang>();

                            for (int i = 4; i < int.MaxValue; i++)
                            {
                                string daiString = ws.Cell($"D{i}").GetString();
                                if (float.TryParse(daiString, out float dai))
                                {
                                    if (dai <= 0)
                                    {
                                        continue;
                                    }

                                    
                                    string khachHang = ws.Cell($"B{i}").GetString();
                                    string maDon = ws.Cell($"C{i}").GetString();
                                    string song = ws.Cell($"I{i}").GetString();

                                    if (!float.TryParse(ws.Cell($"E{i}").GetString(), out float rong))
                                    {
                                        MessageBox.Show($"Chiều rộng thùng phải là số lớn hơn 0 - E{i}");
                                        return;
                                    }
                                    else
                                    {
                                        if (rong <= 0)
                                        {
                                            continue;
                                        }
                                    }

                                    if (!float.TryParse(ws.Cell($"F{i}").GetString(), out float cao))
                                    {
                                        //MessageBox.Show($"Chiều cao thùng phải là số lớn hơn 0 - F{i}");
                                        //return;
                                    }
                                    else
                                    {
                                        //if (cao <= 0)
                                        //{
                                        //    MessageBox.Show($"Chiều cao thùng bằng 0 tại F{i}");
                                        //    return;
                                        //}
                                    }

                                    if (!float.TryParse(ws.Cell($"J{i}").GetString(), out float kho))
                                    {
                                        MessageBox.Show($"Khổ phải là số lớn hơn 0 - J{i}");
                                        return;
                                    }
                                    else
                                    {
                                        if (kho <= 0)
                                        {
                                            MessageBox.Show($"Khổ bằng 0 tại J{i}");
                                            return;
                                        }
                                    }

                                    if (!float.TryParse(ws.Cell($"G{i}").GetString(), out float sl))
                                    {
                                        MessageBox.Show($"Số lượng phải là số lớn hơn 0 - G{i}");
                                        return;
                                    }
                                    else
                                    {
                                        if (sl <= 0)
                                        {
                                            MessageBox.Show($"Số lượng bằng 0 tại G{i}");
                                            return;
                                        }
                                    }

                                    if (!float.TryParse(ws.Cell($"W{i}").GetString(), out float lang))
                                    {
                                        MessageBox.Show($"Kiểu lằng phải là số - W{i}");
                                        return;
                                    }

                                    if (!float.TryParse(ws.Cell($"S{i}").GetString(), out float xa))
                                    {
                                        MessageBox.Show($"Xả phải là số lớn hơn 0 - S{i}");
                                        return;
                                    }
                                    else
                                    {
                                        if (xa <= 0)
                                        {
                                            MessageBox.Show($"Xả bằng 0 tại S{i}");
                                            return;
                                        }
                                    }

                                    float.TryParse(ws.Cell($"T{i}").CachedValue?.ToString(), out float nap1);
                                    float.TryParse(ws.Cell($"U{i}").CachedValue?.ToString(), out float cao1);
                                    float.TryParse(ws.Cell($"V{i}").CachedValue?.ToString(), out float nap2);


                                    var daiCatCell = ws.Cell($"X{i}");
                                    string daiCatStr = daiCatCell.CachedValue?.ToString();

                                    if (!float.TryParse(daiCatStr, out float daiCat))
                                    {
                                        MessageBox.Show($"Dài cắt là số lớn hơn 0 - X{i}");
                                        return;
                                    }
                                    else
                                    {
                                        if (daiCat <= 0)
                                        {
                                            MessageBox.Show($"Dài cắt bằng 0 tại X{i}");
                                            return;
                                        }
                                    }



                                    if (!int.TryParse(ws.Cell($"AA{i}").GetString(), out int pallet))
                                    {
                                        //MessageBox.Show($"Xả phải là số lớn hơn 0 - V{i}");
                                        //return;
                                    }

                                    double.TryParse(ws.Cell($"Y{i}").CachedValue?.ToString(), out double slThung);
                                    int.TryParse(ws.Cell($"A{i}").CachedValue?.ToString(), out int line);

                                    DonHang dh = new DonHang();

                                    dh.Dai = (int)daiCat;
                                    dh.Rong = (int)nap1;
                                    dh.Cao = (int)cao1;
                                    dh.Canh = (int)nap2;
                                    dh.Line = line;
                                    dh.GiayDai = (int)dai;
                                    dh.GiayRong = (int)rong;
                                    dh.GiayCao = (int)cao;

                                    //if (sl % xa != 0)
                                    //    slThung++;

                                    dh.SL = (int)slThung;

                                    dh.KhachHang = khachHang;
                                    dh.Ma = maDon;
                                    dh.Kho = (int)kho;
                                    dh.Xa = (int)xa;
                                    dh.Lang = (int)lang;
                                    dh.Pallet = pallet;
                                    dh.Song = song;
                                    dh.Men = ws.Cell($"K{i}").GetString();
                                    dh.GiaySongE = ws.Cell($"P{i}").GetString();
                                    dh.GiayMatE = ws.Cell($"Q{i}").GetString();
                                    dh.GiaySongB = ws.Cell($"L{i}").GetString();
                                    dh.GiayMatB = ws.Cell($"M{i}").GetString();
                                    dh.GiaySongC = ws.Cell($"N{i}").GetString();
                                    dh.GiayMatC = ws.Cell($"O{i}").GetString();
                                    dh.GhiChu = ws.Cell($"AB{i}").GetString();
                                    dh.TenDonHang = ws.Cell($"AC{i}").GetString();
                                    dh.PO = ws.Cell($"AD{i}").GetString();
                                    dh.MayIn = ws.Cell($"AE{i}").GetString();
                                    dh.TenDonHang = ws.Cell($"AC{i}").GetString();
                                    dh.Chap_Be = ws.Cell($"AF{i}").GetString();
                                    dh.Ghim_Dan = ws.Cell($"AG{i}").GetString();
                                    donHangs.Add(dh);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (donHangs.Count == 0)
                            {
                                MessageBox.Show($"Không có tìm thấy đơn hàng để nhập", "Thông báo");
                            }
                            else
                            {
                                var mbr = MessageBox.Show($"Tìm thấy {donHangs.Count}.\n - Chọn 'Yes' để nạp đè.\n - Chọn 'No' để thêm vào đơn hiện có.\n - Chọn 'Cancel' để hủy", "Xác nhận", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (mbr == MessageBoxResult.Cancel)
                                {
                                    return;
                                }
                                else if (mbr == MessageBoxResult.Yes)
                                {
                                    string error = await XoaTatCaDonHang();
                                    if (string.IsNullOrWhiteSpace(error))
                                    {
                                        foreach (var dh in donHangs)
                                        {
                                             await TaoDonHang(dh, false);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show($"Xóa tất cả đơn hàng không thành công.\n{error}", "Error");
                                    }
                                }
                                else
                                {
                                    foreach (var dh in donHangs)
                                    {
                                        string error = await TaoDonHang(dh, false);
                                    }
                                }
                         
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Lỗi");
            }
            finally
            {
                RefreshTable();
                dataGrid.IsEnabled = true;
                dataGrid.SelectedItem = DonHangDataSource.FirstOrDefault();
            }
        }

        private void BtnLamMoi_Click(object sender, RoutedEventArgs e)
        {
            State = 0;
            dataGrid.SelectedIndex = -1;

            UpdateButton();
            RefreshTable();
            if (DonHangDataSource.Count > 0)
            {
                dataGrid.SelectedItem = DonHangDataSource[0];
                dataGrid.SelectedIndex = 0;
            }
        }

        private async void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            var mbr = MessageBox.Show("Bạn có muốn xóa tất cả các đơn hàng hay không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbr == MessageBoxResult.Yes)
            {
                string error = await XoaTatCaDonHang();
                if (string.IsNullOrEmpty(error))
                {
                    IsChanged = 1;
                    RefreshTable();
                    UpdateButton();
                    dataGrid.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show($"Không thể xóa đơn hàng", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task<string> XoaTatCaDonHang()
        {
            try
            {
                if (mainWindow.hubConnection != null && mainWindow.hubProxy != null && mainWindow.hubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                {
                    return await mainWindow.hubProxy.Invoke<string>("xoaTatCaDonHang");
                }
                else
                {
                    return "Mất kết nối đến phần mềm server";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            if (State != 0)
            {
                State = 0;
                UpdateButton();
                dataGrid.IsEnabled = true;
                FillTextBox(dataGrid.SelectedItem as DonHang);
            }
        }

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is DonHang dh)
            {
                State = 2;
                UpdateButton();
                dataGrid.IsEnabled = false;
                cobMa.Focus();
            }
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {     
            FillTextBox(null);
            State = 1;
            UpdateButton();
            dataGrid.IsEnabled = false;
            STTChanged = false;
            cobMa.Focus();
        }

        private async void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dataGrid.SelectedItem is DonHang dh)
                {
                    if (dh.HoanTatCutter > 0)
                    {
                        MessageBox.Show("Không thể xóa đơn hàng đã hoàn thành hoặc đang chạy", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var mbr = MessageBox.Show("Bạn có chắc là xóa đơn hàng này hay không ?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (mbr == MessageBoxResult.Yes)
                    {

                        string error = await XoaDonHang(dh);
                        if (string.IsNullOrEmpty(error))
                        {
                            IsChanged = 1;
                            RefreshTable();
                        }
                        else
                        {
                            MessageBox.Show($"Không thể xóa đơn hàng: {error}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<string> XoaDonHang(DonHang dh)
        {
            try
            {
                if (mainWindow.hubConnection != null && mainWindow.hubProxy != null && mainWindow.hubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                {
                    return await mainWindow.hubProxy.Invoke<string>("xoaDonHang", dh);
                }
                else
                {
                    return "Mất kết nối đến phần mềm server";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        private async void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            // Create mode
            if (State == 1)
            {
                string error = ValidateDonHang(out DonHang dh);
                if (!string.IsNullOrEmpty(error))
                {
                    MessageBox.Show(error, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    try
                    {
                        error = await TaoDonHang(dh, STTChanged);
                        if (string.IsNullOrEmpty(error))
                        {
                            if (!MaDataSource.Contains(dh.Ma))
                                MaDataSource.Add(dh.Ma);
                            if (!SongDataSource.Contains(dh.Song))
                                SongDataSource.Add(dh.Song);
                            if (!GiayMenDataSource.Contains(dh.Men))
                                GiayMenDataSource.Add(dh.Men);
                            if (!GiaySongEDataSource.Contains(dh.GiaySongE))
                                GiaySongEDataSource.Add(dh.GiaySongE);
                            if (!GiayMatEDataSource.Contains(dh.GiayMatE))
                                GiayMatEDataSource.Add(dh.GiayMatE);
                            if (!GiaySongBDataSource.Contains(dh.GiaySongB))
                                GiaySongBDataSource.Add(dh.GiaySongB);
                            if (!GiayMatBDataSource.Contains(dh.GiayMatB))
                                GiayMatBDataSource.Add(dh.GiayMatB);
                            if (!GiaySongCDataSource.Contains(dh.GiaySongC))
                                GiaySongCDataSource.Add(dh.GiaySongC);
                            if (!GiayMatCDataSource.Contains(dh.GiayMatC))
                                GiayMatCDataSource.Add(dh.GiayMatC);
                            RefreshTable();

                            State = 0;
                            UpdateButton();
                            dataGrid.IsEnabled = true;
                            dataGrid.SelectedItem = DonHangDataSource.FirstOrDefault();
                            STTChanged = false;
                        }
                        else
                        {
                            MessageBox.Show($"Tạo đơn hàng không thành công: {error}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            // Edit mode
            else if (State == 2)
            {
                if (dataGrid.SelectedItem is DonHang selectedDH)
                {
                    long id = selectedDH.Id;
                    string error = ValidateDonHang(out DonHang dh);

                    if (!long.TryParse(txbSTT.Text, out long stt))
                    {
                        txbSTT.Focus();
                        error = "Trường 'STT' phải là một số và lớn hơn 0";
                    }
                    if (stt <= 0)
                    {
                        txbSTT.Focus();
                        error = "Trường 'STT' phải là một số và lớn hơn 0";
                    }
                    dh.STT = stt;

                    if (selectedDH.HoanTatCutter > 0)
                    {
                        MessageBox.Show("Không thể chỉnh sửa đơn hàng đã hoàn thành hoặc đang chạy", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        State = 0;
                        UpdateButton();
                        dataGrid.IsEnabled = true;
                        return;
                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        MessageBox.Show(error, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        try
                        {

                            if (DonHangDataSource != null && DonHangDataSource.Count > 0)
                            {
                                if (DonHangDataSource.LastOrDefault() is DonHang dhToiDa)
                                {
                                    if (dh.STT < dhToiDa.STT)
                                    {
                                        MessageBox.Show("STT mới không thể nhỏ hơn đơn hàng đầu tiên của danh sách đơn hàng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                                        return;
                                    }

                                    foreach (var item in DonHangDataSource)
                                    {
                                        if (item.HoanTatCutter + item.HoanTatSongB + item.HoanTatSongC +
                                            item.HoanTatSongE + item.HoanTatSpliter + item.HoanTatMayMen > 0)
                                        {
                                            if (dh.STT < item.STT)
                                            {
                                                MessageBox.Show("STT mới không thể nhỏ hơn STT của các đơn đã hoàn thành hoặc đang chạy!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                txbSTT.Focus();
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            if (item.STT == dh.STT && item != dataGrid.SelectedItem as DonHang)
                                            {
                                                MessageBox.Show("STT của đơn hàng đã tồn tại xin mời nhập STT khác", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                txbSTT.Focus();
                                                return;
                                            }
                                        }
                                    }
                                }
                            }

                            //if (CutterTags.Instance.STT1 != null && long.TryParse(CutterTags.Instance.STT1.Value, out long sttPLC))
                            //{
                            //    if (dh.STT <= sttPLC)
                            //    {
                            //        MessageBox.Show("STT của đơn hàng không thể nhỏ hơn hoặc bằng STT của đơn hàng đang chạy", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                            //        return;
                            //    }
                            //}
                            //else
                            //{
                            //    MessageBox.Show("Mất kết nối đến server !", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                            //    return;
                            //}

                            error = await EditDonHang(id, dh);
                            if (string.IsNullOrEmpty(error))
                            {

                                if (!MaDataSource.Contains(dh.Ma))
                                    MaDataSource.Add(dh.Ma);
                                if (!SongDataSource.Contains(dh.Song))
                                    SongDataSource.Add(dh.Song);
                                if (!GiayMenDataSource.Contains(dh.Men))
                                    GiayMenDataSource.Add(dh.Men);
                                if (!GiaySongEDataSource.Contains(dh.GiaySongE))
                                    GiaySongEDataSource.Add(dh.GiaySongE);
                                if (!GiayMatEDataSource.Contains(dh.GiayMatE))
                                    GiayMatEDataSource.Add(dh.GiayMatE);
                                if (!GiaySongBDataSource.Contains(dh.GiaySongB))
                                    GiaySongBDataSource.Add(dh.GiaySongB);
                                if (!GiayMatBDataSource.Contains(dh.GiayMatB))
                                    GiayMatBDataSource.Add(dh.GiayMatB);
                                if (!GiaySongCDataSource.Contains(dh.GiaySongC))
                                    GiaySongCDataSource.Add(dh.GiaySongC);
                                if (!GiayMatCDataSource.Contains(dh.GiayMatC))
                                    GiayMatCDataSource.Add(dh.GiayMatC);
                                RefreshTable();

                                State = 0;
                                UpdateButton();
                                dataGrid.IsEnabled = true;
                                dataGrid.SelectedItem = DonHangDataSource.FirstOrDefault(x => x.Id == id);

                            }
                            else
                            {
                                MessageBox.Show($"Sửa đơn hàng không thành công: {error}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    State = 0;
                    UpdateButton();
                    dataGrid.IsEnabled = true;
                }
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillTextBox(dataGrid.SelectedItem as DonHang);
        }

        public void FillTextBox(DonHang dh)
        {
            if (dh != null)
            {
                txbSTT.Text = dh.STT.ToString();
                cobMa.Text = dh.Ma;
                cobSong.Text = dh.Song;
                txbKho.Text = dh.Kho.ToString();
                cobGiayMen.Text = dh.Men;
                cobGiaySongE.Text = dh.GiaySongE;
                cobGiayMatE.Text = dh.GiayMatE;
                cobGiaySongB.Text = dh.GiaySongB;
                cobGiayMatB.Text = dh.GiayMatB;
                cobGiaySongC.Text = dh.GiaySongC;
                cobGiayMatC.Text = dh.GiayMatC;
                txbDai.Text = dh.Dai.ToString();
                txbSoLuong.Text = dh.SL.ToString();
                txbSoLuongThung.Text = $"{dh.SL * dh.Xa}";
                txbPallet.Text = dh.Pallet.ToString();
                txbXa.Text = dh.Xa.ToString();
                txbRong.Text = dh.Rong.ToString();
                txbCanh.Text = dh.Canh.ToString();
                txbCao.Text = dh.Cao.ToString();
                txbLang.Text = dh.Lang.ToString();

                txbKH.Text = dh.KhachHang;
                txbDonHang.Text = dh.TenDonHang;
                txbRongGiay.Text = dh.GiayRong.ToString();
                txbCaoGiay.Text = dh.GiayCao.ToString();
                txbDaiGiay.Text = dh.GiayDai.ToString();
                txbPO.Text = dh.PO;
                txbMayIn.Text = dh.MayIn;
                txbChapBe.Text = dh.Chap_Be;
                txbGhimDan.Text = dh.Ghim_Dan;
                txbLine.Text = dh.Line.ToString();
                txbMayXa.Text = dh.MayXa.ToString();
            }
            else
            {
                //txbSTT.Clear();
                //cobMa.Text = "";
                //cobSong.Text = "";
                //txbKho.Clear();
                //cobGiayMen.Text = "";
                //cobGiaySongE.Text = "";
                //cobGiaySongB.Text = "";
                //cobGiaySongC.Text = "";
                //cobGiayMatE.Text = "";
                //cobGiayMatB.Text = "";
                //cobGiayMatC.Text = "";
                //txbDai.Clear();
                //txbSoLuong.Clear();
                //txbPallet.Clear();
                //txbXa.Clear();
                //txbRong.Clear();
                //txbCanh.Clear();
                //txbCao.Clear();
                //txbLang.Clear();
            }
        }

        public void RefreshTable()
        {
            cobMa.ItemsSource = MaDataSource;
            cobSong.ItemsSource = SongDataSource;
            cobGiayMen.ItemsSource = GiayMenDataSource;
            cobGiaySongE.ItemsSource = GiaySongEDataSource;
            cobGiayMatE.ItemsSource = GiayMatEDataSource;
            cobGiaySongB.ItemsSource = GiaySongBDataSource;
            cobGiayMatB.ItemsSource = GiayMatBDataSource;
            cobGiaySongC.ItemsSource = GiaySongCDataSource;
            cobGiayMatC.ItemsSource = GiayMatCDataSource;
            DonHangDataSource = new ObservableCollection<DonHang>(Repository.Instance.GetDonHangs().OrderByDescending(x => x.STT));
            dataGrid.ItemsSource = DonHangDataSource;
        }

        public async Task<string> TaoDonHang(DonHang dh, bool sttChanged)
        {
            try
            {
                if (mainWindow.hubConnection != null && mainWindow.hubProxy != null && mainWindow.hubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                {
                    return await mainWindow.hubProxy.Invoke<string>("themDonHang", dh, sttChanged);
                }
                else
                {
                    return "Mất kết nối đến phần mềm server";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> EditDonHang(long id, DonHang dh)
        {
            try
            {
                if (mainWindow.hubConnection != null && mainWindow.hubProxy != null && mainWindow.hubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                {
                    return await mainWindow.hubProxy.Invoke<string>("suaDonHang", id, dh);
                }
                else
                {
                    return "Mất kết nối đến phần mềm server";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public string ValidateDonHang(out DonHang dh)
        {
            dh = new DonHang();

            //if (int.TryParse(txbDaiGiay.Text, out int giayDai))
            //    dh.GiayDai = giayDai;
            //if (int.TryParse(txbRongGiay.Text, out int giayRong))
            //    dh.GiayRong = giayRong;
            //if (int.TryParse(txbCaoGiay.Text, out int giayCao))
            //    dh.GiayCao = giayCao;

            //dh.KhachHang = txbKH.Text;
            //dh.TenDonHang = txbDonHang.Text;
            //dh.PO = txbPO.Text;
            //dh.MayIn = txbMayIn.Text;
            //dh.Chap_Be = txbChapBe.Text;
            //dh.Ghim_Dan = txbGhimDan.Text;

            //if (State == 1)
            //{
            //    if (STTChanged)
            //    {
            //        if (long.TryParse(txbSTT.Text, out long stt))
            //        {
            //            if (stt > 0)
            //            {
            //                dh.STT = stt;
            //            }
            //            else
            //            {
            //                return "STT không hợp lệ";
            //            }
            //        }
            //        else
            //        {
            //            return "STT không hợp lệ";
            //        }
            //    }
            //}

            //if (string.IsNullOrWhiteSpace(cobMa.Text))
            //{
            //    cobMa.Focus();
            //    return "Mã đơn không được để trống!";
            //}
            //dh.Ma = cobMa.Text.Trim();

            //if (string.IsNullOrWhiteSpace(cobSong.Text))
            //{
            //    cobSong.Focus();
            //    return "Trường 'Sóng' không được để trống!";
            //}
            //dh.Song = cobSong.Text.Trim();

            //if (!uint.TryParse(txbKho.Text, out uint kho))
            //{
            //    txbKho.Focus();
            //    return "Trường 'Khổ' phải là một số và lớn hơn 0";
            //}
            //dh.Kho = (int)kho;

            ////if (string.IsNullOrWhiteSpace(cobGiayMen.Text))
            ////{
            ////    cobGiayMen.Focus();
            ////    return "Giấy mền không được để trống!";
            ////}
            //dh.Men = cobGiayMen.Text?.Trim();

            //if (!uint.TryParse(txbDai.Text, out uint dai))
            //{
            //    txbDai.Focus();
            //    return "Trường 'Dài' phải là một số và lớn hơn 0";
            //}
            //dh.Dai = (int)dai;

            //if (!uint.TryParse(txbSoLuong.Text, out uint sl))
            //{
            //    txbSoLuong.Focus();
            //    return "Trường 'Khổ' phải là một số và lớn hơn 0";
            //}
            //dh.SL = (int)sl;

            //if (!uint.TryParse(txbPallet.Text, out uint pallet))
            //{
            //    txbPallet.Focus();
            //    return "Trường 'Pallet' phải là một số và lớn hơn 0";
            //}
            //dh.Pallet = (int)pallet;

            //if (!uint.TryParse(txbXa.Text, out uint xa))
            //{
            //    txbXa.Focus();
            //    return "Trường 'Xả' phải là một số và lớn hơn 0";
            //}
            //dh.Xa = (int)xa;

            //if (!uint.TryParse(txbRong.Text, out uint rong))
            //{
            //    txbRong.Focus();
            //    return "Trường 'Rộng' phải là một số và lớn hơn 0";
            //}
            //dh.Rong = (int)rong;

            //if (!uint.TryParse(txbCanh.Text, out uint canh))
            //{
            //    txbCanh.Focus();
            //    return "Trường 'Cánh' phải là một số và lớn hơn 0";
            //}
            //dh.Canh = (int)canh;

            //if (!uint.TryParse(txbCao.Text, out uint cao))
            //{
            //    txbCao.Focus();
            //    return "Trường 'Cao' phải là một số và lớn hơn 0";
            //}
            //dh.Cao = (int)cao;

            //if (!uint.TryParse(txbLang.Text, out uint lang))
            //{
            //    txbLang.Focus();
            //    return "Trường 'Lằng' phải là một số và lớn hơn 0";
            //}

            //dh.Lang = (int)lang;

            //if (!uint.TryParse(txbMayXa.Text, out uint mayXa))
            //{
            //    txbMayXa.Focus();
            //    return "Trường 'Máy xả' phải là một số và lớn hơn 0";
            //}
            //else
            //{
            //    if (mayXa != 1 && mayXa != 2)
            //    {
            //        txbMayXa.Focus();
            //        return "Trường 'Máy xả' không hợp lệ";
            //    }
            //}
            //dh.MayXa = (int)mayXa;

            //if (!uint.TryParse(txbLine.Text, out uint line))
            //{
            //    txbLine.Focus();
            //    return "Trường 'Line' phải là một số và lớn hơn 0";
            //}
            //dh.Line = (int)line;

            //dh.GiaySongE = cobGiaySongE.Text?.Trim();
            //dh.GiaySongB = cobGiaySongB.Text?.Trim();
            //dh.GiaySongC = cobGiaySongC.Text?.Trim();
            //dh.GiayMatE = cobGiayMatE.Text?.Trim();
            //dh.GiayMatB = cobGiayMatB.Text?.Trim();
            //dh.GiayMatC = cobGiayMatC.Text?.Trim();
            //dh.GhiChu = txbGhiChu.Text?.Trim();

            //return string.Empty;

            dh = new DonHang();

            if (State == 1)
            {
                if (STTChanged)
                {
                    if (long.TryParse(txbSTT.Text, out long stt))
                    {
                        if (stt > 0)
                        {
                            dh.STT = stt;
                        }
                        else
                        {
                            return "STT không hợp lệ";
                        }
                    }
                    else
                    {
                        return "STT không hợp lệ";
                    }
                }
            }

            dh.KhachHang = txbKH.Text;
            dh.TenDonHang = txbDonHang.Text;
            dh.PO = txbPO.Text;
            dh.MayIn = txbMayIn.Text;
            dh.Chap_Be = txbChapBe.Text;
            dh.Ghim_Dan = txbGhimDan.Text;

            if (string.IsNullOrWhiteSpace(cobMa.Text))
            {
                cobMa.Focus();
                return "Mã đơn không được để trống!";
            }
            dh.Ma = cobMa.Text.Trim();

            if (string.IsNullOrWhiteSpace(cobSong.Text))
            {
                cobSong.Focus();
                return "Trường 'Sóng' không được để trống!";
            }
            dh.Song = cobSong.Text.Trim();

            if (!uint.TryParse(txbKho.Text, out uint kho))
            {
                txbKho.Focus();
                return "Trường 'Khổ' phải là một số và lớn hơn 0";
            }
            dh.Kho = (int)kho;

            //if (string.IsNullOrWhiteSpace(cobGiayMen.Text))
            //{
            //    cobGiayMen.Focus();
            //    return "Giấy mền không được để trống!";
            //}
            dh.Men = cobGiayMen.Text?.Trim();

            // Nếu sửa đơn
            //if (State == 2)
            //{
            //    //if (!uint.TryParse(txbDai.Text, out uint dai))
            //    //{
            //    //    txbDai.Focus();
            //    //    return "Trường 'Dài' phải là một số và lớn hơn 0";
            //    //}
            //    //dh.Dai = (int)dai;

            //    //if (!uint.TryParse(txbSoLuongThung.Text, out uint sl))
            //    //{
            //    //    txbSoLuong.Focus();
            //    //    return "Trường 'Khổ' phải là một số và lớn hơn 0";
            //    //}
            //    //dh.SL = (int)sl;

            //    if (!uint.TryParse(txbCao.Text, out uint cao))
            //    {
            //        txbCao.Focus();
            //        return "Trường 'Cao' phải là một số và lớn hơn 0";
            //    }
            //    dh.Cao = (int)cao;

            //    if (!uint.TryParse(txbRong.Text, out uint rong))
            //    {
            //        txbRong.Focus();
            //        return "Trường 'Nắp 1' phải là một số và lớn hơn 0";
            //    }
            //    dh.Rong = (int)rong;

            //    if (!uint.TryParse(txbCanh.Text, out uint canh))
            //    {
            //        txbCanh.Focus();
            //        return "Trường 'Nắp 2' phải là một số và lớn hơn 0";
            //    }
            //    dh.Canh = (int)canh;
            //}
            //else
            //{
            //    if (!uint.TryParse(txbDaiGiay.Text, out uint daiThung))
            //    {
            //        txbDaiGiay.Focus();
            //        return "Trường 'Dài thùng' phải là một số và lớn hơn 0";
            //    }

            //    if (daiThung <= 0)
            //        return "Trường 'Dài thùng' phải là một số và lớn hơn 0";

            //    if (!uint.TryParse(txbRongGiay.Text, out uint rongThung))
            //    {
            //        txbRongGiay.Focus();
            //        return "Trường 'Rộng thùng' phải là một số và lớn hơn 0";
            //    }
            //    if (rongThung <= 0)
            //        return "Trường 'Rộng thùng' phải là một số và lớn hơn 0";

            //    if (!uint.TryParse(txbCaoGiay.Text, out uint caoThung))
            //    {
            //        txbCaoGiay.Focus();
            //        return "Trường 'Cao thùng' phải là một số và lớn hơn 0";
            //    }
            //    if (caoThung <= 0)
            //        return "Trường 'Cao thùng' phải là một số và lớn hơn 0";

            //    if (!uint.TryParse(txbSoLuongThung.Text, out uint slThung))
            //    {
            //        txbCaoGiay.Focus();
            //        return "Trường 'SL thùng' phải là một số và lớn hơn 0";
            //    }

            //    if (slThung <= 0)
            //        return "Trường 'SL thùng' phải là một số và lớn hơn 0";

            //    if (!uint.TryParse(txbXa.Text, out uint xaTHung))
            //    {
            //        txbXa.Focus();
            //        return "Trường 'Xả' phải là một số và lớn hơn 0";
            //    }
            //    if (xaTHung <= 0)
            //        return "Trường 'Xả' phải là một số và lớn hơn 0";
            //    dh.Xa = (int)xaTHung;

            //    if (!uint.TryParse(txbLang.Text, out uint langThung))
            //    {
            //        txbLang.Focus();
            //        return "Trường 'Lằng' phải là một số và lớn hơn 0";
            //    }
            //    dh.Lang = (int)langThung;
            //    dh.Dai = (int)((daiThung + rongThung) * 2 + 30);
            //    dh.Cao = (int)caoThung;

            //    dh.Rong = (int)((rongThung / 2) + LayBuHao((int)langThung));
            //    dh.Canh = dh.Rong;

            //    uint sl = slThung / xaTHung;

            //    if (slThung % xaTHung != 0)
            //        sl++;

            //    dh.SL = (int)sl;

            //}

            if (!uint.TryParse(txbDaiGiay.Text, out uint daiThung))
            {
                txbDaiGiay.Focus();
                return "Trường 'Dài thùng' phải là một số và lớn hơn 0";
            }

            if (daiThung <= 0)
                return "Trường 'Dài thùng' phải là một số và lớn hơn 0";

            if (!uint.TryParse(txbRongGiay.Text, out uint rongThung))
            {
                txbRongGiay.Focus();
                return "Trường 'Rộng thùng' phải là một số và lớn hơn 0";
            }
            if (rongThung <= 0)
                return "Trường 'Rộng thùng' phải là một số và lớn hơn 0";

            if (!uint.TryParse(txbCaoGiay.Text, out uint caoThung))
            {
                txbCaoGiay.Focus();
                return "Trường 'Cao thùng' phải là một số và lớn hơn 0";
            }
            if (caoThung <= 0)
                return "Trường 'Cao thùng' phải là một số và lớn hơn 0";

            if (!uint.TryParse(txbSoLuongThung.Text, out uint slThung))
            {
                txbCaoGiay.Focus();
                return "Trường 'SL thùng' phải là một số và lớn hơn 0";
            }

            if (slThung <= 0)
                return "Trường 'SL thùng' phải là một số và lớn hơn 0";

            if (!uint.TryParse(txbXa.Text, out uint xaTHung))
            {
                txbXa.Focus();
                return "Trường 'Xả' phải là một số và lớn hơn 0";
            }
            if (xaTHung <= 0)
                return "Trường 'Xả' phải là một số và lớn hơn 0";
            dh.Xa = (int)xaTHung;

            if (!uint.TryParse(txbLang.Text, out uint langThung))
            {
                txbLang.Focus();
                return "Trường 'Lằng' phải là một số và lớn hơn 0";
            }



            dh.Lang = (int)langThung;

            dh.GiayDai = (int)daiThung;
            dh.GiayRong = (int)rongThung;
            dh.GiayCao = (int)caoThung;

            dh.Dai = (int)((daiThung + rongThung) * 2 + MainWindow.DaiOffset);

            dh.Rong = (int)((rongThung / 2) + LayBuHao((int)langThung));
            dh.Cao = (int)caoThung;
            dh.Canh = dh.Rong;

            uint sl = slThung / xaTHung;

            if (slThung % xaTHung != 0)
                sl++;

            dh.SL = (int)sl;

            if (!uint.TryParse(txbMayXa.Text, out uint mayXa))
            {
                txbMayXa.Focus();
                return "Trường 'Máy xả' phải là một số và lớn hơn 0";
            }
            else
            {
                if (mayXa != 1 && mayXa != 2)
                {
                    txbMayXa.Focus();
                    return "Trường 'Máy xả' không hợp lệ";
                }
            }
            dh.MayXa = (int)mayXa;

            if (!uint.TryParse(txbLine.Text, out uint line))
            {
                txbLine.Focus();
                return "Trường 'Line' phải là một số và lớn hơn 0";
            }
            dh.Line = (int)line;

            if (!uint.TryParse(txbPallet.Text, out uint pallet))
            {
                txbPallet.Focus();
                return "Trường 'Pallet' phải là một số và lớn hơn 0";
            }
            dh.Pallet = (int)pallet;

            if (!uint.TryParse(txbXa.Text, out uint xa))
            {
                txbXa.Focus();
                return "Trường 'Xả' phải là một số và lớn hơn 0";
            }
            dh.Xa = (int)xa;

            if (!uint.TryParse(txbLang.Text, out uint lang))
            {
                txbLang.Focus();
                return "Trường 'Lằng' phải là một số và lớn hơn 0";
            }
            dh.Lang = (int)lang;

            dh.GiaySongE = cobGiaySongE.Text?.Trim();
            dh.GiaySongB = cobGiaySongB.Text?.Trim();
            dh.GiaySongC = cobGiaySongC.Text?.Trim();
            dh.GiayMatE = cobGiayMatE.Text?.Trim();
            dh.GiayMatB = cobGiayMatB.Text?.Trim();
            dh.GiayMatC = cobGiayMatC.Text?.Trim();
            dh.GhiChu = txbGhiChu.Text?.Trim();

            return string.Empty;
        }

        private void UpdateButton()
        {
            if (Properties.Settings.Default.ChoPhepSua)
            {
                // Normal
                if (State == 0)
                {
                    btnThem.IsEnabled = true;
                    btnSua.IsEnabled = true;
                    btnXoa.IsEnabled = true;
                    btnLuu.IsEnabled = false;
                    btnHuy.IsEnabled = false;

                    foreach (var item in grid.Children)
                    {
                        if (item is TextBox tb)
                        {
                            tb.IsEnabled = true;
                            tb.IsReadOnly = true;
                        }
                        if (item is ComboBox cb)
                        {
                            cb.IsReadOnly = false;
                            cb.IsEnabled = true;
                        }
                    }
                }
                // Creating
                else if (State == 1)
                {
                    btnThem.IsEnabled = false;
                    btnSua.IsEnabled = false;
                    btnXoa.IsEnabled = false;
                    btnLuu.IsEnabled = true;
                    btnHuy.IsEnabled = true;

                    foreach (var item in grid.Children)
                    {
                        if (item is TextBox tb)
                        {
                            tb.IsEnabled = true;
                            tb.IsReadOnly = false;
                        }
                        if (item is ComboBox cb)
                        {
                            cb.IsReadOnly = false;
                            cb.IsEnabled = true;
                        }
                    }

                    txbDai.IsEnabled = false;
                    txbSoLuong.IsEnabled = false;
                    txbRong.IsEnabled = false;
                    txbCao.IsEnabled = false;
                    txbCanh.IsEnabled = false;

                    txbDaiGiay.IsEnabled = true;
                    txbRongGiay.IsEnabled = true;
                    txbCaoGiay.IsEnabled = true;
                    txbSoLuongThung.IsEnabled = true;
                }
                // Editing
                else if (State == 2)
                {


                    btnThem.IsEnabled = false;
                    btnSua.IsEnabled = false;
                    btnXoa.IsEnabled = false;
                    btnLuu.IsEnabled = true;
                    btnHuy.IsEnabled = true;

                    foreach (var item in grid.Children)
                    {
                        if (item is TextBox tb)
                        {
                            tb.IsEnabled = true;
                            tb.IsReadOnly = false;
                        }
                        if (item is ComboBox cb)
                        {
                            cb.IsReadOnly = false;
                            cb.IsEnabled = true;
                        }
                    }

                    txbDai.IsEnabled = false;
                    txbSoLuong.IsEnabled = false;
                    txbRong.IsEnabled = false;
                    txbCao.IsEnabled = false;
                    txbCanh.IsEnabled = false;

                    txbDaiGiay.IsEnabled = true;
                    txbRongGiay.IsEnabled = true;
                    txbCaoGiay.IsEnabled = true;
                    txbSoLuongThung.IsEnabled = true;

                    //if (!ChoPhepSuaGiay)
                    //{
                    //    cobMa.IsEnabled = false;
                    //    cobSong.IsEnabled = false;
                    //    cobGiayMen.IsEnabled = false;
                    //    cobGiaySongE.IsEnabled = false;
                    //    cobGiaySongB.IsEnabled = false;
                    //    cobGiaySongC.IsEnabled = false;
                    //    cobGiayMatE.IsEnabled = false;
                    //    cobGiayMatB.IsEnabled = false;
                    //    cobGiayMatC.IsEnabled = false;
                    //    txbSTT.IsEnabled = false;
                    //    txbKho.IsEnabled = false;
                    //    txbXa.IsEnabled = false;
                    //    txbRong.IsEnabled = false;
                    //    txbCanh.IsEnabled = false;
                    //    txbCao.IsEnabled = false;
                    //    txbLang.IsEnabled = false;
                    //}
                }
            }
            else
            {
                btnThem.IsEnabled = false;
                btnSua.IsEnabled = false;
                btnXoa.IsEnabled = false;
                btnClear.IsEnabled = false;
                btnLuu.IsEnabled = false;
                btnHuy.IsEnabled = false;
                btnNhapExcel.IsEnabled = false;
            }
        }

        private int LayBuHao(int lang)
        {
            Dictionary<int, int> dic = null;
            if (!File.Exists("BuHao.json"))
            {
                dic = new Dictionary<int, int>();
                dic[3] = 1;
                dic[5] = 2;
                dic[7] = 3;

                File.WriteAllText("BuHao.json", JsonConvert.SerializeObject(dic, Formatting.Indented));
            }
            else
            {
                dic = JsonConvert.DeserializeObject<Dictionary<int, int>>(File.ReadAllText("BuHao.json"));
                if (dic == null)
                {
                    dic = new Dictionary<int, int>();
                    dic[3] = 1;
                    dic[5] = 2;
                    dic[7] = 3;
                }
            }

            if(dic.ContainsKey(lang))
            {
                return dic[lang];
            }

            return 0;
        }

        public static List<string> GetList(string column, string tableName)
        {
            List<string> result = new List<string>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default["constr"].ToString()))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = $"select distinct {column} from {tableName}";
                        using (MySqlDataAdapter adp = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adp.Fill(dt);
                            foreach (DataRow dtRow in dt.Rows)
                            {
                                result.Add(dtRow[0].ToString());
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return result;
        }

        private void DataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

    }
}
