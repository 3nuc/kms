


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
        public const double startingVehicleHeight = 0;
        public const int routesMaxNumberOfSegments = 5;
        public const int proximityWarningThreshold = 20;
        public static int colisionThreshold = 5;

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

        //private Segment windowTop = new Segment(new Point(0, Constants.mapSizeY), new Point(Constants.mapSizeX, Constants.mapSizeY)); //krawędzie mapy względem dolnego lewego rogu używane do sprawdzania czy generowane trasy nie wykraczają poza mapę
        //private Segment windowBottom = new Segment(new Point(0, 0), new Point(Constants.mapSizeX, 0));
        //private Segment windowLeft = new Segment(new Point(0, 0), new Point(0, Constants.mapSizeY));
        //private Segment windowRight = new Segment(new Point(Constants.mapSizeX, 0), new Point(0, Constants.mapSizeY));

        //powyższe nieużywane, znaleziono inną implementację


        public List<Segment> generateRoutes(int routeLengthInSegments, Vehicle vehicle)
        {
            List<Segment> generatedRoutes = new List<Segment>();
            Random numberGenerator = new Random(Guid.NewGuid().GetHashCode());
            Point lastSegmentsEnd = new Point();



            double generateSpeed()
            {
                double generatedSpeed = 0;

                if (vehicle.GetType().Name == "Helicopter") generatedSpeed = numberGenerator.Next(Convert.ToInt32(Constants.Helicopter.speed / 2), Convert.ToInt32(Constants.Helicopter.speed * 2));
                else if (vehicle.GetType().Name == "Glider") generatedSpeed = numberGenerator.Next(Convert.ToInt32(Constants.Glider.speed / 2), Convert.ToInt32(Constants.Glider.speed * 2));
                else if (vehicle.GetType().Name == "Plane") generatedSpeed = numberGenerator.Next(Convert.ToInt32(Constants.Plane.speed / 2), Convert.ToInt32(Constants.Plane.speed * 2));
                else if (vehicle.GetType().Name == "Balloon") generatedSpeed = numberGenerator.Next(Convert.ToInt32(Constants.Balloon.speed / 2), Convert.ToInt32(Constants.Balloon.speed * 2));
                else throw new NotImplementedException();

                return generatedSpeed;
            }

            double generateHeight()
            {
                return numberGenerator.Next(Convert.ToInt32(Constants.startingVehicleHeight / 2), Convert.ToInt32(Constants.startingVehicleHeight * 2));
            }

            Point generatePoint()
            {
                Point generatedPoint = new Point(numberGenerator.Next() % Constants.mapSizeX, numberGenerator.Next() % Constants.mapSizeY);
                return generatedPoint;
            }


            for (int i = 0; i < routeLengthInSegments; i++)
            {
                Point startingPoint;
                if (i == 0)
                {
                    startingPoint = generatePoint(); //wygeneruj punkt początkowy pojedyńczego odcinka trasy jeżeli jest to pierwszy generowany odcinek ...
                    vehicle.Position = startingPoint;
                }
                else
                    startingPoint = lastSegmentsEnd; // w przeciwnym wypadku ustaw punkt początkowy nowego odcinka na koniec ostatniego (aby trasa była ciągłą linią łamaną)

                Point endingPoint = generatePoint();
                lastSegmentsEnd = endingPoint;

                while (startingPoint.lengthFrom(endingPoint) < 20)
                {
                    endingPoint = generatePoint();
                }

                generatedRoutes.Add(new Segment(startingPoint, endingPoint, generateSpeed(), generateHeight()));
            }



            return generatedRoutes;
        }

        public List<Vehicle> generateVehicles(int numberOfVehicles)
        {
            List<Vehicle> generatedVehicles = new List<Vehicle>();
            Vehicle generatedVehicle = new Vehicle();
            Random numberGenerator = new Random(Guid.NewGuid().GetHashCode());

            for (int i = 0; i < numberOfVehicles; i++)
            {
                int aircraftPickerRandom = numberGenerator.Next() % 4;

                if (aircraftPickerRandom == 0) generatedVehicle = new Helicopter(); //losowanie jakim rodzajem pojazdu będzie wygenerowany pojazd
                else if (aircraftPickerRandom == 1) generatedVehicle = new Glider();
                else if (aircraftPickerRandom == 2) generatedVehicle = new Plane();
                else if (aircraftPickerRandom == 3) generatedVehicle = new Balloon();

                generatedVehicle.Routes = generateRoutes((numberGenerator.Next() % Constants.routesMaxNumberOfSegments) + 1, generatedVehicle);
                //generatedVehicle.Position ustawione przez generateRoutes, speed oraz height też

                generatedVehicle.CurrentSegmentIndex = 0;

                generatedVehicles.Add(generatedVehicle);
            }
            return generatedVehicles;
        }
    }

    //    public List<Obstacle> generateObstacles(int numberOfObstacles) {

    //        List<Obstacle> generatedObstacles = new List<Obstacle>();
    //        Obstacle generatedObstacle = new Obstacle();
    //        Random numberGenerator = new Random(Guid.NewGuid().GetHashCode());

    //        Point generatePoint()
    //        {
    //            Point generatedPoint = new Point(numberGenerator.Next() % Constants.mapSizeX, numberGenerator.Next() % Constants.mapSizeY);
    //            return generatedPoint;
    //        }

    //        generatedObstacle.Position = generatePoint();

    //        return new List<Obstacle>(); }


    //}

    public class Map //OBSTACLE NIE DZIEDZICZY Z MAP (w przeciwienstwie do tego co moze sugerowac UML)
    {
        private List<Vehicle> vehicles; //klasa Vehicle jest później w kodzie
        private List<Obstacle> obstacles;
        private List<Collision> collisions;

        private double mapSizeX, mapSizeY; //rozmiar mapy

        public Map()
        {
            mapSizeX = Constants.mapSizeX;
            mapSizeY = Constants.mapSizeY;

            vehicles = new List<Vehicle>();
            obstacles = new List<Obstacle>();
        }


        public List<Vehicle> Vehicles { get => vehicles; set => vehicles = value; } //klasa Vehicle jest później w kodzie
        public List<Obstacle> Obstacles { get => obstacles; set => obstacles = value; }     //private set po debugach
        public List<Collision> Collisions { get => collisions; set => collisions = value; }

        public void addVehicle(Vehicle _vehicle)
        {  
            vehicles.Add(_vehicle);
            //właściwie lista vehicles jest zakapsułowana i ma własny setter, ale metoda została zachowana ze względu na kompatybilność wsteczną
        }
        /// <summary>
        /// Przesuwa wszystkie pojazdy latające w tej mapie o wartość swojej prędkości w odpowiednim kierunku.
        /// </summary>

                       
        //pierwsza część kodu w tej metodzie polega na obliczaniu wektorów składowych prędkości (prędkość to wartość liczbowa. aby wiedzieć w którą stronę przesunąć samolot musimy znać wektory składowe (wektor poziomy i pionowy)
        //druga część kodu zabezpiecza samolot przed ominięciem punktu końcowego trasy (Route.End) oraz przekierowuje pojazdy latające na odpowiednio dalsze odcinki tras (jeżeli poj. latający dotarł do końca aktualnego)
        public void nextFrame()
        {
            foreach (Vehicle vehicle in vehicles)
            {
                double speed = vehicle.Routes[vehicle.CurrentSegmentIndex].Speed;
                double horizontalDisplacement = vehicle.Routes[vehicle.CurrentSegmentIndex].End.X - vehicle.Routes[vehicle.CurrentSegmentIndex].Begin.X; //przesuniecie na x-ach na route (o ile x-ów się przesuwa między początkiem trasy a końcem)
                double verticalDisplacement = vehicle.Routes[vehicle.CurrentSegmentIndex].End.Y - vehicle.Routes[vehicle.CurrentSegmentIndex].Begin.Y; //przesuniecie na y
                double routeLength = vehicle.Routes[vehicle.CurrentSegmentIndex].getLength(); //długość trasy z tw. pitagorasa (długość odcinka zaczynającego się na Route.Begin i kończącego się na Route.End)

                double displaceByY = (verticalDisplacement / routeLength) * speed; //wyznaczenie przesunięcia na x i y w jednej klatce
                double displaceByX = (horizontalDisplacement / routeLength) * speed;

                // poniższa zmienna odpowiada na pytanie "jaki jest dystans między punktem początkowym trasy a pozycją samolotu PLUS przesunięcie o prędkość". Samolot nie jest jeszcze przesunięty!!!
                double distanceTraveledPlusDisplaceBy = new Segment(vehicle.Routes[vehicle.CurrentSegmentIndex].Begin, new Point(vehicle.Position.X + displaceByX, vehicle.Position.Y + displaceByY)).getLength();


                if (!vehicle.ReachedDestination) //jeżeli samolot nie dotarł do celu
                {

                    if (distanceTraveledPlusDisplaceBy > routeLength) //jeżeli długość trasy po przesunięciu jest dłuższa od trasy, to oznacza że samolot dotrze do celu w tej klatce, jednak nie możemy pozwolić, żeby samolot wyleciał "za" punkt końcowy trasy
                    {
                        if (vehicle.CurrentSegmentIndex == vehicle.Routes.Count - 1) //samolot jest na ostatnim odcinku trasy i chce wykroczyć poza końcowy punkt
                        {
                            Console.WriteLine("ENDENDEND");
                            vehicle.Position = vehicle.Routes[vehicle.Routes.Count - 1].End; //jeżeli samolot próbuje przekroczyć końcowy punkt trasy, ustaw poz. samolotu na koniec trasy
                            vehicle.ReachedDestination = true; //samolot doleciał do celu - nie poruszamy nim już w następnych klatkach (chyba że wywołamy changeRoute później)
                            vehicle.Height = 0; //samolot wylądował
                        }
                        else
                        {
                            vehicle.Position = vehicle.Routes[vehicle.CurrentSegmentIndex].End;
                            vehicle.CurrentSegmentIndex++;
                            vehicle.Route = vehicle.Routes[vehicle.CurrentSegmentIndex];
                            Console.WriteLine("SWITCHING ROUTES");
                        }
                    }
                    else //jeżeli samolot nie próbuje przekroczyć pkt. końcowego trasy, to wszystko ok  - przesuń samolot
                    {
                        vehicle.Position.X += displaceByX;
                        vehicle.Position.Y += displaceByY;
                    }

                }
            }
            collisions = DetectAllCollisions(this);
        }
        public List<Collision> DetectAllCollisions(Map map)
        {
            List<Collision> colls = new List<Collision>();

            foreach (Vehicle vhc in vehicles)
            {
                List<Obstacle> list = vhc.detectCollisions(map);

                foreach (Obstacle obs in list)
                {
                    colls.Add(new Collision(vhc, obs));
                }
            }

            for (int i = 0; i < colls.Count; i++)
            {
                for (int j = i + 1; j < colls.Count; j++)
                {
                    if ( (colls[i].Obs == colls[j].Obs && colls[i].Vhc == colls[j].Vhc) ||
                         (colls[i].Vhc.Position.X == colls[j].Obs.Position.X && colls[i].Vhc.Position.Y == colls[j].Obs.Position.Y))
                    {
                        colls.RemoveAt(j);
                    }
                }
            }

            return colls;
        }

        public List<Collision> DetectAllProximities(Map map)
        {
            List<Collision> colls = new List<Collision>();

            foreach (Vehicle vhc in vehicles)
            {
                List<Obstacle> list = vhc.detectProximity(map);

                foreach (Obstacle obs in list)
                {
                    colls.Add(new Collision(vhc, obs));
                }
            }

            for (int i = 0; i < colls.Count; i++)
            {
                for (int j = i + 1; j < colls.Count; j++)
                {
                    if ((colls[i].Obs == colls[j].Obs && colls[i].Vhc == colls[j].Vhc) ||
                         (colls[i].Vhc.Position.X == colls[j].Obs.Position.X && colls[i].Vhc.Position.Y == colls[j].Obs.Position.Y))
                    {
                        colls.RemoveAt(j);
                    }
                }
            }

            return colls;
        }
    }

    public class Obstacle //nie dziedziczy z Map, jest wolnostojącym obiektem
    {
        Point position = new Point(); //pozycja x, y przeszkody
        double width, length, height; //szerokosc długosc wysokośc przeszkody (Nie ma tego w UMLu a powinno być) WZGLĘDEM LEWEGO GORNEGO ROGU

        public Obstacle() //konstruktor używany głównie podczas testowania
        {
            Position = new Point(Position.X = Constants.mapSizeX / 2, Position.Y = Constants.mapSizeY / 2);
            width = length = height = 20;
        }
        public Obstacle(double _x, double _y, double _width, double _length, double _height)
        {
            position = new Point();
            Position.X = _x;
            Position.Y = _y;
            width = _width;
            length = _length;
            height = _height;
        }

        public double Width { get => width; set => width = value; }
        public double Length { get => length; set => length = value; }
        public double Height { get => height; set => height = value; }
        public Point Position { get => position; set => position = value; }

        /// <summary>
        /// Sprawdza czy jakikolwiek pojazd latający z _map ma zderzy się z tą przeszkodą w następnej klatce
        /// </summary>
        /// <param name="_map">Mapa w której sprawdzamy kolizje</param>
        /// <returns>Zwraca tablicę pojazdów latających które zderzą się z tą przeszkodą w następnej klatce</returns>
        virtual public List<Obstacle> detectCollisions(Map _map) //sprawdzamy czy z przeszkodą naziemną w następnej klatce nie zderzy się żaden samolot (jeżeli się zderzy to następnie w MainWindow.xaml.cs jest to przechwytywane)
        {
            List<Obstacle> collisions = new List<Obstacle>(); //tablica samolotów które mogą kolidować z daną przeszkodą naziemną
            Segment obstacleBottom = new Segment(position, new Point(position.X + width, position.Y));
            Segment obstacleTop = new Segment(new Point(position.X, position.Y + length), new Point(position.X + width, position.Y + length));
            Segment obstacleLeft = new Segment(position, new Point(position.X, position.Y + length));
            Segment obstacleRight = new Segment(new Point(position.X + width, position.Y), new Point(position.X + width, position.Y));

            foreach (Vehicle vehicle in _map.Vehicles) //każdy Obstacle sprawdza czy żaden Vehicle się z nim nie zderzy
            {
                if (vehicle.getGhostRoute().checkIntersection(obstacleBottom) || //jeżeli samolot chce przelecieć przez jakikolwiek z boków przeszkody
                   vehicle.getGhostRoute().checkIntersection(obstacleTop) ||
                   vehicle.getGhostRoute().checkIntersection(obstacleLeft) ||
                   vehicle.getGhostRoute().checkIntersection(obstacleBottom))
                {
                    collisions.Add(vehicle);
                }

            }

            return collisions;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_map"></param>
        /// <returns>Listę pojazdów które są niebezpiecznie blisko tej przeszkody</returns>
        virtual public List<Obstacle> detectProximity(Map _map) //sprawdzamy czy z przeszkodą naziemną w następnej klatce nie zderzy się żaden samolot (jeżeli się zderzy to następnie w MainWindow.xaml.cs jest to przechwytywane)
        {
            List<Obstacle> proximityWarnings = new List<Obstacle>(); //tablica samolotów które mogą kolidować z daną przeszkodą naziemną

            double getVector3Length(double x1, double y1, double z1, double x2, double y2, double z2)
            {
                double xDisplacement = Math.Abs(x2 - x1);
                double yDisplacement = Math.Abs(y2 - y1);
                double zDisplacement = Math.Abs(z2 - z1);

                return Math.Sqrt(Math.Pow(xDisplacement, 2) + Math.Pow(yDisplacement, 2) + Math.Pow(zDisplacement, 2));
            }

            foreach (Vehicle vehicle in _map.Vehicles) //każdy Obstacle sprawdza czy żaden Vehicle się z nim nie zderzy
            {
                if (getVector3Length(Position.X, Position.Y, height, vehicle.Position.X, vehicle.Position.Y, vehicle.Routes[vehicle.CurrentSegmentIndex].Height) <= Constants.proximityWarningThreshold)
                {
                    proximityWarnings.Add(vehicle);
                }

            }

            return proximityWarnings;
        }

    }



    public class Vehicle : Obstacle //Vehicle dziedziczy z obstacle
    {
        //Position odziedziczone z obstacle
        private List<Segment> routes;//trasa samolotu zaczynająca się na (xstart, ystart) a kończąca sie (xend, yend) - patrz konstruktor
        private Segment route; //kopia odcinka po którym porusza się aktualnie samolot
        private int currentSegmentIndex; //indeks odcinka po którym aktualnie porusza się samolot znajdujący się w List<Segment> routes;
        private bool reachedDestination; //czy samolot dotarł do punktu końcowego ostatniego odcinka trasy? jeżeli tak, to nextFrame ignoruje ten samolot i nim nie porusza


        public Vehicle()
        {
            routes = new List<Segment>();
            route = new Segment();
            currentSegmentIndex = 0;
            ReachedDestination = false;
        }

        public Vehicle(List<Segment> _routes)
        {
            routes = _routes;
            currentSegmentIndex = 0;
            if (_routes.Any()) route = routes[currentSegmentIndex];
            ReachedDestination = false;
        }

        public List<Segment> Routes { get => routes; set => routes = value; }
        public bool ReachedDestination { get => reachedDestination; set => reachedDestination = value; }
        public int CurrentSegmentIndex { get => currentSegmentIndex; set => currentSegmentIndex = value; }
        public Segment Route { get => route; set => route = value; }

        /// <summary>
        /// Zmień trasę samolotu. Usuwa aktualną drogę i tworzy prostą linię z obecnej pozycji do wybranej w argumencie.
        /// </summary>
        /// <param name="_xend">Współrzędna X nowego celu</param>
        /// <param name="_yend">Współrzędna Y nowego celu</param>
        /// <param name="_height">Wysokość po której będzie poruszał się samolot w nowej trasie</param>
        public void changeRoute(double _xend, double _yend, double _height)
        {

            //TODO: change of route in the middle of new segment splits segment into two and adds the change route segment to the list

            Routes[currentSegmentIndex].End = new Point(_xend, _yend);
            Height = _height;
            ReachedDestination = false; //na wypadek jeżeli zmieniamy trasę samolotu który dotarł do celu i na nim stoi (logika poruszania jest wyłączona dla samolotów które dotarły do celu)
        }

        /// <summary>
        /// Zwraca odcinek między obecną pozycją pojazdu a pozycją w następnej klatce (przesunięcie o prędkość)
        /// </summary>
        public Segment getGhostRoute()
        {
            if (reachedDestination) return new Segment(new Point(0, 0), new Point(0, 0));
            double speed = Routes[currentSegmentIndex].Speed;
            double horizontalDisplacement = Routes[currentSegmentIndex].End.X - Routes[currentSegmentIndex].Begin.X;
            double verticalDisplacement = Routes[currentSegmentIndex].End.Y - Routes[currentSegmentIndex].Begin.Y;
            double routeLength = Routes[currentSegmentIndex].getLength();
            Segment routeLeft = new Segment(Position, Routes[currentSegmentIndex].End);

            double displaceByY = (verticalDisplacement / routeLength) * speed;
            double displaceByX = (horizontalDisplacement / routeLength) * speed;


            
            Segment distanceTraveledPlusDisplaceBy = new Segment(Position, new Point(Position.X + displaceByX, Position.Y + displaceByY));
            if (routeLeft.getLength() < distanceTraveledPlusDisplaceBy.getLength())
                return routeLeft;
            else
            return distanceTraveledPlusDisplaceBy;
        }
        //metoda public Vehicle detectCollisions() odziedziczony z Obstacle tutaj (dla przypomnienia)

        public override List<Obstacle> detectCollisions(Map _map)
        {
            List<Obstacle> collisions = new List<Obstacle>();

            foreach (Vehicle vehicle in _map.Vehicles) //każdy Vehicle sprawdza czy żaden Vehicle się z nim nie zderzy
            {
                if (vehicle == this) continue;
                if (vehicle.getGhostRoute().checkIntersection(this.getGhostRoute()) && Math.Abs(vehicle.Routes[vehicle.currentSegmentIndex].Height - Routes[CurrentSegmentIndex].Height) <= Constants.colisionThreshold) //jeżeli trasy się przecinają i wysokości różnią się od siebie o max 5 to mamy kolizje
                {
                    collisions.Add(vehicle);
                    Console.WriteLine("collision detected");
                }


            }

            return collisions;
        }

        public override List<Obstacle> detectProximity(Map _map) //sprawdzamy czy z przeszkodą naziemną w następnej klatce nie zderzy się żaden samolot (jeżeli się zderzy to następnie w MainWindow.xaml.cs jest to przechwytywane)
        {
            List<Obstacle> proximityWarnings = new List<Obstacle>(); //tablica samolotów które mogą być zbyt blisko z danym samolotem

            double getVector3Length(double x1, double y1, double z1, double x2, double y2, double z2)

            {
                double xDisplacement = Math.Abs(x2 - x1);
                double yDisplacement = Math.Abs(y2 - y1);
                double zDisplacement = Math.Abs(z2 - z1);

                return Math.Sqrt(Math.Pow(xDisplacement, 2) + Math.Pow(yDisplacement, 2) + Math.Pow(zDisplacement, 2));
            }

            foreach (Vehicle vehicle in _map.Vehicles) //każdy Obstacle sprawdza czy żaden Vehicle się z nim nie zderzy
            {
                if (getVector3Length(Position.X, Position.Y, Routes[CurrentSegmentIndex].Height, vehicle.Position.X, vehicle.Position.Y, vehicle.Routes[vehicle.CurrentSegmentIndex].Height) <= Constants.proximityWarningThreshold)
                {
                    proximityWarnings.Add(vehicle);
                }

            }

            return proximityWarnings;
        }
    }


    //---------------------------------------RODZAJE POJAZDOW DZIEDZICZACE Z VEHICLE (BALON, HELIKOPTER ETC.)--------------------------------------

    public class Helicopter : Vehicle
    {
        public Helicopter() : base()
        {
            Height = Constants.startingVehicleHeight;
        }
    }

    public class Glider : Vehicle
    {
        public Glider() : base()
        {
            Height = Constants.startingVehicleHeight;
        }
    }

    public class Plane : Vehicle
    {
        public Plane() : base()
        {
            Height = Constants.startingVehicleHeight;
        }
    }

    public class Balloon : Vehicle
    {
        public Balloon() : base()
        {
            Height = Constants.startingVehicleHeight;
        }
    }


    public class Point
    {
        private double x;
        private double y;

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }

        ///<summary>Stwórz punkt o podanych współrzędnych</summary>
        public Point(double _x, double _y) { x = _x; y = _y; }

        ///<summary>Stwórz punkt o współrzędnych(0,0)</summary>
        public Point()
        {
            x = 0;
            y = 0;
        }

        ///<summary>Zwraca odległość między tym punktem a punktem z argumentu</summary>
        public double lengthFrom(Point end)
        {
            double horizontalDisplacement = end.X - x;
            double verticalDisplacement = end.Y - y;
            return Math.Sqrt(Math.Abs(horizontalDisplacement * horizontalDisplacement + verticalDisplacement * verticalDisplacement));
        }

    }

    public class Collision
    {
        private Vehicle vhc;
        private Obstacle obs;

        public Vehicle Vhc { get { return vhc; } private set { vhc = value; } }
        public Obstacle Obs { get { return obs; } private set { obs = value; } }

        public Collision(Vehicle _vhc, Obstacle _obs)
        {
            vhc = _vhc;
            obs = _obs;
        }
    }

    public class Segment ///<summary>przechowuje se odcinek heh</summary>
    {

        private Point begin, end;
        private double speed;
        private double height;

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

        public double Speed { get => speed; set => speed = value; }
        public double Height { get => height; set => height = value; }


        ///<summary>Stwórz odcinek z współrzędnych 4 składowych</summary>
        public Segment(double xbegin, double ybegin, double xend, double yend)
        {
            begin = new Point(xbegin, ybegin);
            end = new Point(xend, yend);
        }

        ///<summary>Tworzy zwykły (nietrasowy) odcinek zaczynający się na begin a kończący się na end</summary>
        public Segment(Point _begin, Point _end) ///<summary>przechowuje se odcinek heh</summary>
        {
            begin = _begin;
            end = _end;
        }

        ///<summary>Tworzy odcinek będący częścią trasy (mający prędkość i wysokość)</summary>
        public Segment(Point _begin, Point _end, double _speed, double _height)
        {
            begin = _begin;
            end = _end;
            speed = _speed;
            height = _height;
        }

        ///<summary>Zwraca odcinek idący od (0,0) do górnego prawego rogu mapy</summary>
        public Segment()
        {
            begin = new Point(0, 0);
            end = new Point(Constants.mapSizeX, Constants.mapSizeY);
        }

        ///<summary>Zwraca długość odcinka</summary>
        public double getLength()
        {
            return begin.lengthFrom(end);
        }

        ///<summary>Sprawdza czy ten odcinek przecina się z drugim odcinkiem</summary>
        public bool checkIntersection(Segment segment)
        {
            double crossProductBegin = (segment.End.X - segment.Begin.X) * (Begin.Y - segment.End.Y) - (segment.End.Y - segment.Begin.Y) * (Begin.X - segment.End.X); //zmienne potrzebne do algorytmu sprawdzania czy dwa odcinki się przecinają
            double crossProductEnd = (segment.End.X - segment.Begin.X) * (End.Y - segment.End.Y) - (segment.End.Y - segment.Begin.Y) * (End.X - segment.End.X);

            double intersectionTestResult = Math.Sign(crossProductBegin) * Math.Sign(crossProductEnd); //jeżeli obie zmienne crossProduct są przeciwnych znaków, to odcinki się nie przecinają, jeżeli są tych samych znaków, to się przecinają

            if (intersectionTestResult >= 0) return false; //opisane w powyższym komentarzu
            else return true;
        }

    }
}

