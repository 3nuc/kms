using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO_wpf
{
    public class Obstacle   //changed from heli to obstacle(more general), added public
    {
        public double x, y;
        public string img = "pack://application:,,,/img/placeholder.bmp";

        public Obstacle(double _x, double _y)
        {
            x = _x;
            y = _y;
        }
    }
}
