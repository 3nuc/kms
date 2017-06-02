using projekt_PO;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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

            img.RenderTransformOrigin = new System.Windows.Point(0, 0);         //dla obiektu statycznego x,y określa lewy górny róg
            img.Margin = new Thickness(obs.position.X, 0, 0, obs.position.Y);
            img.Height = obs.lenght;
            img.Width = obs.width;
            img.HorizontalAlignment = HorizontalAlignment.Left;
            img.VerticalAlignment = VerticalAlignment.Bottom;

            Map.Children.Add(img);

            return img;
        }

        public Image AddVehicle(Vehicle vhc)
        {
            Image img = new Image();

            img.Source = new BitmapImage(new Uri(IMG));

            img.Margin = new Thickness(vhc.position.X, 0, 0, vhc.position.Y);
            img.Height = 10;
            img.Width = 10;
            img.HorizontalAlignment = HorizontalAlignment.Left;
            img.VerticalAlignment = VerticalAlignment.Bottom;

            return img;
        }
    }
}
