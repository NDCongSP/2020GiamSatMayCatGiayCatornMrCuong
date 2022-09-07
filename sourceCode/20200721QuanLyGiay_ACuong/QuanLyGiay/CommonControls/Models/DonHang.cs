using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CommonControls
{
    public class DonHang : BindableBase, INotifyPropertyChanged
    {
        public static DonHang Empty { get; } = new DonHang();
        public DonHang()
        {
            Chay = TimeSpan.FromMilliseconds(0);
            Dung = TimeSpan.FromMilliseconds(0);
        }
        public int Ca { get; set; }
        public long Id { get; set; }
        public DateTime NgayTao { get; set; }
        public long STT { get; set; }
        public string Ma { get; set; }
        public string Song { get; set; }
        public int Kho { get; set; }
        public int Dai { get; set; }
        public int SL { get; set; }
        public int SLDat { get; set; }
        public int SLDatTruocDo { get; set; } = -1;
        public double SoMetDatTruocDo { get; set; }
        public int SLLoi { get; set; }
        public int SLConLai
        {
            get { return SL - SLDat; }
        }
        public double SoMetDat
        {
            get { return (double)(Dai * SLDat) / 1000; }
        }
        public double SoMetDatSF { get; set; }

        public double SoMetLoi
        {
            get { return (double)(Dai * SLLoi) / 1000; }
        }

        public double ConLai
        {
            get { return Tong - SoMetDat; }
        }

        public string PhanTramLoi
        {
            get
            {
                try
                {
                    if (SoMetDat == 0.0 && SoMetLoi == 0.0)
                        return "0";
                    else
                    {
                        return (SoMetLoi * 100 / (SoMetLoi + SoMetDat)).ToString("f1");
                    }
                }
                catch { return ""; }
            }
        }
        public int TocDoTB
        {
            get
            {
                if (Chay.TotalSeconds == 0)
                    return 0;
                return (int)(SoMetDat / Chay.TotalSeconds * 60);
            }
        }
        public TimeSpan Chay { get; set; }
        public DateTime TGBatDau { get; set; }
        public DateTime TGKetThuc { get; set; }
        public TimeSpan Dung { get; set; }
        public int SoDung { get; set; }
        public double M2Dat
        {
            get { return Kho * SoMetDat / 1000; }
        }
        public double M2Loi
        {
            get { return Kho * SoMetLoi / 1000; }
        }
        public string TrangThaiTruocDo { get; set; }
        public double Tong
        {
            get { return (double)(Dai * SL) / 1000; }
        }
        public int Pallet { get; set; }
        public int Xa { get; set; }
        public int Rong { get; set; }
        public int Canh { get; set; }
        public int Cao { get; set; }
        public int Lang { get; set; }
        public string GiaySongE { get; set; }
        public string GiayMatE { get; set; }
        public string GiaySongB { get; set; }
        public string GiayMatB { get; set; }
        public string GiaySongC { get; set; }
        public string GiayMatC { get; set; }
        public string GhiChu { get; set; }
        public string Men { get; set; }
        public int HoanTatCutter { get; set; }
        public int HoanTatSongE { get; set; }
        public int HoanTatGiaySongE { get; set; }
        public int HoanTatGiayMatE { get; set; }
        public int HoanTatSongB { get; set; }
        public int HoanTatGiaySongB { get; set; }
        public int HoanTatGiayMatB { get; set; }
        public int HoanTatSongC { get; set; }
        public int HoanTatGiaySongC { get; set; }
        public int HoanTatGiayMatC { get; set; }
        public int HoanTatSpliter { get; set; }
        public int HoanTatMayMen { get; set; }
        public int FirstRow { get; set; }

        public double Le { get => Kho - (Xa * Rong); }
        public double PhanTramLe { get => (Le / Kho) * 100.0f; }
    }
}
