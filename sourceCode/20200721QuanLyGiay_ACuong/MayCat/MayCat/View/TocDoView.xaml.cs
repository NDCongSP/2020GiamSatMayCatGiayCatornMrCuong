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
    /// Interaction logic for TocDoView.xaml
    /// </summary>
    public partial class TocDoView : UserControl
    {
        #region Properties

        public string TocDo1
        {
            get { return (string)GetValue(TocDo1Property); }
            set { SetValue(TocDo1Property, value); }
        }

        public static readonly DependencyProperty TocDo1Property =
            DependencyProperty.Register("TocDo1", typeof(string), typeof(TocDoView), new PropertyMetadata(null));

        public string TocDo2
        {
            get { return (string)GetValue(TocDo2Property); }
            set { SetValue(TocDo2Property, value); }
        }

        public static readonly DependencyProperty TocDo2Property =
            DependencyProperty.Register("TocDo2", typeof(string), typeof(TocDoView), new PropertyMetadata(null));

        public string TocDo3
        {
            get { return (string)GetValue(TocDo3Property); }
            set { SetValue(TocDo3Property, value); }
        }

        public static readonly DependencyProperty TocDo3Property =
            DependencyProperty.Register("TocDo3", typeof(string), typeof(TocDoView), new PropertyMetadata(null));

        public string TocDo4
        {
            get { return (string)GetValue(TocDo4Property); }
            set { SetValue(TocDo4Property, value); }
        }

        public static readonly DependencyProperty TocDo4Property =
            DependencyProperty.Register("TocDo4", typeof(string), typeof(TocDoView), new PropertyMetadata(null));

        public string TocDo5
        {
            get { return (string)GetValue(TocDo5Property); }
            set { SetValue(TocDo5Property, value); }
        }

        public static readonly DependencyProperty TocDo5Property =
            DependencyProperty.Register("TocDo5", typeof(string), typeof(TocDoView), new PropertyMetadata(null));

        public string TocDo6
        {
            get { return (string)GetValue(TocDo6Property); }
            set { SetValue(TocDo6Property, value); }
        }

        public static readonly DependencyProperty TocDo6Property =
            DependencyProperty.Register("TocDo6", typeof(string), typeof(TocDoView), new PropertyMetadata(null));

        public string DoiDon1
        {
            get { return (string)GetValue(DoiDon1Property); }
            set { SetValue(DoiDon1Property, value); }
        }

        public static readonly DependencyProperty DoiDon1Property =
            DependencyProperty.Register("DoiDon1", typeof(string), typeof(TocDoView), new PropertyMetadata(null));

        public string DoiDon2
        {
            get { return (string)GetValue(DoiDon2Property); }
            set { SetValue(DoiDon2Property, value); }
        }

        public static readonly DependencyProperty DoiDon2Property =
            DependencyProperty.Register("DoiDon2", typeof(string), typeof(TocDoView), new PropertyMetadata(null));

        public string DoiDon3
        {
            get { return (string)GetValue(DoiDon3Property); }
            set { SetValue(DoiDon3Property, value); }
        }

        public static readonly DependencyProperty DoiDon3Property =
            DependencyProperty.Register("DoiDon3", typeof(string), typeof(TocDoView), new PropertyMetadata(null));

        public string DoiDon4
        {
            get { return (string)GetValue(DoiDon4Property); }
            set { SetValue(DoiDon4Property, value); }
        }

        public static readonly DependencyProperty DoiDon4Property =
            DependencyProperty.Register("DoiDon4", typeof(string), typeof(TocDoView), new PropertyMetadata(null));

        public string DoiDon5
        {
            get { return (string)GetValue(DoiDon5Property); }
            set { SetValue(DoiDon5Property, value); }
        }

        public static readonly DependencyProperty DoiDon5Property =
            DependencyProperty.Register("DoiDon5", typeof(string), typeof(TocDoView), new PropertyMetadata(null));

        public string DoiDon6
        {
            get { return (string)GetValue(DoiDon6Property); }
            set { SetValue(DoiDon6Property, value); }
        }

        public static readonly DependencyProperty DoiDon6Property =
            DependencyProperty.Register("DoiDon6", typeof(string), typeof(TocDoView), new PropertyMetadata(null));

        public string Dan1
        {
            get { return (string)GetValue(Dan1Property); }
            set { SetValue(Dan1Property, value); }
        }

        public static readonly DependencyProperty Dan1Property =
            DependencyProperty.Register("Dan1", typeof(string), typeof(TocDoView), new PropertyMetadata(null));

        public string Dan2
        {
            get { return (string)GetValue(Dan2Property); }
            set { SetValue(Dan2Property, value); }
        }

        public static readonly DependencyProperty Dan2Property =
            DependencyProperty.Register("Dan2", typeof(string), typeof(TocDoView), new PropertyMetadata(null));

        public string Dan3
        {
            get { return (string)GetValue(Dan3Property); }
            set { SetValue(Dan3Property, value); }
        }

        public static readonly DependencyProperty Dan3Property =
            DependencyProperty.Register("Dan3", typeof(string), typeof(TocDoView), new PropertyMetadata(null));

        public string Dan4
        {
            get { return (string)GetValue(Dan4Property); }
            set { SetValue(Dan4Property, value); }
        }

        public static readonly DependencyProperty Dan4Property =
            DependencyProperty.Register("Dan4", typeof(string), typeof(TocDoView), new PropertyMetadata(null));

        public string Dan5
        {
            get { return (string)GetValue(Dan5Property); }
            set { SetValue(Dan5Property, value); }
        }

        public static readonly DependencyProperty Dan5Property =
            DependencyProperty.Register("Dan5", typeof(string), typeof(TocDoView), new PropertyMetadata(null));

        public string Dan6
        {
            get { return (string)GetValue(Dan6Property); }
            set { SetValue(Dan6Property, value); }
        }

        public static readonly DependencyProperty Dan6Property =
                DependencyProperty.Register("Dan6", typeof(string), typeof(TocDoView), new PropertyMetadata(null));


        public Brush TrangThai1
        {
            get { return (Brush)GetValue(TrangThai1Property); }
            set { SetValue(TrangThai1Property, value); }
        }

        public static readonly DependencyProperty TrangThai1Property =
            DependencyProperty.Register("TrangThai1", typeof(Brush), typeof(TocDoView), new PropertyMetadata(Brushes.Transparent));

        public Brush TrangThai2
        {
            get { return (Brush)GetValue(TrangThai2Property); }
            set { SetValue(TrangThai2Property, value); }
        }

        public static readonly DependencyProperty TrangThai2Property =
            DependencyProperty.Register("TrangThai2", typeof(Brush), typeof(TocDoView), new PropertyMetadata(Brushes.Transparent));

        public Brush TrangThai3
        {
            get { return (Brush)GetValue(TrangThai3Property); }
            set { SetValue(TrangThai3Property, value); }
        }

        public static readonly DependencyProperty TrangThai3Property =
            DependencyProperty.Register("TrangThai3", typeof(Brush), typeof(TocDoView), new PropertyMetadata(Brushes.Transparent));

        public Brush TrangThai4
        {
            get { return (Brush)GetValue(TrangThai4Property); }
            set { SetValue(TrangThai4Property, value); }
        }

        public static readonly DependencyProperty TrangThai4Property =
            DependencyProperty.Register("TrangThai4", typeof(Brush), typeof(TocDoView), new PropertyMetadata(Brushes.Transparent));

        public Brush TrangThai5
        {
            get { return (Brush)GetValue(TrangThai5Property); }
            set { SetValue(TrangThai5Property, value); }
        }

        public static readonly DependencyProperty TrangThai5Property =
            DependencyProperty.Register("TrangThai5", typeof(Brush), typeof(TocDoView), new PropertyMetadata(Brushes.Transparent));

        public Brush TrangThai6
        {
            get { return (Brush)GetValue(TrangThai6Property); }
            set { SetValue(TrangThai6Property, value); }
        }

        public static readonly DependencyProperty TrangThai6Property =
            DependencyProperty.Register("TrangThai6", typeof(Brush), typeof(TocDoView), new PropertyMetadata(Brushes.Transparent));

        #endregion

        public static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public TocDoView()
        {
            InitializeComponent();
        }
    }
}
