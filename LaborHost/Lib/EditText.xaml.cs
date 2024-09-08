using System;
using System.Windows;
using System.Windows.Controls;

namespace LaborHost.Lib
{
    /// <summary>
    /// Interaction logic for EditText.xaml
    /// </summary>
    public partial class EditText : System.Windows.Controls.UserControl
    {
        private bool isUpdating = false;
        public EditText()
        {
            InitializeComponent();
        }

        // Registering a DependencyProperty for Placeholder
        public static readonly DependencyProperty PlaceholderProp = DependencyProperty.Register("Placeholder", typeof(string), typeof(EditText), new PropertyMetadata("Empty text"));

        // Registering a DependencyProperty for Value with a change callback
        public static readonly DependencyProperty ValueProp = DependencyProperty.Register("Value", typeof(string), typeof(EditText), new PropertyMetadata(string.Empty, OnValueChangeCallback));

        // Event that will be raised when the Value property changes
        public event EventHandler<string> ValueChanged;

        // CLR wrapper for the Placeholder property
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProp); }
            set { SetValue(PlaceholderProp, value); }
        }

        // CLR wrapper for the Value property
        public string Value
        {
            get { return (string)GetValue(ValueProp); }
            set { SetValue(ValueProp, value); }
        }

        // The static callback method that is called when the Value property changes
        private static void OnValueChangeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as EditText;
            string newValue = e.NewValue as string;

            // Call the instance method that handles the change
            control.OnValueChanged(newValue);
        }

        // Instance method that raises the ValueChanged event
        protected void OnValueChanged(string newValue)
        {
            // Prevent recursive updates by checking the isUpdating flag
            if (!isUpdating)
            {
                // Set the flag to indicate that we're updating the value
                isUpdating = true;
                // Raise the ValueChanged event if there are any subscribers
                ValueChanged?.Invoke(this, newValue);
                // Reset the flag after updating
                isUpdating = false;
            }
        }

        // Handling TextBox text change event
        private void InputElement_TextChanged(object sender, TextChangedEventArgs e)
        {
            string newValue = ((System.Windows.Controls.TextBox)sender).Text;

            // Update the Value property when the TextBox value changes
            Value = newValue;
        }

        public void PutCursor(int position)
        {
            // Ensure the position is within the valid range of the text length
            if (position >= 0 && position <= InputElement.Text.Length)
            {
                InputElement.CaretIndex = position;
            }
            else
            {
                // If the position is out of range, move the cursor to the end
                InputElement.CaretIndex = InputElement.Text.Length;
            }

            // Optionally bring the caret into view
            InputElement.Focus();
        }

        public string GetValue()
        {
            return Value;
        }

        public void SetValue(string value)
        {
            Value = value;
        }
    }
}
