using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayCat
{
    public class ThemSuaDonHangViewModel : IDataErrorInfo
    {

        public DonHang DonHang { get; set; }
        public virtual string Title { get; set; }
        public virtual string STT { get; set; }
        public virtual string Ma { get; set; }
        public virtual string Song { get; set; }
        public virtual string Xa { get; set; } = "0";
        public virtual string Nap1 { get; set; } = "0";
        public virtual string Cao { get; set; } = "0";
        public virtual string Nap2 { get; set; } = "0";
        public virtual string Nap3 { get; set; } = "0";
        public virtual string Nap4 { get; set; } = "0";
        public virtual string Lang { get; set; } = "0";
        public virtual string GhiChu { get; set; }

        public virtual bool MoRongB { get; set; }
        public virtual string Nap1_B { get; set; } = "0";
        public virtual string Cao_B { get; set; } = "0";
        public virtual string Nap2_B { get; set; } = "0";
        public virtual string Nap3_B { get; set; } = "0";
        public virtual string Nap4_B { get; set; } = "0";

        public virtual bool MoRongC { get; set; }
        public virtual string Nap1_C { get; set; } = "0";
        public virtual string Cao_C { get; set; } = "0";
        public virtual string Nap2_C { get; set; } = "0";
        public virtual string Nap3_C { get; set; } = "0";

        public virtual bool MoRongD { get; set; }
        public virtual string Nap1_D { get; set; } = "0";
        public virtual string Cao_D { get; set; } = "0";
        public virtual string Nap2_D { get; set; } = "0";
        public virtual string Nap3_D { get; set; } = "0";

        public virtual bool IsEditing { get; set; }
        public virtual NotifyCollection<DonHang> Source { get; set; }

        public ICurrentWindowService CurrentWindowService { get => this.GetService<ICurrentWindowService>(); }

        public ThemSuaDonHangViewModel(DonHang donHang, NotifyCollection<DonHang> source)
        {
            Source = source;
            DonHang = donHang;
            if (DonHang != null)
            {
                STT = donHang.STT.ToString();
                Ma = donHang.Ma;
                Song = donHang.Song;
                Xa = donHang.Xa.ToString();
                Nap1 = donHang.Nap1.ToString();
                Nap2 = donHang.Nap2.ToString();
                Nap3 = donHang.Nap3.ToString();
                Nap4 = donHang.Nap4.ToString();
                Cao = donHang.Cao.ToString();
                Lang = donHang.Lang.ToString();
                GhiChu = donHang.GhiChu;
                Title = "Sửa đơn hàng";
                IsEditing = true;

                MoRongB = donHang.MoRongB;
                Nap1_B = donHang.Nap1_B.ToString();
                Nap2_B = donHang.Nap2_B.ToString();
                Nap3_B = donHang.Nap3_B.ToString();
                Nap4_B = donHang.Nap4_B.ToString();
                Cao_B = donHang.Cao_B.ToString();

                MoRongC = donHang.MoRongC;
                Nap1_C = donHang.Nap1_C.ToString();
                Nap2_C = donHang.Nap2_C.ToString();
                Nap3_C = donHang.Nap3_C.ToString();
                Cao_C = donHang.Cao_C.ToString();

                MoRongD = donHang.MoRongD;
                Nap1_D = donHang.Nap1_D.ToString();
                Nap2_D = donHang.Nap2_D.ToString();
                Nap3_D = donHang.Nap3_D.ToString();
                Cao_D = donHang.Cao_D.ToString();
            }
            else
            {
                Title = "Thêm đơn hàng";
            }
        }

        public virtual void Save()
        {
            if (IsEditing)
            {
                DonHang.STT = long.Parse(STT);
                DonHang.Ma = Ma;
                DonHang.Xa = int.Parse(Xa);
                DonHang.Song = Song;
                DonHang.Nap1 = int.Parse(Nap1);
                DonHang.Nap2 = int.Parse(Nap2);
                DonHang.Nap3 = int.Parse(Nap3);
                DonHang.Nap4 = int.Parse(Nap4);
                DonHang.Cao = int.Parse(Cao);
                DonHang.Lang = int.Parse(Lang);
                DonHang.GhiChu = GhiChu;
                DonHang.KheHoLang = GetKheHoLang(Song);

                DonHang.MoRongB = MoRongB;
                DonHang.Nap1_B = int.Parse(Nap1_B);
                DonHang.Nap2_B = int.Parse(Nap2_B);
                DonHang.Nap3_B = int.Parse(Nap3_B);
                DonHang.Nap4_B = int.Parse(Nap4_B);
                DonHang.Cao_B = int.Parse(Cao_B);

                DonHang.MoRongC = MoRongC;
                DonHang.Nap1_C = int.Parse(Nap1_C);
                DonHang.Nap2_C = int.Parse(Nap2_C);
                DonHang.Nap3_C = int.Parse(Nap3_C);
                DonHang.Cao_C = int.Parse(Cao_C);

                DonHang.MoRongD = MoRongD;
                DonHang.Nap1_D = int.Parse(Nap1_D);
                DonHang.Nap2_D = int.Parse(Nap2_D);
                DonHang.Nap3_D = int.Parse(Nap3_D);
                DonHang.Cao_D = int.Parse(Cao_D);

                Messenger.Default.Send(new SuaDonHangMessage(Source, true, DonHang));
                Helper.LuuDonHang(Source, "DonHangTay.json");
                Cancel();
            }
            else
            {
                DonHang dh = new DonHang();
                dh.STT = long.Parse(STT);
                dh.Ma = Ma;
                dh.Song = Song;
                dh.Xa = int.Parse(Xa);
                dh.Nap1 = int.Parse(Nap1);
                dh.Nap2 = int.Parse(Nap2);
                dh.Nap3 = int.Parse(Nap3);
                dh.Nap4 = int.Parse(Nap4);
                dh.Cao = int.Parse(Cao);
                dh.Lang = int.Parse(Lang);
                dh.GhiChu = GhiChu;
                dh.KheHoLang = GetKheHoLang(Song);


                dh.MoRongB = MoRongB;
                dh.Nap1_B = int.Parse(Nap1_B);
                dh.Nap2_B = int.Parse(Nap2_B);
                dh.Nap3_B = int.Parse(Nap3_B);
                dh.Nap4_B = int.Parse(Nap4_B);
                dh.Cao_B = int.Parse(Cao_B);

                dh.MoRongC = MoRongC;
                dh.Nap1_C = int.Parse(Nap1_C);
                dh.Nap2_C = int.Parse(Nap2_C);
                dh.Nap3_C = int.Parse(Nap3_C);
                dh.Cao_C = int.Parse(Cao_C);

                dh.MoRongD = MoRongD;
                dh.Nap1_D = int.Parse(Nap1_D);
                dh.Nap2_D = int.Parse(Nap2_D);
                dh.Nap3_D = int.Parse(Nap3_D);
                dh.Cao_D = int.Parse(Cao_D);

                Source.Add(dh);
                Messenger.Default.Send(new ThemDonHangMessage(Source, true, dh));
                this.RaisePropertiesChanged();
            }
        }

        public bool CanSave()
        {
            return string.IsNullOrEmpty(Error);
        }

        public void Cancel()
        {
            CurrentWindowService.Close();
        }

        public bool CanCancel()
        {
            return true;
        }

        public string Error { get; set; }
        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;
                switch (columnName)
                {
                    case nameof(STT):
                        if (STT.IsDWord(out uint stt))
                        {
                            if (!IsEditing)
                            {
                                if (Source.Any(x => x.STT == stt))
                                    Error = "STT đã tồn tại";
                            }
                        }
                        else
                        {
                            Error = "Giá trị phải là số";
                        }
                        break;
                    case nameof(Song):
                        if (!Helper.DanhSachKheHoLang.Any(x => x.TenSong == Song))
                            Error = "Sóng không tồn tại";
                        break;
                    case nameof(Xa):
                        if (!Xa.IsDWord())
                            Error = "Giá trị phải là số";
                        break;
                    case nameof(Nap1):
                        if (!Nap1.IsDWord())
                            Error = "Giá trị phải là số";
                        break;
                    case nameof(Nap2):
                        if (!Nap2.IsDWord())
                            Error = "Giá trị phải là số";
                        break;
                    case nameof(Nap3):
                        if (!Nap3.IsDWord())
                            Error = "Giá trị phải là số";
                        break;
                    case nameof(Nap4):
                        if (!Nap4.IsDWord())
                            Error = "Giá trị phải là số";
                        break;
                    case nameof(Cao):
                        if (!Cao.IsDWord())
                            Error = "Giá trị phải là số";
                        break;
                    case nameof(Lang):
                        if (!Lang.IsDWord())
                            Error = "Giá trị phải là số";
                        break;

                    case nameof(Nap1_B):
                        if (!Nap1_B.IsDWord())
                            Error = "Giá trị phải là số";
                        break;
                    case nameof(Cao_B):
                        if (!Cao_B.IsDWord())
                            Error = "Giá trị phải là số";
                        break;
                    case nameof(Nap2_B):
                        if (!Nap2_B.IsDWord())
                            Error = "Giá trị phải là số";
                        break;
                    case nameof(Nap3_B):
                        if (!Nap3_B.IsDWord())
                            Error = "Giá trị phải là số";
                        break;

                    case nameof(Nap4_B):
                        if (!Nap4_B.IsDWord())
                            Error = "Giá trị phải là số";
                        break;

                    case nameof(Nap1_C):
                        if (!Nap1_C.IsDWord())
                            Error = "Giá trị phải là số";
                        break;
                    case nameof(Cao_C):
                        if (!Cao_C.IsDWord())
                            Error = "Giá trị phải là số";
                        break;
                    case nameof(Nap2_C):
                        if (!Nap2_C.IsDWord())
                            Error = "Giá trị phải là số";
                        break;
                    case nameof(Nap3_C):
                        if (!Nap3_C.IsDWord())
                            Error = "Giá trị phải là số";
                        break;
                    case nameof(Nap1_D):
                        if (!Nap1_D.IsDWord())
                            Error = "Giá trị phải là số";
                        break;
                    case nameof(Cao_D):
                        if (!Cao_D.IsDWord())
                            Error = "Giá trị phải là số";
                        break;
                    case nameof(Nap2_D):
                        if (!Nap2_D.IsDWord())
                            Error = "Giá trị phải là số";
                        break;
                    case nameof(Nap3_D):
                        if (!Nap3_D.IsDWord())
                            Error = "Giá trị phải là số";
                        break;
                    default:
                        break;
                }
                return Error;
            }
        }


        private double GetKheHoLang(string tenSong)
        {
            foreach (var item in Helper.DanhSachKheHoLang)
            {
                if (item.TenSong == tenSong)
                    return item.KheHo;
            }
            return 0;
        }
    }
}
