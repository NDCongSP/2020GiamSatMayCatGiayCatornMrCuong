using CommonControls;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace MayCat
{
    public class MainViewModel
    {
        #region May1 tags
        public virtual Tag May1_ViTriCaiDatLang1 { get; set; }
        public virtual Tag May1_ViTriCaiDatLang2 { get; set; }
        public virtual Tag May1_ViTriCaiDatLang3 { get; set; }
        public virtual Tag May1_ViTriCaiDatLang4 { get; set; }
        public virtual Tag May1_ViTriCaiDatLang5 { get; set; }
        public virtual Tag May1_ViTriCaiDatLang6 { get; set; }
        public virtual Tag May1_ViTriCaiDatLang7 { get; set; }
        public virtual Tag May1_ViTriCaiDatLang8 { get; set; }

        public virtual Tag May1_ViTriCaiDatDao1 { get; set; }
        public virtual Tag May1_ViTriCaiDatDao2 { get; set; }
        public virtual Tag May1_ViTriCaiDatDao3 { get; set; }
        public virtual Tag May1_ViTriCaiDatDao4 { get; set; }
        public virtual Tag May1_ViTriCaiDatDao5 { get; set; }

        public virtual Tag May1_ViTriLang1 { get; set; }
        public virtual Tag May1_ViTriLang2 { get; set; }
        public virtual Tag May1_ViTriLang3 { get; set; }
        public virtual Tag May1_ViTriLang4 { get; set; }
        public virtual Tag May1_ViTriLang5 { get; set; }
        public virtual Tag May1_ViTriLang6 { get; set; }
        public virtual Tag May1_ViTriLang7 { get; set; }
        public virtual Tag May1_ViTriLang8 { get; set; }

        public virtual Tag May1_ViTriDao1 { get; set; }
        public virtual Tag May1_ViTriDao2 { get; set; }
        public virtual Tag May1_ViTriDao3 { get; set; }
        public virtual Tag May1_ViTriDao4 { get; set; }
        public virtual Tag May1_ViTriDao5 { get; set; }

        public virtual double Opacity1 { get; set; } = 1;

        public virtual TagContainerBase May1 { get; set; }
        #endregion

        #region May2 tags
        public virtual Tag May2_ViTriCaiDatLang1 { get; set; }
        public virtual Tag May2_ViTriCaiDatLang2 { get; set; }
        public virtual Tag May2_ViTriCaiDatLang3 { get; set; }
        public virtual Tag May2_ViTriCaiDatLang4 { get; set; }
        public virtual Tag May2_ViTriCaiDatLang5 { get; set; }
        public virtual Tag May2_ViTriCaiDatLang6 { get; set; }
        public virtual Tag May2_ViTriCaiDatLang7 { get; set; }
        public virtual Tag May2_ViTriCaiDatLang8 { get; set; }

        public virtual Tag May2_ViTriCaiDatDao1 { get; set; }
        public virtual Tag May2_ViTriCaiDatDao2 { get; set; }
        public virtual Tag May2_ViTriCaiDatDao3 { get; set; }
        public virtual Tag May2_ViTriCaiDatDao4 { get; set; }
        public virtual Tag May2_ViTriCaiDatDao5 { get; set; }

        public virtual Tag May2_ViTriLang1 { get; set; }
        public virtual Tag May2_ViTriLang2 { get; set; }
        public virtual Tag May2_ViTriLang3 { get; set; }
        public virtual Tag May2_ViTriLang4 { get; set; }
        public virtual Tag May2_ViTriLang5 { get; set; }
        public virtual Tag May2_ViTriLang6 { get; set; }
        public virtual Tag May2_ViTriLang7 { get; set; }
        public virtual Tag May2_ViTriLang8 { get; set; }

        public virtual Tag May2_ViTriDao1 { get; set; }
        public virtual Tag May2_ViTriDao2 { get; set; }
        public virtual Tag May2_ViTriDao3 { get; set; }
        public virtual Tag May2_ViTriDao4 { get; set; }
        public virtual Tag May2_ViTriDao5 { get; set; }

        public virtual double Opacity2 { get; set; } = 1;

        public virtual TagContainerBase May2 { get; set; }
        #endregion

        #region Properties
        public virtual bool TuDongChuyenDon
        {
            get => Convert.ToBoolean(Properties.Settings.Default["TuDongChuyenDon"]);
            set
            {
                Properties.Settings.Default["TuDongChuyenDon"] = value;
                Properties.Settings.Default.Save();
                this.RaisePropertyChanged(x => x.TuDongChuyenDon);
            }
        }
        public virtual string NguonDonHang
        {
            get => Properties.Settings.Default["NguonDonHang"].ToString();
            set
            {
                Properties.Settings.Default["NguonDonHang"] = value;
                Properties.Settings.Default.Save();
                this.RaisePropertyChanged(x => x.NguonDonHang);
            }
        }

        public virtual DonHang DonHangHienTaiMay1 { get; set; }
        public virtual DonHang DonHangKeTiepMay1 { get; set; }
        public virtual DonHang DonHangHienTaiMay2 { get; set; }
        public virtual DonHang DonHangKeTiepMay2 { get; set; }
        public virtual ThongTinTram ThongTinTram { get; set; } = new ThongTinTram();
        public virtual Brush BackgroundMay1 { get; set; } = Brushes.White;
        public virtual Brush BackgroundMay2 { get; set; } = Brushes.White;
        #endregion

        #region Singleton
        public static MainViewModel Instance { get; } = ViewModelSource.Create(() => new MainViewModel());
        #endregion

        #region Constructors
        public MainViewModel()
        {
            #region Set May1 tags
            May1_ViTriCaiDatLang1 = May1Tags.Instance.Lang1_SV;
            May1_ViTriCaiDatLang2 = May1Tags.Instance.Lang2_SV;
            May1_ViTriCaiDatLang3 = May1Tags.Instance.Lang3_SV;
            May1_ViTriCaiDatLang4 = May1Tags.Instance.Lang4_SV;
            May1_ViTriCaiDatLang5 = May1Tags.Instance.Lang5_SV;
            May1_ViTriCaiDatLang6 = May1Tags.Instance.Lang6_SV;
            May1_ViTriCaiDatLang7 = May1Tags.Instance.Lang7_SV;
            May1_ViTriCaiDatLang8 = May1Tags.Instance.Lang8_SV;

            May1_ViTriCaiDatDao1 = May1Tags.Instance.Dao1_SV;
            May1_ViTriCaiDatDao2 = May1Tags.Instance.Dao2_SV;
            May1_ViTriCaiDatDao3 = May1Tags.Instance.Dao3_SV;
            May1_ViTriCaiDatDao4 = May1Tags.Instance.Dao4_SV;
            May1_ViTriCaiDatDao5 = May1Tags.Instance.Dao5_SV;

            May1_ViTriLang1 = May1Tags.Instance.Lang1_PV;
            May1_ViTriLang2 = May1Tags.Instance.Lang2_PV;
            May1_ViTriLang3 = May1Tags.Instance.Lang3_PV;
            May1_ViTriLang4 = May1Tags.Instance.Lang4_PV;
            May1_ViTriLang5 = May1Tags.Instance.Lang5_PV;
            May1_ViTriLang6 = May1Tags.Instance.Lang6_PV;
            May1_ViTriLang7 = May1Tags.Instance.Lang7_PV;
            May1_ViTriLang8 = May1Tags.Instance.Lang8_PV;

            May1_ViTriDao1 = May1Tags.Instance.Dao1_PV;
            May1_ViTriDao2 = May1Tags.Instance.Dao2_PV;
            May1_ViTriDao3 = May1Tags.Instance.Dao3_PV;
            May1_ViTriDao4 = May1Tags.Instance.Dao4_PV;
            May1_ViTriDao5 = May1Tags.Instance.Dao5_PV;
            #endregion

            #region Set May2 tags
            May2_ViTriCaiDatLang1 = May2Tags.Instance.Lang1_SV;
            May2_ViTriCaiDatLang2 = May2Tags.Instance.Lang2_SV;
            May2_ViTriCaiDatLang3 = May2Tags.Instance.Lang3_SV;
            May2_ViTriCaiDatLang4 = May2Tags.Instance.Lang4_SV;
            May2_ViTriCaiDatLang5 = May2Tags.Instance.Lang5_SV;
            May2_ViTriCaiDatLang6 = May2Tags.Instance.Lang6_SV;
            May2_ViTriCaiDatLang7 = May2Tags.Instance.Lang7_SV;
            May2_ViTriCaiDatLang8 = May2Tags.Instance.Lang8_SV;

            May2_ViTriCaiDatDao1 = May2Tags.Instance.Dao1_SV;
            May2_ViTriCaiDatDao2 = May2Tags.Instance.Dao2_SV;
            May2_ViTriCaiDatDao3 = May2Tags.Instance.Dao3_SV;
            May2_ViTriCaiDatDao4 = May2Tags.Instance.Dao4_SV;
            May2_ViTriCaiDatDao5 = May2Tags.Instance.Dao5_SV;

            May2_ViTriLang1 = May2Tags.Instance.Lang1_PV;
            May2_ViTriLang2 = May2Tags.Instance.Lang2_PV;
            May2_ViTriLang3 = May2Tags.Instance.Lang3_PV;
            May2_ViTriLang4 = May2Tags.Instance.Lang4_PV;
            May2_ViTriLang5 = May2Tags.Instance.Lang5_PV;
            May2_ViTriLang6 = May2Tags.Instance.Lang6_PV;
            May2_ViTriLang7 = May2Tags.Instance.Lang7_PV;
            May2_ViTriLang8 = May2Tags.Instance.Lang8_PV;

            May2_ViTriDao1 = May2Tags.Instance.Dao1_PV;
            May2_ViTriDao2 = May2Tags.Instance.Dao2_PV;
            May2_ViTriDao3 = May2Tags.Instance.Dao3_PV;
            May2_ViTriDao4 = May2Tags.Instance.Dao4_PV;
            May2_ViTriDao5 = May2Tags.Instance.Dao5_PV;
            #endregion

            May1 = May1Tags.Instance;
            May2 = May2Tags.Instance;

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

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(400);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();

            Messenger.Default.Register<LenhChuyenDonMessage>(this, OnChuyenDonHangMessage);
            Messenger.Default.Register<ThemDonHangMessage>(this, OnThemDonHangMessage);
            Messenger.Default.Register<SuaDonHangMessage>(this, OnSuaDonHangMessage);
            Messenger.Default.Register<ThayDoiUuTienMessage>(this, OnThayDoiUuTienMessage);
            Messenger.Default.Register<GiamKheHoLangMessage>(this, OnGiamKheHoLangMessage);
            Messenger.Default.Register<TangKheHoLangMessaage>(this, OnTangKheHoLangMessaage);
            Messenger.Default.Register<ThayDoiKheHoLangMessage>(this, OnThayDoiKheHoLangMessage);

            this.RaisePropertiesChanged();
        }
        #endregion

        #region Members
        private DispatcherTimer dispatcherTimer;
        MySqlConnection conn;
        MySqlCommand cmd;
        #endregion

        #region Eventh handlers
        public void OnLoaded()
        {
            this.RaisePropertiesChanged();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            try
            {
                List<DonHang> Source = null;
                if (NguonDonHang == "Đơn hàng tay")
                {
                    Source = Helper.DonHangTay.ToList();
                }
                else
                {
                    Source = Helper.DonHangLink.ToList();
                }

                if (May1.STT1 != null && May1.STT1.Value != 0)
                {
                    DonHangHienTaiMay1 = Source.FirstOrDefault(x => x.STT == May1.STT1.Value);
                }
                else
                {
                    DonHangHienTaiMay1 = null;
                }

                if (May1.STT2 != null && May1.STT2.Value != 0)
                {
                    DonHangKeTiepMay1 = Source.FirstOrDefault(x => x.STT == May1.STT2.Value);
                }
                else
                {
                    DonHangKeTiepMay1 = null;
                }

                if (May2.STT1 != null && May2.STT1.Value != 0)
                {
                    DonHangHienTaiMay2 = Source.FirstOrDefault(x => x.STT == May2.STT1.Value);
                }
                else
                {
                    DonHangHienTaiMay2 = null;
                }

                if (May2.STT2 != null && May2.STT2.Value != 0)
                {
                    DonHangKeTiepMay2 = Source.FirstOrDefault(x => x.STT == May2.STT2.Value);
                }
                else
                {
                    DonHangKeTiepMay2 = null;
                }

                if (May1.Run != null)
                {
                    if (May1.Run.Value == 0)
                    {
                        BackgroundMay1 = Brushes.White;
                    }
                    else
                    {
                        BackgroundMay1 = Brushes.Lime;
                    }
                }

                if (May2.Run != null)
                {
                    if (May2.Run.Value == 0)
                    {
                        BackgroundMay2 = Brushes.White;
                    }
                    else
                    {
                        BackgroundMay2 = Brushes.Lime;
                    }
                }

                if (cmd != null)
                {
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
                                ThongTinTram.TocDo1 = wrapData.ThongTinTram.TocDo1;
                                ThongTinTram.TocDo2 = wrapData.ThongTinTram.TocDo2;
                                ThongTinTram.TocDo3 = wrapData.ThongTinTram.TocDo3;
                                ThongTinTram.TocDo4 = wrapData.ThongTinTram.TocDo4;
                                ThongTinTram.TocDo5 = wrapData.ThongTinTram.TocDo5;

                                ThongTinTram.DoiDon1 = wrapData.ThongTinTram.DoiDon1;
                                ThongTinTram.DoiDon2 = wrapData.ThongTinTram.DoiDon2;
                                ThongTinTram.DoiDon3 = wrapData.ThongTinTram.DoiDon3;
                                ThongTinTram.DoiDon4 = wrapData.ThongTinTram.DoiDon4;
                                ThongTinTram.DoiDon5 = wrapData.ThongTinTram.DoiDon5;

                                ThongTinTram.Dan1 = wrapData.ThongTinTram.Dan1;
                                ThongTinTram.Dan2 = wrapData.ThongTinTram.Dan2;
                                ThongTinTram.Dan3 = wrapData.ThongTinTram.Dan3;
                                ThongTinTram.Dan4 = wrapData.ThongTinTram.Dan4;
                                ThongTinTram.Dan5 = wrapData.ThongTinTram.Dan5;

                                ThongTinTram.TrangThai1 = wrapData.ThongTinTram.TrangThai1;
                                ThongTinTram.TrangThai2 = wrapData.ThongTinTram.TrangThai2;
                                ThongTinTram.TrangThai3 = wrapData.ThongTinTram.TrangThai3;
                                ThongTinTram.TrangThai4 = wrapData.ThongTinTram.TrangThai4;
                                ThongTinTram.TrangThai5 = wrapData.ThongTinTram.TrangThai5;
                                this.RaisePropertyChanged(x => x.ThongTinTram);

                                List<CommonControls.DonHang> donHangs = wrapData.DanhSachDonHang?.Where(x => x.HoanTatCutter == 0)?.OrderBy(x => x.STT)?.ToList();
                                if (donHangs == null)
                                    donHangs = new List<CommonControls.DonHang>();

                                UpdateDonHangLink(donHangs);
                            }
                        }
                    }
                }

                if (May1.ChoPhepMayChay)
                    Opacity1 = 1;
                else
                    Opacity1 = 0.5;

                if (May2.ChoPhepMayChay)
                    Opacity2 = 1;
                else
                    Opacity2 = 0.5;
            }
            catch (Exception ex)
            {
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
            }
            finally { dispatcherTimer.Start(); }
        }

        public void OnChuyenDonHangMessage(LenhChuyenDonMessage message)
        {
            if (message != null)
            {
                if (TuDongChuyenDon && message.STT1 > 0)
                {
                    NotifyCollection<DonHang> Source = null;
                    if (NguonDonHang == "Đơn hàng tay")
                    {
                        Source = Helper.DonHangTay;
                    }
                    else
                    {
                        Source = Helper.DonHangLink;
                    }

                    int may = message.TagContainer == May1 ? 1 : 2;

                    DonHang donHang1 = Source.FirstOrDefault(x => x.STT == message.STT1);

                    if (donHang1 == null)
                        donHang1 = Source.FirstOrDefault(x => x.STT > 0);

                    if (donHang1 != null)
                    {
                        DonHang dhKeTiep = Helper.LayDonHangKeTiep(Source, donHang1, may);

                        if (dhKeTiep != null)
                        {
                            Helper.NapDon(dhKeTiep, 2, may);
                        }
                    }
                    else
                    {

                    }
                }

                message.TagContainer.LenhChuyenDon.Write(0);
            }
        }

        public void OnThemDonHangMessage(ThemDonHangMessage message)
        {
            if (message != null)
            {

            }
        }

        public void OnSuaDonHangMessage(SuaDonHangMessage message)
        {
            if (message != null)
            {
                if (message.DonHangSua != null)
                {
                    if (message.DonHangSource == Helper.DonHangLink && NguonDonHang == "Đơn hàng link")
                    {

                    }
                    else if (message.DonHangSource == Helper.DonHangTay && NguonDonHang == "Đơn hàng tay")
                    {
                        if (May1.ChoPhepMayChay)
                        {
                            // Nạp lại đơn 2 cho máy 1 nếu STT của đơn 2 bằng với STT sửa
                            if (May1.STT2 != null && May1.STT2.Value == message.DonHangSua.STT)
                            {
                                Helper.NapDon(message.DonHangSua, 2, 1);
                            }
                        }
                        
                        if (May2.ChoPhepMayChay)
                        {
                            // Nạp lại đơn 2 cho máy 2 nếu STT của đơn 2 bằng với STT sửa
                            if (May2.STT2 != null && May2.STT2.Value == message.DonHangSua.STT)
                            {
                                Helper.NapDon(message.DonHangSua, 2, 2);
                            }
                        }
                    }
                }
            }
        }

        public void OnTangKheHoLangMessaage(TangKheHoLangMessaage message)
        {
            if (message != null)
            {
                if (message.May == 1)
                    May1.KheHo_Inc.Write(100);
                else
                    May2.KheHo_Inc.Write(100);
            }
        }

        public void OnGiamKheHoLangMessage(GiamKheHoLangMessage message)
        {
            if (message != null)
            {
                if (message.May == 1)
                    May1.KheHo_Dec.Write(100);
                else
                    May2.KheHo_Dec.Write(100);
            }
        }

        public void OnThayDoiUuTienMessage(ThayDoiUuTienMessage message)
        {
            if (message != null)
            {
                if (message.DonHang != null)
                {
                    if (message.DonHangSource == Helper.DonHangLink && NguonDonHang == "Đơn hàng link")
                    {

                    }
                    else if (message.DonHangSource == Helper.DonHangTay && NguonDonHang == "Đơn hàng tay")
                    {

                    }
                }
            }
        }

        public void OnThayDoiKheHoLangMessage(ThayDoiKheHoLangMessage message)
        {
        }
        #endregion

        #region Methods
        public TrangThaiDao GetTrangThaiDao(double runValue, double daoRunValue)
        {
            if (runValue != 0 && daoRunValue != 0)
                return TrangThaiDao.Down;
            return TrangThaiDao.Up;
        }

        private void UpdateDonHangLink(List<CommonControls.DonHang> source)
        {
            int index = 0;
            for (int i = 0; i < source.Count; i++)
            {
                if (i < Helper.DonHangLink.Count)
                {
                    // Cập nhật lại đơn
                    CapNhatDonHang(source[i], Helper.DonHangLink[i]);
                }
                else
                {
                    // Tạo đơn hàng
                    DonHang newDH = TaoDonHang(source[i]);
                    if (newDH != null)
                        Helper.DonHangLink.Add(newDH);
                }
                index = i;
            }

            index++;
            for (int i = Helper.DonHangLink.Count - 1; i >= index; i++)
            {
                Helper.DonHangLink.RemoveAt(i);
            }
        }

        private void CapNhatDonHang(CommonControls.DonHang dhSource, DonHang target)
        {
            target.Song = dhSource.Song;
            target.Nap1 = dhSource.Rong;
            target.Cao = dhSource.Cao;
            target.Nap2 = dhSource.Canh;
            target.STT = dhSource.STT;
            target.Xa = dhSource.Xa;
            target.Ma = dhSource.Ma;
            target.GhiChu = dhSource.GhiChu;
            target.Lang = dhSource.Lang;
            target.MayXa = dhSource.MayXa;

            //starget.DuKien = dhSource
        }

        private DonHang TaoDonHang(CommonControls.DonHang dhSource)
        {
            DonHang target = new DonHang();
            target.Song = dhSource.Song;
            target.Nap1 = dhSource.Rong;
            target.Cao = dhSource.Cao;
            target.Nap2 = dhSource.Canh;
            target.STT = dhSource.STT;
            target.Xa = dhSource.Xa;
            target.Ma = dhSource.Ma;
            target.GhiChu = dhSource.GhiChu;
            target.Lang = dhSource.Lang;
            return target;
        }
        #endregion

        #region Commands
        public void MoveLeft1(object param)
        {
            string name = param.ToString();
            if (May1.TagSource.FirstOrDefault(x => x.Name == name) is Tag tag)
            {
                tag.Write(100);
            }
        }
        public bool CanMoveLeft1(object param)
        {
            string name = param.ToString();
            return May1.TagSource.Any(x => x.Name == name);
        }

        public void MoveRight1(object param)
        {
            string name = param.ToString();
            if (May1.TagSource.FirstOrDefault(x => x.Name == name) is Tag tag)
            {
                tag.Write(100);
            }
        }
        public bool CanMoveRight1(object param)
        {
            string name = param.ToString();
            return May1.TagSource.Any(x => x.Name == name);
        }

        public void MoveLeft2(object param)
        {
            string name = param.ToString();
            if (May2.TagSource.FirstOrDefault(x => x.Name == name) is Tag tag)
            {
                tag.Write(100);
            }
        }
        public bool CanMoveLeft2(object param)
        {
            string name = param.ToString();
            return May2.TagSource.Any(x => x.Name == name);
        }

        public void MoveRight2(object param)
        {
            string name = param.ToString();
            if (May2.TagSource.FirstOrDefault(x => x.Name == name) is Tag tag)
            {
                tag.Write(100);
            }
        }
        public bool CanMoveRight2(object param)
        {
            string name = param.ToString();
            return May2.TagSource.Any(x => x.Name == name);
        }
        #endregion
    }
}
