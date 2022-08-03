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

namespace CommonControls
{
    /// <summary>
    /// Interaction logic for ProgressControl.xaml
    /// </summary>
    public partial class ProgressControl : UserControl
    {
        public string TGChay
        {
            get { return (string)GetValue(TGChayProperty); }
            set { SetValue(TGChayProperty, value); }
        }
        public static readonly DependencyProperty TGChayProperty =
            DependencyProperty.Register("TGChay", typeof(string), typeof(ProgressControl), new PropertyMetadata("00:00:00"));

        public string TGConLai
        {
            get { return (string)GetValue(TGConLaiProperty); }
            set { SetValue(TGConLaiProperty, value); }
        }
        public static readonly DependencyProperty TGConLaiProperty =
            DependencyProperty.Register("TGConLai", typeof(string), typeof(ProgressControl), new PropertyMetadata("0"));

        public double PhanTram
        {
            get { return (double)GetValue(PhanTramProperty); }
            set { SetValue(PhanTramProperty, value); }
        }
        public static readonly DependencyProperty PhanTramProperty =
            DependencyProperty.Register("PhanTram", typeof(Double), typeof(ProgressControl), new PropertyMetadata(0.0));

        public ProgressControl()
        {
            InitializeComponent();
        }
    }
}
