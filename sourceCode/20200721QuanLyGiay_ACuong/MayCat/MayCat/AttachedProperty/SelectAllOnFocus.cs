using System.Windows;
using System.Windows.Controls;

namespace MayCat
{
    public class SelectAllOnFocus : DependencyObject
    {
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.RegisterAttached(
          "Enabled", typeof(bool), typeof(SelectAllOnFocus), new PropertyMetadata(false, OnEnabledChanged));

        public static bool GetEnabled(DependencyObject d)
        {
            return (bool)d.GetValue(EnabledProperty);
        }
        public static void SetEnabled(DependencyObject d, bool value)
        {
            d.SetValue(EnabledProperty, value);
        }

        private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                textBox.GotFocus += TextBox_GotFocus;
                textBox.GotKeyboardFocus += TextBox_GotKeyboardFocus;
            }
        }

        private static void TextBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        private static void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }
    }
}
