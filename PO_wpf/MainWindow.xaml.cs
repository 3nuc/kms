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
        //public Random random = new Random();
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

            InitializeComponent();

            Width = Constants.mapSizeY+300;             //
            Height = Constants.mapSizeX+100;            //rozmiar okna
            MapBorder.Width = Constants.mapSizeY + 4;       //i mapy
            MapBorder.Height = Constants.mapSizeX + 4;      //
            MapCanvas.Width = Constants.mapSizeY;       
            MapCanvas.Height = Constants.mapSizeX;

            Image obsimg = AddObstacle(obs);

            foreach (Vehicle v in map.Vehicles)
            {
                VehicleObject obj = new VehicleObject(v, AddVehicle(v), AddLine(v));
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
                Task.Delay(1000).Wait();
                map.nextFrame();
                //BindingOperations.GetBindingExpressionBase(VehicleList, ListView.ItemsSourceProperty).UpdateTarget();
                this.Dispatcher.Invoke(() =>        //gdy inny wątek chce zmienić UI wątku głównego używamy tej instrukcji
                {
                    VehicleList.ItemsSource = null;         //Update Binding
                    VehicleList.ItemsSource = list;         //Refreshes Values
                    foreach (VehicleObject obj in list)
                    {
                        //obj.Img.Margin = new Thickness(obj.Vhc.Position.X, 0, 0, obj.Vhc.Position.Y);
                        Canvas.SetBottom(obj.Img, obj.Vhc.Position.Y - (obj.Img.Height)/2);
                        Canvas.SetLeft(obj.Img, obj.Vhc.Position.X - (obj.Img.Width)/2);
                    }
                });
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

            //img.RenderTransformOrigin = new System.Windows.Point(0.5,0.5);
            //img.Margin = new Thickness(vhc.Position.X, 0, 0, vhc.Position.Y);
            img.Height = 5;
            img.Width = 5;
            //img.HorizontalAlignment = HorizontalAlignment.Left;
            //img.VerticalAlignment = VerticalAlignment.Bottom;

            MapCanvas.Children.Add(img);

            Canvas.SetBottom(img, vhc.Position.Y - (img.Height)/2);
            Canvas.SetLeft(img, vhc.Position.X - (img.Width)/2);

            return img;
        }

        public Line AddLine(Vehicle vhc)
        {
            Line line = new Line();

            line.Margin = new Thickness(0, 0, 0, 0);
            //line.HorizontalAlignment = HorizontalAlignment.Left;
            //line.VerticalAlignment = VerticalAlignment.Bottom;
            line.StrokeThickness = 1;
            line.Stroke = System.Windows.Media.Brushes.Gray;
            line.X1 = vhc.Route.Begin.X;
            line.X2 = vhc.Route.End.X;
            line.Y1 = Constants.mapSizeY - vhc.Route.Begin.Y;
            line.Y2 = Constants.mapSizeY - vhc.Route.End.Y;

            MapCanvas.Children.Add(line);

            return line;
        }
    }

    public class VehicleObject   //każdy pojazd ma swój odpowiednik na mapie
    {
        private Line line;
        private Image img;
        private Vehicle vhc;
        private string vehicleType;

        public Line Line
        {
            get { return line; }
            private set
            {
                line = value;
            }
        }
        public Image Img
        {
            get { return img; }
            private set
            {
                img = value;
            }
        }
        public Vehicle Vhc
        {
            get { return vhc; }
            private set
            {
                vhc = value;
            }
        }
        public string VehicleType
        {
            get { return vehicleType; }
            private set
            {
                vehicleType = value;
            }
        }

        public VehicleObject(Vehicle _vhc, Image _img, Line _line)
        {
            vhc = _vhc;
            img = _img;
            line = _line;
            vehicleType = _vhc.GetType().Name;
        }
    }
}
