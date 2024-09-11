using System.Windows;

namespace Host.Controls
{
    /// <summary>
    /// Interaction logic for LabCard.xaml
    /// </summary>
    public partial class LabCard : System.Windows.Controls.UserControl
    {

        DependencyProperty labipProp = DependencyProperty.Register("ip", typeof(string), typeof(LabCard), new PropertyMetadata("NULL"));
        DependencyProperty lablabelProp = DependencyProperty.Register("label", typeof(string), typeof(LabCard), new PropertyMetadata("NULL"));
        DependencyProperty labmediaProp = DependencyProperty.Register("media", typeof(string), typeof(LabCard), new PropertyMetadata(@"pack://application:,,,/Host;component/assets/images/computerlab.jpg"));
        public LabCard()
        {
            InitializeComponent();
        }

        public string Ip
        {
            get { return (string)GetValue(labipProp); }
            set { SetValue(labipProp, value);  }
        }

        public string Label
        {
            get { return (string)GetValue(lablabelProp); }
            set { SetValue(lablabelProp, value); }
        }

        public string Media
        {
            get { return (string)GetValue(labmediaProp); }
            set { SetValue(labmediaProp, value); }
        }

    }
}
