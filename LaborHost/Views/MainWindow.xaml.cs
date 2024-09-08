using MSTSCLib; // For the RDP control
using System.Windows;

namespace LaborHost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<MsRdpClient7NotSafeForScripting> rdpClients;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClientContainer_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
