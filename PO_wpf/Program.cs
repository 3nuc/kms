


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt_PO
{

    public static class Constants //klasa zawierające wszystkie stałe używane w projekcie (wielkośc mapy, prędkości pojazdów)
    {
        public const float mapSizeX = 1000; //podpiąć do MainWindow.xaml
        public const float mapSizeY = 1000;
        public const float startingVehicleHeight = 1000;
        public class Plane
        {
            public const float speed = 200;
        }
        public class Balloon
        {
            public const float speed = 50;
        }

        public class Glider
        {
            public const float speed = 120;
        }

        public class Helicopter
        {
            public const float speed = 150;
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
        private float mapSizeX, mapSizeY; //rozmiar mapy

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
        public void nextFrame() {
            foreach (Vehicle vehicle in vehicles)
            {
               // vehicle.position.
            }
        } //przeskocz do następnej klatki (przesuwa wszystkie samoloty do przodu o ich wartośc prędkości)
    }

    public class Obstacle //nie dziedziczy z Map, jest wolnostojącym objektem
    {
        public Point position = new Point(); //pozycja x, y przeszkody
        public float width, lenght, height; //szerokosc długosc wysokośc przeszkody (Nie ma tego w UMLu a powinno być)

        public Obstacle()
        {
            position.X = Constants.mapSizeX / 2;
            position.Y = Constants.mapSizeY / 2;
            width = lenght = height = 20;
        }
        public Obstacle(float _x, float _y, float _width, float _lenght, float _height)
        {
            position.X = _x;
            position.Y = _y;
            width = _width;
            lenght = _lenght;
            height = _height;
        }//czy my w ogóle musimy robić przeszkody naziemne o zmiennych rozmiarach? w specyfikacji projektu nic o tym nie ma 

        public Vehicle detectCollisions(Map _map) { return new Vehicle(); /*pusty return żeby kod się kompilował, zmienić jak projekt się rozrośnie*/ } //sprawdź czy program powinien wyrzucić zawiadomienie o możliwej kolizji między tym Obstacle a jakimś pojazdem, jeżeli tak to zwróć jego Obiekt, jeżeli nie to pewnie zwróć NULL czy coś
    }


    public class Vehicle : Obstacle //Vehicle dziedziczy z obstacle
    {
        //Position odziedziczone z obstacle
        protected float speed, height; //stała prędkość poruszania się samolotu oraz wysokosc na ktorej aktualnie się znajduje
        Segment route; //trasa samolotu zaczynająca się na (xstart, ystart) a kończąca sie (xend, yend) - patrz konstruktor

        public void changeRoute(float _xend, float _yend, float _height) //zmien trase lotu pojazdu. poczatkowa pozycja to ta na ktorej aktualnie znajduje sie samolot w aktualnej klatce, a argumenty opisywanej właśnie funkcji to nowy cel. heightnew to nowy pułap na którym leci pojazd
        {
            route.end = new Point(_xend, _yend);
            height = _height;
        }

        //metoda public Vehicle detectCollisions() odziedziczony z Obstacle tutaj (dla przypomnienia)
    }

    //---------------------------------------RODZAJE POJAZDOW DZIEDZICZACE Z VEHICLE (BALON, HELIKOPTER ETC.)--------------------------------------

    public class Helicopter : Vehicle
    {
        Helicopter()
        {
            speed = Constants.Helicopter.speed;
            height = Constants.startingVehicleHeight;
        }
    }

    public class Glider : Vehicle
    {
        Glider()
        {
            speed = Constants.Glider.speed;
            height = Constants.startingVehicleHeight;
        }
    }

    public class Plane : Vehicle
    {
        Plane()
        {
            speed = Constants.Plane.speed;
            height = Constants.startingVehicleHeight;
        }
    }

    public class Balloon : Vehicle
    {
        Balloon()
        {
            speed = Constants.Balloon.speed;
            height = Constants.startingVehicleHeight;
        }
    }


    public class Point
    {
        private float x,y;
        public float X
        {
            get { return x; }
            set { x = value; }
        }
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        public Point(float _x, float _y) { X = _x; Y = _y; }

        public Point()
        {
            X = 0; Y = 0;
        }
    }

    public class Segment //odcinek
    {
        public Point begin
        {
            get { return begin; }
            set { begin = value; }
        }
        public Point end
        {
            get { return end; }
            set { end = value; }
        }
        Segment(float xbegin, float ybegin, float xend, float yend) //professional constructor tyvm gg
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
