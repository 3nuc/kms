


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt_PO
{

    static class Constants //klasa zawierające wszystkie stałe używane w projekcie (wielkośc mapy, prędkości pojazdów)
    {
        public const float mapSizeX = 1000;
        public const float mapSizeY = 1000;
        class Plane
        {
            public const float speed = 200;
        }
        class Balloon
        {
            public const float speed = 50;
        }

        class Glider
        {
            public const float speed = 120;
        }

        class Helicopter
        {
            public const float speed = 150;
        }
    }

    class Generator /*chyba przydałoby się zrobić taka klase Generator w której sa wszystkie
        metody np. generateRoute, generateVehicles, generateObstacles żeby nie robić gigantycznej metody generate() w klasie Map.
        Ogólnie to w samych zaleceniach projektu na cezie2 jest napisane żeby rozbijac wielkie metody na mniejsze.
        
         Potem po prostu te metody by się podpieło do klasy Map i tyle, chociaz nie wiem czy ta klasa Generator ma sens xD*/


    {

    }

    class Map //OBSTACLE NIE DZIEDZICZY Z MAP (w przeciwienstwie do tego co moze sugerowac UML)
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
        public void addVehicle(Vehicle _vehicle) {
            //przypomnienie: metoda ma dodać pojazd do listy vehicles
        } //dodaj pojazd latajacy
        public void nextFrame() { } //przeskocz do następnej klatki (przesuwa wszystkie samoloty do przodu o ich wartośc prędkości)
    }

    public class Obstacle //nie dziedziczy z Map, jest wolnostojącym objektem
    {
        public float x, y; //pozycja x, y przeszkody
        public float width, lenght, height; //szerokosc długosc wysokośc przeszkody (Nie ma tego w UMLu a powinno być)

        public Obstacle()
        {
            x = Constants.mapSizeX / 2;
            y = Constants.mapSizeY / 2;
            width = lenght = height = 20;
        }
        public Obstacle(float _x, float _y, float _width, float _lenght, float _height)
        {
            x = _x;
            y = _y;
            width = _width;
            lenght = _lenght;
            height = _height;
        }//czy my w ogóle musimy robić przeszkody naziemne o zmiennych rozmiarach? w specyfikacji projektu nic o tym nie ma 

        //public Vehicle detectCollisions(Map _map) { return new Vehicle(); /*pusty return żeby kod się kompilował, zmienić jak projekt się rozrośnie*/ } //sprawdź czy program powinien wyrzucić zawiadomienie o możliwej kolizji między tym Obstacle a jakimś pojazdem, jeżeli tak to zwróć jego Obiekt, jeżeli nie to pewnie zwróć NULL czy coś
    }


    class Vehicle : Obstacle //Vehicle dziedziczy z obstacle
    {
        //x i y odziedziczone z obstacle
        float speed, height; //stała prędkość poruszania się samolotu oraz wysokosc na ktorej aktualnie się znajduje
        Segment route; //trasa samolotu zaczynająca się na (xstart, ystart) a kończąca sie (xend, yend) - patrz konstruktor

        public Vehicle() {
       
        }

        public void changeRoute(float xendnew, float yendnew, float heightnew) //zmien trase lotu pojazdu. poczatkowa pozycja to ta na ktorej aktualnie znajduje sie samolot w aktualnej klatce, a argumenty opisywanej właśnie funkcji to nowy cel. heightnew to nowy pułap na którym leci pojazd
        {
        }

        //public Vehicle detectCollisions() odziedziczony z Obstacle (dla przypomnienia)
    }

    class Point
    {
        float x, y;
        public Point(float _x, float _y) { x = _x; y = _y; }
        
    }

    class Segment //odcinek
    {
        Point begin, end;
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
