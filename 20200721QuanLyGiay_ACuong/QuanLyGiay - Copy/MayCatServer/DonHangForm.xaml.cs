using CommonControls;
using EasyDriverPlugin;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
using System.Windows.Shapes;

namespace MayCatServer
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
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            var mbr = MessageBox.Show("Bạn có muốn xóa tất cả các đơn hàng hay không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbr == MessageBoxResult.Yes)
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default["constr"].ToString()))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = $"delete from dhdangchay where Ca = '{Repository.Instance.Ca}'";
                        int result = cmd.ExecuteNonQuery();
                        IsChanged = 1;
                        RefreshTable();
                        if (result != 0)
                        {
                            try
                            {

                                mainWindow.ClearDonHang();
                                mainWindow.DonHangHienThi.Clear();
                                NapDon(true, true);

                                State = 0;
                                UpdateButton();
                                dataGrid.IsEnabled = true;
                            }
                            catch { }
                            finally { }
                        }
                    }
                }
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
                if (KiemTra(dh) <= 0)
                {
                    MessageBox.Show("Không thể sửa đơn hàng đã hoàn thành hoặc sắp hoàn thành.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                State = 2;
                UpdateButton();
                dataGrid.IsEnabled = false;
                //cobMa.Focus();
            }
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            FillTextBox(null);
            State = 1;
            UpdateButton();
            dataGrid.IsEnabled = false;
            //cobMa.Focus();
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dataGrid.SelectedItem is DonHang dh)
                {
                    if (dh.HoanTatCutter > 0 ||
                        KiemTra(dh) <= 0)
                    {
                        MessageBox.Show("Không thể xóa đơn hàng đã hoàn thành hoặc đang chạy", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var mbr = MessageBox.Show("Bạn có chắc là xóa đơn hàng này hay không ?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (mbr == MessageBoxResult.Yes)
                    {
                        using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default["constr"].ToString()))
                        {
                            conn.Open();
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.Connection = conn;
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = $"delete from dhdangchay where Id = {dh.Id}";
                                IsChanged = 1;
                                int result = cmd.ExecuteNonQuery();
                                RefreshTable();
                                if (result == 1)
                                {
                                    DonHang dhXoa = MainWindow.DonHangDataSource.FirstOrDefault(x => x.Id == dh.Id);
                                    if (dhXoa != null)
                                    {
                                        mainWindow.RemoveDonHang(dhXoa);
                                        NapDon(true, true);
                                        mainWindow.DonHangHienThi.Clear();
                                        mainWindow.DonHangHienThi.AddRange(DonHangDataSource.Where(x => x.HoanTatCutter == 0).OrderBy(x => x.STT));

                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
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
                        if (TaoDonHang(dh) != 0)
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

                            long sttLonNhat = DonHangDataSource.Max(x => x.STT);
                            if (sttLonNhat != 0)
                            {
                                DonHang dhMoi = DonHangDataSource.FirstOrDefault(x => x.STT == sttLonNhat);
                                if (dhMoi != null)
                                {
                                    try
                                    {
                                        mainWindow.AddDonHang(dhMoi);
                                        mainWindow.DonHangHienThi.Add(dhMoi);
                                        NapDon(true, true);

                                    }
                                    catch { }
                                    finally { }
                                }
                            }

                            State = 0;
                            UpdateButton();
                            dataGrid.IsEnabled = true;
                            dataGrid.SelectedItem = DonHangDataSource.FirstOrDefault();
                        }
                        else
                        {
                            MessageBox.Show("Tạo đơn hàng không thành công!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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

                    if (selectedDH.HoanTatCutter > 0 ||
                        KiemTra(selectedDH) <= 0)
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

                            if (CutterTags.Instance.STT1 != null && long.TryParse(CutterTags.Instance.STT1.Value, out long sttPLC))
                            {
                                if (dh.STT <= sttPLC)
                                {
                                    MessageBox.Show("STT của đơn hàng không thể nhỏ hơn hoặc bằng STT của đơn hàng đang chạy", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Mất kết nối đến server !", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            if (EditDonHang(id, dh) != 0)
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

                                DonHang dhSua = DonHangDataSource.FirstOrDefault(x => x.Id == id);
                                if (dhSua != null)
                                {
                                    try
                                    {
                                        var dhMain = MainWindow.DonHangDataSource.FirstOrDefault(x => x.Id == id);
                                        dhMain.STT = dhSua.STT;
                                        dhMain.Ma = dhSua.Ma;
                                        dhMain.Song = dhSua.Song;
                                        dhMain.Kho = dhSua.Kho;
                                        dhMain.Men = dhSua.Men;
                                        dhMain.GiaySongE = dhSua.GiaySongE;
                                        dhMain.GiayMatE = dhSua.GiayMatE;
                                        dhMain.GiaySongB = dhSua.GiaySongB;
                                        dhMain.GiayMatB = dhSua.GiayMatB;
                                        dhMain.GiaySongC = dhSua.GiaySongC;
                                        dhMain.GiayMatC = dhSua.GiayMatC;
                                        dhMain.Dai = dhSua.Dai;
                                        dhMain.SL = dhSua.SL;
                                        dhMain.Pallet = dhSua.Pallet;
                                        dhMain.Xa = dhSua.Xa;
                                        dhMain.Rong = dhSua.Rong;
                                        dhMain.Canh = dhSua.Canh;
                                        dhMain.Cao = dhSua.Cao;
                                        dhMain.Lang = dhSua.Lang;

                                        NapDon(true, true);

                                        mainWindow.DonHangHienThi.Clear();
                                        if (CutterTags.Instance.STT1 != null && long.TryParse(CutterTags.Instance.STT1.Value, out long sttDH))
                                            mainWindow.DonHangHienThi.AddRange(DonHangDataSource.Where(x => x.HoanTatCutter == 0 && x.STT >= sttDH).OrderBy(x => x.STT));
                                        else
                                            mainWindow.DonHangHienThi.AddRange(DonHangDataSource.Where(x => x.HoanTatCutter == 0).OrderBy(x => x.STT));

                                    }
                                    catch { }
                                    finally { }
                                }

                                State = 0;
                                UpdateButton();
                                dataGrid.IsEnabled = true;
                                dataGrid.SelectedItem = DonHangDataSource.FirstOrDefault(x => x.Id == id);

                            }
                            else
                            {
                                MessageBox.Show($"Sửa đơn hàng không thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                txbPallet.Text = dh.Pallet.ToString();
                txbXa.Text = dh.Xa.ToString();
                txbRong.Text = dh.Rong.ToString();
                txbCanh.Text = dh.Canh.ToString();
                txbCao.Text = dh.Cao.ToString();
                txbLang.Text = dh.Lang.ToString();
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

        public int TaoDonHang(DonHang dh)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default["constr"].ToString()))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = $"call proc_insert_donhang('{dh.Ma}', '{dh.Song}', '{dh.Kho}', '{dh.Men}', " +
                            $"'{dh.GiaySongE}', '{dh.GiayMatE}', '{dh.GiaySongB}', '{dh.GiayMatB}', '{dh.GiaySongC}', '{dh.GiayMatC}', " +
                            $"'{dh.Dai}', '{dh.SL}', '{dh.Pallet}', '{dh.Xa}', '{dh.Rong}', '{dh.Canh}', '{dh.Cao}', " +
                            $"'{dh.Lang}', '{Repository.Instance.Ca}', '{dh.GhiChu}');";
                        IsChanged = 1;
                        int result = cmd.ExecuteNonQuery();
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int EditDonHang(long id, DonHang dh)
        {
            try
            {
                if (id != 0)
                {
                    using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default["constr"].ToString()))
                    {
                        conn.Open();
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = $"call proc_update_donhang({id}, {dh.STT}, '{dh.Ma}', '{dh.Song}', {dh.Kho}, '{dh.Men}', " +
                                $"'{dh.GiaySongE}', '{dh.GiayMatE}', '{dh.GiaySongB}', '{dh.GiayMatB}', '{dh.GiaySongC}', '{dh.GiayMatC}', " +
                                $"{dh.Dai}, {dh.SL}, {dh.Pallet}, {dh.Xa}, {dh.Rong}, {dh.Canh}, {dh.Cao}, " +
                                $"{dh.Lang}, '{dh.GhiChu}');";
                            IsChanged = 2;
                            int result = cmd.ExecuteNonQuery();
                            return result;
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public string ValidateDonHang(out DonHang dh)
        {
            dh = new DonHang();

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

            if (!uint.TryParse(txbDai.Text, out uint dai))
            {
                txbDai.Focus();
                return "Trường 'Dài' phải là một số và lớn hơn 0";
            }
            dh.Dai = (int)dai;

            if (!uint.TryParse(txbSoLuong.Text, out uint sl))
            {
                txbSoLuong.Focus();
                return "Trường 'Khổ' phải là một số và lớn hơn 0";
            }
            dh.SL = (int)sl;

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

            if (!uint.TryParse(txbRong.Text, out uint rong))
            {
                txbRong.Focus();
                return "Trường 'Rộng' phải là một số và lớn hơn 0";
            }
            dh.Rong = (int)rong;

            if (!uint.TryParse(txbCanh.Text, out uint canh))
            {
                txbCanh.Focus();
                return "Trường 'Cánh' phải là một số và lớn hơn 0";
            }
            dh.Canh = (int)canh;

            if (!uint.TryParse(txbCao.Text, out uint cao))
            {
                txbCao.Focus();
                return "Trường 'Cao' phải là một số và lớn hơn 0";
            }
            dh.Cao = (int)cao;

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

        public void NapDon(bool napViTri1, bool napViTri2)
        {
            //await Task.Run(() =>
            //{
            //    try
            //    {


            //    }
            //    catch { }
            //    finally
            //    { //semaphore.Release(); 
            //    }
            //});
            mainWindow.NapDonAct();



            //BackgroundWorker bw = new BackgroundWorker();
            //bw.DoWork += async (s, e) =>
            //{
            //    await Task.Run(() =>
            //    {
            //        List<DonHang> dhNap = MainWindow.DonHangDataSource.ToList();
            //        CutterController.Instance.NapDanhSachDonHang(dhNap, napViTri1, napViTri2);
            //        if (double.TryParse(SongETags.Instance.HeSoSong?.Value, out double heSoSong))
            //            SongEController.Instance.NapDanhSachDonHang(dhNap, heSoSong, napViTri1);
            //        if (double.TryParse(SongBTags.Instance.HeSoSong?.Value, out double heSoSongB))
            //            SongBController.Instance.NapDanhSachDonHang(dhNap, heSoSongB, napViTri1);
            //        if (double.TryParse(SongCTags.Instance.HeSoSong?.Value, out double heSoSongC))
            //            SongCController.Instance.NapDanhSachDonHang(dhNap, heSoSongC, napViTri1);
            //        MayMenController.Instance.NapDanhSachDonHang(dhNap, 1, napViTri1);
            //    });
            //};
            //bw.RunWorkerAsync();
        }


        private int KiemTra(DonHang dh)
        {
            if (CutterTags.Instance.DoiDon != null && CutterTags.Instance.DoiDon.Quality == Quality.Good)
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
                        if (dh.STT >= sttSongE && dh.STT >= sttMatE)
                        {

                        }
                        else { choPhepNap = false; }

                    }

                    if (!string.IsNullOrEmpty(dh.GiaySongB) || !string.IsNullOrEmpty(dh.GiayMatB))
                    {
                        // Kiểm tra STT sửa phải bé hơn hoặc bằng stt đang chạy
                        if (dh.STT >= sttSongB && dh.STT >= sttMatB)
                        {

                        }
                        else { choPhepNap = false; }

                    }

                    if (!string.IsNullOrEmpty(dh.GiaySongC) || !string.IsNullOrEmpty(dh.GiayMatC))
                    {
                        // Kiểm tra STT sửa phải bé hơn hoặc bằng stt đang chạy
                        if (dh.STT >= sttSongC && dh.STT >= sttMatC)
                        {

                        }
                        else { choPhepNap = false; }

                    }

                    if (!string.IsNullOrEmpty(dh.Men))
                    {
                        // Kiểm tra STT sửa phải bé hơn hoặc bằng stt đang chạy
                        if (dh.STT >= sttMatMen)
                        {

                        }
                        else { choPhepNap = false; }

                    }


                    if (choPhepNap &&
                        dh.STT >= sttCut)
                    {
                        choPhepNap = true;
                        if (dh.STT == sttSongE ||
                            dh.STT == sttSongB ||
                            dh.STT == sttSongC ||
                            dh.STT == sttMatMen)
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

                        return choPhepNap ? 1 : 0;
                    }

                }

            }
            return 0;

        }
    }
}
