using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Host.Controls.Window
{
    /// <summary>
    /// Interaction logic for WindowHead.xaml
    /// </summary>
    public partial class Head : System.Windows.Controls.UserControl
    {

        // Define a DependencyProperty for the Children
        public static readonly DependencyProperty ChildrenProperty =
            DependencyProperty.Register(nameof(Children), typeof(UIElementCollection), typeof(Head),
            new PropertyMetadata(null));

        // This will hold the children of the UserControl
        public UIElementCollection Children
        {
            get { return (UIElementCollection)GetValue(ChildrenProperty); }
            set { SetValue(ChildrenProperty, value); }
        }




        // Define a DependencyProperty for the Children
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(Head),
            new PropertyMetadata(null));

        // This will hold the children of the UserControl
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }




        System.Windows.Window? parentWindow;
        private bool isClick = false;
        private Action? MouseClickCallback;

        public Head()
        {
            InitializeComponent();
            Children = MenuWrapper.Children;
            Mouse.AddMouseMoveHandler(this, MouseMoveHandler);
            Mouse.AddMouseUpHandler(this, MouseClickHandler);
        }

        private void MouseClickHandler(object sender, MouseButtonEventArgs e)
        {
            if(isClick)
            {
               // MouseClickCallback?.Invoke();
            }
            MouseClickCallback = null;
        }

        private void MouseMoveHandler(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isClick = false;
        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            parentWindow = System.Windows.Window.GetWindow(this);
            if (!parentWindow.Equals(App.Current.MainWindow))
            {
                CloseBtn.Visibility = Visibility.Hidden;
                MaximizeBtn.Visibility = Visibility.Hidden;
            }
            if(parentWindow != null && parentWindow.WindowState == WindowState.Maximized)
            {
                // System.Windows.Controls.Image image = (System.Windows.Controls.Image)MaximizeBtn.Child;
                IconMinimize.Visibility = Visibility.Visible;
                IconMaximize.Visibility = Visibility.Hidden;
            }
        }


        private void ExitClicked(object sender, RoutedEventArgs e)
        {
            MouseClickCallback += () =>
            {
                if (parentWindow != null)
                {
                    // parentWindow.Close();
                }
            };
        }



        private void CloseClick(object sender, RoutedEventArgs e)
        {
            MouseClickCallback += () =>
            {
                if (parentWindow != null)
                {
                    // parentWindow.WindowState = WindowState.Minimized;

                }
            };
        }

        private void MaximizeClick(object sender, RoutedEventArgs e)
        {
            if (parentWindow != null)
            {
                if (parentWindow.WindowState == WindowState.Maximized)
                {
                    parentWindow.WindowState = WindowState.Normal;
                    // System.Windows.Controls.Image image = (System.Windows.Controls.Image)MaximizeBtn.Child;
                    IconMinimize.Visibility = Visibility.Hidden;
                    IconMaximize.Visibility = Visibility.Visible;

                } else
                {
                    parentWindow.WindowState = WindowState.Maximized;
                    IconMinimize.Visibility = Visibility.Visible;
                    IconMaximize.Visibility = Visibility.Hidden;
                }
            }
        }


        private void MouseOverEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if(sender is Border)
            {
                Border b = (Border)sender;
                b.Opacity = 1;
            }
        }

        private void MouseOverLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is Border)
            {
                Border b = (Border)sender;
                b.Opacity = 0.75;
            }
        }
    }
}
