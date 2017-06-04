//TODO: zrobić wczytywanie Obstacle z pliku
//TODO: zrobić wykrywanie zbliżeń
//TODO: zrobić zmienianie trasy


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt_PO
{

    /// <summary>
    /// Klasa zawierająca wszelkie globalne zmienne konfiguracyjne (średnie prędkości dla poszczególnych pojazdów, globalny rozmiar mapy itd.). Więcej szczegółów niżej
    /// </summary>
    public static class Constants
    {
        public const double mapSizeX = 500; //rozmiar mapy
        public const double mapSizeY = 500;
        public const double startingVehicleHeight = 0;  //Wartości poczatkowej wysokosci są skonfigurowane aby zawierać się w granicy [startingVehicleHeight/2, startingVehicleHeight*2]. Zatem samoloty są generowane w taki sposób, że ich wysokość początkowa (w zerowej klatce) zawiera się w tej granicy
        public const int routesMaxNumberOfSegments = 5; //Maksymalna ilość odcinków z której składa się trasa generowanych poj. latających
        public const int proximityWarningThreshold = 100; //Jeżeli odległosć między dwoma poj. latającymi albo poj. latającym i przeszkodą jest mniejsza niż ta, to wysyłane jest ostrzeżenie o zbliżeniu
        public static int colisionThreshold = 5; //Jeżeli odległość między dwoma poj. latającymi albo poj. latającym i przeszkodą w następnej klatce będzie wynosić mniej niż wartość tu podana, to program traktuje takie zbliżenie jako kolizję - program jest pauzowany.
        public static double routeMinLength = 20;
        public static double routeMaxLength = 300;

        public class Plane
        {
            public const double speed = 50; //średnia wartość prędkości dla obiektu Plane. Prędkość poruszania na poszczególnych odcinkach tras obiektów Plane jest obliczana na podstawie tej wartości. Prędkości są generowane w granicach [speed/2, speed*2]
        }
        public class Balloon
        {
            public const double speed = 10; //to samo co wyżej, tylko dla Balloon
        }

        public class Glider
        {
            public const double speed = 40;
        }

        public class Helicopter
        {
            public const double speed = 30;
        }
    }

    /// <summary>
    /// Klasa zajmująca się losową generacją pojazdów, preszkód i tras dla pojazdów
    /// </summary>
    public class Generator
    {
        /// <summary>
        /// Generuje losowo trasę (z odpowiednią wysokością i prędkością (w zależności od pojazdu))
        /// </summary>
        /// <param name="routeLengthInSegments">Z ilu odcinków ma się składać trasa</param>
        /// <param name="vehicle">dla jakiego rodzaju pojazdu jest generowana trasa</param>
        /// <returns>Lista zawierająca odcinki składowe trasy</returns>
        public List<Segment> generateRoutes(int routeLengthInSegments, Vehicle vehicle)
        {
            List<Segment> generatedRoutes = new List<Segment>(); //lista tras zwracana na końcu programu
            Random numberGenerator = new Random(Guid.NewGuid().GetHashCode()); //generator liczb losowych
            Point lastSegmentsEnd = new Point(); //zmienna przechowywująca koniec ostatniego wygenerowanego odcinka


            ///Generuje losowo prędkość na danym odcinku. Prędkość zależy od "średniej" prędkości dla danego pojazdu. np. Plane jest w 100% przypadków szybszy niż Balloon
            double generateSpeed()
            {
                double generatedSpeed = 0;

                //prędkość zależy od typu pojazdu latającego               | Wygeneruj prędkość między wartością                        tą\/                                             a tą \/
                if (vehicle.GetType().Name == "Helicopter") generatedSpeed = numberGenerator.Next(Convert.ToInt32(Constants.Helicopter.speed / 2), Convert.ToInt32(Constants.Helicopter.speed * 2));
                else if (vehicle.GetType().Name == "Glider") generatedSpeed = numberGenerator.Next(Convert.ToInt32(Constants.Glider.speed / 2), Convert.ToInt32(Constants.Glider.speed * 2));
                else if (vehicle.GetType().Name == "Plane") generatedSpeed = numberGenerator.Next(Convert.ToInt32(Constants.Plane.speed / 2), Convert.ToInt32(Constants.Plane.speed * 2));
                else if (vehicle.GetType().Name == "Balloon") generatedSpeed = numberGenerator.Next(Convert.ToInt32(Constants.Balloon.speed / 2), Convert.ToInt32(Constants.Balloon.speed * 2));
                else throw new NotImplementedException();

                return generatedSpeed;
            }

            ///Generuje losowo wysokość dla danego odcinka na podstawie wartości konfiguracyjnych z klasy Constants
            double generateHeight()
            {
                return numberGenerator.Next(Convert.ToInt32(Constants.startingVehicleHeight / 2), Convert.ToInt32(Constants.startingVehicleHeight * 2));
            }
            ///Generuje punkt o losowych współrzędnych X i Y
            Point generatePoint()
            {
                Point generatedPoint = new Point(numberGenerator.Next(0, Convert.ToInt32(Constants.mapSizeX)), numberGenerator.Next(0, Convert.ToInt32(Constants.mapSizeY)));
                return generatedPoint;
            }


            //wygeneruj routeLengthInSegments odcinków
            for (int i = 0; i < routeLengthInSegments; i++)
            {
                Point startingPoint;
                if (i == 0) //jeżeli jest to pierwszy generowany odcinek
                {
                    startingPoint = generatePoint(); //wygeneruj punkt początkowy trasy ...
                    vehicle.Position = startingPoint; //i ustaw ten punkt jako pozycję pojazdu latającego
                }
                else
                    startingPoint = lastSegmentsEnd; //jeżeli jest to n-ty generowany odcinek (nie pierwszy), to wiemy że np. drugi odcinek będzie miał początek tam gdzie pierwszy miał koniec. zatem ustawiamy początek nowego odcinka tam gdzie ostatnio stworzony się kończył

                Point endingPoint = generatePoint(); //wygeneruj losowo punkt końcowy



                //jeżeli wygenerowany odcinek jest zbyt krótki lub długi, wygeneruj nowy
                while (startingPoint.lengthFrom(endingPoint) < Constants.routeMinLength || startingPoint.lengthFrom(endingPoint) > Constants.routeMaxLength)
                {
                    endingPoint = generatePoint();
                }

                lastSegmentsEnd = endingPoint; //odśwież wartość punktu końcowego (aby kolejny odcinek wiedział gdzie postawić punkt początkowy)

                //dodaj odcinek o wygenerowanych wcześniej parametrach do listy odcinków zwracanej na końcu programu
                generatedRoutes.Add(new Segment(startingPoint, endingPoint, generateSpeed(), generateHeight()));
            }

            for (int i = 1; i < routeLengthInSegments; i++)
            {
                if(generatedRoutes[i-1].End != generatedRoutes[i].Begin)
                {
                    Console.WriteLine("YOU DUN GOOFED");
                    Console.WriteLine(vehicle.Position.X + " " + vehicle.Position.Y);
                    break;
                }
            }



            return generatedRoutes;
        }

        /// <summary>
        /// Generuje podaną ilośc pojazdów wraz z trasami (na podstawie wartości konfiguracyjnych w klasie Constants)
        /// </summary>
        /// <param name="numberOfVehicles">Ile pojazdów wygenerować</param>
        /// <returns>Lista wygenerowanych pojazdów</returns>
        public List<Vehicle> generateVehicles(int numberOfVehicles)
        {
            List<Vehicle> generatedVehicles = new List<Vehicle>();
            Vehicle generatedVehicle = new Vehicle();
            Random numberGenerator = new Random(Guid.NewGuid().GetHashCode());

            for (int i = 0; i < numberOfVehicles; i++) // wygeneruj numberOfVehicles pojazdów
            {
                int aircraftPickerRandom = numberGenerator.Next() % 4; //wybierz losowo jaki pojazd zostanie utworzony

                if (aircraftPickerRandom == 0) generatedVehicle = new Helicopter(); //losowanie jakim rodzajem pojazdu będzie wygenerowany pojazd
                else if (aircraftPickerRandom == 1) generatedVehicle = new Glider();
                else if (aircraftPickerRandom == 2) generatedVehicle = new Plane();
                else if (aircraftPickerRandom == 3) generatedVehicle = new Balloon();


                //wygeneruj trasę składającą się z losowej liczby odcinków
                generatedVehicle.Routes = generateRoutes((numberGenerator.Next() % Constants.routesMaxNumberOfSegments) + 1, generatedVehicle); //routesMaxNumberOfSegments+1 bo nie może być 0 odcinków, a jednym z wyników modulo jest 0

                generatedVehicle.CurrentSegmentIndex = 0; //poj. latający po stworzeniu zaczyna zawsze od pierwszego odcinka trasy

                generatedVehicles.Add(generatedVehicle); //dodaj wygenerowany pojazd do listy wygenerowanych pojazdów
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
        private List<Vehicle> vehicles; //lista pojazdów na mapie
        private List<Obstacle> obstacles; //lista przeszkód na mapie
        private List<Collision> collisions; //lista kolizji na mapie (zmienia się w trakcie wykonywania programu)
        private List<Collision> proximities; //lista zbliżeń na mapie

        private double mapSizeX, mapSizeY; //rozmiar mapy

        public Map()
        {
            mapSizeX = Constants.mapSizeX;
            mapSizeY = Constants.mapSizeY;

            vehicles = new List<Vehicle>();
            obstacles = new List<Obstacle>();
            collisions = new List<Collision>();
        }


        public List<Vehicle> Vehicles { get => vehicles; set => vehicles = value; }
        public List<Obstacle> Obstacles { get => obstacles; set => obstacles = value; }
        public List<Collision> Collisions { get => collisions; set => collisions = value; }
        public List<Collision> Proximities { get => proximities; set => proximities = value; }

        /// <summary>
        /// Dodaje pojazd latający do Map
        /// </summary>
        /// <param name="_vehicle">Dodawany pojazd</param>
        public void addVehicle(Vehicle _vehicle)
        {
            vehicles.Add(_vehicle);
            //właściwie lista vehicles jest zakapsułowana i ma własny setter, ale metoda została zachowana ze względu na kompatybilność wsteczną
        }

        /// <summary>
        /// Wprowadza dane przeszkód naziemnych do Mapy
        /// </summary>
        /// <param name="PATH">Ścieżka do pliku w formacie "X:\folder\text.txt"</param>
        /// <returns>Powodzenie operacji ładowania</returns>
        public bool loadObstaclesFromFile(String PATH)
        {
            String[] data = System.IO.File.ReadAllLines(@PATH); //plik z PATH ma specyfikacje Obstacle każdy w oddzielnej linii (5 Obstacle = 5 linii)
            foreach (String item in data)
            {
                String[] line = item.Split(null); //rozdzielaj po spacji
                Obstacle currentlyAdded = new Obstacle(Convert.ToInt32(line[0]), Convert.ToInt32(line[1]), Convert.ToInt32(line[2]), Convert.ToInt32(line[3]), Convert.ToInt32(line[4]));
            }

            
            return true;
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
            proximities = DetectAllProximities(this);
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
                    if ((colls[i].Obs == colls[j].Obs && colls[i].Vhc == colls[j].Vhc) ||
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
                if(vehicle == this) { continue; }

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


            Routes.RemoveRange(currentSegmentIndex - 1, Routes.Count() - currentSegmentIndex - 1); //wyrzuć wszystkie odcinki przed aktualnym oraz aktualny
            Routes.Add(new Segment(Routes[currentSegmentIndex - 1].End, Position)); //stwórz nowy odcinek między końcem ostatniego istniejącego a obecną pozycją samolotu
            currentSegmentIndex++; //jesteśmy teraz na nowo stworzonym odcinku
            Routes.Add(new Segment(Position, new Point(_xend, _yend))); //tworzymy nowy odcinek do przebycia

            //cały algorytm jest tak skomplikowany poniewaz podejrzewam że kolega Michał z którym robię projekt będzie wyświetlał trasę na podstawie tablicy Routes, dlatego wolę jej nie kasować całej i zaczynać od nowego odcinka tylko ją "przetworzyć"

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
            if (reachedDestination) return new List<Obstacle>(); //nie zderza się z niczym jeżeli wylądował

            else
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
                if (vehicle == this) { continue; }

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

        //kod autorstwa Martin Thoma (licencja poniżej)
        public Point[] getBoundingBox()
        {
            Point[] result = new Point[2];
            result[0] = new Point(Math.Min(Begin.X, End.X), Math.Min(Begin.Y,
                    End.Y));
            result[1] = new Point(Math.Max(Begin.X, End.X), Math.Max(Begin.Y,
                    End.Y));
            return result;
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
            return Geometry.doLinesIntersect(segment, new Segment(Begin, End));
        }

    }
    
    //The MIT License

    //Copyright(c) 2012-2013 Martin Thoma

    //Permission is hereby granted, free of charge, to any person obtaining a copy
    //of this software and associated documentation files (the "Software"), to deal
    //in the Software without restriction, including without limitation the rights
    //to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    //copies of the Software, and to permit persons to whom the Software is
    //furnished to do so, subject to the following conditions:

    //The above copyright notice and this permission notice shall be included in
    //all copies or substantial portions of the Software.

    //THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    //IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    //FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    //AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    //LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    //OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    //THE SOFTWARE.

    //Zmodyfikowany kod autorstwa Martin Thoma (https://github.com/MartinThoma/algorithms/blob/master/crossingLineCheck/Geometry/src/Geometry.java)


    public class Geometry
    {

        public static double EPSILON = 0.000001;

        /**
         * Calculate the cross product of two points.
         * @param a first point
         * @param b second point
         * @return the value of the cross product
         */
        public static double crossProduct(Point a, Point b)
        {
            return a.X * b.Y - b.X * a.Y;
        }

        /**
         * Check if bounding boxes do intersect. If one bounding box
         * touches the other, they do intersect.
         * @param a first bounding box
         * @param b second bounding box
         * @return <code>true</code> if they intersect,
         *         <code>false</code> otherwise.
         */
        public static bool doBoundingBoxesIntersect(Point[] a, Point[] b)
        {
            return a[0].X <= b[1].X && a[1].X >= b[0].X && a[0].Y <= b[1].Y
                    && a[1].Y >= b[0].Y;
        }

        /**
         * Checks if a Point is on a line
         * @param a line (interpreted as line, although given as line
         *                segment)
         * @param b point
         * @return <code>true</code> if point is on line, otherwise
         *         <code>false</code>
         */
        public static bool isPointOnLine(Segment a, Point b)
        {
            // Move the image, so that a.first is on (0|0)
            Segment aTmp = new Segment(new Point(0, 0), new Point(
                    a.End.X - a.Begin.X, a.End.Y - a.Begin.Y));
            Point bTmp = new Point(b.X - a.Begin.X, b.Y - a.Begin.Y);
            double r = crossProduct(aTmp.End, bTmp);
            return Math.Abs(r) < EPSILON;
        }

        /**
         * Checks if a point is right of a line. If the point is on the
         * line, it is not right of the line.
         * @param a line segment interpreted as a line
         * @param b the point
         * @return <code>true</code> if the point is right of the line,
         *         <code>false</code> otherwise
         */
        public static bool isPointRightOfLine(Segment a, Point b)
        {
            // Move the image, so that a.first is on (0|0)
            Segment aTmp = new Segment(new Point(0, 0), new Point(
                    a.End.X - a.Begin.X, a.End.Y - a.Begin.Y));
            Point bTmp = new Point(b.X - a.Begin.X, b.Y - a.Begin.Y);
            return crossProduct(aTmp.End, bTmp) < 0;
        }

        /**
         * Check if line segment first touches or crosses the line that is
         * defined by line segment second.
         *
         * @param first line segment interpreted as line
         * @param second line segment
         * @return <code>true</code> if line segment first touches or
         *                           crosses line second,
         *         <code>false</code> otherwise.
         */
        public static bool lineSegmentTouchesOrCrossesLine(Segment a,
                Segment b)
        {
            return isPointOnLine(a, b.Begin)
                    || isPointOnLine(a, b.End)
                    || (isPointRightOfLine(a, b.Begin) ^ isPointRightOfLine(a,
                            b.End));
        }

        /**
         * Check if line segments intersect
         * @param a first line segment
         * @param b second line segment
         * @return <code>true</code> if lines do intersect,
         *         <code>false</code> otherwise
         */
        public static bool doLinesIntersect(Segment a, Segment b)
        {
            Point[] box1 = a.getBoundingBox();
            Point[] box2 = b.getBoundingBox();
            return doBoundingBoxesIntersect(box1, box2)
                    && lineSegmentTouchesOrCrossesLine(a, b)
                    && lineSegmentTouchesOrCrossesLine(b, a);
        }
    }


}

