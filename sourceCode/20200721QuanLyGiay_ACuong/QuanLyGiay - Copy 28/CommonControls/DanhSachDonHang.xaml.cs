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
using System.Collections.ObjectModel;

namespace CommonControls
{
    /// <summary>
    /// Interaction logic for DanhSachDonHang.xaml
    /// </summary>
    public partial class DanhSachDonHang : UserControl
    {
        #region Properties
        internal bool _isEditing;
        public NotifyCollection<DonHang> DonHangDataSource
        {
            get { return (NotifyCollection<DonHang>)GetValue(DonHangDataSourceProperty); }
            set { SetValue(DonHangDataSourceProperty, value); }
        }

        public static readonly DependencyProperty DonHangDataSourceProperty =
            DependencyProperty.Register("DonHangDataSource", typeof(NotifyCollection<DonHang>), typeof(DanhSachDonHang),
                new PropertyMetadata(null, DonHangDataSourceChanged));

        public event EventHandler<DataGridBeginningEditEventArgs> BeginEdit;
        public event EventHandler<DataGridCellEditEndingEventArgs> EndEdit;
        public event EventHandler<DataGridRowEditEndingEventArgs> EndRowEdit;

        private static void DonHangDataSourceChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is DanhSachDonHang control)
            {
                if (!control._isEditing)
                {
                    control.Datagrid.ItemsSource = args.NewValue as NotifyCollection<DonHang>;
                }
            }
        }
        #endregion

        public void NotifyResetCollection()
        {
            if (!_isEditing)
                DonHangDataSource?.NotifyResetCollection();
        }

        public DataGrid Datagrid => datagrid;

        public DanhSachDonHang()
        {
            InitializeComponent();
            datagrid.BeginningEdit += Datagrid_BeginningEdit;
            datagrid.CellEditEnding += Datagrid_CellEditEnding;
            datagrid.RowEditEnding += Datagrid_RowEditEnding;
        }

        private void Datagrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            _isEditing = false;
            if (DonHangDataSource != null)
            {
                DonHangDataSource.DisableNotifyChanged = false;
                DonHangDataSource.NotifyResetCollection();
            }

            if (datagrid.ItemsSource != DonHangDataSource)
                datagrid.ItemsSource = DonHangDataSource;
        }

        private void Datagrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            _isEditing = true;
            if (DonHangDataSource != null)
            {
                DonHangDataSource.DisableNotifyChanged = true;
            }
        }

        private void Datagrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            //EndRowEdit?.Invoke(sender, e);
        }

        private void DataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            BeginEdit?.Invoke(sender, e);
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            EndEdit?.Invoke(sender, e);
        }
    }
}
