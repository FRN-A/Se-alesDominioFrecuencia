using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSeñales
{
    class SeñalPersonalizada : Señal
    {
        
        public SeñalPersonalizada()
        {
            muestras = new List<Muestra>();
        }

        public override double evaluar(double tiempo)
        {
            return 0;
        }
    }
}
