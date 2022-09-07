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
    /// Interaction logic for ChonSongWindow.xaml
    /// </summary>
    public partial class ChonSongWindow : Window
    {
        public ChonSongWindow()
        {
            InitializeComponent();
            cobSong.ItemsSource = new List<string>() { "E", "B", "C", "M" };
            btnSave.Click += BtnSave_Click;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(cobSong.Text))
            {
                Properties.Settings.Default["May"] = cobSong.SelectedItem.ToString();
                Properties.Settings.Default.Save();
                MessageBox.Show("Lưu thành công! Khởi động lại phần mềm để sử dụng.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
