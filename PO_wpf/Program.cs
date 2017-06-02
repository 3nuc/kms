


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt_PO
{

    public static class Constants //klasa zawierające wszystkie stałe używane w projekcie (wielkośc mapy, prędkości pojazdów)
    {
        public const double mapSizeX = 1000; //podpiąć do MainWindow.xaml
        public const double mapSizeY = 1000;
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
            public const double speed = 150;
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

        Map()
        {
            mapSizeX = Constants.mapSizeX;
            mapSizeY = Constants.mapSizeY;

            vehicles = new List<Vehicle>();
            obstacles = new List<Obstacle>();
        }


        public void generate() { } //wygeneruj świat (samoloty i przeszkody)
        public void addVehicle(Vehicle _vehicle) {   //dodaj pojazd latajacy do mapy
            vehicles.Add(_vehicle);
            
            //przypomnienie: metoda ma dodać pojazd do listy vehicles
        } 
        public void nextFrame() { //przeskocz do następnej klatki (przesuwa wszystkie samoloty do przodu o ich wartośc prędkości)
            foreach (Vehicle vehicle in vehicles)
            {
                double speed = vehicle.Speed;
                double horizontalDisplacement = vehicle.Route.End.X - vehicle.Route.Begin.X; //przesuniecie na x-ach na route (o ile x-ów się przesuwa między początkiem trasy a końcem)
                double verticalDisplacement = vehicle.Route.End.Y - vehicle.Route.Begin.Y; //przesuniecie na y
                double routeLenght = Math.Sqrt(Math.Abs(horizontalDisplacement * horizontalDisplacement + verticalDisplacement * verticalDisplacement));
                
                //finding angle
                double angle = Math.Asin(verticalDisplacement / routeLenght);
                double displaceByY = Math.Sin(angle) * speed * Math.Sign(verticalDisplacement);
                double displaceByX = Math.Cos(angle) * speed * Math.Sign(horizontalDisplacement);


                vehicle.Position.X += displaceByX;
                vehicle.Position.Y += displaceByY;

                //
            }
        } 
    }

    public class Obstacle //nie dziedziczy z Map, jest wolnostojącym objektem
    {
        Point position = new Point(); //pozycja x, y przeszkody
        double width, lenght, height; //szerokosc długosc wysokośc przeszkody (Nie ma tego w UMLu a powinno być) WZGLĘDEM LEWEGO GORNEGO ROGU

        public Obstacle() //debug constructor i guess
        {
            Position = new Point(Position.X = Constants.mapSizeX / 2, Position.Y = Constants.mapSizeY / 2);
            width = lenght = height = 20;
        }
        public Obstacle(double _x, double _y, double _width, double _lenght, double _height)
        {
            position = new Point();
            Position.X = _x;
            Position.Y = _y;
            width = _width;
            lenght = _lenght;
            height = _height;
        }//czy my w ogóle musimy robić przeszkody naziemne o zmiennych rozmiarach? w specyfikacji projektu nic o tym nie ma 

        public double Width { get => width; set => width = value; }
        public double Lenght { get => lenght; set => lenght = value; }
        public double Height { get => height; set => height = value; }
        public Point Position { get => position; set => position = value; }

        public Vehicle detectCollisions(Map _map) { return new Vehicle(); /*pusty return żeby kod się kompilował, zmienić jak projekt się rozrośnie*/ } //sprawdź czy program powinien wyrzucić zawiadomienie o możliwej kolizji między tym Obstacle a jakimś pojazdem, jeżeli tak to zwróć jego Obiekt, jeżeli nie to pewnie zwróć NULL czy coś
    }


    public class Vehicle : Obstacle //Vehicle dziedziczy z obstacle
    {
        //Position odziedziczone z obstacle
        private double height1; //stała prędkość poruszania się samolotu oraz wysokosc na ktorej aktualnie się znajduje
        private Segment route; //trasa samolotu zaczynająca się na (xstart, ystart) a kończąca sie (xend, yend) - patrz konstruktor
        private double speed;

        public Segment Route { get => route; set => route = value; }
        public double Speed { get => speed; set => speed = value; }
        public double Height1 { get => height1; set => height1 = value; }

        public void changeRoute(double _xend, double _yend, double _height) //zmien trase lotu pojazdu. poczatkowa pozycja to ta na ktorej aktualnie znajduje sie samolot w aktualnej klatce, a argumenty opisywanej właśnie funkcji to nowy cel. heightnew to nowy pułap na którym leci pojazd
        {
            Route.End = new Point(_xend, _yend);
            Height1 = _height;
        }

        //metoda public Vehicle detectCollisions() odziedziczony z Obstacle tutaj (dla przypomnienia)
    }

    //---------------------------------------RODZAJE POJAZDOW DZIEDZICZACE Z VEHICLE (BALON, HELIKOPTER ETC.)--------------------------------------

    public class Helicopter : Vehicle
    {
        Helicopter()
        {
            Speed = Constants.Helicopter.speed;
            Height1 = Constants.startingVehicleHeight;
        }
    }

    public class Glider : Vehicle
    {
        Glider()
        {
            Speed = Constants.Glider.speed;
            Height1 = Constants.startingVehicleHeight;
        }
    }

    public class Plane : Vehicle
    {
        Plane()
        {
            Speed = Constants.Plane.speed;
            Height1 = Constants.startingVehicleHeight;
        }
    }

    public class Balloon : Vehicle
    {
        Balloon()
        {
            Speed = Constants.Balloon.speed;
            Height1 = Constants.startingVehicleHeight;
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
        Segment(double xbegin, double ybegin, double xend, double yend) //professional constructor tyvm gg
        {
            begin = new Point(xbegin, ybegin);
            end = new Point(xend, yend);
        }

        Segment(Point _begin, Point _end) //pleb constructor kek
        {
            begin = _begin;
            end = _end;
        }
    }
}
