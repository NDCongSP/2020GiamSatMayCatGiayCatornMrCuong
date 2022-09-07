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
using System.Windows.Shapes;

namespace MayCat
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            PreviewKeyDown += SettingsWindow_PreviewKeyDown;
            may1.DataContext = ViewModelSource.Create(() => new CaiDatMayCatViewModel(May1Tags.Instance));
            may2.DataContext = ViewModelSource.Create(() => new CaiDatMayCatViewModel(May2Tags.Instance));
        }

        private void SettingsWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key ==  Key.F1)
            {
                (tabControl.Items[0] as TabItem).IsSelected = true;
                if ((tabControl.Items[0] as TabItem).Content is UserControl)
                {
                    ((tabControl.Items[0] as TabItem).Content as UserControl).Focus();
                }
                        
            }
            else if (e.Key == Key.F2)
            {
                (tabControl.Items[1] as TabItem).IsSelected = true;
                if ((tabControl.Items[1] as TabItem).Content is UserControl)
                {
                    ((tabControl.Items[1] as TabItem).Content as UserControl).Focus();
                }
            }
            else if (e.Key == Key.F3)
            {
                (tabControl.Items[2] as TabItem).IsSelected = true;
                if ((tabControl.Items[2] as TabItem).Content is UserControl)
                {
                    ((tabControl.Items[2] as TabItem).Content as UserControl).Focus();
                }
            }
            else if (e.Key == Key.F4)
            {
                (tabControl.Items[3] as TabItem).IsSelected = true;
                if ((tabControl.Items[3] as TabItem).Content is UserControl)
                {
                    ((tabControl.Items[3] as TabItem).Content as UserControl).Focus();
                }
            }
            else if (e.Key == Key.Escape)
            {
                this.Close();
            }

            if ((tabControl.Items[tabControl.SelectedIndex] as TabItem).Content is IHaveKey haveKey)
            {
                foreach (var kvp in haveKey.KeyButtons)
                {
                    if (kvp.Key == e.Key)
                    {
                        if (kvp.Value.Command != null)
                        {
                            if (kvp.Value.Command.CanExecute(null))
                                kvp.Value.Command.Execute(null);
                        }
                    }
                }
            }
        }
    }
}
