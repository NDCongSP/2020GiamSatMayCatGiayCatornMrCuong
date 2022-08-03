using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MayCat
{
    public class DonHangViewModel
    {
        public static DonHangViewModel Instance { get; } = ViewModelSource.Create(() => new DonHangViewModel());

        public List<string> DanhSachMayUuTien { get; set; }
        public NotifyCollection<DonHang> DonHangTay { get; set; }
        public NotifyCollection<DonHang> DonHangLink { get; set; }
        public DonHang SelectedItemTay { get; set; }
        public DonHang SelectedItemLink { get; set; }
        public virtual int SelectedTabIndex { get; set; }
        public virtual string NguonDonHang { get; set; }
        public virtual int SelectedIndexTay { get; set; }
        public virtual int SelectedIndexLink { get; set; }


        public DonHangViewModel()
        {
            DanhSachMayUuTien = new List<string>()
            {
                "",
                "Máy 1",
                "Máy 2"
            };
            DonHangTay = Helper.DonHangTay;
            DonHangLink = Helper.DonHangLink;
            NguonDonHang = MainViewModel.Instance.NguonDonHang;
            if (NguonDonHang == "Đơn hàng link")
                SelectedTabIndex = 0;
            else
                SelectedTabIndex = 1;
        }

        #region Commands

        public void Them()
        {
            ThemSuaDonHangWindow window = new ThemSuaDonHangWindow();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.DataContext = ViewModelSource.Create(() => new ThemSuaDonHangViewModel(null, DonHangTay));
            window.ShowDialog();
        }

        public void Sua()
        {
            ThemSuaDonHangWindow window = new ThemSuaDonHangWindow();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.DataContext = ViewModelSource.Create(() => new ThemSuaDonHangViewModel(SelectedItemTay, DonHangTay));
            window.ShowDialog();
        }
        public bool CanSua()
        {
            return SelectedItemTay != null;
        }

        public void Xoa()
        {
            var mbr = MessageBox.Show("Bạn có muốn xóa đơn hàng này hay không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbr == MessageBoxResult.Yes)
            {
                DonHangTay.Remove(SelectedItemTay);
            }
        }
        public bool CanXoa()
        {
            return SelectedItemTay != null;
        }

        public void XoaTatCa()
        {
            var mbr = MessageBox.Show("Bạn có muốn xóa tất cả đơn hàng hay không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbr == MessageBoxResult.Yes)
            {
                DonHangTay.Clear();
            }

        }

        public bool CanXoaTatCa()
        {
            return DonHangTay != null && DonHangTay.Count > 0;
        }

        public void XoaUuTienTay()
        {
            var mbr = MessageBox.Show("Bạn có muốn xóa tất cả ưu tiên của đơn hàng hay không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbr == MessageBoxResult.Yes)
            {
                DonHangTay.ToList().ForEach(x => x.UuTien = "");
            }
        }

        public bool CanXoaUuTienTay()
        {
            return DonHangTay != null && DonHangTay.Count > 0;
        }

        public void XoaUuTienLink()
        {
            var mbr = MessageBox.Show("Bạn có muốn xóa tất cả ưu tiên của đơn hàng hay không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbr == MessageBoxResult.Yes)
            {
                DonHangLink.ToList().ForEach(x => x.UuTien = "");
            }
        }
        public bool CanXoaUuTienLink()
        {
            return DonHangLink != null && DonHangLink.Count > 0;
        }

        public void CapNhatDonHangLink()
        {

        }

        [Command(CanExecuteMethodName = "CanNapDonMay1")]
        public void SetUuTienMay1()
        {
            if (SelectedTabIndex == 0)
            {
                SelectedItemLink.UuTien = "1";
            }
            else if (SelectedTabIndex == 1)
            {
                SelectedItemTay.UuTien = "1";
            }
        }

        [Command(CanExecuteMethodName = "CanNapDonMay2")]
        public void SetUuTienMay2()
        {
            if (SelectedTabIndex == 0)
            {
                SelectedItemLink.UuTien = "2";
            }
            else if (SelectedTabIndex == 1)
            {
                SelectedItemTay.UuTien = "2";
            }
        }

        [Command(CanExecuteMethodName = "CanNapDonMay1")]
        public void NapDonMay1()
        {
            string error1 = string.Empty;

            if (SelectedTabIndex == 0)
            {
                error1 = Helper.NapDon(SelectedItemLink, 2, 1);
                if (string.IsNullOrEmpty(error1))
                {
                    SelectedItemLink.DuKien = "1";
                    List<DonHang> source = DonHangLink.OrderBy(x => x.STT).ToList();
                    if (source != null && source.Count > 0)
                    {
                        foreach (var item in Helper.LayDanhSachDonHangKeTiep(source, SelectedItemLink, 1))
                        {
                            item.DuKien = "1";
                        }
                    }
                }

                NguonDonHang = "Đơn hàng link";
                MainViewModel.Instance.NguonDonHang = NguonDonHang;
            }
            else if (SelectedTabIndex == 1)
            {
                error1 = Helper.NapDon(SelectedItemTay, 2, 1);
                NguonDonHang = "Đơn hàng tay";
                MainViewModel.Instance.NguonDonHang = NguonDonHang;
            }

            if (string.IsNullOrEmpty(error1))
                MessageBox.Show("Nạp thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show($"{error1}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        [Command(CanExecuteMethodName = "CanNapDonMay2")]
        public void NapDonMay2()
        {
            string error1 = string.Empty;

            if (SelectedTabIndex == 0)
            {
                error1 = Helper.NapDon(SelectedItemLink, 2, 2);
                NguonDonHang = "Đơn hàng link";
                MainViewModel.Instance.NguonDonHang = NguonDonHang;
            }
            else if (SelectedTabIndex == 1)
            {
                error1 = Helper.NapDon(SelectedItemTay, 2, 2);
                NguonDonHang = "Đơn hàng tay";
                MainViewModel.Instance.NguonDonHang = NguonDonHang;
            }

            if (string.IsNullOrEmpty(error1))
                MessageBox.Show("Nạp thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show($"{error1}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public bool CanNapDonMay2()
        {
            if (May2Tags.Instance.ChoPhepMayChay && May2Tags.Instance.Run != null)
            {
                if (SelectedTabIndex == 0)
                {
                    return SelectedItemLink != null && SelectedItemLink.UuTien != "1";
                }
                else if (SelectedTabIndex == 1)
                {
                    return SelectedItemTay != null && SelectedItemTay.UuTien != "1";
                }
            }
            return false;
        }

        public bool CanNapDonMay1()
        {
            if (May1Tags.Instance.ChoPhepMayChay && May1Tags.Instance.Run != null)
            {
                if (SelectedTabIndex == 0)
                {
                    return SelectedItemLink != null && SelectedItemLink.UuTien != "2";
                }
                else if (SelectedTabIndex == 1)
                {
                    return SelectedItemTay != null && SelectedItemTay.UuTien != "2";
                }
            }
            return false;
        }

        #endregion
    }
}
