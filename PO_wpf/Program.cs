


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt_PO
{
    public static class Constants //klasa zawierające wszystkie stałe używane w projekcie (wielkośc mapy, prędkości pojazdów)
    {
        public const double mapSizeX = 500; //podpiąć do MainWindow.xaml
        public const double mapSizeY = 500;
        public const double startingVehicleHeight = 1000;
        public class Plane
        {
            public const double speed = 200;
        }
        public class Balloon
        {
            public const double speed = 50;
        }

        public class Glider
        {
            public const double speed = 120;
        }

        public class Helicopter
        {
            public const double speed = 20;
        }
    }

    public class Generator /*chyba przydałoby się zrobić taka klase Generator w której sa wszystkie
        metody np. generateRoute, generateVehicles, generateObstacles żeby nie robić gigantycznej metody generate() w klasie Map.
        Ogólnie to w samych zaleceniach projektu na cezie2 jest napisane żeby rozbijac wielkie metody na mniejsze.
        
         Potem po prostu te metody by się podpieło do klasy Map i tyle, chociaz nie wiem czy ta klasa Generator ma sens xD*/


    {

    }

    public class Map //OBSTACLE NIE DZIEDZICZY Z MAP (w przeciwienstwie do tego co moze sugerowac UML)
    {
        private List<Vehicle> vehicles; //klasa Vehicle jest później w kodzie
        private List<Obstacle> obstacles;
        private double mapSizeX, mapSizeY; //rozmiar mapy

        public Map()
        {
            mapSizeX = Constants.mapSizeX;
            mapSizeY = Constants.mapSizeY;

            vehicles = new List<Vehicle>();
            obstacles = new List<Obstacle>();
        }


        public List<Vehicle> Vehicles { get => vehicles; } //klasa Vehicle jest później w kodzie
        public List<Obstacle> Obstacles { get => obstacles; }

        public void generate() { } //wygeneruj świat (samoloty i przeszkody)
        public void addVehicle(Vehicle _vehicle)
        {   //dodaj pojazd latajacy do mapy
            vehicles.Add(_vehicle);

            //przypomnienie: metoda ma dodać pojazd do listy vehicles
        }
        public void nextFrame() //przeskocz do następnej klatki (przesuń wszystkie samoloty o wartość ich prędkości)
        {                       //pierwsza część kodu w tej metodzie polega na obliczaniu wektorów składowych prędkości (prędkość to wartość liczbowa. aby wiedzieć w którą stronę przesunąć samolot musimy znać wektory składowe (wektor poziomy i pionowy)
                                //druga część kodu zabezpiecza samolot przed ominięciem punktu końcowego trasy (Route.End)
            foreach (Vehicle vehicle in vehicles)
            {
                double speed = vehicle.Speed;
                double horizontalDisplacement = vehicle.Route.End.X - vehicle.Route.Begin.X; //przesuniecie na x-ach na route (o ile x-ów się przesuwa między początkiem trasy a końcem)
                double verticalDisplacement = vehicle.Route.End.Y - vehicle.Route.Begin.Y; //przesuniecie na y
                double routeLength = vehicle.Route.getLength(); //długość trasy z tw. pitagorasa (długość odcinka zaczynającego się na Route.Begin i kończącego się na Route.End)

                double displaceByY = (verticalDisplacement / routeLength) * speed; //wyznaczenie przesunięcia na x i y w jednej klatce
                double displaceByX = (horizontalDisplacement / routeLength) * speed;

                // poniższa zmienna odpowiada na pytanie "jaki jest dystans między punktem początkowym trasy a pozycją samolotu PLUS przesunięcie o prędkość". Samolot nie jest jeszcze przesunięty!!!
                double distanceTraveledPlusDisplaceBy = new Segment(vehicle.Route.Begin, new Point(vehicle.Position.X + displaceByX, vehicle.Position.Y + displaceByY)).getLength();


                if (!vehicle.ReachedDestination) //jeżeli samolot nie dotarł do celu
                {

                    if (distanceTraveledPlusDisplaceBy > routeLength) //jeżeli długość trasy po przesunięciu jest dłuższa od trasy, to oznacza że samolot dotrze do celu w tej klatce, jednak nie możemy pozwolić, żeby samolot wyleciał "za" punkt końcowy trasy
                    {
                        vehicle.Position = new Point(vehicle.Route.End.X, vehicle.Route.End.Y); //jeżeli samolot próbuje przekroczyć końcowy punkt trasy, ustaw poz. samolotu na koniec trasy
                        vehicle.ReachedDestination = true; //samolot doleciał do celu - nie poruszamy nim już w następnych klatkach (chyba że wywołamy changeRoute później)
                        vehicle.Height = 0; //samolot wylądował
                    }
                    else //jeżeli samolot nie próbuje przekroczyć pkt. końcowego trasy, to wszystko ok  - przesuń samolot
                    {
                        vehicle.Position.X += displaceByX;
                        vehicle.Position.Y += displaceByY;
                    }

                }
            }
        }
    }

    public class Obstacle //nie dziedziczy z Map, jest wolnostojącym obiektem
    {
        Point position = new Point(); //pozycja x, y przeszkody
        double width, length, height; //szerokosc długosc wysokośc przeszkody (Nie ma tego w UMLu a powinno być) WZGLĘDEM LEWEGO GORNEGO ROGU

        public Obstacle() //konstruktor do debugu
        {
            Position = new Point(Position.X = Constants.mapSizeX / 2, Position.Y = Constants.mapSizeY / 2);
            width = length = height = 20;
        }
        public Obstacle(double _x, double _y, double _width, double _length, double _height) //właściwy konstruktor
        {
            position = new Point();
            Position.X = _x;
            Position.Y = _y;
            width = _width;
            length = _length;
            height = _height;
        }//czy my w ogóle musimy robić przeszkody naziemne o zmiennych rozmiarach? w specyfikacji projektu nic o tym nie ma 

        public double Width { get => width; set => width = value; }
        public double Length { get => length; set => length = value; }
        public double Height { get => height; set => height = value; }
        public Point Position { get => position; set => position = value; }

        public Vehicle detectCollisions(Map _map) { return new Vehicle(); /*pusty return żeby kod się kompilował, zmienić jak projekt się rozrośnie*/ } //sprawdź czy program powinien wyrzucić zawiadomienie o możliwej kolizji między tym Obstacle a jakimś pojazdem, jeżeli tak to zwróć jego Obiekt, jeżeli nie to pewnie zwróć NULL czy coś
    }


    public class Vehicle : Obstacle //Vehicle dziedziczy z obstacle
    {
        //Position odziedziczone z obstacle
        private double height; //stała prędkość poruszania się samolotu oraz wysokosc na ktorej aktualnie się znajduje
        private Segment route;//trasa samolotu zaczynająca się na (xstart, ystart) a kończąca sie (xend, yend) - patrz konstruktor
        private double speed;
        private bool reachedDestination;


        public Vehicle()
        {
            route = new Segment();
            ReachedDestination = false;
        }

        public Segment Route { get => route; set => route = value; }
        public double Speed { get => speed; set => speed = value; }
        public bool ReachedDestination { get => reachedDestination; set => reachedDestination = value; }

        public void changeRoute(double _xend, double _yend, double _height) //zmien trase lotu pojazdu. poczatkowa pozycja to ta na ktorej aktualnie znajduje sie samolot w aktualnej klatce, a argumenty opisywanej właśnie funkcji to nowy cel. heightnew to nowy pułap na którym leci pojazd
        {
            Route.End = new Point(_xend, _yend);
            Height = _height;
            ReachedDestination = false; //na wypadek jeżeli zmieniamy trasę samolotu który dotarł do celu i na nim stoi (logika poruszania jest wyłączona dla samolotów które dotarły do celu)
        }

        //metoda public Vehicle detectCollisions() odziedziczony z Obstacle tutaj (dla przypomnienia)
    }

    //---------------------------------------RODZAJE POJAZDOW DZIEDZICZACE Z VEHICLE (BALON, HELIKOPTER ETC.)--------------------------------------

    public class Helicopter : Vehicle
    {
        public Helicopter() : base()
        {

            Speed = Constants.Helicopter.speed;
            Height = Constants.startingVehicleHeight;
        }
    }

    public class Glider : Vehicle
    {
        public Glider() : base()
        {
            Speed = Constants.Glider.speed;
            Height = Constants.startingVehicleHeight;
        }
    }

    public class Plane : Vehicle
    {
        public Plane() : base()
        {
            Speed = Constants.Plane.speed;
            Height = Constants.startingVehicleHeight;
        }
    }

    public class Balloon : Vehicle
    {
        public Balloon() : base()
        {
            Speed = Constants.Balloon.speed;
            Height = Constants.startingVehicleHeight;
        }
    }


    public class Point
    {
        private double x;
        private double y;

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }

        public Point(double _x, double _y) { x = _x; y = _y; }

        public Point()
        {
            x = 0;
            y = 0;
        }

        public double lengthFrom(Point end)
        {
            double horizontalDisplacement = end.X - x;
            double verticalDisplacement = end.Y - y;
            return Math.Sqrt(Math.Abs(horizontalDisplacement * horizontalDisplacement + verticalDisplacement * verticalDisplacement));
        }

    }

    public class Segment //odcinek
    {

        private Point begin, end;

        public Point Begin
        {
            get { return begin; }
            set { begin = value; }
        }
        public Point End
        {
            get { return end; }
            set { end = value; }
        }
        public Segment(double xbegin, double ybegin, double xend, double yend) //professional constructor tyvm gg
        {
            begin = new Point(xbegin, ybegin);
            end = new Point(xend, yend);
        }

        public Segment(Point _begin, Point _end) //pleb constructor kek
        {
            begin = _begin;
            end = _end;
        }

        public double getLength()
        {
            return begin.lengthFrom(end);
        }


        public Segment()
        {
            begin = new Point(0, 0);
            end = new Point(300, 300);
        }
    }

    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        Map map = new Map();
    //        Helicopter ehh = new Helicopter();

    //        ehh.Position = new Point(150, 450);
    //        ehh.Route.Begin = new Point(150, 450);
    //        ehh.Route.End = new Point(400, 400);

    //        map.addVehicle(ehh);

    //        Console.WriteLine("X: " + ehh.Position.X + "Y: " + ehh.Position.Y);
    //        map.nextFrame();
    //        Console.WriteLine("\nX: " + ehh.Position.X + "Y: " + ehh.Position.Y);
    //        map.nextFrame();
    //        Console.WriteLine("\nX: " + ehh.Position.X + "Y: " + ehh.Position.Y);
    //        map.nextFrame();
    //        map.nextFrame();
    //        map.nextFrame();
    //        map.nextFrame();
    //        map.nextFrame();
    //        Console.WriteLine("\nX: " + ehh.Position.X + "Y: " + ehh.Position.Y);
    //    }
    //}
}
