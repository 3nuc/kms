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

namespace PO_wpf
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Obstacle h = new Obstacle(10,100);

            InitializeComponent();

            AddObstacle(h);
            AddObstacle(new Obstacle(10, 10));
            AddObstacle(new Obstacle(100, 10));
            AddObstacle(new Obstacle(50, 50));
        }

        public void AddObstacle(Obstacle obs)
        {
            Image hi = new Image();

            hi.Source = new BitmapImage(new Uri(obs.img));

            hi.Margin = new Thickness(obs.x, obs.y, 0, 0);
            hi.Height = 10;
            hi.Width = 10;
            hi.HorizontalAlignment = HorizontalAlignment.Left;
            hi.VerticalAlignment = VerticalAlignment.Top;

            Map.Children.Add(hi);
        }
    }
}
