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
    /// Interaction logic for CaiDatSettings.xaml
    /// </summary>
    public partial class CaiDatSettings : UserControl, IHaveKey
    {
        public CaiDatSettings()
        {
            InitializeComponent();
            btnCancel.Click += BtnCancel_Click;

            KeyButtons.Add(Key.Enter, btnSave);
            this.DataContext = ViewModelSource.Create(() => new CaiDatSettingsViewModel());
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        public Dictionary<Key, Button> KeyButtons { get; set; } = new Dictionary<Key, Button>();


        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            setting1.Focus();
        }
    }
}
