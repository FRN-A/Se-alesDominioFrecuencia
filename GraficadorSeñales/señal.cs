using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSeñales
{
    abstract class Señal
    {
        public List<Muestra> muestras { get; set; }
        public double amplitudMaxima { get; set; }
        public double tiempoInicial { get; set; }
        public double tiempoFinal { get; set; }
        public double frecuenciaMuestreo { get; set; }

        public abstract double evaluar(double tiempo);

        public void construirSeñalDigital()
        {
            double periodoMuestro = 1 / frecuenciaMuestreo;

            for (double i = tiempoInicial; i <= tiempoFinal; i += periodoMuestro)
            {
                double valorMuestra = this.evaluar(i);

                this.muestras.Add(new Muestra(i, valorMuestra));

            }
        }

        public void escalar(double factor)
        {
            foreach (Muestra muestra in muestras)
            {
                muestra.y *= factor;
            } 
        }

        public void actualizarAmplitudMaxima()
        {
            foreach (Muestra muestra in muestras)
            {
                if (Math.Abs(muestra.y) > this.amplitudMaxima)
                {
                    this.amplitudMaxima = Math.Abs(muestra.y);
                }
            }
            
        }

        public void desplazarY(double desplazamiento)
        {
            foreach (Muestra muestra in muestras)
            {
                muestra.y += desplazamiento;
            }
        }

        public void truncar(double n)
        {
            foreach (Muestra muestra in muestras)
            {
                if (muestra.y > n)
                    muestra.y = n;
                else if (muestra.y < -n)
                    muestra.y = -n;
            }
        }

        public static Señal sumar(Señal sumando1, Señal sumando2)
        {
            SeñalPersonalizada resultado = new SeñalPersonalizada();

            resultado.tiempoInicial = sumando1.tiempoInicial;
            resultado.tiempoFinal = sumando1.tiempoFinal;
            resultado.frecuenciaMuestreo = sumando1.frecuenciaMuestreo;

            int indice = 0;
            foreach (Muestra muestra in sumando1.muestras)
            {
                Muestra muestraResultado = new Muestra();
                muestraResultado.x = muestra.x;
                muestraResultado.y = muestra.y + sumando2.muestras[indice++].y;

                resultado.muestras.Add(muestraResultado);
            }

            return resultado;
        }

        public static Señal multiplicar(Señal señal1, Señal señal2)
        {
            SeñalPersonalizada resultado = new SeñalPersonalizada();

            resultado.tiempoInicial = señal1.tiempoInicial;
            resultado.tiempoFinal = señal1.tiempoFinal;
            resultado.frecuenciaMuestreo = señal1.frecuenciaMuestreo;

            int indice = 0;
            foreach (Muestra muestra in señal1.muestras)
            {
                Muestra muestraResultado = new Muestra();
                muestraResultado.x = muestra.x;
                muestraResultado.y = muestra.y * señal2.muestras[indice++].y;

                resultado.muestras.Add(muestraResultado);
            }

            return resultado;
        }

        public static Señal convolucionar(Señal señal1, Señal señal2)
        {
            SeñalPersonalizada resultado = new SeñalPersonalizada();

            resultado.tiempoInicial = señal1.tiempoInicial + señal2.tiempoInicial;
            resultado.tiempoFinal = señal1.tiempoFinal + señal2.tiempoFinal;
            resultado.frecuenciaMuestreo = señal1.frecuenciaMuestreo;

            double periodoMuestreo = 1 / resultado.frecuenciaMuestreo;

           
            double cantidadMuestrasResultado = resultado.frecuenciaMuestreo * (resultado.tiempoFinal - resultado.tiempoInicial);
            double instanteActual = resultado.tiempoInicial;
            for (int n = 0; n < cantidadMuestrasResultado; n++)
            {
                double valorMuestra = 0;
                for (int k = 0; k < señal2.muestras.Count; k++)
                {
                    if ((n-k) >= 0 && (n-k) < señal2.muestras.Count)
                        valorMuestra += señal1.muestras[k].y * señal2.muestras[n - k].y;
                }
                valorMuestra /= resultado.frecuenciaMuestreo;
                Muestra muestra = new Muestra(instanteActual, valorMuestra);
                resultado.muestras.Add(muestra);
                instanteActual += periodoMuestreo;
            }

         

            return resultado;
        }
    }
}
