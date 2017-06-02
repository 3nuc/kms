using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO_wpf
{
    class Heli
    {
        public double x, y;
        public string img = "pack://application:,,,/img/placeholder.bmp";

        public Heli(double _x, double _y)
        {
            x = _x;
            y = _y;
        }
    }
}
