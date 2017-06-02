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
        public Random random = new Random();
        public MainWindow()
        {
            Obstacle obs = new Obstacle(20,20,10,10,200);
            Helicopter heli = new Helicopter();

            List<Image> list = new List<Image>();

            InitializeComponent();

            Image obsimg = AddObstacle(obs);
            
            list.Add(AddVehicle(heli));
            list.Add(AddVehicle(heli));
            list.Add(AddVehicle(heli));
            list.Add(AddVehicle(heli));
            list.Add(AddVehicle(heli));

            ast(list);
        }

        private async void ast(List<Image> list)           //metoda asynchroniczna
        {
            await Task.Run(() => keepadding(list));
        }

        private void keepadding(List<Image> list)
        {
            while (true)
            {
                this.Dispatcher.Invoke(() =>        //gdy inny wątek chce zmienić UI wątku głównego używamy tej instrukcji
                {
                    foreach (Image img in list)
                    {
                        img.Margin = new Thickness(img.Margin.Left + random.Next(0,10), 0, 0, img.Margin.Bottom + random.Next(0,10));
                    }
                });
                Task.Delay(500).Wait();
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
            img.Height = 5;
            img.Width = 5;
            img.HorizontalAlignment = HorizontalAlignment.Left;
            img.VerticalAlignment = VerticalAlignment.Bottom;

            Map.Children.Add(img);

            return img;
        }
    }
}
