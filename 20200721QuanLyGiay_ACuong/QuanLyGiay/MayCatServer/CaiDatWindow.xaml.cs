using CommonControls;
using EasyScada.Core;
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

namespace MayCatServer
{
    /// <summary>
    /// Interaction logic for CaiDatWindow.xaml
    /// </summary>
    public partial class CaiDatWindow : Window
    {
        public CaiDat CaiDat { get; set; }
        public IEasyDriverConnector DriverConnector { get; } = EasyDriverConnectorProvider.GetEasyDriverConnector();

        public CaiDatWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            btnLuuE.Click += BtnLuuE_Click;
            btnLuuB.Click += BtnLuuB_Click;
            btnLuuC.Click += BtnLuuC_Click;
            btnLuuCut.Click += BtnLuuCut_Click;
            btnLuuMen.Click += BtnLuuMen_Click;
            Reload();
        }

        private async void BtnLuuMen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!uint.TryParse(txbSettingMen1.Text, out uint settingC1))
                {
                    MessageBox.Show("Trường 'Setting1' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingMen2.Text, out uint settingC2))
                {
                    MessageBox.Show("Trường 'Setting2' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingMen3.Text, out uint settingC3))
                {
                    MessageBox.Show("Trường 'Setting3' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingMen4.Text, out uint settingC4))
                {
                    MessageBox.Show("Trường 'Setting4' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingMen5.Text, out uint settingC5))
                {
                    MessageBox.Show("Trường 'Setting5' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingMen6.Text, out uint settingC6))
                {
                    MessageBox.Show("Trường 'Setting6' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingMen7.Text, out uint settingC7))
                {
                    MessageBox.Show("Trường 'Setting7' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingMen8.Text, out uint settingC8))
                {
                    MessageBox.Show("Trường 'Setting8' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingMen9.Text, out uint settingC9))
                {
                    MessageBox.Show("Trường 'Setting9' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                if (!double.TryParse(txbSoMetNapMen.Text, out double soMetNapGiay))
                {
                    MessageBox.Show("Trường 'Số mét nạp giấy' phải là một số.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(txbSoMetChuanBiMen.Text, out double soMetChuanBiGiay))
                {
                    MessageBox.Show("Trường 'Số mét chuẩn bị nạp giấy' phải là một số.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (soMetChuanBiGiay <= soMetNapGiay)
                {
                    MessageBox.Show("'Số mét chuẩn bị nạp giấy' phải lớn hơn 'Số mét nạp giấy'", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (MayMenTags.Instance.Setting1 == null)
                {
                    MessageBox.Show("Không thể lưu thay đổi.", "Crror", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                List<WriteCommand> writeCommands = new List<WriteCommand>();
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = MayMenTags.Instance.Setting1?.Path,
                    Value = settingC1.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = MayMenTags.Instance.Setting2?.Path,
                    Value = settingC2.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = MayMenTags.Instance.Setting3?.Path,
                    Value = settingC3.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = MayMenTags.Instance.Setting4?.Path,
                    Value = settingC4.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = MayMenTags.Instance.Setting5?.Path,
                    Value = settingC5.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = MayMenTags.Instance.Setting6?.Path,
                    Value = settingC6.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = MayMenTags.Instance.Setting7?.Path,
                    Value = settingC7.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = MayMenTags.Instance.Setting8?.Path,
                    Value = settingC8.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = MayMenTags.Instance.Setting9?.Path,
                    Value = settingC9.ToString()
                });
                //writeCommands.Add(new WriteCommand()
                //{
                //    PathToTag = SongCTags.Instance.Setting10?.Path,
                //    Value = settingC10.ToString()
                //});
                //writeCommands.Add(new WriteCommand()
                //{
                //    PathToTag = SongCTags.Instance.HeSoSong?.Path,
                //    Value = heSoSong.ToString()
                //});

                if (writeCommands.Count > 0)
                {
                    var result = await DriverConnector.WriteMultiTagAsync(writeCommands);
                    string checkRes = CheckResult(result);
                    if (!string.IsNullOrEmpty(checkRes))
                        MessageBox.Show(checkRes, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    int updateResult = Repository.Instance.UpdateColumns("caidat", $" set SoMetBaoChuyenDonMayMen = '{soMetNapGiay}', " +
                        $"SoMetBaoChuanBiMen = '{soMetChuanBiGiay}'");
                    if (updateResult == 0)
                        MessageBox.Show("Lưu 'Số mét nạp đơn' và 'Số mét chuẩn bị giấy' thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    else
                    {
                        MainWindow.CaiDat.SoMetBaoChuyenDonMayMen = soMetNapGiay;
                        MainWindow.CaiDat.SoMetBaoChuanBiMen = soMetChuanBiGiay;

                        if (string.IsNullOrEmpty(checkRes))
                            MessageBox.Show("Lưu thành công", "Thôn báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Crror", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnLuuCut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!uint.TryParse(txbSettingCut1.Text, out uint settingE1))
                {
                    MessageBox.Show("Trường 'Setting1' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingCut2.Text, out uint settingE2))
                {
                    MessageBox.Show("Trường 'Setting2' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingCut3.Text, out uint settingE3))
                {
                    MessageBox.Show("Trường 'Setting3' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingCut4.Text, out uint settingE4))
                {
                    MessageBox.Show("Trường 'Setting4' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingCut5.Text, out uint settingE5))
                {
                    MessageBox.Show("Trường 'Setting5' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingCut6.Text, out uint settingE6))
                {
                    MessageBox.Show("Trường 'Setting6' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingCut7.Text, out uint settingE7))
                {
                    MessageBox.Show("Trường 'Setting7' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingCut8.Text, out uint settingE8))
                {
                    MessageBox.Show("Trường 'Setting8' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingCut9.Text, out uint settingE9))
                {
                    MessageBox.Show("Trường 'Setting9' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingCut10.Text, out uint settingE10))
                {
                    MessageBox.Show("Trường 'Setting10' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbChieuDaiChoPhepSuaDon.Text, out uint chieuDaiChoPhepSuaDon))
                {
                    MessageBox.Show("Trường 'Chiều dài cho phép sua don' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(txbSoMetToiMayMen.Text, out double soMetMayMen))
                {
                    MessageBox.Show("Trường 'Số mét tới máy mền' phải là một số.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(txbSoMetToiMayXa.Text, out double soMetMayXa))
                {
                    MessageBox.Show("Trường 'Số mét tới máy xả' phải là một số.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (CutterTags.Instance.Setting1 == null)
                {
                    MessageBox.Show("Không thể lưu thay đổi.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                List<WriteCommand> writeCommands = new List<WriteCommand>();
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = CutterTags.Instance.Setting1?.Path,
                    Value = settingE1.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = CutterTags.Instance.Setting2?.Path,
                    Value = settingE2.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = CutterTags.Instance.Setting3?.Path,
                    Value = settingE3.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = CutterTags.Instance.Setting4?.Path,
                    Value = settingE4.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = CutterTags.Instance.Setting5?.Path,
                    Value = settingE5.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = CutterTags.Instance.Setting6?.Path,
                    Value = settingE6.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = CutterTags.Instance.Setting7?.Path,
                    Value = settingE7.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = CutterTags.Instance.Setting8?.Path,
                    Value = settingE8.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = CutterTags.Instance.Setting9?.Path,
                    Value = settingE9.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = CutterTags.Instance.Setting10?.Path,
                    Value = settingE10.ToString()
                });

                if (writeCommands.Count > 0)
                {
                    var result = await DriverConnector.WriteMultiTagAsync(writeCommands);
                    string checkRes = CheckResult(result);
                    if (!string.IsNullOrEmpty(checkRes))
                        MessageBox.Show(checkRes, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);

                    int updateResult = Repository.Instance.UpdateColumns("caidat", $" set DanMayXa = '{soMetMayXa}', " +
                        $"DanMayMen = '{soMetMayMen}', ThoiGianTinhTocDoTrungBinh = '{chieuDaiChoPhepSuaDon}'");

                    if (updateResult == 0)
                        MessageBox.Show("Lưu 'Số mét tới máy xả' và 'Số mét tới máy mền' thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    else
                    {
                        MainWindow.CaiDat.DanMayMen = soMetMayMen;
                        MainWindow.CaiDat.DanMayXa = soMetMayXa;
                        MainWindow.CaiDat.ChieuDaiToiThieuChoPhepSuaDon = chieuDaiChoPhepSuaDon;

                        if (string.IsNullOrEmpty(checkRes))
                            MessageBox.Show("Lưu thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnLuuC_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!uint.TryParse(txbSettingC1.Text, out uint settingC1))
                {
                    MessageBox.Show("Trường 'Setting1' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingC2.Text, out uint settingC2))
                {
                    MessageBox.Show("Trường 'Setting2' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingC3.Text, out uint settingC3))
                {
                    MessageBox.Show("Trường 'Setting3' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingC4.Text, out uint settingC4))
                {
                    MessageBox.Show("Trường 'Setting4' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingC5.Text, out uint settingC5))
                {
                    MessageBox.Show("Trường 'Setting5' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingC6.Text, out uint settingC6))
                {
                    MessageBox.Show("Trường 'Setting6' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingC7.Text, out uint settingC7))
                {
                    MessageBox.Show("Trường 'Setting7' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingC8.Text, out uint settingC8))
                {
                    MessageBox.Show("Trường 'Setting8' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingC9.Text, out uint settingC9))
                {
                    MessageBox.Show("Trường 'Setting9' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                //if (!uint.TryParse(txbSettingC10.Text, out uint settingC10))
                //{
                //    MessageBox.Show("Trường 'Setting10' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
                //}

                if (!double.TryParse(txbHeSoSongC.Text, out double heSoSong))
                {
                    MessageBox.Show("Trường 'Hệ số sóng' phải là một số.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(txbSoMetNapGiayC.Text, out double soMetNapGiay))
                {
                    MessageBox.Show("Trường 'Số mét nạp giấy' phải là một số.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(txbSoMetChuanBiGiayC.Text, out double soMetChuanBiGiay))
                {
                    MessageBox.Show("Trường 'Số mét chuẩn bị nạp giấy' phải là một số.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (soMetChuanBiGiay <= soMetNapGiay)
                {
                    MessageBox.Show("'Số mét chuẩn bị nạp giấy' phải lớn hơn 'Số mét nạp giấy'", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (SongCTags.Instance.Setting1 == null)
                {
                    MessageBox.Show("Không thể lưu thay đổi.", "Crror", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                List<WriteCommand> writeCommands = new List<WriteCommand>();
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongCTags.Instance.Setting1?.Path,
                    Value = settingC1.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongCTags.Instance.Setting2?.Path,
                    Value = settingC2.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongCTags.Instance.Setting3?.Path,
                    Value = settingC3.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongCTags.Instance.Setting4?.Path,
                    Value = settingC4.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongCTags.Instance.Setting5?.Path,
                    Value = settingC5.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongCTags.Instance.Setting6?.Path,
                    Value = settingC6.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongCTags.Instance.Setting7?.Path,
                    Value = settingC7.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongCTags.Instance.Setting8?.Path,
                    Value = settingC8.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongCTags.Instance.Setting9?.Path,
                    Value = settingC9.ToString()
                });
                //writeCommands.Add(new WriteCommand()
                //{
                //    PathToTag = SongCTags.Instance.Setting10?.Path,
                //    Value = settingC10.ToString()
                //});
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongCTags.Instance.HeSoSong?.Path,
                    Value = heSoSong.ToString()
                });

                if (writeCommands.Count > 0)
                {
                    var result = await DriverConnector.WriteMultiTagAsync(writeCommands);
                    string checkRes = CheckResult(result);
                    if (!string.IsNullOrEmpty(checkRes))
                        MessageBox.Show(checkRes, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    int updateResult = Repository.Instance.UpdateColumns("caidat", $" set SoMetBaoChuyenDonSongC = '{soMetNapGiay}', " +
                        $"SoMetBaoChuanBiGiaySongC = '{soMetChuanBiGiay}'");
                    if (updateResult == 0)
                        MessageBox.Show("Lưu 'Số mét nạp đơn' và 'Số mét chuẩn bị giấy' thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    else
                    {
                        MainWindow.CaiDat.SoMetBaoChuyenDonSongC = soMetNapGiay;
                        MainWindow.CaiDat.SoMetBaoChuanBiGiaySongC = soMetChuanBiGiay;

                        if (string.IsNullOrEmpty(checkRes))
                            MessageBox.Show("Lưu thành công", "Thôn báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Crror", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnLuuB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!uint.TryParse(txbSettingB1.Text, out uint settingB1))
                {
                    MessageBox.Show("Trường 'Setting1' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingB2.Text, out uint settingB2))
                {
                    MessageBox.Show("Trường 'Setting2' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingB3.Text, out uint settingB3))
                {
                    MessageBox.Show("Trường 'Setting3' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingB4.Text, out uint settingB4))
                {
                    MessageBox.Show("Trường 'Setting4' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingB5.Text, out uint settingB5))
                {
                    MessageBox.Show("Trường 'Setting5' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingB6.Text, out uint settingB6))
                {
                    MessageBox.Show("Trường 'Setting6' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingB7.Text, out uint settingB7))
                {
                    MessageBox.Show("Trường 'Setting7' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingB8.Text, out uint settingB8))
                {
                    MessageBox.Show("Trường 'Setting8' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingB9.Text, out uint settingB9))
                {
                    MessageBox.Show("Trường 'Setting9' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                //if (!uint.TryParse(txbSettingB10.Text, out uint settingB10))
                //{
                //    MessageBox.Show("Trường 'Setting10' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
                //}

                if (!double.TryParse(txbHeSoSongB.Text, out double heSoSong))
                {
                    MessageBox.Show("Trường 'Hệ số sóng' phải là một số.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(txbSoMetNapGiayB.Text, out double soMetNapGiay))
                {
                    MessageBox.Show("Trường 'Số mét nạp giấy' phải là một số.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(txbSoMetChuanBiGiayB.Text, out double soMetChuanBiGiay))
                {
                    MessageBox.Show("Trường 'Số mét chuẩn bị nạp giấy' phải là một số.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (soMetChuanBiGiay <= soMetNapGiay)
                {
                    MessageBox.Show("'Số mét chuẩn bị nạp giấy' phải lớn hơn 'Số mét nạp giấy'", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (SongBTags.Instance.Setting1 == null)
                {
                    MessageBox.Show("Không thể lưu thay đổi.", "Brror", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                List<WriteCommand> writeCommands = new List<WriteCommand>();
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongBTags.Instance.Setting1?.Path,
                    Value = settingB1.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongBTags.Instance.Setting2?.Path,
                    Value = settingB2.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongBTags.Instance.Setting3?.Path,
                    Value = settingB3.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongBTags.Instance.Setting4?.Path,
                    Value = settingB4.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongBTags.Instance.Setting5?.Path,
                    Value = settingB5.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongBTags.Instance.Setting6?.Path,
                    Value = settingB6.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongBTags.Instance.Setting7?.Path,
                    Value = settingB7.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongBTags.Instance.Setting8?.Path,
                    Value = settingB8.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongBTags.Instance.Setting9?.Path,
                    Value = settingB9.ToString()
                });
                //writeCommands.Add(new WriteCommand()
                //{
                //    PathToTag = SongBTags.Instance.Setting10?.Path,
                //    Value = settingB10.ToString()
                //});
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongBTags.Instance.HeSoSong?.Path,
                    Value = heSoSong.ToString()
                });

                if (writeCommands.Count > 0)
                {
                    var result = await DriverConnector.WriteMultiTagAsync(writeCommands);
                    string checkRes = CheckResult(result);
                    if (!string.IsNullOrEmpty(checkRes))
                        MessageBox.Show(checkRes, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    int updateResult = Repository.Instance.UpdateColumns("caidat", $" set SoMetBaoChuyenDonSongB = '{soMetNapGiay}', " +
                        $"SoMetBaoChuanBiGiaySongB = '{soMetChuanBiGiay}'");
                    if (updateResult == 0)
                        MessageBox.Show("Lưu 'Số mét nạp đơn' và 'Số mét chuẩn bị giấy' thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    else
                    {
                        MainWindow.CaiDat.SoMetBaoChuyenDonSongB = soMetNapGiay;
                        MainWindow.CaiDat.SoMetBaoChuanBiGiaySongB = soMetChuanBiGiay;

                        if (string.IsNullOrEmpty(checkRes))
                            MessageBox.Show("Lưu thành công", "Thôn báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Brror", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnLuuE_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!uint.TryParse(txbSettingE1.Text, out uint settingE1))
                {
                    MessageBox.Show("Trường 'Setting1' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingE2.Text, out uint settingE2))
                {
                    MessageBox.Show("Trường 'Setting2' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingE3.Text, out uint settingE3))
                {
                    MessageBox.Show("Trường 'Setting3' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingE4.Text, out uint settingE4))
                {
                    MessageBox.Show("Trường 'Setting4' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingE5.Text, out uint settingE5))
                {
                    MessageBox.Show("Trường 'Setting5' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingE6.Text, out uint settingE6))
                {
                    MessageBox.Show("Trường 'Setting6' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingE7.Text, out uint settingE7))
                {
                    MessageBox.Show("Trường 'Setting7' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingE8.Text, out uint settingE8))
                {
                    MessageBox.Show("Trường 'Setting8' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!uint.TryParse(txbSettingE9.Text, out uint settingE9))
                {
                    MessageBox.Show("Trường 'Setting9' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                //if (!uint.TryParse(txbSettingE10.Text, out uint settingE10))
                //{
                //    MessageBox.Show("Trường 'Setting10' phải là một số nguyên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
                //}

                if (!double.TryParse(txbHeSoSongE.Text, out double heSoSong))
                {
                    MessageBox.Show("Trường 'Hệ số sóng' phải là một số.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(txbSoMetNapGiayE.Text, out double soMetNapGiay))
                {
                    MessageBox.Show("Trường 'Số mét nạp giấy' phải là một số.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(txbSoMetChuanBiGiayE.Text, out double soMetChuanBiGiay))
                {
                    MessageBox.Show("Trường 'Số mét chuẩn bị nạp giấy' phải là một số.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (soMetChuanBiGiay <= soMetNapGiay)
                {
                    MessageBox.Show("'Số mét chuẩn bị nạp giấy' phải lớn hơn 'Số mét nạp giấy'", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (SongETags.Instance.Setting1 == null)
                {
                    MessageBox.Show("Không thể lưu thay đổi.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                List<WriteCommand> writeCommands = new List<WriteCommand>();
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongETags.Instance.Setting1?.Path,
                    Value = settingE1.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongETags.Instance.Setting2?.Path,
                    Value = settingE2.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongETags.Instance.Setting3?.Path,
                    Value = settingE3.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongETags.Instance.Setting4?.Path,
                    Value = settingE4.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongETags.Instance.Setting5?.Path,
                    Value = settingE5.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongETags.Instance.Setting6?.Path,
                    Value = settingE6.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongETags.Instance.Setting7?.Path,
                    Value = settingE7.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongETags.Instance.Setting8?.Path,
                    Value = settingE8.ToString()
                });
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongETags.Instance.Setting9?.Path,
                    Value = settingE9.ToString()
                });
                //writeCommands.Add(new WriteCommand()
                //{
                //    PathToTag = SongETags.Instance.Setting10?.Path,
                //    Value = settingE10.ToString()
                //});
                writeCommands.Add(new WriteCommand()
                {
                    PathToTag = SongETags.Instance.HeSoSong?.Path,
                    Value = heSoSong.ToString()
                });

                if (writeCommands.Count > 0)
                {
                    var result = await DriverConnector.WriteMultiTagAsync(writeCommands);
                    string checkRes = CheckResult(result);
                    if (!string.IsNullOrEmpty(checkRes))
                        MessageBox.Show(checkRes, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    int updateResult = Repository.Instance.UpdateColumns("caidat", $" set SoMetBaoChuyenDonSongE = '{soMetNapGiay}', " +
                        $"SoMetBaoChuanBiGiaySongE = '{soMetChuanBiGiay}'");
                    if (updateResult == 0)
                        MessageBox.Show("Lưu 'Số mét nạp đơn' và 'Số mét chuẩn bị giấy' thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    else
                    {
                        MainWindow.CaiDat.SoMetBaoChuyenDonSongE = soMetNapGiay;
                        MainWindow.CaiDat.SoMetBaoChuanBiGiaySongE = soMetChuanBiGiay;
                        if (string.IsNullOrEmpty(checkRes))
                            MessageBox.Show("Lưu thành công", "Thôn báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string CheckResult(List<WriteResponse> writeResponses)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in writeResponses)
            {
                if (!item.IsSuccess)
                    sb.AppendLine($"Ghi '{item.WriteCommand?.PathToTag}' thất bại.");
            }
            return sb.ToString(); ;
        }

        private void Reload()
        {
            try
            {
                CaiDat = Repository.Instance.GetCaiDat();

                txbChieuDaiChoPhepSuaDon.Text = CaiDat.ChieuDaiToiThieuChoPhepSuaDon.ToString("f0");

                txbSettingE1.Text = SongETags.Instance.Setting1?.Value;
                txbSettingE2.Text = SongETags.Instance.Setting2?.Value;
                txbSettingE3.Text = SongETags.Instance.Setting3?.Value;
                txbSettingE4.Text = SongETags.Instance.Setting4?.Value;
                txbSettingE5.Text = SongETags.Instance.Setting5?.Value;
                txbSettingE6.Text = SongETags.Instance.Setting6?.Value;
                txbSettingE7.Text = SongETags.Instance.Setting7?.Value;
                txbSettingE8.Text = SongETags.Instance.Setting8?.Value;
                txbSettingE9.Text = SongETags.Instance.Setting9?.Value;
                txbSettingE10.Text = SongETags.Instance.Setting10?.Value;
                txbHeSoSongE.Text = SongETags.Instance.HeSoSong?.Value;
                txbSoMetChuanBiGiayE.Text = CaiDat.SoMetBaoChuanBiGiaySongE.ToString();
                txbSoMetNapGiayE.Text = CaiDat.SoMetBaoChuyenDonSongE.ToString();

                txbSettingB1.Text = SongBTags.Instance.Setting1?.Value;
                txbSettingB2.Text = SongBTags.Instance.Setting2?.Value;
                txbSettingB3.Text = SongBTags.Instance.Setting3?.Value;
                txbSettingB4.Text = SongBTags.Instance.Setting4?.Value;
                txbSettingB5.Text = SongBTags.Instance.Setting5?.Value;
                txbSettingB6.Text = SongBTags.Instance.Setting6?.Value;
                txbSettingB7.Text = SongBTags.Instance.Setting7?.Value;
                txbSettingB8.Text = SongBTags.Instance.Setting8?.Value;
                txbSettingB9.Text = SongBTags.Instance.Setting9?.Value;
                txbSettingB10.Text = SongBTags.Instance.Setting10?.Value;
                txbHeSoSongB.Text = SongBTags.Instance.HeSoSong?.Value;
                txbSoMetChuanBiGiayB.Text = CaiDat.SoMetBaoChuanBiGiaySongB.ToString();
                txbSoMetNapGiayB.Text = CaiDat.SoMetBaoChuyenDonSongB.ToString();

                txbSettingC1.Text = SongCTags.Instance.Setting1?.Value;
                txbSettingC2.Text = SongCTags.Instance.Setting2?.Value;
                txbSettingC3.Text = SongCTags.Instance.Setting3?.Value;
                txbSettingC4.Text = SongCTags.Instance.Setting4?.Value;
                txbSettingC5.Text = SongCTags.Instance.Setting5?.Value;
                txbSettingC6.Text = SongCTags.Instance.Setting6?.Value;
                txbSettingC7.Text = SongCTags.Instance.Setting7?.Value;
                txbSettingC8.Text = SongCTags.Instance.Setting8?.Value;
                txbSettingC9.Text = SongCTags.Instance.Setting9?.Value;
                txbSettingC10.Text = SongCTags.Instance.Setting10?.Value;
                txbHeSoSongC.Text = SongCTags.Instance.HeSoSong?.Value;
                txbSoMetChuanBiGiayC.Text = CaiDat.SoMetBaoChuanBiGiaySongC.ToString();
                txbSoMetNapGiayC.Text = CaiDat.SoMetBaoChuyenDonSongC.ToString();

                txbSettingCut1.Text = CutterTags.Instance.Setting1?.Value;
                txbSettingCut2.Text = CutterTags.Instance.Setting2?.Value;
                txbSettingCut3.Text = CutterTags.Instance.Setting3?.Value;
                txbSettingCut4.Text = CutterTags.Instance.Setting4?.Value;
                txbSettingCut5.Text = CutterTags.Instance.Setting5?.Value;
                txbSettingCut6.Text = CutterTags.Instance.Setting6?.Value;
                txbSettingCut7.Text = CutterTags.Instance.Setting7?.Value;
                txbSettingCut8.Text = CutterTags.Instance.Setting8?.Value;
                txbSettingCut9.Text = CutterTags.Instance.Setting9?.Value;
                txbSettingCut10.Text = CutterTags.Instance.Setting10?.Value;


                txbSettingMen1.Text = MayMenTags.Instance.Setting1?.Value;
                txbSettingMen2.Text = MayMenTags.Instance.Setting2?.Value;
                txbSettingMen3.Text = MayMenTags.Instance.Setting3?.Value;
                txbSettingMen4.Text = MayMenTags.Instance.Setting4?.Value;
                txbSettingMen5.Text = MayMenTags.Instance.Setting5?.Value;
                txbSettingMen6.Text = MayMenTags.Instance.Setting6?.Value;
                txbSettingMen7.Text = MayMenTags.Instance.Setting7?.Value;
                txbSettingMen8.Text = MayMenTags.Instance.Setting8?.Value;
                txbSettingMen9.Text = MayMenTags.Instance.Setting9?.Value;


                txbSoMetNapMen.Text = CaiDat.SoMetBaoChuyenDonMayMen.ToString();
                txbSoMetChuanBiMen.Text = CaiDat.SoMetBaoChuanBiMen.ToString();
                txbSoMetToiMayXa.Text = CaiDat.DanMayXa.ToString();
                txbSoMetToiMayMen.Text = CaiDat.DanMayMen.ToString();
            }
            catch { }
        }
    }
}
