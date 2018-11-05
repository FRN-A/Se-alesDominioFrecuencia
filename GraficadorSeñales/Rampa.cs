using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSeñales
{
    class Rampa : Señal
    {
        

        public Rampa()
        {
            muestras = new List<Muestra>();
        }

        public override double evaluar(double tiempo)
        {
            double resultado;
            if (tiempo < 0)
                resultado = 0;
            else
                resultado = tiempo;
            return resultado;
        }
    }
}