//class Program
//{
//    static void Main(string[] args)
//    {
//        Map map = new Map();
//        Plane a = new Plane();
//        Plane b = new Plane();


//        a.Position = new Point(0, 0);
//        Segment A1 = new Segment(new Point(0, 0), new Point(200, 200), 100);
//        Segment A2 = new Segment(new Point(200, 200), new Point(0, 400), 50);

//        a.Routes = new List<Segment> { A1, A2 };

//        map.addVehicle(a);


//        b.Position = new Point(200, 0);
//        Segment B1 = new Segment(new Point(200, 0), new Point(0, 200), 100);
//        Segment B2 = new Segment(new Point(0, 200), new Point(200, 400), 50);
//        b.Routes = new List<Segment> { B1, B2 };

//        Console.WriteLine(b.Routes[0].Begin.X);

//        map.addVehicle(b);
//        map.nextFrame();

//        while(true)
//        {
//            Console.WriteLine(a.Position.X + " " + a.Position.Y);
//            Console.WriteLine(b.Position.X + " " + b.Position.Y);
//            foreach (Obstacle vehicle in map.Vehicles)
//            {
//                List<Obstacle> colli = vehicle.detectCollisions(map);
//                Console.WriteLine(colli.Count);
//                foreach (Obstacle collider in colli)
//                {
//                    Console.WriteLine(nameof(vehicle) + " colliding with " + nameof(collider));
//                }
//            }
//            map.nextFrame();
//            int input = Convert.ToInt32(Console.ReadLine());
//            if (input == 1) break;
//        }
//    }


