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
        public virtual string Xa { get; set; }
        public virtual string Nap1 { get; set; }
        public virtual string Cao { get; set; }
        public virtual string Nap2 { get; set; }
        public virtual string Lang { get; set; }
        public virtual string GhiChu { get; set; }
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
                Cao = donHang.Cao.ToString();
                Lang = donHang.Lang.ToString();
                GhiChu = donHang.GhiChu;
                Title = "Sửa đơn hàng";
                IsEditing = true;
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
                DonHang.Song = Song;
                DonHang.Nap1 = int.Parse(Nap1);
                DonHang.Nap2 = int.Parse(Nap2);
                DonHang.Cao = int.Parse(Cao);
                DonHang.Lang = int.Parse(Lang);
                DonHang.GhiChu = GhiChu;

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
                dh.Nap1 = int.Parse(Nap1);
                dh.Nap2 = int.Parse(Nap2);
                dh.Cao = int.Parse(Cao);
                dh.Lang = int.Parse(Lang);
                dh.GhiChu = GhiChu;
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
                    case nameof(Cao):
                        if (!Cao.IsDWord())
                            Error = "Giá trị phải là số";
                        break;
                    case nameof(Lang):
                        if (!Lang.IsDWord())
                            Error = "Giá trị phải là số";
                        break;
                    default:
                        break;
                }
                return Error;
            }
        }

    }
}
