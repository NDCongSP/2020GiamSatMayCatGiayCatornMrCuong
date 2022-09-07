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
    /// Interaction logic for ThemSuaDonHangWindow.xaml
    /// </summary>
    public partial class ThemSuaDonHangWindow : Window
    {
        public ThemSuaDonHangWindow()
        {
            InitializeComponent();

            PreviewKeyDown += ThemSuaDonHangWindow_PreviewKeyDown;
            btnThoat.Click += BtnThoat_Click;
            Loaded += ThemSuaDonHangWindow_Loaded;
        }

        private void ThemSuaDonHangWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txbSTT.Focus(); 
        }

        private void BtnThoat_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ThemSuaDonHangWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (btnSave.Command != null)
                {
                    if (btnSave.Command.CanExecute(null))
                        btnSave.Command.Execute(null);
                }
            }
            else if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
