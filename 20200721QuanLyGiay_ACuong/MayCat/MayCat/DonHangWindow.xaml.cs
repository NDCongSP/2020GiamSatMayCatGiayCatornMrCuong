using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MayCat
{
    /// <summary>
    /// Interaction logic for DonHangWindow.xaml
    /// </summary>
    public partial class DonHangWindow : Window
    {
        public DonHangWindow()
        {
            InitializeComponent();
            DataContext = DonHangViewModel.Instance;

            dataGridTay.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription(dataGridTay.Columns[0].SortMemberPath, System.ComponentModel.ListSortDirection.Ascending));
            dataGridLink.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription(dataGridLink.Columns[0].SortMemberPath, System.ComponentModel.ListSortDirection.Ascending));

            dataGridTay.BeginningEdit += DataGridTay_BeginningEdit;
            dataGridTay.CellEditEnding += DataGridTay_CellEditEnding;

            dataGridLink.BeginningEdit += DataGridLink_BeginningEdit;
            dataGridTay.CellEditEnding += DataGridTay_CellEditEnding1;

            PreviewKeyDown += DonHangWindow_PreviewKeyDown;
        }

        private void DataGridTay_CellEditEnding1(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void DataGridLink_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Column.Header.ToString() != "Ưu tiên")
                e.Cancel = true;
        }

        private void DonHangWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                if (tabControl.SelectedIndex == 1)
                {
                    ExecuteCommand(btnThem);
                }
            }
            else if (e.Key == Key.F2)
            {
                if (tabControl.SelectedIndex == 1)
                {
                    ExecuteCommand(btnSua);
                }
            }
            else if (e.Key == Key.Delete)
            {
                if (tabControl.SelectedIndex == 1)
                {
                    ExecuteCommand(btnXoa);
                }

            }
            else if (e.Key == Key.F5)
            {
                ExecuteCommand(btnUuTien1);
            }
            else if (e.Key == Key.F6)
            {
                ExecuteCommand(btnUuTien2);
            }
            else if (e.Key == Key.F7)
            {
                ExecuteCommand(btnNapMay1);
            }
            else if (e.Key == Key.F8)
            {
                ExecuteCommand(btnNapMay2);
            }
            else if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void ExecuteCommand(Button btn)
        {
            if (btn.Command != null)
            {
                if (btn.Command.CanExecute(null))
                    btn.Command.Execute(null);
            }
        }

        public NotifyCollection<DonHang> DonHangTaySource => dataGridTay.ItemsSource as NotifyCollection<DonHang>;

        private void DataGridTay_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                DonHang dhEdit = e.Row.DataContext as DonHang;
                if (dhEdit != null)
                {
                    string colHeader = e.Column.Header.ToString();
                    if (colHeader == "STT")
                    {
                        string sttMoiStr = (e.EditingElement as TextBox).Text;
                        if (uint.TryParse(sttMoiStr, out uint sttMoi))
                        {
                            if (sttMoi > 0 && !DonHangTaySource.Any(x => x.STT == sttMoi && x != dhEdit))
                            {
                                if (dhEdit.STT != sttMoi)
                                {
                                    dhEdit.STT = sttMoi;
                                    Messenger.Default.Send(new SuaDonHangMessage(DonHangTaySource, true, dhEdit));
                                    e.Cancel = false;
                                    return;
                                }
                            }
                        }
                        dataGridTay.CancelEdit();
                        e.Cancel = true;
                    }
                    else
                    {
                        string newValueStr = (e.EditingElement as TextBox)?.Text;
                        // Mã
                        if (e.Column.DisplayIndex == 1)
                        {
                            if (newValueStr != dhEdit.Ma)
                            {
                                dhEdit.Ma = newValueStr;
                                Helper.LuuDonHang(DonHangTaySource, "DonHangTay.json");
                            }
                            dataGridTay.CancelEdit();
                        }
                        // Sóng
                        else if (e.Column.DisplayIndex == 2)
                        {
                            if (newValueStr != dhEdit.Song && Helper.DanhSachKheHoLang.Any(x => x.TenSong == newValueStr))
                            {
                                dhEdit.Song = newValueStr;
                                Messenger.Default.Send(new SuaDonHangMessage(DonHangTaySource, true, dhEdit));
                                Helper.LuuDonHang(DonHangTaySource, "DonHangTay.json");
                            }
                            dataGridTay.CancelEdit();
                        }
                        // Xả 
                        else if (e.Column.DisplayIndex == 3)
                        {
                            if (uint.TryParse(newValueStr, out uint newValue))
                            {
                                if (newValue >= 0)
                                {
                                    if (dhEdit.Xa != newValue)
                                    {
                                        dhEdit.Xa = (int)newValue;
                                        Messenger.Default.Send(new SuaDonHangMessage(DonHangTaySource, true, dhEdit));
                                        Helper.LuuDonHang(DonHangTaySource, "DonHangTay.json");
                                    }
                                }
                            }
                            dataGridTay.CancelEdit();
                        }
                        // Nắp
                        else if (e.Column.DisplayIndex == 4)
                        {
                            if (uint.TryParse(newValueStr, out uint newValue))
                            {
                                if (newValue >= 0)
                                {
                                    if (dhEdit.Nap1 != newValue)
                                    {
                                        dhEdit.Nap1 = (int)newValue;
                                        Messenger.Default.Send(new SuaDonHangMessage(DonHangTaySource, true, dhEdit));
                                        Helper.LuuDonHang(DonHangTaySource, "DonHangTay.json");
                                    }
                                }
                            }
                            dataGridTay.CancelEdit();
                        }
                        // Cao
                        else if (e.Column.DisplayIndex == 5)
                        {
                            if (uint.TryParse(newValueStr, out uint newValue))
                            {
                                if (newValue >= 0)
                                {
                                    if (dhEdit.Cao != newValue)
                                    {
                                        dhEdit.Cao = (int)newValue;
                                        Messenger.Default.Send(new SuaDonHangMessage(DonHangTaySource, true, dhEdit));
                                        Helper.LuuDonHang(DonHangTaySource, "DonHangTay.json");
                                    }
                                }
                            }
                            dataGridTay.CancelEdit();
                        }
                        // Nắp
                        else if (e.Column.DisplayIndex == 6)
                        {
                            if (uint.TryParse(newValueStr, out uint newValue))
                            {
                                if (newValue >= 0)
                                {
                                    if (dhEdit.Nap2 != newValue)
                                    {
                                        dhEdit.Nap2 = (int)newValue;
                                        Messenger.Default.Send(new SuaDonHangMessage(DonHangTaySource, true, dhEdit));
                                        Helper.LuuDonHang(DonHangTaySource, "DonHangTay.json");
                                    }
                                }
                            }
                            dataGridTay.CancelEdit();
                        }
                        // Lằng
                        else if (e.Column.DisplayIndex == 7)
                        {
                            if (uint.TryParse(newValueStr, out uint newValue))
                            {
                                if (newValue >= 0)
                                {
                                    if (dhEdit.Lang != newValue)
                                    {
                                        dhEdit.Lang = (int)newValue;
                                        Messenger.Default.Send(new SuaDonHangMessage(DonHangTaySource, true, dhEdit));
                                        Helper.LuuDonHang(DonHangTaySource, "DonHangTay.json");
                                    }
                                }
                            }
                            dataGridTay.CancelEdit();

                        }
                        // Ghi chú
                        else if (e.Column.DisplayIndex == 8)
                        {
                            if (newValueStr != dhEdit.GhiChu)
                            {
                                dhEdit.GhiChu = newValueStr;
                                Helper.LuuDonHang(DonHangTaySource, "DonHangTay.json");
                            }
                            dataGridTay.CancelEdit();
                        }
                        // Ưu tiên
                        else if (e.Column.DisplayIndex == 9)
                        {
                            if (string.IsNullOrEmpty(newValueStr) ||
                                newValueStr == "1" ||
                                newValueStr == "2")
                            {
                                if (dhEdit.UuTien != newValueStr)
                                {
                                    dhEdit.UuTien = newValueStr;
                                    Messenger.Default.Send(new ThayDoiUuTienMessage(DonHangTaySource, true, dhEdit));
                                    Helper.LuuDonHang(DonHangTaySource, "DonHangTay.json");
                                }
                            }
                            dataGridTay.CancelEdit();
                        }
                    }
                }
            }

            e.Cancel = false;
        }

        private void DataGridTay_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
        }
    }
}
 