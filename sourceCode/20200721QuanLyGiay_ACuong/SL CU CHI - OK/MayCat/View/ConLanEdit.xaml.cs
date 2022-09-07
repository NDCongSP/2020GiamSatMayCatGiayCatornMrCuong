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
    /// Interaction logic for ConLanEdit.xaml
    /// </summary>
    public partial class ConLanEdit : UserControl
    {
        public ConLanEdit()
        {
            InitializeComponent();
            UpdateTrangThaiDao();
        }

        public double MinRange { get; set; } = 0;
        public double MaxRange { get; set; } = 2000;
        public double CenterValue { get; set; } = 1000;

        public double ViTriHienTai
        {
            get { return (double)GetValue(ViTriHienTaiProperty); }
            set { SetValue(ViTriHienTaiProperty, value); }
        }

        public static readonly DependencyProperty ViTriHienTaiProperty =
            DependencyProperty.Register("ViTriHienTai", typeof(double), typeof(ConLanEdit), new PropertyMetadata(0.0, OnViTriHienTaiChanged, OnCoerceViTriHienTai));

        private static object OnCoerceViTriHienTai(DependencyObject d, object baseValue)
        {
            if (d is ConLanEdit daoCat)
            {
                double viTriMoi = (double)baseValue;

                double left = (viTriMoi + daoCat.CenterValue);

                if (left > daoCat.MaxRange)
                    left = daoCat.MaxRange;
                else if (left < daoCat.MinRange)
                    left = daoCat.MinRange;

                left -= (daoCat.Width / 2);
                if (double.IsNaN(left))
                    left = 0;
                daoCat.Margin = new Thickness(left, 0, 0, 0);

            }
            return baseValue;
        }

        private static void OnViTriHienTaiChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ConLanEdit daoCat)
            {
                double viTriMoi = (double)e.NewValue;

                double left = (viTriMoi + daoCat.CenterValue);

                if (left > daoCat.MaxRange)
                    left = daoCat.MaxRange;
                else if (left < daoCat.MinRange)
                    left = daoCat.MinRange;

                left -= (daoCat.Width / 2);
                if (double.IsNaN(left))
                    left = 0;
                daoCat.Margin = new Thickness(left, 0, 0, 0);
                daoCat.UpdateConLai();
            }
        }

        public double ViTriCaiDat
        {
            get { return (double)GetValue(ViTriCaiDatProperty); }
            set { SetValue(ViTriCaiDatProperty, value); }
        }

        public static readonly DependencyProperty ViTriCaiDatProperty =
            DependencyProperty.Register("ViTriCaiDat", typeof(double), typeof(ConLanEdit), new PropertyMetadata(0.0, OnViTriCaiDatChanged));

        private static void OnViTriCaiDatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ConLanEdit daoCat)
            {
                daoCat.UpdateConLai();
            }
        }

        public void UpdateConLai()
        {
            ConLai = Math.Abs(ViTriCaiDat - ViTriHienTai);
        }

        public TrangThaiDao TrangThaiDao
        {
            get { return (TrangThaiDao)GetValue(TrangThaiDaoProperty); }
            set { SetValue(TrangThaiDaoProperty, value); }
        }

        public static readonly DependencyProperty TrangThaiDaoProperty =
            DependencyProperty.Register("TrangThaiDao", typeof(TrangThaiDao), typeof(ConLanEdit), new PropertyMetadata(TrangThaiDao.Lock, OnTranThaiDaoChanged));

        private static void OnTranThaiDaoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ConLanEdit daoCat)
            {
                daoCat.UpdateTrangThaiDao();
            }
        }


        public void UpdateTrangThaiDao()
        {
            switch (TrangThaiDao)
            {
                case TrangThaiDao.Lock:
                    this.Foreground = Brushes.Green;
                    break;
                case TrangThaiDao.Moving:
                    this.Foreground = Brushes.Blue;
                    break;
                case TrangThaiDao.Down:
                    this.Foreground = Brushes.OrangeRed;
                    break;
                case TrangThaiDao.Up:
                    this.Foreground = Brushes.Blue;
                    break;
                default:
                    break;
            }
        }

        public double ConLai
        {
            get { return (double)GetValue(ConLaiProperty); }
            set { SetValue(ConLaiProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConLai.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConLaiProperty =
            DependencyProperty.Register("ConLai", typeof(double), typeof(ConLanEdit), new PropertyMetadata(0.0));

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PressEnter?.Invoke(this, (sender as TextBox).Text);

                if (double.TryParse((sender as TextBox).Text, out double value))
                {
                    if (EnterCommand != null)
                    {
                        if (EnterCommand.CanExecute(EnterCommandParameter))
                            EnterCommand.Execute(EnterCommandParameter);
                    }
                }
            }
        }

        public event Action<object, string> PressEnter;



        public ICommand EnterCommand
        {
            get { return (ICommand)GetValue(EnterCommandProperty); }
            set { SetValue(EnterCommandProperty, value); }
        }

        public static readonly DependencyProperty EnterCommandProperty =
            DependencyProperty.Register("EnterCommand", typeof(ICommand), typeof(ConLanEdit), new PropertyMetadata(null));


        public object EnterCommandParameter
        {
            get { return (object)GetValue(EnterCommandParameterProperty); }
            set { SetValue(EnterCommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EnterCommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnterCommandParameterProperty =
            DependencyProperty.Register("EnterCommandParameter", typeof(object), typeof(ConLanEdit), new PropertyMetadata(null));


    }
}
