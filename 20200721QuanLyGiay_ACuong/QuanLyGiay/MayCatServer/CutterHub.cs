using CommonControls;
using EasyScada.Core;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            //this.broadcastService = broadcastService;
        }

        public async void GhiHeSoSong(string song, string value)
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
                        var response = await SongETags.Instance.SoMetLoi.WriteAsync(uintValue.ToString());
                        return response.IsSuccess;
                    }
                }
            }
            else if (song?.ToLower() == "b")
            {
                if (SongBTags.Instance.SoMetLoi != null && SongBTags.Instance.SoMetLoi.Quality == Quality.Good)
                {
                    if (int.TryParse(value, out int uintValue))
                    {
                        var response = await SongBTags.Instance.SoMetLoi.WriteAsync(uintValue.ToString());
                        return response.IsSuccess;
                    }
                }
            }
            else if (song?.ToLower() == "c")
            {
                if (SongCTags.Instance.SoMetLoi != null && SongCTags.Instance.SoMetLoi.Quality == Quality.Good)
                {
                    if (int.TryParse(value, out int uintValue))
                    {
                        var response = await SongCTags.Instance.SoMetLoi.WriteAsync(uintValue.ToString());
                        return response.IsSuccess;
                    }
                }
            }
            else if (song?.ToLower() == "m")
            {
                if (MayMenTags.Instance.SoMetLoi != null && MayMenTags.Instance.SoMetLoi.Quality == Quality.Good)
                {
                    if (int.TryParse(value, out int uintValue))
                    {
                        var response = await MayMenTags.Instance.SoMetLoi.WriteAsync(uintValue.ToString());
                        return response.IsSuccess;
                    }
                }
            }
            return false;
        }

    }

    public class BroadcastService
    {
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
            while( true)
            {
                try
                {
                    //// Gửi danh sách đơn hàng
                    //Clients.All.broadcastDanhSachDonHang(JsonConvert.SerializeObject(MainWindow.DonHangDataSource.ToList()));

                    //// Gửi trạng thái đơn hàng đang chạy
                    //Clients.All.broadcastTrangThaiDonHang(JsonConvert.SerializeObject(MainWindow.TrangThaiDonHang));

                    //// Gửi thông tin tốc độ đổi đơi của các trạm
                    //Clients.All.broadcastThongTinTram(JsonConvert.SerializeObject(MainWindow.ThongTinTram));

                    //// Gửi đơn hàng sóng E
                    //Clients.All.broadcastGiayE(JsonConvert.SerializeObject(MainWindow.GiaySongGiayMatDangChayE));

                    //// Gửi đơn hàng sóng B
                    //Clients.All.broadcastGiayB(JsonConvert.SerializeObject(MainWindow.GiaySongGiayMatDangChayB));

                    //// Gửi đơn hàng sóng C
                    //Clients.All.broadcastGiayC(JsonConvert.SerializeObject(MainWindow.GiaySongGiayMatDangChayC));
                }
                catch { }
                finally { Thread.Sleep(100); }
            }
        }

    }
}
