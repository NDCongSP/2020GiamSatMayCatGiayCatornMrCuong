using DevExpress.Mvvm;
using EasyScada.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MayCatServer.Tags
{
    public abstract class SongTags
    {
        public string StationName { get; set; } = "Local Station";
        public string ChannelName { get; set; } = "Channel";
        public string DeviceName { get; set; } = "SongE";
        public IEasyDriverConnector DriverConnector => EasyDriverConnectorProvider.GetEasyDriverConnector();

        public ITag ChieuDaiMat1 { get; set; }
        public ITag ChieuDaiSong1 { get; set; }
        public ITag STT1 { get; set; }
        public ITag ChieuDaiMat2 { get; set; }
        public ITag ChieuDaiSong2 { get; set; }
        public ITag STT2 { get; set; }
        public ITag HeSoSong { get; set; }
        public ITag TocDo { get; set; }
        public ITag Run { get; set; }
        public ITag CheDoChuyenDon { get; set; }
        public ITag STTChot { get; set; }
        public ITag SoMetChot { get; set; }
        public ITag DoiDon { get; set; }
        public ITag Setting1 { get; set; }
        public ITag Setting2 { get; set; }
        public ITag Setting3 { get; set; }
        public ITag Setting4 { get; set; }
        public ITag Setting5 { get; set; }
        public ITag Setting6 { get; set; }
        public ITag Setting7 { get; set; }
        public ITag Setting8 { get; set; }
        public ITag Setting9 { get; set; }
        public ITag Setting10 { get; set; }
        public ITag LenhChuyenDon { get; set; }
        public ITag DoiGiaySong { get; set; }
        public ITag DoiGiayMat { get; set; }
        public ITag CoBaoChuanBiGiay { get; set; }
        public ITag CoBaoNapGiay { get; set; }
        public ITag CoKiemTraChieuDai { get; set; }
        public ITag ChieuDaiKiemTra { get; set; }
        public ITag STTSong1 { get; set; }
        public ITag STTMat1 { get; set; }
        public ITag STTSong2 { get; set; }
        public ITag STTMat2 { get; set; }
        public ITag LenhChuyenDonGiaySong { get; set; }
        public ITag LenhChuyenDonGiayMat { get; set; }
        public ITag STTSongChot { get; set; }
        public ITag STTMatChot { get; set; }
        public ITag ChieuDaiDon1 { get; set; }
        public ITag ChieuDaiDon2 { get; set; }
        public ITag SoMetDatChotGiaySong { get; set; }
        public ITag SoMetDatChotGiayMat { get; set; }
        public ITag SoMetLoiChotGiaySong { get; set; }
        public ITag SoMetLoiChotGiayMat { get; set; }
        public ITag Dan { get; set; }
        public ITag SoMetLoi { get; set; }

        public SongTags(string deviceName)
        {
            DeviceName = deviceName;
        }

        public async void GetTags()
        {
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                if (propertyInfo.PropertyType == typeof(ITag))
                {
                    propertyInfo.SetValue(this, GetTag(propertyInfo.Name));
                }
            }

            if (LenhChuyenDon != null && STTChot != null)
            {
                var response = await DriverConnector.WriteTagAsync(LenhChuyenDon.Path, "0");
                LenhChuyenDon.ValueChanged += LenhChuyenDon_ValueChanged;
            }

            if (LenhChuyenDonGiayMat != null && STTMatChot != null)
            {
                var response = await DriverConnector.WriteTagAsync(LenhChuyenDonGiayMat.Path, "0");
                LenhChuyenDonGiayMat.ValueChanged += LenhChuyenDonGiayMat_ValueChanged;
            }

            if (LenhChuyenDonGiaySong != null && STTSongChot != null)
            {
                var response = await DriverConnector.WriteTagAsync(LenhChuyenDonGiaySong.Path, "0");
                LenhChuyenDonGiaySong.ValueChanged += LenhChuyenDonGiaySong_ValueChanged;
            }
            if (CoKiemTraChieuDai != null)
            {
                var response = await DriverConnector.WriteTagAsync(CoKiemTraChieuDai.Path, "0");
                CoKiemTraChieuDai.ValueChanged += CoKiemTraChieuDai_ValueChanged;
            }
        }

        private async void CoKiemTraChieuDai_ValueChanged(object sender, TagValueChangedEventArgs e)
        {
            if (e.NewValue != "0" && (sender as ITag).Quality == Quality.Good)
            {
                var response = await DriverConnector.WriteTagAsync((sender as ITag).Path, "0");
                if (CutterTags.Instance.STT1 != null &&
                STT1 != null)
                {
                    Debug.WriteLine($"Phát hiện cờ báo kiểm tra chiều dài Máy - {DeviceName}");
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Messenger.Default.Send(new MessageKiemTraChieuDai(DeviceName));

                    });
                }
            }
        }

        private async void LenhChuyenDonGiaySong_ValueChanged(object sender, TagValueChangedEventArgs e)
        {
            if (e.NewValue != "0" && (sender as ITag).Quality == Quality.Good)
            {
                var response = await DriverConnector.WriteTagAsync((sender as ITag).Path, "0");
                Debug.WriteLine($"Ghi '{(sender as ITag).Path}' Value = 0, Quality = {response.IsSuccess}");
                Debug.WriteLine($"Phát hiện lệnh chuyển đơn giấy sóng - Máy: {DeviceName}");
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Messenger.Default.Send(new MessageChuyenDonGiaySong(DeviceName, STTSongChot.Value));

                });
            }
        }

        private async void LenhChuyenDonGiayMat_ValueChanged(object sender, TagValueChangedEventArgs e)
        {
            if (e.NewValue != "0" && (sender as ITag).Quality == Quality.Good)
            {
                var response = await DriverConnector.WriteTagAsync((sender as ITag).Path, "0");
                Debug.WriteLine($"Ghi '{(sender as ITag).Path}' Value = 0, Quality = {response.IsSuccess}");
                Debug.WriteLine($"Phát hiện lệnh chuyển đơn giấy mặt - Máy: {DeviceName}");
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Messenger.Default.Send(new MessageChuyenDonGiayMat(DeviceName, STTMatChot.Value));

                });
            }
        }

        private async void LenhChuyenDon_ValueChanged(object sender, TagValueChangedEventArgs e)
        {
            if (e.NewValue != "0" && (sender as ITag).Quality == Quality.Good)
            {
                var response = await DriverConnector.WriteTagAsync((sender as ITag).Path, "0");
                Debug.WriteLine($"Ghi '{(sender as ITag).Path}' Value = 0, Quality = {response.IsSuccess}");
                Debug.WriteLine($"Phát hiện lệnh chuyển đơn - Máy: {DeviceName}");
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Messenger.Default.Send(new MessageChuyenDonHang(DeviceName, STTChot.Value));

                });
            }
        }

        private ITag GetTag(string tagName)
        {
            return DriverConnector.GetTag($"{StationName}/{ChannelName}/{DeviceName}/{tagName}");
        }
    }
}
