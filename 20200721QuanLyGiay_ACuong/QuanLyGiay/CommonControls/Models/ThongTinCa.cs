using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonControls
{
    public class ThongTinCa
    {
        public DateTime ThoiGian { get; set; }
        public int Ca { get; set; }
        public int SoMet { get; set; }
        public int SoMetDat { get; set; }
        public int SoMetLoi { get; set; }
        public int TocDoTB { get; set; }
        public TimeSpan Chay { get; set; } = TimeSpan.FromMilliseconds(0);
        public TimeSpan Dung { get; set; } = TimeSpan.FromMilliseconds(0);
        public int SoDung { get; set; }
        public int M2Dat { get; set; }
        public int M2Loi { get; set; }
        public int Count { get; set; }
    }
}
