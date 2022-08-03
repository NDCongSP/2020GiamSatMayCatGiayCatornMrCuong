using DevExpress.Mvvm.POCO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MayCat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            PreviewKeyDown += MainWindow_PreviewKeyDown;
            btnCaiDat.Click += BtnCaiDat_Click;
            btnDonHang.Click += BtnDonHang_Click;
            DataContext = MainViewModel.Instance;
        }

        private void BtnDonHang_Click(object sender, RoutedEventArgs e)
        {
            DonHangWindow window = new DonHangWindow();
            window.Owner = this;
            window.ShowDialog();
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F4)
            {
                BtnCaiDat_Click(null, null);
            }
            else if (e.Key == Key.F1)
            {
                BtnDonHang_Click(null, null);
            }
        }

        private void BtnCaiDat_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow window = new SettingsWindow();
            window.Owner = this;
            window.ShowDialog();
        }
    }
}
