using DevExpress.Mvvm;
using EasyDriverPlugin;
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

        public ITagCore DaiCat1 { get; set; }
        public ITagCore DaiCat2 { get; set; }
        public ITagCore DaiCat3 { get; set; }
        public ITagCore SLCat1 { get; set; }
        public ITagCore SLCat2 { get; set; }
        public ITagCore SLCat3 { get; set; }
        public ITagCore Pallet1 { get; set; }
        public ITagCore Pallet2 { get; set; }
        public ITagCore Pallet3 { get; set; }
        public ITagCore STT1 { get; set; }
        public ITagCore STT2 { get; set; }
        public ITagCore STT3 { get; set; }
        public ITagCore STTXa1 { get; set; }
        public ITagCore STTXa2 { get; set; }
        public ITagCore STTXa3 { get; set; }
        public ITagCore Line1 { get; set; }
        public ITagCore Line2 { get; set; }
        public ITagCore Line3 { get; set; }
        public ITagCore SLDat { get; set; }
        public ITagCore SLLoi { get; set; }
        public ITagCore TocDo { get; set; }
        public ITagCore DoiDon { get; set; }
        public ITagCore Run { get; set; }
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
        public ITagCore Setting11 { get; set; }
        public ITagCore Setting12 { get; set; }
        public ITagCore LenhChuyenDon { get; set; }
        public ITagCore DaiCatChot { get; set; }
        public ITagCore STTChot { get; set; }
        public ITagCore SLDatChot { get; set; }
        public ITagCore SLLoiChot { get; set; }
        public ITagCore CheDoChuyenDon { get; set; }

        public CutterTags()
        {

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
                LenhChuyenDon.ValueChanged += LenhChuyenDon_ValueChanged;

            }
            if (STT1 != null)
            {
                STT1.ValueChanged += STT1_ValueChanged;
                STT1_ValueChanged(STT1, new TagValueChangedEventArgs("", STT1.Value));

                if (SLLoi != null)
                {
                    SLLoi.ValueChanged += SLLoi_ValueChanged;
                    SLLoi_ValueChanged(SLLoi, new TagValueChangedEventArgs("", SLLoi.Value));
                }

                if (DaiCat1 != null)
                {
                    DaiCat1.ValueChanged += DaiCat1_ValueChanged;
                    DaiCat1_ValueChanged(DaiCat1, new TagValueChangedEventArgs("", DaiCat1.Value));
                }
            }

            if (DoiDon != null)
            {
                DoiDon.ValueChanged += DoiDon_ValueChanged;
            }
        }

        private  void DoiDon_ValueChanged(object sender, TagValueChangedEventArgs e)
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
                                 MayMenTags.Instance.Setting1.Write("100");
                        }
                        else
                        {
                            if (MayMenTags.Instance.Setting1.Value != "0")
                                 MayMenTags.Instance.Setting1.Write("0");
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
            try
            {
                if (int.TryParse(e.NewValue, out int slLoi) && int.TryParse(DaiCat1.Value, out int dai))
                    Messenger.Default.Send(new MessageSLLoiMayCatThayDoi(STT1.Value, slLoi, dai));
            }
            catch { }
        }

        private void STT1_ValueChanged(object sender, TagValueChangedEventArgs e)
        {
            if ((sender as ITagCore).Quality == Quality.Good)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Messenger.Default.Send(new MessageSTTDonHangThayDoi(e.OldValue, e.NewValue));
                });
            }
        }

        private  void LenhChuyenDon_ValueChanged(object sender, TagValueChangedEventArgs e)
        {
            if (e.NewValue != "0" && (sender as ITagCore).Quality == Quality.Good)
            {
                var response = (sender as ITagCore).Write("0");
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
