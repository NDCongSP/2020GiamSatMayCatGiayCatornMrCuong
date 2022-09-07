using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonControls
{
    public class CaiDat
    {
        public double DanCatTam { get; set; }
        public double DanMayXa { get; set; }
        public double DanMayMen { get; set; }

        public double SoMetBaoChuyenDonSongE { get; set; } = 100;
        public double SoMetBaoChuyenDonSongB { get; set; } = 100;
        public double SoMetBaoChuyenDonSongC { get; set; } = 100;
        public double SoMetBaoChuyenDonMayMen { get; set; } = 100;

        public double SoMetBaoChuanBiGiaySongE { get; set; } = 150;
        public double SoMetBaoChuanBiGiaySongB { get; set; } = 150;
        public double SoMetBaoChuanBiGiaySongC { get; set; } = 150;
        public double SoMetBaoChuanBiMen { get; set; } = 150;
        public double ChieuDaiToiThieuChoPhepSuaDon { get; set; } = 100;
    }
}
