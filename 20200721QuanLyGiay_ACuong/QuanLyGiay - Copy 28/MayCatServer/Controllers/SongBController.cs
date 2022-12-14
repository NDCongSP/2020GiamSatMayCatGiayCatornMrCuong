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
    public class SongBController
    {
        #region Singleton
        public static SongBController Instance { get; } = new SongBController();
        #endregion

        #region Constructors
        public SongBController()
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

        public void NapDanhSachDonHang(List<DonHang> donHangs, double heSoSong, string context, bool napViTri1 = true, long sttBatDau = 0)
        {
            try
            {
                if (donHangs != null && SongBTags.Instance.STT2.Quality == Quality.Good)
                {
                    if (long.TryParse(SongBTags.Instance.STTSong1.Value, out long sttSong))
                    {
                        StartWrite(writeCommands1);
                        List<DonHang> donHangSong = new List<DonHang>();
                        if (sttBatDau <= 0)
                        {
                            donHangSong = donHangs.Where(x =>
                                 x.STT >= sttSong).OrderBy(x => x.STT).ToList();
                        }
                        else
                        {
                            if (sttSong > sttBatDau)
                                sttSong = sttBatDau;
                            donHangSong = donHangs.Where(x =>
                                x.STT >= sttSong).OrderBy(x => x.STT).ToList();
                        }

                        TinhToanGiaySong(writeCommands1, donHangSong, heSoSong, napViTri1);

                        if (long.TryParse(SongBTags.Instance.STTMat1.Value, out long sttMat))
                        {
                            List<DonHang> donHangMat = new List<DonHang>();
                            if (sttBatDau <= 0)
                            {
                                donHangMat = donHangs.Where(x =>
                                     x.STT >= sttMat).OrderBy(x => x.STT).ToList();
                            }
                            else
                            {
                                if (sttMat > sttBatDau)
                                    sttMat = sttBatDau;

                                donHangMat = donHangs.Where(x =>
                                    x.STT >= sttMat).OrderBy(x => x.STT).ToList();
                            }
                            TinhToanGiayMat(writeCommands1, donHangMat, napViTri1);

                            if (long.TryParse(SongBTags.Instance.STT1.Value, out long sttDon) &&
                                long.TryParse(CutterTags.Instance.STT1.Value, out long sttCutter))
                            {
                                long stt = sttDon;
                                if (sttCutter > sttDon)
                                    stt = sttCutter;
                                var dhs = donHangs.OrderBy(x => x.STT).ToList();
                                DonHang donHang = new DonHang();
                                if (sttBatDau <= 0)
                                {
                                    donHang = dhs.FirstOrDefault(x => x.STT >= stt);
                                }
                                else
                                {
                                    if (stt > sttBatDau)
                                        stt = sttBatDau;
                                    donHang = dhs.FirstOrDefault(x => x.STT >= stt);
                                }
                                int index = dhs.IndexOf(donHang);
                                DonHang donhangKe = DonHang.Empty;
                                if (index + 1 < dhs.Count && index > -1)
                                    donhangKe = dhs[index + 1];
                                if (donHang == null)
                                    donHang = DonHang.Empty;

                                if (!string.IsNullOrWhiteSpace(donHang.GiaySongB) &&
                                    !string.IsNullOrWhiteSpace(donHang.GiayMatB))
                                {
                                    long dai1 = donHang.Dai * donHang.SL;
                                    long dai2 = donhangKe.Dai * donhangKe.SL;

                                    if (napViTri1)
                                    {
                                        if (SoSanhDonHang(dai1, donHang.STT, 1))
                                            NapDon(writeCommands1, dai1, donHang.STT, 1);
                                        //NapDon(writeCommands1, dai1, donHang.STT, 1);
                                    }

                                    if (!string.IsNullOrWhiteSpace(donhangKe.GiaySongB) &&
                                        !string.IsNullOrWhiteSpace(donhangKe.GiayMatB))
                                    {
                                        if (SoSanhDonHang(dai2, donhangKe.STT, 2))
                                            NapDon(writeCommands1, dai2, donhangKe.STT, 2);
                                        //NapDon(writeCommands1, dai2, donhangKe.STT, 2);
                                    }
                                }
                            }
                        }

                        CommitWrite(writeCommands1, context, "Nap danh sach don hang");
                    }

                    if (long.TryParse(SongBTags.Instance.STT1.Value, out long sttDonHang))
                    {
                        NapDonHang(donHangs, sttDonHang, context, napViTri1);
                    }
                }
            }
            catch { }
        }

        public void NapGiaySong(List<DonHang> donHangs, long sttSong, double heSoSong, string context, bool napViTri1 = true)
        {
            try
            {
                if (donHangs != null && SongBTags.Instance.STT2.Quality == Quality.Good)
                {
                    if (SongBTags.Instance.STTSong1 != null)
                    {
                        if (long.TryParse(SongBTags.Instance.STTSong1.Value, out long sttSong1))
                        {
                            StartWrite(writeCommands2);
                            List<DonHang> donHangSong = donHangs.Where(x =>
                                                         x.HoanTatGiaySongB == 0 && x.STT >= sttSong && x.STT >= sttSong1).OrderBy(x => x.STT).ToList();
                            TinhToanGiaySong(writeCommands2, donHangSong, heSoSong, napViTri1);
                            CommitWrite(writeCommands2, context, "Nap giay song");
                        }
                    }
                }
            }
            catch { }
        }

        public void NapGiayMat(List<DonHang> donHangs, long sttMat, string context, bool napViTri1 = true)
        {
            try
            {

                if (donHangs != null && SongBTags.Instance.STT2.Quality == Quality.Good)
                {
                    if (SongBTags.Instance.STTMat1 != null)
                    {
                        if (long.TryParse(SongBTags.Instance.STTMat1.Value, out long sttMat1))
                        {
                            StartWrite(writeCommands3);
                            List<DonHang> donHangMat = donHangs.Where(x =>
                                                        x.HoanTatGiayMatB == 0 && x.STT >= sttMat && x.STT >= sttMat1).OrderBy(x => x.STT).ToList();
                            TinhToanGiayMat(writeCommands3, donHangMat, napViTri1);
                            CommitWrite(writeCommands3, context, "Nap giay mat");
                        }
                    }

                }
            }
            catch { }
        }

        public void NapDonHang(List<DonHang> donHangs, long sttDon, string context, bool napViTri1 = true)
        {
            if (long.TryParse(CutterTags.Instance.STT1.Value, out long sttCutter) &&
                long.TryParse(SongBTags.Instance.STT1.Value, out long sttDon1) && SongBTags.Instance.STT2.Quality == Quality.Good)
            {
                StartWrite(writeCommands4);
                long stt = sttDon;
                if (sttCutter > sttDon)
                    stt = sttCutter;
                if (sttDon1 > stt)
                    stt = sttDon1;
                var dhs = donHangs.OrderBy(x => x.STT).ToList();
                DonHang donHang = dhs.FirstOrDefault(x => x.HoanTatSongB == 0 && x.STT >= stt && x.STT >= sttDon1);
                int index = dhs.IndexOf(donHang);

                DonHang donhangKe = DonHang.Empty;
                if (index + 1 < dhs.Count && index > -1)
                    donhangKe = dhs[index + 1];

                if (donhangKe != DonHang.Empty &&
                    (string.IsNullOrEmpty(donhangKe.GiaySongB) || string.IsNullOrEmpty(donhangKe.GiayMatB)))
                {
                    if (index > -1)
                    {
                        for (int i = index + 1; i < dhs.Count; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(dhs[i].GiaySongB) &&
                                !string.IsNullOrWhiteSpace(dhs[i].GiayMatB))
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
                if (string.IsNullOrWhiteSpace(donHang.GiaySongB) ||
                    string.IsNullOrWhiteSpace(donHang.GiayMatB))
                {
                    dai1 = 0;
                    stt1 = 0;
                }

                //if (napViTri1)
                //{
                //    if (SoSanhDonHang(dai1, stt1, 1))
                //        NapDon(writeCommands4, dai1, stt1, 1);
                //    //NapDon(writeCommands4, dai1, stt1, 1);
                //}

                if (SoSanhDonHang(dai1, stt1, 1))
                    NapDon(writeCommands4, dai1, stt1, 1);

                long dai2 = donhangKe.Dai * donhangKe.SL;
                long stt2 = donhangKe.STT;
                if (string.IsNullOrWhiteSpace(donhangKe.GiaySongB) ||
                    string.IsNullOrWhiteSpace(donhangKe.GiayMatB))
                {
                    dai2 = 0;
                    stt2 = 0;
                }
                if (SoSanhDonHang(dai2, stt2, 2))
                    NapDon(writeCommands4, dai2, stt2, 2);
                //NapDon(writeCommands4, dai2, stt2, 2);
                CommitWrite(writeCommands4, context, "Nap don hang");
            }
        }

        public void TinhToanGiaySong(List<WriteCommand> writeCommands, List<DonHang> donHangs, double heSoSong, bool napViTri1 = true)
        {
            List<DonHang> donHangNap1 = new List<DonHang>();
            List<DonHang> donHangNap2 = new List<DonHang>();
            string khoGiayDonHang = string.Empty;
            bool khongCoGiay = false;
            int index = 0;

            for (int i = 0; i < donHangs.Count; i++)
            {
                string khoGiay = donHangs[i].Kho.ToString() + donHangs[i].GiaySongB;
                khongCoGiay = string.IsNullOrWhiteSpace(donHangs[i].GiaySongB);
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
                    string khoGiay = donHangs[i].Kho.ToString() + donHangs[i].GiaySongB;
                    khongCoGiay = string.IsNullOrWhiteSpace(donHangs[i].GiaySongB);
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
                int khoTruocDo = donHangNap1.Count > 0 ? donHangNap1[0].Kho : 0;
                string giayTruocDo = donHangNap1.Count > 0 ? donHangNap1[0].GiaySongB : string.Empty;
                string giaySongDon1 = giayTruocDo;
                for (int i = index + 1; i < donHangs.Count; i++)
                {
                    if (!string.IsNullOrEmpty(donHangs[i].GiaySongB))
                    {
                        if (string.IsNullOrEmpty(giaySongDon1))
                        {
                            canTinhChieuDaiDon2 = true;
                            dhBatDau2 = donHangs[i];
                            break;
                        }
                        else
                        {
                            if (giaySongDon1 != donHangs[i].GiaySongB)
                            {
                                canTinhChieuDaiDon2 = true;
                                dhBatDau2 = donHangs[i];
                                break;
                            }
                            else
                            {
                                if (giayTruocDo != giaySongDon1)
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
                        giayTruocDo = donHangs[i].GiaySongB;
                    }
                }
            }

            long daiMat = 0;
            long daiSong = 0;
            if (canTinhChieuDaiDon2)
                Repository.Instance.TinhTongChieuDaiGiay(donHangs, dhBatDau2, "b", out daiSong, out daiMat);

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

            dai1 = Convert.ToInt64(heSoSong * dai1);
            dai2 = Convert.ToInt64(heSoSong * dai2);
            daiSong = Convert.ToInt64(heSoSong * daiSong);

            if (napViTri1)
            {
                if (SoSanhGiaySong(dai1, stt1, 1))
                    NapDonGiaySong(writeCommands, dai1, stt1, 1);
            }

            if (!canTinhChieuDaiDon2)
            {
                if (SoSanhGiaySong(dai2, stt2, 2))
                    NapDonGiaySong(writeCommands, dai2, stt2, 2);
                // NapDonGiaySong(writeCommands, dai2, stt2, 2);
            }
            else
            {
                if (dhBatDau2 != null && dhBatDau2.STT != 0)
                {
                    if (SoSanhGiaySong(daiSong, dhBatDau2.STT, 2))
                        NapDonGiaySong(writeCommands, daiSong, dhBatDau2.STT, 2);
                    //NapDonGiaySong(writeCommands, daiSong, dhBatDau2.STT, 2);
                }
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
                string khoGiay = donHangs[i].Kho.ToString() + donHangs[i].GiayMatB;
                khongCoGiay = string.IsNullOrWhiteSpace(donHangs[i].GiayMatB);
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
                    string khoGiay = donHangs[i].Kho.ToString() + donHangs[i].GiayMatB;
                    khongCoGiay = string.IsNullOrWhiteSpace(donHangs[i].GiayMatB);
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
                string giayTruocDo = donHangNap1.Count > 0 ? donHangNap1[0].GiayMatB : string.Empty;
                string giayMatDon1 = giayTruocDo;
                for (int i = index + 1; i < donHangs.Count; i++)
                {
                    if (!string.IsNullOrEmpty(donHangs[i].GiayMatB))
                    {
                        if (string.IsNullOrEmpty(giayMatDon1))
                        {
                            canTinhChieuDaiDon2 = true;
                            dhBatDau2 = donHangs[i];
                            break;
                        }
                        else
                        {
                            if (giayMatDon1 != donHangs[i].GiayMatB)
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
                        giayTruocDo = donHangs[i].GiayMatB;
                    }
                }
            }
            long daiMat = 0;
            long daiSong = 0;
            if (canTinhChieuDaiDon2)
                Repository.Instance.TinhTongChieuDaiGiay(donHangs, dhBatDau2, "b", out daiSong, out daiMat);

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

        public int NapDonGiaySong(List<WriteCommand> writeCommands, long dai, long stt, int viTri = 1, int soLanThuToiDa = 1)
        {
            try
            {
                if (soLanThuToiDa < 1)
                    soLanThuToiDa = 1;
                string prefix = SongBTags.Instance.StationName + "/" +
                    SongBTags.Instance.ChannelName + "/" +
                    SongBTags.Instance.DeviceName + "/";

                string tagDai = prefix + "ChieuDaiSong" + viTri;
                string tagSTT = prefix + "STTSong" + viTri;

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

        public int NapDonGiayMat(List<WriteCommand> writeCommands, long dai, long stt, int viTri = 1, int soLanThuToiDa = 1)
        {
            try
            {
                if (soLanThuToiDa < 1)
                    soLanThuToiDa = 1;
                string prefix = SongBTags.Instance.StationName + "/" +
                    SongBTags.Instance.ChannelName + "/" +
                    SongBTags.Instance.DeviceName + "/";

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
                string prefix = SongBTags.Instance.StationName + "/" +
                    SongBTags.Instance.ChannelName + "/" +
                    SongBTags.Instance.DeviceName + "/";

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

        public int CommitWrite(List<WriteCommand> writeCommands, string context, string message, int soLanThuToiDa = 1)
        {
            if (writeCommands.Count > 0)
            {
                for (int i = 0; i < soLanThuToiDa; i++)
                {
                    Task.Run(() =>
                    {
                        int count = 0;
                        foreach (var item in writeCommands)
                        {
                            var res = WriteTagExtensions.Write(item.PathToTag, item.Value);
                            if (res == Quality.Good)
                                count++;
                        }
                    });

                }
            }
            return writeCommands.Count;
        }

        public bool SoSanhGiaySong(long dai, long stt, int viTri = 1)
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
                        DaiSongTag = SongBTags.Instance.ChieuDaiSong1;
                        STTSongTag = SongBTags.Instance.STTSong1;
                        break;
                    case 2:
                        DaiSongTag = SongBTags.Instance.ChieuDaiSong2;
                        STTSongTag = SongBTags.Instance.STTSong2;
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
                        DaiSongTag = SongBTags.Instance.ChieuDaiMat1;
                        STTSongTag = SongBTags.Instance.STTMat1;
                        break;
                    case 2:
                        DaiSongTag = SongBTags.Instance.ChieuDaiMat2;
                        STTSongTag = SongBTags.Instance.STTMat2;
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
                        DaiSongTag = SongBTags.Instance.ChieuDaiDon1;
                        STTSongTag = SongBTags.Instance.STT1;
                        break;
                    case 2:
                        DaiSongTag = SongBTags.Instance.ChieuDaiDon2;
                        STTSongTag = SongBTags.Instance.STT2;
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
