using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LaborHost
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            InputClientIp.ValueChanged += HandleValueChanged;
        }

        private void HandleValueChanged(object? sender, string e)
        {
            // Regular expression to insert a dot after every three digits
            string pattern = @"(\d{3})(?!$)"; // Matches every group of 3 digits, but ignores the last group to avoid trailing dot

            // Format the value by adding a dot every 3 digits
            string formattedValue = Regex.Replace(e, pattern, "$1.");
            formattedValue = Regex.Replace(formattedValue, @"[.]+", ".");
            formattedValue = Regex.Replace(formattedValue, @"[^0-9.]+", "");

            // Set the formatted value back to the input
            InputClientIp.Value = formattedValue;
            InputClientIp.PutCursor(formattedValue.Length);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string ClientIp = InputClientIp.GetValue();
            Debug.WriteLine("CLIENT IP : "+ClientIp);

        }
    }
}
