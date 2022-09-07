using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayCat
{
    public class ThemDonHangMessage
    {
        public NotifyCollection<DonHang> DonHangSource { get; set; }
        public bool IsDonHangTay { get; set; }
        public DonHang DonHangMoi { get; set; }

        public ThemDonHangMessage(NotifyCollection<DonHang> source, bool isDonHangtay, DonHang donHang)
        {
            DonHangSource = source;
            IsDonHangTay = isDonHangtay;
            DonHangMoi = donHang;
        }
    }

    public class SuaDonHangMessage
    {
        public NotifyCollection<DonHang> DonHangSource { get; set; }
        public bool IsDonHangTay { get; set; }
        public DonHang DonHangSua { get; set; }

        public SuaDonHangMessage(NotifyCollection<DonHang> source, bool isDonHangtay, DonHang donHang)
        {
            DonHangSource = source;
            IsDonHangTay = isDonHangtay;
            DonHangSua = donHang;
        }
    }

    public class ThayDoiUuTienMessage
    {
        public DonHang DonHang { get; set; }
        public bool IsDonHangTay { get; set; }
        public NotifyCollection<DonHang> DonHangSource { get; set; }
        public ThayDoiUuTienMessage(NotifyCollection<DonHang> source, bool isDonHangtay, DonHang donHang)
        {
            DonHangSource = source;
            IsDonHangTay = isDonHangtay;
            DonHang = donHang;
        }
    }

    public class LenhChuyenDonMessage
    {
        public long STT1 { get; set; }
        public TagContainerBase TagContainer { get; set; }

        public LenhChuyenDonMessage(long sttChot, TagContainerBase tagContainer)
        {
            STT1 = sttChot;
            TagContainer = tagContainer;
        }
    }

    public class TangKheHoLangMessaage
    {
        public int May { get; set; }
    }

    public class GiamKheHoLangMessage
    {
        public int May { get; set; }
    }

    public class ThayDoiKheHoLangMessage
    {
        
    }
}
