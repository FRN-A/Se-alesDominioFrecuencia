using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSeñales
{
    class SeñalExponencial : Señal
    {
        double alpha { get; set; }

        public SeñalExponencial(double alpha)
        {
            this.alpha = alpha;
            muestras = new List<Muestra>();
        }

        public override double evaluar(double tiempo)
        {
            double exponente = alpha * tiempo;
            return Math.Exp(exponente);
        }
    }
}
