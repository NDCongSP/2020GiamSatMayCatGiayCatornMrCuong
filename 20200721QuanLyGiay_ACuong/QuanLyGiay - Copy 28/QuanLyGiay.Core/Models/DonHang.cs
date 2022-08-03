using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyGiay.Core
{
    public class DonHang
    {
        public long Id { get; set; }
        public long STT { get; set; }
        public DateTime NgayTao { get; set; }
        public string Ma { get; set; }
        public string Song { get; set; }
        public int Kho { get; set; }
        public int Dai { get; set; }
        public int SoLuong { get; set; }
        public int SoLuongDat { get; set; }
        public int SoLuongLoi { get; set; }

        public TimeSpan TGChay { get; set; }
        public TimeSpan TGDung { get; set; }
        public DateTime TGBatDau { get; set; }
        public DateTime TGKetThuc { get; set; }
        public int TocDo { get; set; }
        public int SoLanDung { get; set; }
        public int Pallet { get; set; }
    }
}
