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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using projekt_PO;

namespace PO_wpf
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string IMG = "pack://application:,,,/img/placeholder.bmp";
        public MainWindow()
        {
            Obstacle h = new Obstacle(20,20,10,10,200);

            InitializeComponent();

            DoubleAnimation da = new DoubleAnimation();

            Image ob = AddObstacle(h);
            //AddObstacle(new Obstacle(10, 10));
            //AddObstacle(new Obstacle(100, 10));
            //AddObstacle(new Obstacle(50, 50));

            ast(ob);
        }

        private async void ast(Image img)
        {
            await Task.Run(() => keepadding(img));
        }

        private void keepadding(Image img)
        {
            while (true)
            {
                this.Dispatcher.Invoke(() =>        //gdy inny wątek chce zmienić UI wątku głównego używamy tej instrukcji
                {
                    img.Margin = new Thickness(img.Margin.Left, img.Margin.Top + 5, 0, 0);
                });
                Task.Delay(1000).Wait();
            }
        }

        public Image AddObstacle(Obstacle obs)
        {
            Image img = new Image();

            img.Source = new BitmapImage(new Uri(IMG));

            img.RenderTransformOrigin = new System.Windows.Point(0, 0);
            img.Margin = new Thickness(obs.Position.X, obs.Position.Y, 0, 0);
            img.Height = obs.Lenght;
            img.Width = obs.Width;
            img.HorizontalAlignment = HorizontalAlignment.Left;
            img.VerticalAlignment = VerticalAlignment.Top;

            Map.Children.Add(img);

            return img;
        }
    }
}
