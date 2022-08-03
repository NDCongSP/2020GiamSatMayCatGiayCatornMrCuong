using CommonControls;
using DevExpress.Mvvm;
using EasyDriver.Core;
using EasyDriverPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MayCatServer
{
    public class CutterController
    {
        #region Singleton
        public static CutterController Instance { get; } = new CutterController();
        #endregion

        #region Constructors
        public CutterController()
        {
            task = Task.Factory.StartNew(KiemTraThongBaoChuyenDon, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
        #endregion

        #region Members
        List<WriteCommand> writeCommands = new List<WriteCommand>();
        private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private Task task;
        #endregion

        #region Public properties
        public bool ThongBaoChuanBiGiaySongE { get; set; }
        public bool ThongBaoChuanBiGiaySongB { get; set; }
        public bool ThongBaoChuanBiGiaySongC { get; set; }
        public bool ThongBaoNapDonSongE { get; set; }
        public bool ThongBaoNapDonSongB { get; set; }
        public bool ThongBaoNapDonSongC { get; set; }
        #endregion

        #region Public methods
        public void NapDanhSachDonHang(List<DonHang> donHangs, bool napViTri1, bool napViTri2)
        {
            semaphore.Wait();
            try
            {
                if (donHangs != null)
                {
                    if (long.TryParse(CutterTags.Instance.STT1.Value, out long stt))
                    {
                        // CutterTags.Instance.LenhChuyenDon?.Write("0");
                        // Lọc các đơn chưa hoàn thành cutter và có stt lớn hơn STT đang chạy
                        List<DonHang> DanhSachNap = donHangs.Where(x => 
                            x.STT >= stt).OrderBy(x => x.STT).ToList();

                        StartWrite();

                        // Nạp đơn hàng 1
                        if (napViTri1)
                        {
                            if (DanhSachNap.Count > 0)
                            {
                                if (SoSanhDonHang(DanhSachNap[0], 1))
                                    NapDon(DanhSachNap[0], 1);
                                //NapDon(DanhSachNap[0], 1);
                            }
                            else
                            {
                                if (SoSanhDonHang(DonHang.Empty, 1))
                                    NapDon(DonHang.Empty, 1);
                                //NapDon(DonHang.Empty, 1);
                            }
                        }

                        // Nạp đơn hàng 2
                        if (napViTri2)
                        {
                            if (DanhSachNap.Count > 1)
                            {
                                if (SoSanhDonHang(DanhSachNap[1], 2))
                                    NapDon(DanhSachNap[1], 2);
                                //NapDon(DanhSachNap[1], 2);
                            }
                            else
                            {
                                if (SoSanhDonHang(DonHang.Empty, 2))
                                    NapDon(DonHang.Empty, 2);
                                //NapDon(DonHang.Empty, 2);
                            }
                        }

                        // Nạp đơn hàng 3
                        if (DanhSachNap.Count > 2)
                        {
                            if (SoSanhDonHang(DanhSachNap[2], 3))
                                NapDon(DanhSachNap[2], 3);
                            //NapDon(DanhSachNap[2], 3);
                        }
                        else
                        {
                            if (SoSanhDonHang(DonHang.Empty, 3))
                                NapDon(DonHang.Empty, 3);
                            //NapDon(DonHang.Empty, 3);
                        }


                        CommitWrite();
                    }
                }
            }
            catch { }
            finally { semaphore.Release(); }
        }

        public void StartWrite()
        {
            writeCommands.Clear();
        }

        public void CommitWrite(int soLanThuToiDa = 1)
        {
            if (writeCommands.Count > 0)
            {
                for (int i = 0; i < soLanThuToiDa; i++)
                {
                    int count = 0;
                    foreach (var item in writeCommands)
                    {
                        var res = WriteTagExtensions.Write(item.PathToTag, item.Value);
                        if (res == Quality.Good)
                            count++;
                    }
                    if (count == writeCommands.Count)
                        break;
                    else
                        continue;
                }
            }
        
        }

        public int NapDon(DonHang dh, int viTri = 1, int soLanThuToiDa = 1)
        {
            if (dh != null)
            {
                try
                {
                    if (soLanThuToiDa < 1)
                        soLanThuToiDa = 1;
                    string prefix = CutterTags.Instance.StationName + "/" +
                        CutterTags.Instance.ChannelName + "/" +
                        CutterTags.Instance.DeviceName + "/";

                    string tagDaiCat = prefix + "DaiCat" + viTri;
                    string tagSL = prefix + "SLCat" + viTri;
                    string tagPallet = prefix + "Pallet" + viTri;
                    string tagSTT = prefix + "STT" + viTri;

                    if (viTri >= 1 && viTri <= 3)
                    {
                        if (dh.Id > 0 && dh.TGBatDau == DateTime.MinValue && viTri == 1)
                        {
                            dh.TGBatDau = DateTime.Now;
                            Repository.Instance.UpdateColumn("dhdangchay", "TGBatDau", dh.TGBatDau.ToString("yyyy-MM-dd HH:mm:ss"), $"where Id = {dh.Id}");
                        }
                        
                        if (viTri == 1 && dh.STT == 0)
                        {
                        }
                        else
                        {
                            writeCommands.Add(new WriteCommand() { PathToTag = tagSTT, Value = dh.STT.ToString() });
                        }
                        writeCommands.Add(new WriteCommand() { PathToTag = tagDaiCat, Value = dh.Dai.ToString() });
                        writeCommands.Add(new WriteCommand() { PathToTag = tagSL, Value = dh.SL.ToString() });
                        writeCommands.Add(new WriteCommand() { PathToTag = tagPallet, Value = dh.Pallet.ToString() });
                        return 1;
                    }
                }
                catch { }
            }
            return 0;
        }

        public bool SoSanhDonHang(DonHang dh, int viTri)
        {
            if (viTri >= 1 && viTri <= 3 && dh != null)
            {
                string stt = dh.STT.ToString();
                string daiCat = dh.Dai.ToString();
                string sl = dh.SL.ToString();
                string pallet = dh.Pallet.ToString();

                ITagCore sttTag = null;
                ITagCore daiCatTag = null;
                ITagCore slTag = null;
                ITagCore palletTag = null;

                switch (viTri)
                {
                    case 1:
                        sttTag = CutterTags.Instance.STT1;
                        daiCatTag = CutterTags.Instance.DaiCat1;
                        slTag = CutterTags.Instance.SLCat1;
                        palletTag = CutterTags.Instance.Pallet1;
                        break;
                    case 2:
                        sttTag = CutterTags.Instance.STT2;
                        daiCatTag = CutterTags.Instance.DaiCat2;
                        slTag = CutterTags.Instance.SLCat2;
                        palletTag = CutterTags.Instance.Pallet2;
                        break;
                    case 3:
                        sttTag = CutterTags.Instance.STT3;
                        daiCatTag = CutterTags.Instance.DaiCat3;
                        slTag = CutterTags.Instance.SLCat3;
                        palletTag = CutterTags.Instance.Pallet3;
                        break;
                    default:
                        break;
                }

                int compareValue = 0;
                if (sttTag == null)
                    return false;
                else
                {
                    if (sttTag.Value != stt)
                        compareValue++;
                }

                if (daiCatTag == null)
                    return false;
                else
                {
                    if (daiCatTag.Value != daiCat)
                        compareValue++;
                }

                if (slTag == null)
                    return false;
                else
                {
                    if (slTag.Value != sl)
                        compareValue++;
                }

                if (palletTag == null)
                    return false;
                else
                {
                    if (palletTag.Value != pallet)
                        compareValue++;
                }

                if (compareValue != 0)
                    return true;
                
            }
            return false;
        }
        #endregion

        #region Private methods
        private void KiemTraThongBaoChuyenDon()
        {
            while(true)
            {
                try
                {
                    if (CutterTags.Instance.DoiDon != null && CutterTags.Instance.DoiDon.Quality == Quality.Good)
                    {
                        if (double.TryParse(CutterTags.Instance.DoiDon.Value, out double doiDonCutter))
                        {
                            double conlai = doiDonCutter / 1000;
                            string sttMayCat = CutterTags.Instance.STT1.Value;
                            DonHang dhHienTai = MainWindow.DonHangDataSource.FirstOrDefault(x => x.STT.ToString() == sttMayCat);
                            int indexDonHangMayCatHienTai = MainWindow.DonHangDataSource.IndexOf(dhHienTai);
                            DonHang dhKeTiep = null;
                            if (indexDonHangMayCatHienTai + 1 < MainWindow.DonHangDataSource.Count)
                                dhKeTiep = MainWindow.DonHangDataSource[indexDonHangMayCatHienTai + 1];

                            if (dhHienTai != null && dhKeTiep != null)
                            {
                                // Thông báo chuẩn bị giấy sóng E
                                if (!ThongBaoChuanBiGiaySongE)
                                {
                                    string giayHienTai = dhHienTai.GiaySongE + dhHienTai.GiayMatE;
                                    string giayKeTiep = dhKeTiep.GiaySongE + dhKeTiep.GiayMatE;
                                    if (!string.IsNullOrWhiteSpace(giayKeTiep) &&
                                        string.IsNullOrWhiteSpace(giayHienTai) &&
                                        giayHienTai != giayKeTiep &&
                                        conlai <= MainWindow.CaiDat.SoMetBaoChuanBiGiaySongE &&
                                        conlai > MainWindow.CaiDat.SoMetBaoChuyenDonSongE)
                                    {
                                        ThongBaoChuanBiGiaySongE = true;
                                        ThongBaoNapDonSongE = false;
                                        Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            Messenger.Default.Send(new MessageChuanBiGiay("SongE", dhKeTiep));
                                        });
                                        
                                    }
                                }
                                else
                                {
                                    if (conlai > MainWindow.CaiDat.SoMetBaoChuanBiGiaySongE)
                                    {
                                        ThongBaoChuanBiGiaySongE = false;
                                    }
                                }

                                // Thông báo chuẩn bị giấy sóng B
                                if (!ThongBaoChuanBiGiaySongB)
                                {
                                    string giayHienTai = dhHienTai.GiaySongB + dhHienTai.GiayMatB;
                                    string giayKeTiep = dhKeTiep.GiaySongB + dhKeTiep.GiayMatB;
                                    if (!string.IsNullOrWhiteSpace(giayKeTiep) &&
                                        string.IsNullOrWhiteSpace(giayHienTai) &&
                                        giayHienTai != giayKeTiep &&
                                        conlai <= MainWindow.CaiDat.SoMetBaoChuanBiGiaySongB &&
                                        conlai > MainWindow.CaiDat.SoMetBaoChuyenDonSongB)
                                    {
                                        ThongBaoChuanBiGiaySongB = true;
                                        ThongBaoNapDonSongB = false;
                                        Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            Messenger.Default.Send(new MessageChuanBiGiay("SongB", dhKeTiep));
                                        });
                                    }
                                }
                                else
                                {
                                    if (conlai > MainWindow.CaiDat.SoMetBaoChuanBiGiaySongB)
                                        ThongBaoChuanBiGiaySongB = false;
                                }

                                // Thông báo chuẩn bị giấy sóng C
                                if (!ThongBaoChuanBiGiaySongC)
                                {
                                    string giayHienTai = dhHienTai.GiaySongC + dhHienTai.GiayMatC;
                                    string giayKeTiep = dhKeTiep.GiaySongC + dhKeTiep.GiayMatC;
                                    if (!string.IsNullOrWhiteSpace(giayKeTiep) &&
                                        string.IsNullOrWhiteSpace(giayHienTai) &&
                                        giayHienTai != giayKeTiep &&
                                        conlai <= MainWindow.CaiDat.SoMetBaoChuanBiGiaySongC &&
                                        conlai > MainWindow.CaiDat.SoMetBaoChuyenDonSongC)
                                    {
                                        ThongBaoChuanBiGiaySongC = true;
                                        ThongBaoNapDonSongC = false;
                                        Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            Messenger.Default.Send(new MessageChuanBiGiay("SongC", dhKeTiep));
                                        });
                                    }
                                }
                                else
                                {
                                    if (conlai > MainWindow.CaiDat.SoMetBaoChuanBiGiaySongC)
                                        ThongBaoChuanBiGiaySongC = false;
                                }

                                // Thông báo nạp đơn sóng E
                                if (!ThongBaoNapDonSongE)
                                {
                                    string giayHienTai = dhHienTai.GiaySongE + dhHienTai.GiayMatE;
                                    string giayKeTiep = dhKeTiep.GiaySongE + dhKeTiep.GiayMatE;
                                    if (!string.IsNullOrWhiteSpace(giayKeTiep) &&
                                        string.IsNullOrWhiteSpace(giayHienTai) &&
                                        giayHienTai != giayKeTiep &&
                                        conlai <= MainWindow.CaiDat.SoMetBaoChuyenDonSongE)
                                    {
                                        ThongBaoNapDonSongE = true;
                                        ThongBaoChuanBiGiaySongE = false;
                                        Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            Messenger.Default.Send(new MessageNapDonMaySong("SongE", dhHienTai, dhKeTiep));
                                        });
                                    }
                                }
                                else
                                {
                                    if (conlai > MainWindow.CaiDat.SoMetBaoChuyenDonSongE)
                                        ThongBaoNapDonSongE = false;
                                }

                                // Thông báo nạp đơn sóng B
                                if (!ThongBaoNapDonSongB)
                                {
                                    string giayHienTai = dhHienTai.GiaySongB + dhHienTai.GiayMatB;
                                    string giayKeTiep = dhKeTiep.GiaySongB + dhKeTiep.GiayMatB;
                                    if (!string.IsNullOrWhiteSpace(giayKeTiep) &&
                                        string.IsNullOrWhiteSpace(giayHienTai) &&
                                        giayHienTai != giayKeTiep &&
                                        conlai <= MainWindow.CaiDat.SoMetBaoChuyenDonSongB)
                                    {
                                        ThongBaoNapDonSongB = true;
                                        ThongBaoChuanBiGiaySongB = false;
                                        Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            Messenger.Default.Send(new MessageNapDonMaySong("SongB", dhHienTai, dhKeTiep));
                                        });
                                    }
                                }
                                else
                                {
                                    if (conlai > MainWindow.CaiDat.SoMetBaoChuyenDonSongB)
                                        ThongBaoNapDonSongB = false;
                                }

                                // Thông báo nạp đơn sóng C
                                if (!ThongBaoNapDonSongC)
                                {
                                    string giayHienTai = dhHienTai.GiaySongC + dhHienTai.GiayMatC;
                                    string giayKeTiep = dhKeTiep.GiaySongC + dhKeTiep.GiayMatC;
                                    if (!string.IsNullOrWhiteSpace(giayKeTiep) &&
                                        string.IsNullOrWhiteSpace(giayHienTai) &&
                                        giayHienTai != giayKeTiep &&
                                        conlai <= MainWindow.CaiDat.SoMetBaoChuyenDonSongC)
                                    {
                                        ThongBaoNapDonSongC = true;
                                        ThongBaoChuanBiGiaySongC = false;
                                        Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            Messenger.Default.Send(new MessageNapDonMaySong("SongC", dhHienTai, dhKeTiep));
                                        });
                                    }
                                }
                                else
                                {
                                    if (conlai > MainWindow.CaiDat.SoMetBaoChuyenDonSongC)
                                        ThongBaoNapDonSongC = false;
                                }
                            }
                            else
                            {
                                ThongBaoChuanBiGiaySongE = false;
                                ThongBaoChuanBiGiaySongB = false;
                                ThongBaoChuanBiGiaySongC = false;
                                ThongBaoNapDonSongE = false;
                                ThongBaoNapDonSongB = false;
                                ThongBaoNapDonSongC = false;
                            }
                        }
                    }
                }
                catch { }
                finally {  Thread.Sleep(100); }
            }
        }
        #endregion
    }
}
