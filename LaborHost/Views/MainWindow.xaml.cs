using System.Windows;
using System.Windows.Controls;

namespace Host
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private List<MsRdpClient7NotSafeForScripting> rdpClients;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClientContainer_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void sidebar_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ExitClicked(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void MinimizeClicked(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }




        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem)
            {
                MenuItem item = (MenuItem)sender;
                var uid = item.GetValue(UIElement.UidProperty);
                if (uid != null && uid is string)
                {
                    if(uid.Equals("route-config"))
                    {
                        Views.ConfigWindow w = new();
                        w.Owner = App.Current.MainWindow;
                        w.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        w.Show();
                    } else if (uid.Equals("route-client_add"))
                    {
                        Views.AddClient w = new();
                        w.Owner = App.Current.MainWindow;
                        w.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        w.Show();
                    } else if (uid.Equals("route-client_manager"))
                    {
                        Views.ClientManager w = new();
                        w.Owner = App.Current.MainWindow;
                        w.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        w.Show();
                    }
                    else if (uid.Equals("route-schedule_add"))
                    {
                        Views.AddSchedule w = new();
                        w.Owner = App.Current.MainWindow;
                        w.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        w.Show();
                    }
                    else if (uid.Equals("route-schedule_manager"))
                    {
                        Views.ScheduleManager w = new();
                        w.Owner = App.Current.MainWindow;
                        w.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        w.Show();
                    }
                    else if (uid.Equals("route-proxy"))
                    {
                        Views.Proxy w = new();
                        w.Owner = App.Current.MainWindow;
                        w.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        w.Show();
                    }
                }
            }

        }

        private void Head_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
