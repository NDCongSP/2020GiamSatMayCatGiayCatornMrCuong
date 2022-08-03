using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MayCat
{
    /// <summary>
    /// Interaction logic for LineDaoCat.xaml
    /// </summary>
    public partial class LineDaoCat : UserControl
    {
        public LineDaoCat()
        {
            InitializeComponent();
            DanhSachDaoCat = new ObservableCollection<object>();
            if (!DesignerProperties.GetIsInDesignMode(this))
                DanhSachDaoCat.CollectionChanged += DanhSachDaoCat_CollectionChanged;
        }

        private void DanhSachDaoCat_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is UserControl control)
                    {
                        if (control.Parent == null)
                        {
                            if (!container.Children.Contains(control))
                                container.Children.Add(control);
                        }
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    if (item is UserControl control)
                    {
                        if (control.Parent != null)
                            (control.Parent as Panel).Children.Remove(control);
                    }
                }
            }
        }

        public ObservableCollection<object> DanhSachDaoCat
        {
            get { return (ObservableCollection<object>)GetValue(DanhSachDaoCatProperty); }
            set { SetValue(DanhSachDaoCatProperty, value); }
        }
        public static readonly DependencyProperty DanhSachDaoCatProperty =
            DependencyProperty.Register("DanhSachDaoCat", typeof(ObservableCollection<object>), typeof(LineDaoCat), new PropertyMetadata(new ObservableCollection<object>()));


    }
}
