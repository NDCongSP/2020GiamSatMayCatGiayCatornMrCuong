using DevExpress.Mvvm;
using EasyDriverPlugin;
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

        public ITagCore ChieuDaiMat1 { get; set; }
        public ITagCore ChieuDaiSong1 { get; set; }
        public ITagCore STT1 { get; set; }
        public ITagCore ChieuDaiMat2 { get; set; }
        public ITagCore ChieuDaiSong2 { get; set; }
        public ITagCore STT2 { get; set; }
        public ITagCore HeSoSong { get; set; }
        public ITagCore TocDo { get; set; }
        public ITagCore Run { get; set; }
        public ITagCore CheDoChuyenDon { get; set; }
        public ITagCore STTChot { get; set; }
        public ITagCore SoMetChot { get; set; }
        public ITagCore DoiDon { get; set; }
        public ITagCore Setting1 { get; set; }
        public ITagCore Setting2 { get; set; }
        public ITagCore Setting3 { get; set; }
        public ITagCore Setting4 { get; set; }
        public ITagCore Setting5 { get; set; }
        public ITagCore Setting6 { get; set; }
        public ITagCore Setting7 { get; set; }
        public ITagCore Setting8 { get; set; }
        public ITagCore Setting9 { get; set; }
        public ITagCore Setting10 { get; set; }
        public ITagCore LenhChuyenDon { get; set; }
        public ITagCore DoiGiaySong { get; set; }
        public ITagCore DoiGiayMat { get; set; }
        public ITagCore CoBaoChuanBiGiay { get; set; }
        public ITagCore CoBaoNapGiay { get; set; }
        public ITagCore CoKiemTraChieuDai { get; set; }
        public ITagCore ChieuDaiKiemTra { get; set; }
        public ITagCore STTSong1 { get; set; }
        public ITagCore STTMat1 { get; set; }
        public ITagCore STTSong2 { get; set; }
        public ITagCore STTMat2 { get; set; }
        public ITagCore LenhChuyenDonGiaySong { get; set; }
        public ITagCore LenhChuyenDonGiayMat { get; set; }
        public ITagCore STTSongChot { get; set; }
        public ITagCore STTMatChot { get; set; }
        public ITagCore ChieuDaiDon1 { get; set; }
        public ITagCore ChieuDaiDon2 { get; set; }
        public ITagCore SoMetDatChotGiaySong { get; set; }
        public ITagCore SoMetDatChotGiayMat { get; set; }
        public ITagCore SoMetLoiChotGiaySong { get; set; }
        public ITagCore SoMetLoiChotGiayMat { get; set; }
        public ITagCore Dan { get; set; }
        public ITagCore SoMetLoi { get; set; }

        public SongTags(string deviceName)
        {
            DeviceName = deviceName;
        }

        public  void GetTags()
        {
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                if (propertyInfo.PropertyType == typeof(ITagCore))
                {
                    propertyInfo.SetValue(this, GetTag(propertyInfo.Name));
                }
            }

            if (LenhChuyenDon != null && STTChot != null)
            {
                //var response =  DriverConnector.WriteTag(LenhChuyenDon.Path, "0");
                LenhChuyenDon.ValueChanged += LenhChuyenDon_ValueChanged;
            }

            if (LenhChuyenDonGiayMat != null && STTMatChot != null)
            {
                //var response =  DriverConnector.WriteTag(LenhChuyenDonGiayMat.Path, "0");
                LenhChuyenDonGiayMat.ValueChanged += LenhChuyenDonGiayMat_ValueChanged;
            }

            if (LenhChuyenDonGiaySong != null && STTSongChot != null)
            {
                //var response =  DriverConnector.WriteTag(LenhChuyenDonGiaySong.Path, "0");
                LenhChuyenDonGiaySong.ValueChanged += LenhChuyenDonGiaySong_ValueChanged;
            }
            if (CoKiemTraChieuDai != null)
            {
                //var response =  DriverConnector.WriteTag(CoKiemTraChieuDai.Path, "0");
                CoKiemTraChieuDai.ValueChanged += CoKiemTraChieuDai_ValueChanged;
            }
        }

        private  void CoKiemTraChieuDai_ValueChanged(object sender, TagValueChangedEventArgs e)
        {
            if (e.NewValue != "0" && (sender as ITagCore).Quality == Quality.Good)
            {
                var response = (sender as ITagCore).WriteTag("0");
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

        private  void LenhChuyenDonGiaySong_ValueChanged(object sender, TagValueChangedEventArgs e)
        {
            if (e.NewValue != "0" && (sender as ITagCore).Quality == Quality.Good)
            {
                var response = (sender as ITagCore).WriteTag("0");
                Debug.WriteLine($"Ghi '{(sender as ITagCore).Path}' Value = 0, Quality = {response}");
                Debug.WriteLine($"Phát hiện lệnh chuyển đơn giấy sóng - Máy: {DeviceName}");
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Messenger.Default.Send(new MessageChuyenDonGiaySong(DeviceName, STTSongChot.Value));

                });
            }
        }

        private  void LenhChuyenDonGiayMat_ValueChanged(object sender, TagValueChangedEventArgs e)
        {
            if (e.NewValue != "0" && (sender as ITagCore).Quality == Quality.Good)
            {
                var response = (sender as ITagCore).WriteTag("0");
                Debug.WriteLine($"Ghi '{(sender as ITagCore).Path}' Value = 0, Quality = {response}");
                Debug.WriteLine($"Phát hiện lệnh chuyển đơn giấy mặt - Máy: {DeviceName}");
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Messenger.Default.Send(new MessageChuyenDonGiayMat(DeviceName, STTMatChot.Value));

                });
            }
        }

        private  void LenhChuyenDon_ValueChanged(object sender, TagValueChangedEventArgs e)
        {
            if (e.NewValue != "0" && (sender as ITagCore).Quality == Quality.Good)
            {
                var response = (sender as ITagCore).WriteTag("0");
                Debug.WriteLine($"Ghi '{(sender as ITagCore).Path}' Value = 0, Quality = {response}");
                Debug.WriteLine($"Phát hiện lệnh chuyển đơn - Máy: {DeviceName}");
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Messenger.Default.Send(new MessageChuyenDonHang(DeviceName, STTChot.Value));

                });
            }
        }

        private ITagCore GetTag(string tagName)
        {
            if (EasyProject.Instance.Project.Browse(new string[] { StationName, ChannelName, DeviceName }, 0) is IHaveTag device)
            {
                return device.Tags.Find(tagName);
            }
            return null;
        }
    }
}
