using CommonControls;
using EasyDriverPlugin;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MayCatServer
{
    public class UpdaterDonHang
    {
        public static UpdaterDonHang Instance { get; } = new UpdaterDonHang();

        public ThongTinCa ThongTinCa { get; set; }

        public UpdaterDonHang()
        {
            //conn = new MySqlConnection(Repository.Instance.ConnectionString);
            //string con = Repository.Instance.ConnectionString;
            //conn.Open();
            //cmd = new MySqlCommand();
            //cmd.Connection = conn;
            
            updateTimer = new System.Timers.Timer();
            updateTimer.Interval = 500;
            updateTimer.Elapsed += UpdateTimer_Elapsed;
            updateTimer.Start();
        }

        string lastSTT;
        private void UpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            updateTimer.Stop();
            try
            {
                using (var conn = new MySqlConnection(Repository.Instance.ConnectionString))
                {
                    using (var cmd = new MySqlCommand())
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = System.Data.CommandType.Text;
                        //if (ThongTinCa == null)
                        //    ThongTinCa = Repository.Instance.GetThongTinCa();

                        if (IsStarted)
                        {
                            if (DonHangNap != DonHang)
                            {
                                ChotDon(DonHang);

                                if (DonHang != null)
                                {
                                    if (DonHang.Id > 0 && DonHang.TGBatDau == DateTime.MinValue)
                                    {
                                        DonHang.TGBatDau = DateTime.Now;
                                        Repository.Instance.UpdateColumn("dhdangchay", "TGBatDau", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), $"where Id = {DonHang.Id}");
                                    }
                                }

                                DonHang = DonHangNap;
                            }

                            if (DonHang != null && DonHang.STT > 0 && DonHang.Id != 0 && DonHang.TGKetThuc == DateTime.MinValue && DonHang.HoanTatCutter == 0)
                            {

                                if (DonHang.STT > 0)
                                {
                                    cmd.CommandText = string.Empty;
                                    string tgChay = $"{DonHang.Chay.Hours}:{DonHang.Chay.Minutes.ToString("00")}:{DonHang.Chay.Seconds.ToString("00")}";
                                    string tgDung = $"{DonHang.Dung.Hours}:{DonHang.Dung.Minutes.ToString("00")}:{DonHang.Dung.Seconds.ToString("00")}";
                                    string tgBatDau = $"{DonHang.TGBatDau.ToString("yyyy/MM/dd HH:mm:ss")}";
                                    string tgKetThuc = $"{DonHang.TGKetThuc.ToString("yyyy-MM-dd HH:mm:ss")}";
                                    string ngayTao = $"{DonHang.NgayTao.ToString("yyyy/MM/dd HH:mm:ss")}";

                                    string updateQuery = $"update dhdangchay set Dai = '{DonHang.Dai}', SL = '{DonHang.SL}', " +
                                        $"SLDat = '{DonHang.SLDat}', SLLoi = '{DonHang.SLLoi}', TGChay = '{tgChay}', TGDung = '{tgDung}', " +
                                        $"TGBatDau = '{DonHang.TGBatDau.ToString("yyyy-MM-dd HH:mm:ss")}', TGKetThuc = '{DonHang.TGKetThuc.ToString("yyyy-MM-dd HH:mm:ss")}', " +
                                        $"SoLanDung = '{DonHang.SoDung}', Pallet = '{DonHang.Pallet}', Xa = '{DonHang.Xa}', Rong = '{DonHang.Rong}', " +
                                        $"Canh = '{DonHang.Canh}', Cao = '{DonHang.Cao}', Lang = '{DonHang.Lang}', GhiChu = '{DonHang.GhiChu}', SoLanDung = '{DonHang.SoDung}' where Id = {DonHang.Id};";

                                    //cmd.CommandText = $"call proc_insert_donhangdachay({DonHang.STT}, '{DonHang.NgayTao.ToString("yyyy/MM/dd HH:mm:ss")}', {DonHang.SLDat}, {DonHang.SLLoi}, " +
                                    //        $"'{tgChay}', '{tgDung}', '{DonHang.TGBatDau.ToString("yyyy/MM/dd HH:mm:ss")}', " +
                                    //        $"'{DonHang.TGKetThuc.ToString("yyyy/MM/dd HH:mm:ss")}', '{DonHang.Ma}', '{DonHang.Song}', '{DonHang.Kho}', '{DonHang.Men}', " +
                                    //        $"'{DonHang.GiaySongE}', '{DonHang.GiayMatE}', '{DonHang.GiaySongB}', '{DonHang.GiayMatB}', '{DonHang.GiaySongC}', '{DonHang.GiayMatC}', " +
                                    //        $"'{DonHang.Dai}', '{DonHang.SL}', '{DonHang.Pallet}', '{DonHang.Xa}', '{DonHang.Rong}', '{DonHang.Canh}', '{DonHang.Cao}', " +
                                    //        $"'{DonHang.Lang}', '{Repository.Instance.Ca}', '{DonHang.GhiChu}', '{DonHang.SoDung}', '{DonHang.TocDoTB}'); ";

                                    cmd.CommandText += updateQuery;
                                    cmd.ExecuteNonQuery();

                                    cmd.CommandText = $"select count(*) from donhang where STT = '{DonHang.STT}'";

                                    using (MySqlDataAdapter adp = new MySqlDataAdapter(cmd))
                                    {
                                        DataTable dt = new DataTable();
                                        adp.Fill(dt);
                                        if (dt.Rows.Count > 0 && dt.Columns.Count > 0)
                                        {
                                            if (int.TryParse(dt.Rows[0][0].ToString(), out int count))
                                            {
                                                if (count > 0)
                                                {
                                                    string query = $"update `cutter`.`donhang` set `SLDat` = '{DonHang.SLDat}', `SLLoi` = '{DonHang.SLLoi}', `TGChay` = '{tgChay}', `TGDung` = '{tgDung}', `TGBatDau` = '{tgBatDau}', `TGKetThuc` = '{tgKetThuc}', `NgayTao` = '{ngayTao}', " +
                                                        $"`Xa` = '{DonHang.Xa}', `Rong` = '{DonHang.Rong}', `Canh` = '{DonHang.Canh}', `Cao` = '{DonHang.Cao}', `Lang` = '{DonHang.Lang}', `GhiChu` = '{DonHang.GhiChu}', `Dai` = '{DonHang.Dai}', `SL` = '{DonHang.SL}', `SoLanDung` = '{DonHang.SoDung}', `Pallet` = '{DonHang.Pallet}', `TocDo` = '{DonHang.TocDoTB}' where `STT` = '{DonHang.STT}';";
                                                    cmd.CommandText = query;
                                                    cmd.ExecuteNonQuery();
                                                }
                                                else
                                                {

                                                    string query = $"INSERT INTO `cutter`.`donhang` (`TocDo`, `STT`, `SLDat`, `SLLoi`, `TGChay`, `TGDung`, `TGBatDau`, `TGKetThuc`, `NgayTao`, `Ma`, `Song`, `Kho`, `Dai`, `SL`, `Pallet`, `Xa`, `Rong`, `Canh`, `Cao`, `Lang`, `GhiChu`, `GiaySongE`, `GiayMatE`, `GiaySongB`, `GiayMatB`, `GiaySongC`, `GiayMatC`, `GiayMen`, `Ca`, `SoLanDung`) " +
                                                        $"VALUES('{DonHang.TocDoTB}', '{DonHang.STT}', '{DonHang.SLDat}', '{DonHang.SLLoi}', '{tgChay}', '{tgDung}', " +
                                                        $"'{tgBatDau}', '{tgKetThuc}', '{ngayTao}', '{DonHang.Ma}', '{DonHang.Song}', '{DonHang.Kho}', '{DonHang.Dai}', '{DonHang.SL}', " +
                                                        $"'{DonHang.Pallet}', '{DonHang.Xa}', '{DonHang.Rong}', '{DonHang.Canh}', '{DonHang.Cao}', '{DonHang.Lang}', '{DonHang.GhiChu}', " +
                                                        $"'{DonHang.GiaySongE}', '{DonHang.GiayMatE}', '{DonHang.GiaySongB}', '{DonHang.GiayMatB}', '{DonHang.GiaySongC}', " +
                                                        $"'{DonHang.GiayMatC}', '{DonHang.Men}', '{DonHang.Ca}', '{DonHang.SoDung}');";

                                                    cmd.CommandText = query;
                                                    cmd.ExecuteNonQuery();
                                                    Debug.WriteLine($"Tao don hang da chay {DonHang.STT}");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // MessageBox.Show($"Lỗi đồng bộ xin hãy liên hệ với quản trị viên. {ex.ToString()}");
                });
            }
            finally { updateTimer.Start(); }
        }
       // MySqlCommand cmd;
       // private MySqlConnection conn;
        private System.Timers.Timer updateTimer;
        public bool IsStarted { get; private set; }
        public DonHang DonHang { get; set; }
        //public long LockSTT { get; set; }
        protected DonHang DonHangNap { get; private set; }

        public void Start(DonHang dh)
        {
            IsStarted = true;
            DonHangNap = dh;
        }

        public void Stop()
        {
            IsStarted = false;
        }

        public void ChotDon(DonHang dh)
        {
            if (dh != null && dh.STT > 0)
            {
                if (CutterTags.Instance.STTChot != null &&
                    CutterTags.Instance.STTChot.Quality == Quality.Good &&
                    long.TryParse(CutterTags.Instance.STTChot.Value, out long sttChot))
                {
                    if (dh.STT == sttChot)
                    {
                        if (CutterTags.Instance.SLDatChot != null &&
                           CutterTags.Instance.SLDatChot.Quality == Quality.Good &&
                           long.TryParse(CutterTags.Instance.SLDatChot.Value, out long slDatChot))
                        {
                            if (CutterTags.Instance.SLLoiChot != null &&
                               CutterTags.Instance.SLLoiChot.Quality == Quality.Good &&
                               long.TryParse(CutterTags.Instance.SLLoiChot.Value, out long slLoiChot))
                            {
                                Debug.WriteLine($"Chot don: {dh.STT}");
                                Repository.Instance.UpdateColumns("donhang", $" set SLDat = '{slDatChot}', SLLoi = '{slLoiChot}' where STT = '{sttChot}'");
                            }
                        }
                    }
                }
            }
        }
    }
}
