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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using projekt_PO;

namespace PO_wpf
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public VehicleObject vhco1;
        public VehicleObject vhco2;
        public MainWindow mainwindow;
        public Collision collision;
        public List<VehicleObject> vehicleobjectlist;
        public Map map;
        public Window1(List<VehicleObject> vhcobjlist, Map mp, Collision coll, MainWindow mw)
        {
            InitializeComponent();

            mainwindow = mw;
            collision = coll;
            vehicleobjectlist = vhcobjlist;
            map = mp;

            vhco1 = vhcobjlist.First(x => x.Vhc == coll.Vhc);
            vhco2 = vhcobjlist.First(x => x.Vhc.Position == coll.Obs.Position);

            v1Type.Text = vhco1.VehicleType;
            v2Type.Text = vhco2.VehicleType;

            BindAll1(vhco1);
            BindAll2(vhco2);
        }

        private void BindAll2(VehicleObject vhco2)
        {
            X7.Text = vhco2.Vhc.Position.X.ToString();
            Y7.Text = vhco2.Vhc.Position.Y.ToString();
            Z7.Text = vhco2.Vhc.Height.ToString();

            X8.Text = vhco2.Vhc.Routes[0].End.X.ToString();
            Y8.Text = vhco2.Vhc.Routes[0].End.Y.ToString();
            Z8.Text = vhco2.Vhc.Routes[0].Height.ToString();

            if (vhco2.Vhc.Routes.Count == 2)
            {
                X9.Text = vhco2.Vhc.Routes[1].End.X.ToString();
                Y9.Text = vhco2.Vhc.Routes[1].End.Y.ToString();
                Z9.Text = vhco2.Vhc.Routes[1].Height.ToString();
            }
            else
            {
                X9.IsReadOnly = true;
                Y9.IsReadOnly = true;
                Z9.IsReadOnly = true;
            }
            if (vhco2.Vhc.Routes.Count == 3)
            {
                X10.Text = vhco2.Vhc.Routes[2].End.X.ToString();
                Y10.Text = vhco2.Vhc.Routes[2].End.Y.ToString();
                Z10.Text = vhco2.Vhc.Routes[2].Height.ToString();
            }
            else
            {
                X10.IsReadOnly = true;
                Y10.IsReadOnly = true;
                Z10.IsReadOnly = true;
            }
            if (vhco2.Vhc.Routes.Count == 4)
            {
                X11.Text = vhco2.Vhc.Routes[3].End.X.ToString();
                Y11.Text = vhco2.Vhc.Routes[3].End.Y.ToString();
                Z11.Text = vhco2.Vhc.Routes[3].Height.ToString();
            }
            else
            {
                X11.IsReadOnly = true;
                Y11.IsReadOnly = true;
                Z11.IsReadOnly = true;
            }
            if (vhco2.Vhc.Routes.Count == 5)
            {
                X12.Text = vhco2.Vhc.Routes[4].End.X.ToString();
                Y12.Text = vhco2.Vhc.Routes[4].End.Y.ToString();
                Z12.Text = vhco2.Vhc.Routes[4].Height.ToString();
            }
            else
            {
                X12.IsReadOnly = true;
                Y12.IsReadOnly = true;
                Z12.IsReadOnly = true;
            }
        }

        private void BindAll1(VehicleObject vhco1)
        {
            X1.Text = vhco1.Vhc.Position.X.ToString();
            Y1.Text = vhco1.Vhc.Position.Y.ToString();
            Z1.Text = vhco1.Vhc.Height.ToString();

            X2.Text = vhco1.Vhc.Routes[0].End.X.ToString();
            Y2.Text = vhco1.Vhc.Routes[0].End.Y.ToString();
            Z2.Text = vhco1.Vhc.Routes[0].Height.ToString();

            if (vhco1.Vhc.Routes.Count == 2)
            {
                X3.Text = vhco1.Vhc.Routes[1].End.X.ToString();
                Y3.Text = vhco1.Vhc.Routes[1].End.Y.ToString();
                Z3.Text = vhco1.Vhc.Routes[1].Height.ToString();
            }
            else
            {
                X3.IsReadOnly = true;
                Y3.IsReadOnly = true;
                Z3.IsReadOnly = true;
            }
            if (vhco1.Vhc.Routes.Count == 3)
            {
                X4.Text = vhco1.Vhc.Routes[2].End.X.ToString();
                Y4.Text = vhco1.Vhc.Routes[2].End.Y.ToString();
                Z4.Text = vhco1.Vhc.Routes[2].Height.ToString();
            }
            else
            {
                X4.IsReadOnly = true;
                Y4.IsReadOnly = true;
                Z4.IsReadOnly = true;
            }
            if (vhco1.Vhc.Routes.Count == 4)
            {
                X5.Text = vhco1.Vhc.Routes[3].End.X.ToString();
                Y5.Text = vhco1.Vhc.Routes[3].End.Y.ToString();
                Z5.Text = vhco1.Vhc.Routes[3].Height.ToString();
            }
            else
            {
                X5.IsReadOnly = true;
                Y5.IsReadOnly = true;
                Z5.IsReadOnly = true;
            }
            if (vhco1.Vhc.Routes.Count == 5)
            {
                X6.Text = vhco1.Vhc.Routes[4].End.X.ToString();
                Y6.Text = vhco1.Vhc.Routes[4].End.Y.ToString();
                Z6.Text = vhco1.Vhc.Routes[4].Height.ToString();
            }
            else
            {
                X6.IsReadOnly = true;
                Y6.IsReadOnly = true;
                Z6.IsReadOnly = true;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            vhco1.Lines[vhco1.CurrentLineIndex].Stroke = System.Windows.Media.Brushes.Gray;
            vhco2.Lines[vhco2.CurrentLineIndex].Stroke = System.Windows.Media.Brushes.Gray;

            List<Segment> newRoutes = new List<Segment>();
            vhco1.CurrentLineIndex = 0;
            vhco2.CurrentLineIndex = 0;
            //vhco1.Vhc.CurrentSegmentIndex = 0;

            newRoutes.Add(new Segment(new projekt_PO.Point(Double.Parse(X1.Text), Double.Parse(Y1.Text)), new projekt_PO.Point(Double.Parse(X2.Text), Double.Parse(Y2.Text)), vhco1.Vhc.Routes[vhco1.Vhc.CurrentSegmentIndex].Speed, Double.Parse(Z2.Text)));
            if (X3.Text != "X")
            {
                newRoutes.Add(new Segment(new projekt_PO.Point(Double.Parse(X2.Text), Double.Parse(Y2.Text)), new projekt_PO.Point(Double.Parse(X3.Text), Double.Parse(Y3.Text)), vhco1.Vhc.Routes[vhco1.Vhc.CurrentSegmentIndex+1].Speed, Double.Parse(Z3.Text)));
            }
            if (X4.Text != "X")
            {
                newRoutes.Add(new Segment(new projekt_PO.Point(Double.Parse(X3.Text), Double.Parse(Y3.Text)), new projekt_PO.Point(Double.Parse(X4.Text), Double.Parse(Y4.Text)), vhco1.Vhc.Routes[vhco1.Vhc.CurrentSegmentIndex+2].Speed, Double.Parse(Z4.Text)));
            }
            if (X5.Text != "X")
            {
                newRoutes.Add(new Segment(new projekt_PO.Point(Double.Parse(X4.Text), Double.Parse(Y4.Text)), new projekt_PO.Point(Double.Parse(X5.Text), Double.Parse(Y5.Text)), vhco1.Vhc.Routes[vhco1.Vhc.CurrentSegmentIndex+3].Speed, Double.Parse(Z5.Text)));
            }
            if (X6.Text != "X")
            {
                newRoutes.Add(new Segment(new projekt_PO.Point(Double.Parse(X5.Text), Double.Parse(Y5.Text)), new projekt_PO.Point(Double.Parse(X6.Text), Double.Parse(Y6.Text)), vhco1.Vhc.Routes[vhco1.Vhc.CurrentSegmentIndex+4].Speed, Double.Parse(Z6.Text)));
            }

            vhco1.Vhc.Routes = newRoutes;

            foreach (Line l in vhco1.Lines)
            {
                mainwindow.MapCanvas.Children.Remove(l);
            }
            vhco1.Lines = mainwindow.AddLines(vhco1.Vhc);

            map.Collisions.RemoveAll(x => x.Vhc == vhco1.Vhc || x.Vhc.Position == collision.Obs.Position);

            List<Obstacle> list = vhco1.Vhc.detectCollisions(map);

            foreach (Obstacle obs in list)
            {
                map.Collisions.Add(new Collision(vhco1.Vhc, obs));
            }

            //VHCO2
            newRoutes = new List<Segment>();

            newRoutes.Add(new Segment(new projekt_PO.Point(Double.Parse(X7.Text), Double.Parse(Y7.Text)), new projekt_PO.Point(Double.Parse(X8.Text), Double.Parse(Y8.Text)), vhco2.Vhc.Routes[vhco2.Vhc.CurrentSegmentIndex].Speed, Double.Parse(Z8.Text)));
            if (X9.Text != "X")
            {
                newRoutes.Add(new Segment(new projekt_PO.Point(Double.Parse(X8.Text), Double.Parse(Y8.Text)), new projekt_PO.Point(Double.Parse(X9.Text), Double.Parse(Y9.Text)), vhco2.Vhc.Routes[vhco2.Vhc.CurrentSegmentIndex + 1].Speed, Double.Parse(Z9.Text)));
            }
            if (X10.Text != "X")
            {
                newRoutes.Add(new Segment(new projekt_PO.Point(Double.Parse(X9.Text), Double.Parse(Y9.Text)), new projekt_PO.Point(Double.Parse(X10.Text), Double.Parse(Y10.Text)), vhco2.Vhc.Routes[vhco2.Vhc.CurrentSegmentIndex + 2].Speed, Double.Parse(Z10.Text)));
            }
            if (X11.Text != "X")
            {
                newRoutes.Add(new Segment(new projekt_PO.Point(Double.Parse(X10.Text), Double.Parse(Y10.Text)), new projekt_PO.Point(Double.Parse(X11.Text), Double.Parse(Y11.Text)), vhco2.Vhc.Routes[vhco2.Vhc.CurrentSegmentIndex + 3].Speed, Double.Parse(Z11.Text)));
            }
            if (X12.Text != "X")
            {
                newRoutes.Add(new Segment(new projekt_PO.Point(Double.Parse(X11.Text), Double.Parse(Y11.Text)), new projekt_PO.Point(Double.Parse(X12.Text), Double.Parse(Y12.Text)), vhco2.Vhc.Routes[vhco2.Vhc.CurrentSegmentIndex + 4].Speed, Double.Parse(Z12.Text)));
            }

            vhco2.Vhc.Routes = newRoutes;

            foreach (Line l in vhco2.Lines)
            {
                mainwindow.MapCanvas.Children.Remove(l);
            }
            vhco2.Lines = mainwindow.AddLines(vhco2.Vhc);

            list = vhco2.Vhc.detectCollisions(map);

            foreach (Obstacle obs in list)
            {
                map.Collisions.Add(new Collision(vhco2.Vhc, obs));
            }

            map.Collisions.RemoveAll(x => x.Vhc == vhco1.Vhc || x.Vhc.Position == collision.Obs.Position);

            for (int i = 0; i < map.Collisions.Count; i++)
            {
                for (int j = i + 1; j < map.Collisions.Count; j++)
                {
                    if ((map.Collisions[i].Obs == map.Collisions[j].Obs && map.Collisions[i].Vhc == map.Collisions[j].Vhc) ||
                         (map.Collisions[i].Vhc.Position.X == map.Collisions[j].Obs.Position.X && map.Collisions[i].Vhc.Position.Y == map.Collisions[j].Obs.Position.Y))
                    {
                        map.Collisions.RemoveAt(j);
                    }
                }
            }

            mainwindow.CollisionsList.ItemsSource = null;
            mainwindow.CollisionsList.ItemsSource = map.Collisions;

            //vhco1 = vehicleobjectlist.First(x => x.Vhc == collision.Vhc);
        }
    }
}
