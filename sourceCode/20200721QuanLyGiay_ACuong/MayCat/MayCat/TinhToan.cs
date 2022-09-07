using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayCat
{
    public static class TinhToan
    {
        /// <summary>
        /// Hàm tính toán giá trị nạp xuống PLC
        /// </summary>
        /// <param name="dao1_sv"></param>
        /// <param name="dao2_sv"></param>
        /// <param name="dao3_sv"></param>
        /// <param name="dao4_sv"></param>
        /// <param name="dao5_sv"></param>
        /// <param name="hut"></param>
        /// <param name="lang1_sv"></param>
        /// <param name="lang2_sv"></param>
        /// <param name="lang3_sv"></param>
        /// <param name="lang4_sv"></param>
        /// <param name="lang5_sv"></param>
        /// <param name="lang6_sv"></param>
        /// <param name="lang7_sv"></param>
        /// <param name="lang8_sv"></param>
        public static void TinhToanGiaTri(DonHang donHang,  ref int dao1_sv, ref int dao2_sv, ref int dao3_sv, ref int dao4_sv, ref int dao5_sv,
            ref int hut, ref int lang1_sv, ref int lang2_sv, ref int lang3_sv, ref int lang4_sv, ref int lang5_sv, ref int lang6_sv, ref int lang7_sv, ref int lang8_sv)
        {
            int test = donHang.Cao;
            //dao1_sv = 100;
            //dao2_sv = 200;
            //dao1_sv = 500;
            //May2Tags.Instance.STT2
            //double ketQua = May1Tags.Instance.Lang6_U.Value + 200;
            //dao2_sv = (int)ketQua;
        }
    }
}
