using CommonControls;
using EasyDriverPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MayCatServer
{
    public class MayMenController
    {
        #region Singleton
        public static MayMenController Instance { get; } = new MayMenController();
        #endregion

        #region Constructors
        public MayMenController()
        {
        }
        #endregion

        #region Members
        List<WriteCommand> writeCommands1 = new List<WriteCommand>();
        List<WriteCommand> writeCommands2 = new List<WriteCommand>();
        List<WriteCommand> writeCommands3 = new List<WriteCommand>();
        List<WriteCommand> writeCommands4 = new List<WriteCommand>();
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        #endregion

        #region Public properties
        #endregion

        #region Public methods

        public void NapDanhSachDonHang(List<DonHang> donHangs, double heSoSong, bool napViTri1 = true, bool napDeSTT = false)
        {
            try
            {
                if (donHangs != null && MayMenTags.Instance.STT1 != null)
                {
                    if (long.TryParse(MayMenTags.Instance.STTMat1.Value, out long sttMat))
                    {
                        StartWrite(writeCommands1);

                        List<DonHang> donHangMat = donHangs.Where(x =>
                            x.STT >= sttMat).OrderBy(x => x.STT).ToList();
                        TinhToanGiayMat(writeCommands1, donHangMat, napViTri1);

                        if (long.TryParse(MayMenTags.Instance.STT1.Value, out long sttDon) &&
                            long.TryParse(CutterTags.Instance.STT1.Value, out long sttCutter))
                        {
                            long stt = sttDon;
                            if (sttCutter > sttDon)
                                stt = sttCutter;
                            var dhs = donHangs.OrderBy(x => x.STT).ToList();
                            DonHang donHang = dhs.FirstOrDefault(x => x.STT >= stt);
                            int index = dhs.IndexOf(donHang);
                            DonHang donhangKe = DonHang.Empty;
                            if (index + 1 < dhs.Count && index > -1)
                                donhangKe = dhs[index + 1];
                            if (donHang == null)
                                donHang = DonHang.Empty;

                            if (!string.IsNullOrWhiteSpace(donHang.Men))
                            {
                                long dai1 = donHang.Dai * donHang.SL;
                                long dai2 = donhangKe.Dai * donhangKe.SL;

                                if (napViTri1)
                                {
                                    if (SoSanhDonHang(dai1, donHang.STT, 1))
                                        NapDon(writeCommands1, dai1, donHang.STT, 1);
                                    //NapDon(writeCommands1, dai1, donHang.STT, 1);
                                }

                                if (!string.IsNullOrWhiteSpace(donhangKe.Men))
                                {
                                    if (SoSanhDonHang(dai2, donhangKe.STT, 2))
                                        NapDon(writeCommands1, dai2, donhangKe.STT, 2);
                                    //NapDon(writeCommands1, dai2, donhangKe.STT, 2);
                                }
                            }
                        }
                        CommitWrite(writeCommands1);
                    }

                    if (long.TryParse(MayMenTags.Instance.STT1.Value, out long sttDonHang))
                    {
                        NapDonHang(donHangs, sttDonHang, napViTri1);
                    }
                }
            }
            catch { }
        }

        //public void NapGiaySong(List<DonHang> donHangs, long sttSong, double heSoSong, bool napViTri1 = true)
        //{
        //    try
        //    {
        //        if (DriverConnector.IsStarted &&
        //            DriverConnector.ConnectionStatus == ConnectionStatus.Connected &&
        //            donHangs != null)
        //        {
        //            if (MayMenTags.Instance.STTSong1 != null)
        //            {
        //                if (long.TryParse(MayMenTags.Instance.STTSong1.Value, out long sttSong1))
        //                {
        //                    StartWrite(writeCommands2);
        //                    List<DonHang> donHangSong = donHangs.Where(x =>
        //                                                 x.HoanTatGiaySongE == 0 && x.STT >= sttSong && x.STT >= sttSong1).OrderBy(x => x.STT).ToList();
        //                    TinhToanGiaySong(writeCommands2, donHangSong, heSoSong, napViTri1);
        //                    CommitWrite(writeCommands2);
        //                }
        //            }
        //        }
        //    }
        //    catch { }
        //}

        public void NapGiayMat(List<DonHang> donHangs, long sttMat, bool napViTri1 = true)
        {
            try
            {

                if (donHangs != null)
                {
                    if (MayMenTags.Instance.STTMat1 != null)
                    {
                        if (long.TryParse(MayMenTags.Instance.STTMat1.Value, out long sttMat1))
                        {
                            StartWrite(writeCommands3);
                            List<DonHang> donHangMat = donHangs.Where(x =>
                                                        x.HoanTatMayMen == 0 && x.STT >= sttMat && x.STT >= sttMat1).OrderBy(x => x.STT).ToList();
                            TinhToanGiayMat(writeCommands3, donHangMat, napViTri1);
                            CommitWrite(writeCommands3);
                        }
                    }
                    
                }
            }
            catch { }
        }

        public void NapDonHang(List<DonHang> donHangs, long sttDon, bool napViTri1 = true)
        {
            if (long.TryParse(CutterTags.Instance.STT1.Value, out long sttCutter) &&
                long.TryParse(MayMenTags.Instance.STT1.Value, out long sttDon1))
            {
                StartWrite(writeCommands4);
                long stt = sttDon;
                if (sttCutter > sttDon)
                    stt = sttCutter;
                if (sttDon1 > stt)
                    stt = sttDon1;
                var dhs = donHangs.OrderBy(x => x.STT).ToList();
                DonHang donHang = dhs.FirstOrDefault(x => x.HoanTatSongE == 0 && x.STT >= stt && x.STT >= sttDon1);
                int index = dhs.IndexOf(donHang);

                DonHang donhangKe = DonHang.Empty;
                if (index + 1 < dhs.Count && index > -1)
                    donhangKe = dhs[index + 1];

                if (donhangKe != DonHang.Empty &&
                    (string.IsNullOrEmpty(donhangKe.Men)))
                {
                    if (index > -1)
                    {
                        for (int i = index + 1; i < dhs.Count; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(dhs[i].Men))
                            {
                                donhangKe = dhs[i];
                                break;
                            }
                        }
                    }
                }

                if (donHang == null)
                    donHang = DonHang.Empty;

                long dai1 = donHang.Dai * donHang.SL;
                long stt1 = donHang.STT;
                if (string.IsNullOrWhiteSpace(donHang.Men))
                {
                    dai1 = 0;
                    stt1 = 0;
                }

                if (napViTri1)
                {
                    if (SoSanhDonHang(dai1, stt1, 1))
                        NapDon(writeCommands4, dai1, stt1, 1);
                    //NapDon(writeCommands4, dai1, stt1, 1);
                }

                long dai2 = donhangKe.Dai * donhangKe.SL;
                long stt2 = donhangKe.STT;
                if (string.IsNullOrWhiteSpace(donhangKe.Men))
                {
                    dai2 = 0;
                    stt2 = 0;
                }
                if (SoSanhDonHang(dai2, stt2, 2))
                    NapDon(writeCommands4, dai2, stt2, 2);
                //NapDon(writeCommands4, dai2, stt2, 2);
                CommitWrite(writeCommands4);
            }
        }

        public void TinhToanGiayMat(List<WriteCommand> writeCommands, List<DonHang> donHangs, bool napViTri1 = true)
        {
            List<DonHang> donHangNap1 = new List<DonHang>();
            List<DonHang> donHangNap2 = new List<DonHang>();
            string khoGiayDonHang = string.Empty;
            bool khongCoGiay = false;
            int index = 0;

            for (int i = 0; i < donHangs.Count; i++)
            {
                string khoGiay = donHangs[i].Kho.ToString() + donHangs[i].Men;
                khongCoGiay = string.IsNullOrWhiteSpace(donHangs[i].Men);
                if (khongCoGiay)
                    break;
                if (string.IsNullOrEmpty(khoGiayDonHang))
                {
                    donHangNap1.Add(donHangs[i]);
                    khoGiayDonHang = khoGiay;
                }
                else
                {
                    if (khoGiay != khoGiayDonHang)
                    {
                        break;
                    }
                    else
                    {
                        donHangNap1.Add(donHangs[i]);
                    }
                }
                index = i;
            }
            if (!khongCoGiay)
            {
                index++;
                khoGiayDonHang = string.Empty;
                for (int i = index; i < donHangs.Count; i++)
                {
                    string khoGiay = donHangs[i].Kho.ToString() + donHangs[i].Men;
                    khongCoGiay = string.IsNullOrWhiteSpace(donHangs[i].Men);
                    if (khongCoGiay)
                        break;
                    if (string.IsNullOrEmpty(khoGiayDonHang))
                    {
                        donHangNap2.Add(donHangs[i]);
                        khoGiayDonHang = khoGiay;
                    }
                    else
                    {
                        if (khoGiay != khoGiayDonHang)
                        {
                            break;
                        }
                        else
                        {
                            donHangNap2.Add(donHangs[i]);
                        }
                    }
                }
            }
            else
            {

            }

            bool canTinhChieuDaiDon2 = false;
            DonHang dhBatDau2 = DonHang.Empty;
            if (donHangNap2.Count == 0)
            {
                string giayTruocDo = donHangNap1.Count > 0 ? donHangNap1[0].Men : string.Empty;
                string giayMatDon1 = giayTruocDo;
                for (int i = index + 1; i < donHangs.Count; i++)
                {
                    if (!string.IsNullOrEmpty(donHangs[i].Men))
                    {
                        if (string.IsNullOrEmpty(giayMatDon1))
                        {
                            canTinhChieuDaiDon2 = true;
                            dhBatDau2 = donHangs[i];
                            break;
                        }
                        else
                        {
                            if (giayMatDon1 != donHangs[i].Men)
                            {
                                canTinhChieuDaiDon2 = true;
                                dhBatDau2 = donHangs[i];
                                break;
                            }
                            else
                            {
                                if (giayTruocDo != giayMatDon1)
                                {
                                    canTinhChieuDaiDon2 = true;
                                    dhBatDau2 = donHangs[i];
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        giayTruocDo = donHangs[i].Men;
                    }
                }
            }
            long daiMat = 0;
            long daiSong = 0;
            if (canTinhChieuDaiDon2)
                Repository.Instance.TinhTongChieuDaiGiay(donHangs, dhBatDau2, "m", out daiSong, out daiMat);

            foreach (var item in donHangNap1)
            {
                if (item.Id > 0 && item.TGBatDau == DateTime.MinValue)
                {
                    item.TGBatDau = DateTime.Now;
                    Repository.Instance.UpdateColumn("dhdangchay", "TGBatDau", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), $"where Id = {item.Id}");
                }
            }

            long dai1 = donHangNap1.Sum(x => x.Dai * x.SL);
            long stt1 = 0;
            if (donHangNap1.FirstOrDefault() is DonHang dh1)
                stt1 = dh1.STT;

            long dai2 = donHangNap2.Sum(x => x.Dai * x.SL);
            long stt2 = 0;
            if (donHangNap2.FirstOrDefault() is DonHang dh2)
                stt2 = dh2.STT;

            if (napViTri1)
            {
                if (SoSanhGiayMat(dai1, stt1, 1))
                    NapDonGiayMat(writeCommands, dai1, stt1, 1);
            }

            if (!canTinhChieuDaiDon2)
            {
                if (SoSanhGiayMat(dai2, stt2, 2))
                    NapDonGiayMat(writeCommands, dai2, stt2, 2);
                //NapDonGiayMat(writeCommands, dai2, stt2, 2);
            }
            else
            {
                if (dhBatDau2 != null && dhBatDau2.STT != 0)
                {
                    if (SoSanhGiayMat(daiMat, dhBatDau2.STT, 2))
                        NapDonGiayMat(writeCommands, daiMat, dhBatDau2.STT, 2);
                    //NapDonGiayMat(writeCommands, daiMat, dhBatDau2.STT, 2);
                }
            }
        }

        public int NapDonGiayMat(List<WriteCommand> writeCommands, long dai, long stt, int viTri = 1, int soLanThuToiDa = 1)
        {
            try
            {
                if (soLanThuToiDa < 1)
                    soLanThuToiDa = 1;
                string prefix = MayMenTags.Instance.StationName + "/" +
                    MayMenTags.Instance.ChannelName + "/" +
                    MayMenTags.Instance.DeviceName + "/";

                string tagDai = prefix + "ChieuDaiMat" + viTri;
                string tagSTT = prefix + "STTMat" + viTri;

                if (viTri >= 1 && viTri <= 2)
                {
                    writeCommands.Add(new WriteCommand() { PathToTag = tagDai, Value = dai.ToString() });
                    if (viTri == 1 && stt == 0)
                    {
                    }
                    else
                    {
                        writeCommands.Add(new WriteCommand() { PathToTag = tagSTT, Value = stt.ToString() });
                    }

                    return 1;
                }
            }
            catch { }
            return 0;
        }

        public int NapDon(List<WriteCommand> writeCommands, long dai, long stt, int viTri = 1, int soLanThuToiDa = 1)
        {
            try
            {
                if (soLanThuToiDa < 1)
                    soLanThuToiDa = 1;
                string prefix = MayMenTags.Instance.StationName + "/" +
                    MayMenTags.Instance.ChannelName + "/" +
                    MayMenTags.Instance.DeviceName + "/";

                string tagDai = prefix + "ChieuDaiDon" + viTri;
                string tagSTT = prefix + "STT" + viTri;

                if (viTri >= 1 && viTri <= 2)
                {
                    writeCommands.Add(new WriteCommand() { PathToTag = tagDai, Value = dai.ToString() });
                    writeCommands.Add(new WriteCommand() { PathToTag = tagSTT, Value = stt.ToString() });
                    return 1;
                }
            }
            catch { }
            return 0;
        }

        public void StartWrite(List<WriteCommand> writeCommands)
        {
            writeCommands.Clear();
        }


        public void CommitWrite(List<WriteCommand> writeCommands, int soLanThuToiDa = 1)
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

        public bool SoSanhGiayMat(long dai, long stt, int viTri = 1)
        {
            if (viTri >= 1 && viTri <= 2)
            {
                ITagCore DaiSongTag = null;
                ITagCore STTSongTag = null;

                string daiStr = dai.ToString();
                string sttStr = stt.ToString();

                switch (viTri)
                {
                    case 1:
                        DaiSongTag = MayMenTags.Instance.ChieuDaiMat1;
                        STTSongTag = MayMenTags.Instance.STTMat1;
                        break;
                    case 2:
                        DaiSongTag = MayMenTags.Instance.ChieuDaiMat2;
                        STTSongTag = MayMenTags.Instance.STTMat2;
                        break;
                    default:
                        break;
                }

                int compareValue = 0;

                if (DaiSongTag == null)
                    return false;
                else
                {
                    if (DaiSongTag.Value != daiStr)
                        compareValue++;
                }

                if (STTSongTag == null)
                    return false;
                else
                {
                    if (STTSongTag.Value != sttStr)
                        compareValue++;
                }

                if (compareValue != 0)
                    return true;
            }
            return false;
        }

        public bool SoSanhDonHang(long dai, long stt, int viTri = 1)
        {
            if (viTri >= 1 && viTri <= 2)
            {
                ITagCore DaiSongTag = null;
                ITagCore STTSongTag = null;

                string daiStr = dai.ToString();
                string sttStr = stt.ToString();

                switch (viTri)
                {
                    case 1:
                        DaiSongTag = MayMenTags.Instance.ChieuDaiDon1;
                        STTSongTag = MayMenTags.Instance.STT1;
                        break;
                    case 2:
                        DaiSongTag = MayMenTags.Instance.ChieuDaiDon2;
                        STTSongTag = MayMenTags.Instance.STT2;
                        break;
                    default:
                        break;
                }

                int compareValue = 0;

                if (DaiSongTag == null)
                    return false;
                else
                {
                    if (DaiSongTag.Value != daiStr)
                        compareValue++;
                }

                if (STTSongTag == null)
                    return false;
                else
                {
                    if (STTSongTag.Value != sttStr)
                        compareValue++;
                }

                if (compareValue != 0)
                    return true;
            }
            return false;
        }

        #endregion
    }
}
