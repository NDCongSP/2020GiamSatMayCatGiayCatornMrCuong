using DevExpress.Mvvm;
using EasyScada.Core;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MayCatServer
{
    public class CutterTags
    {
        public static CutterTags Instance { get; } = new CutterTags();

        public string StationName { get; set; } = "Local Station";
        public string ChannelName { get; set; } = "Channel";
        public string DeviceName { get; set; } = "CUTTER";
        public IEasyDriverConnector DriverConnector => EasyDriverConnectorProvider.GetEasyDriverConnector();

        public ITag DaiCat1 { get; set; }
        public ITag DaiCat2 { get; set; }
        public ITag DaiCat3 { get; set; }
        public ITag SLCat1 { get; set; }
        public ITag SLCat2 { get; set; }
        public ITag SLCat3 { get; set; }
        public ITag Pallet1 { get; set; }
        public ITag Pallet2 { get; set; }
        public ITag Pallet3 { get; set; }
        public ITag STT1 { get; set; }
        public ITag STT2 { get; set; }
        public ITag STT3 { get; set; }
        public ITag SLDat { get; set; }
        public ITag SLLoi { get; set; }
        public ITag TocDo { get; set; }
        public ITag DoiDon { get; set; }
        public ITag Run { get; set; }
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
        public ITag DaiCatChot { get; set; }
        public ITag STTChot { get; set; }
        public ITag SLDatChot { get; set; }
        public ITag SLLoiChot { get; set; }
        public ITag CheDoChuyenDon { get; set; }

        public CutterTags()
        {
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
            if (STT1 != null)
            {
                STT1.ValueChanged += STT1_ValueChanged;
                STT1_ValueChanged(STT1, new TagValueChangedEventArgs(STT1, "", STT1.Value));

                if (SLLoi != null)
                {
                    SLLoi.ValueChanged += SLLoi_ValueChanged;
                    SLLoi_ValueChanged(SLLoi, new TagValueChangedEventArgs(STT1, "", SLLoi.Value));
                }

                if (DaiCat1 != null)
                {
                    DaiCat1.ValueChanged += DaiCat1_ValueChanged;
                    DaiCat1_ValueChanged(DaiCat1, new TagValueChangedEventArgs(STT1, "", DaiCat1.Value));
                }
            }

            if (DoiDon != null)
            {
                DoiDon.ValueChanged += DoiDon_ValueChanged;
            }
        }

        private async void DoiDon_ValueChanged(object sender, TagValueChangedEventArgs e)
        {
            try
            {
                if (MayMenTags.Instance.Setting1 != null)
                {
                    if (double.TryParse(e.NewValue, out double doiDonCut))
                    {
                        doiDonCut = doiDonCut / 1000;

                        double doiDon3 = doiDonCut - MainWindow.CaiDat.DanMayMen;
                        if (doiDon3 <= MainWindow.CaiDat.SoMetBaoChuyenDonMayMen)
                        {
                            if (MayMenTags.Instance.Setting1.Value != "100")
                                await MayMenTags.Instance.Setting1.WriteAsync("100");
                        }
                        else
                        {
                            if (MayMenTags.Instance.Setting1.Value != "0")
                                await MayMenTags.Instance.Setting1.WriteAsync("0");
                        }
                    }
                }
            }
            catch { }
        }

        private void DaiCat1_ValueChanged(object sender, TagValueChangedEventArgs e)
        {
            try
            {

                //if (int.TryParse(e.NewValue, out int dai) && int.TryParse(SLLoi.Value, out int slLoi))
                    //Messenger.Default.Send(new MessageSLLoiMayCatThayDoi(STT1.Value, slLoi, dai));
            }
            catch { }
        }

        private void SLLoi_ValueChanged(object sender, TagValueChangedEventArgs e)
        {
            //try
            //{
            //    if (int.TryParse(e.NewValue, out int slLoi) && int.TryParse(DaiCat1.Value, out int dai))
            //        Messenger.Default.Send(new MessageSLLoiMayCatThayDoi(STT1.Value, slLoi, dai));
            //}
            //catch { }
        }

        private void STT1_ValueChanged(object sender, TagValueChangedEventArgs e)
        {
            if ((sender as ITag).Quality == Quality.Good)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Messenger.Default.Send(new MessageSTTDonHangThayDoi(e.OldValue, e.NewValue));
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