//class Program
//    {
//        static void Main(string[] args)
//        {
//            Map map = new Map();
//            Helicopter ehh = new Helicopter();

//            ehh.Position = new Point(150, 450);
//            ehh.Route.Begin = new Point(150, 450);
//            ehh.Route.End = new Point(400, 400);

//            map.addVehicle(ehh);

//            Console.WriteLine("X: " + ehh.Position.X + "Y: " + ehh.Position.Y);
//            map.nextFrame();
//            Console.WriteLine("\nX: " + ehh.Position.X + "Y: " + ehh.Position.Y);
//            map.nextFrame();
//            Console.WriteLine("\nX: " + ehh.Position.X + "Y: " + ehh.Position.Y);
//            map.nextFrame();
//            map.nextFrame();
//            map.nextFrame();
//            map.nextFrame();
//            map.nextFrame();
//            Console.WriteLine("\nX: " + ehh.Position.X + "Y: " + ehh.Position.Y);
//        }
//    }
// }

// foreach (Obstacle vehicle in map.Vehicles)
// {
//                    List<Obstacle> colli = vehicle.detectCollisions(map);
//                    if(colli.Any()) Console.WriteLine("unresolved collisions - pausing radar");
//}

//class Program
//{
//    static void Main(string[] args)
//    {
//        Map map = new Map();
//        Plane a = new Plane();
//        Plane b = new Plane();

//        a.Position = new Point(0, 0);
//        a.Route.Begin = new Point(0, 0);
//        a.Route.End = new Point(400, 400);

//        map.addVehicle(a);

//        b.Position = new Point(400, 0);
//        b.Route.Begin = new Point(400, 0);
//        b.Route.End = new Point(0, 400);

//        map.addVehicle(b);
//        map.nextFrame();

//        Console.WriteLine(a.Route.checkIntersection(b.Route));
//        Console.WriteLine(a.getGhostRoute().checkIntersection(b.getGhostRoute()));
//        foreach (Obstacle vehicle in b.detectCollisions(map))
//        {
//            Console.WriteLine(vehicle.Position.X + " " + vehicle.Position.Y);
//        }

//    }
//}


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