using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using anbar.Windows;

namespace anbar.User_Controls
{
    /// <summary>
    /// Interaction logic for Developer.xaml
    /// </summary>
    public partial class Developer : UserControl
    {
        public Developer()
        {
            InitializeComponent();
        }

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            while (parent != null)
            {
                if (parent is T parentT)
                    return parentT;

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var parent = this.Parent as Grid;
            if (parent != null)
            {
                var mainControl = FindParent<MainWindow>(this);
                if (mainControl != null)
                {
                    mainControl.btnHome.IsEnabled = true;
                    mainControl.btnMenu.IsEnabled = true;
                    mainControl.Introduction.IsEnabled = true;
                    mainControl.sellPart.IsEnabled = true;
                    mainControl.buyPart.IsEnabled = true;
                    mainControl.toolsPart.IsEnabled = true;
                    mainControl.MainGrid.Background = new SolidColorBrush(Colors.Transparent);
                    mainControl.FincialGrid.Background = new SolidColorBrush(Colors.Transparent);
                }
                parent.Children.Remove(this);
                parent.Visibility = Visibility.Collapsed;
                parent.Background = new SolidColorBrush(Colors.Transparent);
            }
        }
    }
}
