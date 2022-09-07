using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonControls
{
    public class WrapData
    {
        public ThongTinTram ThongTinTram { get; set; }
        public List<DonHang> DanhSachDonHang { get; set; }
        public TrangThaiDonHang TrangThaiDonHang { get; set; }
        public GiaySongGiayMatDangChay GiaySongGiayMatDangChayE { get; set; }
        public GiaySongGiayMatDangChay GiaySongGiayMatDangChayB { get; set; }
        public GiaySongGiayMatDangChay GiaySongGiayMatDangChayC { get; set; }
        public GiaySongGiayMatDangChay GiayMenDangChay { get; set; }
    }
}
