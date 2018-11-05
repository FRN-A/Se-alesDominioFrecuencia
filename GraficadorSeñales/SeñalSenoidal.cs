using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSeñales
{
    class SeñalSenoidal : Señal
    {
        public double Amplitud { get; set; }
        public double Fase { get; set; }
        public double Frecuencia { get; set; }

        public SeñalSenoidal()
        {
            Amplitud = 1.0;
            Fase = 0.0;
            Frecuencia = 1.0;
            muestras = new List<Muestra>();
            amplitudMaxima = 0;
        }

        public SeñalSenoidal(double amplitud, double fase, double frecuencia)
        {
            this.Amplitud = amplitud;
            this.Fase = fase;
            this.Frecuencia = frecuencia;
            muestras = new List<Muestra>();
            amplitudMaxima = 0;
        }

        public override double evaluar(double tiempo)
        {
            double resultado;
            resultado = Amplitud * Math.Sin(((2 * Math.PI * Frecuencia) * tiempo) + Fase);
            return resultado;
        }
    }
}
