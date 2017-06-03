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
using System.ComponentModel;

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
            Map map = new Map();
            Obstacle obs = new Obstacle(20,20,10,10,200);
            Helicopter heli = new Helicopter();
            Helicopter heli2 = new Helicopter();
            Helicopter heli3 = new Helicopter();
            Helicopter heli4 = new Helicopter();

            //List<Vehicle> vehiclelist = new List<Vehicle>();
            List<VehicleObject> vehicleobjectlist = new List<VehicleObject>();

            heli.Position = new projekt_PO.Point(0, 0);
            heli.Route.Begin = new projekt_PO.Point(0, 0);
            heli.Route.End = new projekt_PO.Point(400, 400);

            map.Vehicles.Add(heli);

            heli2.Position = new projekt_PO.Point(100, 100);
            heli2.Route.Begin = new projekt_PO.Point(100, 100);
            heli2.Route.End = new projekt_PO.Point(400, 400);

            map.Vehicles.Add(heli2);

            heli3.Position = new projekt_PO.Point(200, 0);
            heli3.Route.Begin = new projekt_PO.Point(200, 0);
            heli3.Route.End = new projekt_PO.Point(400, 400);

            map.Vehicles.Add(heli3);

            heli4.Position = new projekt_PO.Point(150, 450);
            heli4.Route.Begin = new projekt_PO.Point(150, 450);
            heli4.Route.End = new projekt_PO.Point(400, 400);

            map.Vehicles.Add(heli4);
            //map.vehicles.Add(heli);
            //map.vehicles.Add(heli);
            //map.vehicles.Add(heli);

            InitializeComponent();

            Width = Constants.mapSizeY+300;             //
            Height = Constants.mapSizeX+100;            //rozmiar okna
            MapBorder.Width = Constants.mapSizeY;       //i mapy
            MapBorder.Height = Constants.mapSizeX;      //

            Image obsimg = AddObstacle(obs);

            foreach (Vehicle v in map.Vehicles)
            {
                VehicleObject obj = new VehicleObject(v, AddVehicle(v));
                vehicleobjectlist.Add(obj);
            }

            VehicleList.ItemsSource = vehicleobjectlist;

            ast(map, vehicleobjectlist);
        }

        private async void ast(Map map, List<VehicleObject> list)           //metoda asynchroniczna
        {
            await Task.Run(() => keepadding(map, list));
        }

        private void keepadding(Map map, List<VehicleObject> list)
        {
            while (true)
            {
                map.nextFrame();
                //BindingOperations.GetBindingExpressionBase(VehicleList, ListView.ItemsSourceProperty).UpdateTarget();
                this.Dispatcher.Invoke(() =>        //gdy inny wątek chce zmienić UI wątku głównego używamy tej instrukcji
                {
                    VehicleList.ItemsSource = null;         //Update Binding
                    VehicleList.ItemsSource = list;         //Refreshes Values
                    foreach (VehicleObject obj in list)
                    {
                        obj.Img.Margin = new Thickness(obj.Vhc.Position.X, 0, 0, obj.Vhc.Position.Y);
                    }
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
            img.Height = obs.Length;
            img.Width = obs.Width;
            img.HorizontalAlignment = HorizontalAlignment.Left;
            img.VerticalAlignment = VerticalAlignment.Bottom;

            MapCanvas.Children.Add(img);

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

            MapCanvas.Children.Add(img);

            return img;
        }

        
    }

    public class VehicleObject : INotifyPropertyChanged   //każdy pojazd ma swój odpowiednik na mapie
    {
        private Image img;
        private Vehicle vhc;

        public event PropertyChangedEventHandler PropertyChanged;

        public Image Img
        {
            get { return img; }
            private set
            {
                if(img != value)
                {
                    img = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Img"));
                }
            }
        }
        public Vehicle Vhc
        {
            get { return vhc; }
            private set
            {
                if (vhc != value)
                {
                    vhc = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Vhc"));
                }
            }
        }

        public VehicleObject(Vehicle _vhc, Image _img)
        {
            vhc = _vhc;
            img = _img;
        }
    }
}
