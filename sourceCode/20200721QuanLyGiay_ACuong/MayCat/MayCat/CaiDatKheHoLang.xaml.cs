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
    /// Interaction logic for CaiDatKheHoLang.xaml
    /// </summary>
    public partial class CaiDatKheHoLang : UserControl, IHaveKey
    {
        public CaiDatKheHoLang()
        {
            InitializeComponent();
            KeyButtons.Add(Key.F5, btnThem);
            KeyButtons.Add(Key.Delete, btnXoa);
            KeyButtons.Add(Key.Enter, btnSave);

            btnThem.Click += BtnThem_Click;
            btnThem.MouseDown += BtnThem_MouseDown;
            DataContext = ViewModelSource.Create(() => new CaiDatKheHoLangViewModel());
        }

        private void BtnThem_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            btnThem.Focus();
        }

        public Dictionary<Key, Button> KeyButtons { get; set; } = new Dictionary<Key, Button>();
    }
}
