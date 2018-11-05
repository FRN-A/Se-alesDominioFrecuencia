using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSeñales
{
    class SeñalRectangular : Señal
    {
        public SeñalRectangular()
        {
            muestras = new List<Muestra>();
        }

        public override double evaluar(double tiempo)
        {
            double resultado;
            if (Math.Abs(tiempo) > 0.5)
                resultado = 0;
            else if (Math.Abs(tiempo) == 0.5)
                resultado = 0.5;
            else
                resultado = 1;

            return resultado;
        }
    }
}
