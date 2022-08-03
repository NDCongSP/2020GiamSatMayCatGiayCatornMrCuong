using CommonControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayCatServer
{
    public class MessageSLLoiMayCatThayDoi
    {
        public string STT { get; private set; }
        public int SLLoi { get; private set; }
        public int Dai { get; private set; }
        public MessageSLLoiMayCatThayDoi(string stt, int slLoi, int dai)
        {
            Dai = dai;
            STT = stt;
            SLLoi = slLoi;
        }
    }

    public class MessageSLLoiThayDoi
    {
        public string STT { get; private set; }
    }

    public class MessageSTTDonHangThayDoi
    {
        public string OldSTT { get; private set; }
        public string NewSTT { get; private set; }
        public MessageSTTDonHangThayDoi(string oldSTT, string newSTT)
        {
            OldSTT = oldSTT;
            NewSTT = newSTT;
        }
    }

    public class MessageChuyenDonHang
    {
        public string DeviceName { get; private set; }
        public string STT { get; private set; }

        public MessageChuyenDonHang(string deviceName, string stt)
        {
            DeviceName = deviceName;
            STT = stt;
        }
    }

    public class MessageChuyenDonGiaySong
    {
        public string DeviceName { get; private set; }
        public string STT { get; private set; }

        public MessageChuyenDonGiaySong(string deviceName, string stt)
        {
            DeviceName = deviceName;
            STT = stt;
        }
    }

    public class MessageChuyenDonGiayMat
    {
        public string DeviceName { get; private set; }
        public string STT { get; private set; }

        public MessageChuyenDonGiayMat(string deviceName, string stt)
        {
            DeviceName = deviceName;
            STT = stt;
        }
    }

    public class MessageChuanBiGiay
    {
        public string DeviceName { get; private set; }
        public DonHang DonHangKeTiep { get; private set; }
        public MessageChuanBiGiay(string deviceName, DonHang dhKe)
        {
            DeviceName = deviceName;
            DonHangKeTiep = dhKe;
        }
    }

    public class MessageNapDonMaySong
    {
        public string DeviceName { get; private set; }
        public DonHang DonHangMayCatHienTai { get; private set; }
        public DonHang DonHangMayCatKeTiep { get; private set; }
        public MessageNapDonMaySong(string deviceName, DonHang dhMayCatHienTai, DonHang dhMayCatKeTiep)
        {
            DeviceName = deviceName;
            DonHangMayCatHienTai = dhMayCatHienTai;
            DonHangMayCatKeTiep = dhMayCatKeTiep;
        }
    }

    public class MessageKiemTraChieuDai
    {
        public string DeviceName { get; private set; }
        public MessageKiemTraChieuDai(string deviceName)
        {
            DeviceName = deviceName;
        }
    }
}
