using Microsoft.AspNet.SignalR.Client;
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

namespace MaySong
{
    /// <summary>
    /// Interaction logic for ChinhSuaMetLoi.xaml
    /// </summary>
    public partial class ChinhSuaMetLoi : Window
    {
        public ChinhSuaMetLoi()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            txbLoi.TextAlignment = TextAlignment.Center;
            txbLoi.VerticalContentAlignment = VerticalAlignment.Center;
            txbLoi.Focus();
            txbLoi.SelectionStart = 0;
            txbLoi.SelectionLength = txbLoi.Text.Length;
            txbLoi.KeyDown += TxbLoi_KeyDown;
        }

        MainWindow mainWindow;

        public ChinhSuaMetLoi(MainWindow main)
        {
            mainWindow = main;
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
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


        private async void BtnGhi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mainWindow.hubConnection != null && mainWindow.hubConnection.State == ConnectionState.Connected)
                {
                    if (int.TryParse(txbLoi.Text, out int soMetLoi))
                    {
                        await mainWindow.hubProxy.Invoke("ghiSoMetLoi", Properties.Settings.Default["May"].ToString(), soMetLoi.ToString());
                        MessageBox.Show("Ghi thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Số mét lỗi phải là một số !", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Mất kết nối đến máy chủ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mất kết nối đến máy chủ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
