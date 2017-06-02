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
            Obstacle obs = new Obstacle(20,20,10,10,200);

            InitializeComponent();

            Image obsimg = AddObstacle(obs);

            ast(obsimg);
        }

        private async void ast(Image img)           //metoda asynchroniczna
        {
            await Task.Run(() => keepadding(img));
        }

        private void keepadding(Image img)
        {
            while (true)
            {
                this.Dispatcher.Invoke(() =>        //gdy inny wątek chce zmienić UI wątku głównego używamy tej instrukcji
                {
                    img.Margin = new Thickness(img.Margin.Left, 0, 0, img.Margin.Bottom + 5);
                });
                Task.Delay(1000).Wait();
            }
        }

        public Image AddObstacle(Obstacle obs)
        {
            Image img = new Image();

            img.Source = new BitmapImage(new Uri(IMG));     //Uri do zdjęcia; Source ma typ ImageSource

            img.RenderTransformOrigin = new System.Windows.Point(0, 0);
            img.Margin = new Thickness(obs.Position.X, obs.Position.Y, 0, 0);
            img.Height = obs.Lenght;
            img.Width = obs.Width;
            img.HorizontalAlignment = HorizontalAlignment.Left;
            img.VerticalAlignment = VerticalAlignment.Bottom;

            Map.Children.Add(img);

            return img;
        }

        public Image AddVehicle(Vehicle vhc)
        {
            Image img = new Image();

            img.Source = new BitmapImage(new Uri(IMG));

            img.Margin = new Thickness(vhc.Position.X, 0, 0, vhc.Position.Y);
            img.Height = 10;
            img.Width = 10;
            img.HorizontalAlignment = HorizontalAlignment.Left;
            img.VerticalAlignment = VerticalAlignment.Bottom;

            return img;
        }
    }
}
