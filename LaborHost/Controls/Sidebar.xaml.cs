using Host.Entity;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Button = System.Windows.Controls.Button;
using Color = System.Windows.Media.Color;
using Cursors = System.Windows.Input.Cursors;
using Point = System.Windows.Point;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Host.Controls
{
    /// <summary>
    /// Interaction logic for Sidebar.xaml
    /// </summary>
    public partial class Sidebar : System.Windows.Controls.UserControl
    {

        private bool isMouseDown = false;
        private bool isDragging  = false;
        private Point initialMousePosition;
        private double initialWidth;
        private double initialHeight;

        public Sidebar()
        {
            InitializeComponent();
            Handler.Fill = new SolidColorBrush(Color.FromArgb(255 / 4, 50, 104, 168));

            HostManager.getInstance().each((e) => {
                var Card   = new LabCard();
                Card.Label = e.getLabel();
                Card.Ip    = e.getIp();
                // Card.Media = e.getMedia();
                HostWrapper.Children.Add(Card);
            });
        }

        private void HandlerOnMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Rectangle rectangle = (Rectangle)sender;
            rectangle.Fill      = new SolidColorBrush(Color.FromArgb(255, 50, 104, 168));
            Cursor = Cursors.Hand;
        }

        private void HandlerOnMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Rectangle rectangle = (Rectangle)sender;
            rectangle.Fill      = new SolidColorBrush(Color.FromArgb(255/4, 50, 104, 168));  // Set the Fill to black
        }

        private void HandlerOnMouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
            {
                // Capture the initial mouse position and element size
                initialMousePosition = e.GetPosition((IInputElement)sender);
                var element = sender as FrameworkElement;
                if (element != null)
                {
                    initialWidth = element.ActualWidth;
                    initialHeight = element.ActualHeight;
                }

                // Set flags
                isMouseDown = true;
                isDragging = false;

                // Capture the mouse to ensure it tracks movements outside the control bounds
                Mouse.Capture(sender as UIElement);
            }
        }

        private void HandlerOnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {


            if (isMouseDown)
            {
                Cursor = Cursors.SizeWE;
                // Get the current mouse position
                Point currentMousePosition = e.GetPosition(null);

                // Calculate the difference in mouse position
                double deltaX = currentMousePosition.X - initialMousePosition.X;

                const double DragThreshold = 5.0; // You can adjust this threshold
                if (deltaX > DragThreshold)
                {
                    isDragging = true;
                    // Update the size of the element
                    Width = Math.Max(10, Math.Min(initialWidth + deltaX, 200));
                }
            }
        }

        private void HandlerOnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Cursor = Cursors.Arrow;
            if (isMouseDown)
            {
                Mouse.Capture(null); // Release the mouse capture

                if (!isDragging)
                {
                    // Handle click event here
                    HandlerOnToggle();
                }

                if(Width <= 100)
                {
                    Width = 10;
                }

                // Reset flags
                isMouseDown = false;
                isDragging = false;
            }
        }

        private void HandlerOnToggle()
        {
            if(Width <= 50)
            {
                Width = 200;
            } else
            {
                Width = 10;
            }

        }

        private void RefreshStatRequest(object sender, RoutedEventArgs e)
        {
            // Cast the content of the button to UIElement
            UIElement? icon = (sender as Button).Content as UIElement;

            if (icon != null)
            {
                DoubleAnimation da = new DoubleAnimation(360, 0, new Duration(TimeSpan.FromSeconds(1.5)));

                RotateTransform rt = new RotateTransform();
                icon.RenderTransform = rt;
                icon.RenderTransformOrigin = new Point(0.5, 0.5);
                da.RepeatBehavior = RepeatBehavior.Forever;
                rt.BeginAnimation(RotateTransform.AngleProperty, da);
            }
        }
    }
}
