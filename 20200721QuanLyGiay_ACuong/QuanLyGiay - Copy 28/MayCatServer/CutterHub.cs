using CommonControls;
using EasyDriverPlugin;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace MayCatServer
{
    public class CutterHub : Hub
    {
        private readonly BroadcastService broadcastService;

        public CutterHub() : this(BroadcastService.Instance) { }

        public CutterHub(BroadcastService broadcastService)
        {
            this.broadcastService = broadcastService;
        }

        public  void GhiHeSoSong(string song, string value)
        {
        }

        public async Task<bool> GhiSoMetLoi(string song, string value)
        {
            if (song?.ToLower() == "e")
            {
                if (SongETags.Instance.SoMetLoi != null && SongETags.Instance.SoMetLoi.Quality == Quality.Good)
                {
                    if (int.TryParse(value, out int uintValue))
                    {
                        var response =  SongETags.Instance.SoMetLoi.Write(uintValue.ToString());
                        return response == Quality.Good;
                    }
                }
            }
            else if (song?.ToLower() == "b")
            {
                if (SongBTags.Instance.SoMetLoi != null && SongBTags.Instance.SoMetLoi.Quality == Quality.Good)
                {
                    if (int.TryParse(value, out int uintValue))
                    {
                        var response =  SongBTags.Instance.SoMetLoi.Write(uintValue.ToString());
                        return response == Quality.Good;
                    }
                }
            }
            else if (song?.ToLower() == "c")
            {
                if (SongCTags.Instance.SoMetLoi != null && SongCTags.Instance.SoMetLoi.Quality == Quality.Good)
                {
                    if (int.TryParse(value, out int uintValue))
                    {
                        var response =  SongCTags.Instance.SoMetLoi.Write(uintValue.ToString());
                        return response == Quality.Good;
                    }
                }
            }
            else if (song?.ToLower() == "m")
            {
                if (MayMenTags.Instance.SoMetLoi != null && MayMenTags.Instance.SoMetLoi.Quality == Quality.Good)
                {
                    if (int.TryParse(value, out int uintValue))
                    {
                        var response =  MayMenTags.Instance.SoMetLoi.Write(uintValue.ToString());
                        return response == Quality.Good;
                    }
                }
            }
            return false;
        }

        public void CapNhat(string song)
        {
            try
            {
                BackgroundWorker bw = new BackgroundWorker();

                if (!string.IsNullOrEmpty(song))
                {
                    switch (song.ToLower())
                    {
                        case "e":
                            bw.DoWork += (s, e) =>
                            {
                                List<DonHang> dhNap = MainWindow.DonHangDataSource.ToList();
                                if (double.TryParse(SongETags.Instance.HeSoSong?.Value, out double heSoSong))
                                    SongEController.Instance.NapDanhSachDonHang(dhNap, heSoSong, "Cap nhat song tu app song", true);
                            };
                            bw.RunWorkerAsync();
                            break;
                        case "b":
                            bw.DoWork += (s, e) =>
                            {
                                List<DonHang> dhNap = MainWindow.DonHangDataSource.ToList();
                                if (double.TryParse(SongBTags.Instance.HeSoSong?.Value, out double heSoSong))
                                    SongBController.Instance.NapDanhSachDonHang(dhNap, heSoSong, "Cap nhat song tu app song", true);
                            };
                            bw.RunWorkerAsync();
                            break;
                        case "c":
                            bw.DoWork += (s, e) =>
                            {
                                List<DonHang> dhNap = MainWindow.DonHangDataSource.ToList();
                                if (double.TryParse(SongCTags.Instance.HeSoSong?.Value, out double heSoSong))
                                    SongCController.Instance.NapDanhSachDonHang(dhNap, heSoSong, "Cap nhat song tu app song", true);
                            };
                            break;
                        case "m":
                            bw.DoWork += (s, e) =>
                            {
                                List<DonHang> dhNap = MainWindow.DonHangDataSource.ToList();
                                MayMenController.Instance.NapDanhSachDonHang(dhNap, 1, true);
                            };
                            break;
                        default:
                            break;
                    }
                }
            }
            catch { }
        }

        public string ThemDonHang(DonHang dh, bool sttChanged)
        {
            try
            {
                int isChanged = 0;
                int result = DonHangForm.TaoDonHang(dh, sttChanged, ref isChanged);

                if (result != 0)
                {
                     var DonHangDataSource = Repository.Instance.GetDonHangs().OrderByDescending(x => x.STT).ToList();

                    long sttLonNhat = 0;
                    if (sttChanged)
                    {
                        sttLonNhat = dh.STT;
                    }
                    else
                    {
                        sttLonNhat = DonHangDataSource.Max(x => x.STT);
                    }
                    if (sttLonNhat != 0)
                    {
                        DonHangDataSource = Repository.Instance.GetDonHangs().OrderByDescending(x => x.STT).ToList();
                        DonHang dhMoi = DonHangDataSource.FirstOrDefault(x => x.STT == sttLonNhat);
                        if (dhMoi != null)
                        {
                            try
                            {
                                broadcastService.MainWindow.Dispatcher.Invoke(() =>
                                {
                                    broadcastService.MainWindow.AddDonHang(dhMoi);

                                    if (sttChanged)
                                    {
                                        broadcastService.MainWindow.RefreshDonHangHienThi();
                                    }
                                    else
                                    {
                                        broadcastService.MainWindow.DonHangHienThi.Add(dhMoi);
                                    }
                                    broadcastService.MainWindow.NapDonAct();
                                });
                                return string.Empty;
                            }
                            catch (Exception ex)
                            {
                                return ex.ToString();
                            }
                        }
                    }
                }

                return "Không thể tạo đơn hàng";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public string SuaDonHang(long id, DonHang dh)
        {
            try
            {
                var CaiDat = Repository.Instance.GetCaiDat();
                if (dh.HoanTatCutter > 0 ||
                    DonHangForm.KiemTra(dh, CaiDat) <= 0)
                {
                    return "Không thể chỉnh sửa đơn hàng đã hoàn thành hoặc đang chạy";
                }
                else
                {
                    var DonHangDataSource = Repository.Instance.GetDonHangs().OrderByDescending(x => x.STT).ToList();
                    if (DonHangDataSource != null && DonHangDataSource.Count > 0)
                    {
                        if (DonHangDataSource.LastOrDefault() is DonHang dhToiDa)
                        {
                            if (dh.STT < dhToiDa.STT)
                            {
                                return "STT mới không thể nhỏ hơn đơn hàng đầu tiên của danh sách đơn hàng!";
                            }

                            foreach (var item in DonHangDataSource)
                            {
                                if (item.HoanTatCutter + item.HoanTatSongB + item.HoanTatSongC +
                                    item.HoanTatSongE + item.HoanTatSpliter + item.HoanTatMayMen > 0)
                                {
                                    if (dh.STT < item.STT)
                                    {
                                        return "STT mới không thể nhỏ hơn STT của các đơn đã hoàn thành hoặc đang chạy!";
                                    }
                                }
                            }

                            if (DonHangDataSource.Any(x => x.STT == dh.STT && x.Id != id))
                                return "STT của đơn hàng đã tồn tại xin mời nhập STT khác";
                        }
                    }

                    if (CutterTags.Instance.STT1 != null && long.TryParse(CutterTags.Instance.STT1.Value, out long sttPLC))
                    {
                        if (dh.STT <= sttPLC)
                        {
                            return "STT của đơn hàng không thể nhỏ hơn hoặc bằng STT của đơn hàng đang chạy";
                        }
                    }
                    else
                    {
                        return "Mất kết nối đến server !";
                    }

                    if (DonHangForm.EditDonHang(id, dh) != 0)
                    {
                        DonHangDataSource = Repository.Instance.GetDonHangs().OrderByDescending(x => x.STT).ToList();
                        DonHang dhSua = DonHangDataSource.FirstOrDefault(x => x.Id == id);
                        if (dhSua != null)
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

                            broadcastService.MainWindow.Dispatcher.Invoke(() =>
                            {

                                broadcastService.MainWindow.NapDonAct();

                                broadcastService.MainWindow.DonHangHienThi.Clear();
                                if (CutterTags.Instance.STT1 != null && long.TryParse(CutterTags.Instance.STT1.Value, out long sttDH))
                                    broadcastService.MainWindow.DonHangHienThi.AddRange(DonHangDataSource.Where(x => x.HoanTatCutter == 0 && x.STT >= sttDH).OrderBy(x => x.STT));
                                else
                                    broadcastService.MainWindow.DonHangHienThi.AddRange(DonHangDataSource.Where(x => x.HoanTatCutter == 0).OrderBy(x => x.STT));

                            });
                        }

                        return string.Empty;
                    }
                    else
                    {
                        return "Sửa đơn hàng không thành công";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
           // return "Không thể sửa đơn hàng";
        }

        public string XoaDonHang(DonHang dh)
        {
            try
            {
                var CaiDat = Repository.Instance.GetCaiDat();
                if (dh != null)
                {
                    if (dh.HoanTatCutter > 0 ||
                        DonHangForm.KiemTra(dh, CaiDat) <= 0)
                    {
                        return "Không thể xóa đơn hàng đã hoàn thành hoặc đang chạy";
                    }

                    using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default["constr"].ToString()))
                    {
                        conn.Open();
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = $"delete from dhdangchay where Id = {dh.Id}";

                            int result = cmd.ExecuteNonQuery();
                            var DonHangDataSource = Repository.Instance.GetDonHangs().OrderByDescending(x => x.STT).ToList();
                            DonHang dhXoa = DonHangDataSource.FirstOrDefault(x => x.Id == dh.Id);
                            if (dhXoa == null)
                            {
                                broadcastService.MainWindow.Dispatcher.Invoke(() =>
                                {
                                    broadcastService.MainWindow.RemoveDonHang(x => x.Id == dh.Id);
                                    broadcastService.MainWindow.NapDonAct();
                                    broadcastService.MainWindow.DonHangHienThi.Clear();
                                    broadcastService.MainWindow.DonHangHienThi.AddRange(DonHangDataSource.Where(x => x.HoanTatCutter == 0).OrderBy(x => x.STT));
                                });
                                return string.Empty;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return "Không thể xóa đơn hàng";
        }

        public string XoaTatCaDonHang()
        {
           try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default["constr"].ToString()))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        int.TryParse(CutterTags.Instance.STT1.Value, out int stt);
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = $"delete from dhdangchay where Ca = '{Repository.Instance.Ca}' and STT != '{stt}'";
                        int result = cmd.ExecuteNonQuery();
                        if (result != 0)
                        {
                            broadcastService.MainWindow.Dispatcher.Invoke(() =>
                            {
                                Func<DonHang, bool> predicate = (x) =>
                                {
                                    return x.STT != stt;
                                };

                                broadcastService.MainWindow.RemoveDonHang(predicate);

                                var dhHienThi = broadcastService.MainWindow.DonHangHienThi.ToArray();
                                foreach (var item in dhHienThi)
                                {
                                    if (predicate(item))
                                        broadcastService.MainWindow.DonHangHienThi.Remove(item);
                                }

                                // broadcastService.MainWindow.DonHangHienThi.Clear();
                                broadcastService.MainWindow.NapDonAct();
                            });

                        }
                        return string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "Không thể xóa đơn hàng";
        }
    }

    public class BroadcastService
    {
        public MainWindow MainWindow { get; set; }
        private readonly static Lazy<BroadcastService> _instance = new Lazy<BroadcastService>(() => new BroadcastService(GlobalHost.ConnectionManager.GetHubContext<CutterHub>().Clients));
        public static BroadcastService Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private IHubConnectionContext<dynamic> Clients { get; set; }

        private readonly Task task;
        public BroadcastService(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
            //task = Task.Factory.StartNew(Broadcast, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private void Broadcast()
        {
            //while( true)
            //{
            //    try
            //    {
            //        //// Gửi danh sách đơn hàng
            //        //Clients.All.broadcastDanhSachDonHang(JsonConvert.SerializeObject(MainWindow.DonHangDataSource.ToList()));

            //        //// Gửi trạng thái đơn hàng đang chạy
            //        //Clients.All.broadcastTrangThaiDonHang(JsonConvert.SerializeObject(MainWindow.TrangThaiDonHang));

            //        //// Gửi thông tin tốc độ đổi đơi của các trạm
            //        //Clients.All.broadcastThongTinTram(JsonConvert.SerializeObject(MainWindow.ThongTinTram));

            //        //// Gửi đơn hàng sóng E
            //        //Clients.All.broadcastGiayE(JsonConvert.SerializeObject(MainWindow.GiaySongGiayMatDangChayE));

            //        //// Gửi đơn hàng sóng B
            //        //Clients.All.broadcastGiayB(JsonConvert.SerializeObject(MainWindow.GiaySongGiayMatDangChayB));

            //        //// Gửi đơn hàng sóng C
            //        //Clients.All.broadcastGiayC(JsonConvert.SerializeObject(MainWindow.GiaySongGiayMatDangChayC));
            //    }
            //    catch { }
            //    finally { Thread.Sleep(100); }
            //}
        }

    }
}
