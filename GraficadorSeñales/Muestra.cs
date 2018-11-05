using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSeñales
{
    class Muestra
    {
        //El instante del tiempo en que fue tomada la muestra
        //lamuestra
        public double x { get; set; }
        //El valor de esa  muestra en ese instante
        public double y { get; set; }

        public Muestra()
        {
            x = 0;
            y = 0;
        }

        public Muestra(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
