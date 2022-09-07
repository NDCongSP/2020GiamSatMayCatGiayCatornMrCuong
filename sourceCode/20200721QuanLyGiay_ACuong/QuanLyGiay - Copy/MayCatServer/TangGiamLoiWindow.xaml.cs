using DevExpress.Mvvm;
using EasyDriverPlugin;
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
    /// Interaction logic for TangGiamLoiWindow.xaml
    /// </summary>
    public partial class TangGiamLoiWindow : Window
    {
        public TangGiamLoiWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (CutterTags.Instance.SLLoi != null)
            {
                //txbLoi.Text = CutterTags.Instance.SLLoi.Value;
                txbLoi.Text = "0";
            }
            else
            {
                MessageBox.Show("Không tìm thấy tag Lỗi", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            txbLoi.TextAlignment = TextAlignment.Center;
            txbLoi.VerticalContentAlignment = VerticalAlignment.Center;
            txbLoi.Focus();
            txbLoi.SelectionStart = 0;
            txbLoi.SelectionLength = txbLoi.Text.Length;
            txbLoi.KeyDown += TxbLoi_KeyDown;
        }

        private void TxbLoi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnGhi_Click(null, null);
            }
            else if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }


        private  void BtnGhi_Click(object sender, RoutedEventArgs e)
        {
            if (CutterTags.Instance.SLLoi != null && CutterTags.Instance.SLLoi.Quality == Quality.Good)
            {
                if (int.TryParse(txbLoi.Text, out int loi) &&
                    int.TryParse(CutterTags.Instance.SLLoi.Value, out int slLoiMayCat))
                {
                    if (loi != 0)
                    {
                        int value = slLoiMayCat + loi;
                        if (value < 0)
                        {
                            MessageBox.Show("Số lượng lỗi không thể nhỏ hơn 0", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            var res = WriteTagExtensions.WriteTag(CutterTags.Instance.SLLoi, value.ToString());

                            if (res == Quality.Good)
                            {
                                if (int.TryParse(CutterTags.Instance.DaiCat1.Value, out int daiCat))
                                    Messenger.Default.Send(new MessageSLLoiMayCatThayDoi(CutterTags.Instance.STT1?.Value.ToString(), value, daiCat));
                                MessageBox.Show("Ghi thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ghi thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show($"Ghi không thành công !", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
