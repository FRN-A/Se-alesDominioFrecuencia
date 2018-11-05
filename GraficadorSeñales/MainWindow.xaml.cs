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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraficadorSeñales
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double amplitudMaxima;
        Señal señal;
        Señal segundaSeñal;
        Señal señalResultado;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void graficar_Click(object sender, RoutedEventArgs e)
        {
            double tiempoInicial = double.Parse(txtTiempoInicial.Text);
            double tiempoFinal = double.Parse(txtTiempoFinal.Text);
            double frecuenciaMuestreo = double.Parse(txtFrecuenciaMuestreo.Text);

            switch (cbTipoSeñal.SelectedIndex)
            {
                //Seniodal
                case 0:
                    double amplitud = double.Parse(((ConfiguracionSeñalSenoidal)panelConfiguracion.Children[0]).txtAmplitud.Text);
                    double fase = double.Parse(((ConfiguracionSeñalSenoidal)panelConfiguracion.Children[0]).txtFase.Text);
                    double frecuencia = double.Parse(((ConfiguracionSeñalSenoidal)panelConfiguracion.Children[0]).txtFrecuencia.Text);
                    señal = new SeñalSenoidal(amplitud, fase, frecuencia);
                    break;
                //Rampa
                case 1: señal = new Rampa();
                    break;
                    // Exponencial
                case 2:
                    double alpha = double.Parse(((ConfiguracionSeñalExponencial)panelConfiguracion.Children[0]).txtAlpha.Text);
                    señal = new SeñalExponencial(alpha);
                    break;
                case 3: //rectangular
                    señal = new SeñalRectangular();
                    break;
                default: señal = null;
                    break;
            }

            switch (cbTipoSeñal_segundaSeñal.SelectedIndex)
            {
                //Seniodal
                case 0:
                    double amplitud = double.Parse(((ConfiguracionSeñalSenoidal)panelConfiguracion_segundaSeñal.Children[0]).txtAmplitud.Text);
                    double fase = double.Parse(((ConfiguracionSeñalSenoidal)panelConfiguracion_segundaSeñal.Children[0]).txtFase.Text);
                    double frecuencia = double.Parse(((ConfiguracionSeñalSenoidal)panelConfiguracion_segundaSeñal.Children[0]).txtFrecuencia.Text);
                    segundaSeñal = new SeñalSenoidal(amplitud, fase, frecuencia);
                    break;
                //Rampa
                case 1:
                    segundaSeñal = new Rampa();
                    break;
                // Exponencial
                case 2:
                    double alpha = double.Parse(((ConfiguracionSeñalExponencial)panelConfiguracion_segundaSeñal.Children[0]).txtAlpha.Text);
                    segundaSeñal = new SeñalExponencial(alpha);
                    break;
                case 3: //rectangular
                    segundaSeñal = new SeñalRectangular();
                    break;
                default:
                    segundaSeñal = null;
                    break;
            }







            plnGrafica.Points.Clear();
            plnGrafica2.Points.Clear();



            if (señal != null)
            {
                señal.tiempoFinal = tiempoFinal;
                señal.tiempoInicial = tiempoInicial;
                señal.frecuenciaMuestreo = frecuenciaMuestreo;
                segundaSeñal.tiempoFinal = tiempoFinal;
                segundaSeñal.tiempoInicial = tiempoInicial;
                segundaSeñal.frecuenciaMuestreo = frecuenciaMuestreo;

                //contruir señal
                señal.construirSeñalDigital();
                segundaSeñal.construirSeñalDigital();

                //ecalar
                if ((bool)chbEscala.IsChecked)
                {
                    double factorEscala = double.Parse(txtEscalaAmplitud.Text);
                    señal.escalar(factorEscala);
                }
                if ((bool)chbEscala_segundaSeñal.IsChecked)
                {
                    double factorEscala = double.Parse(txtEscalaAmplitud_segundaSeñal.Text);
                    segundaSeñal.escalar(factorEscala);
                }
                //desplazamiento
                if ((bool)chbDesplazamiento.IsChecked)
                {
                    double desplazamiento = double.Parse(txtDesplazamientoY.Text);
                    señal.desplazarY(desplazamiento);
                }
                if ((bool)chbDesplazamiento_segundaSeñal.IsChecked)
                {
                    double desplazamiento = double.Parse(txtDesplazamientoY_segundaSeñal.Text);
                    segundaSeñal.desplazarY(desplazamiento);
                }
                //Truncar
                if ((bool)chbTruncar.IsChecked)
                {
                    double umbral = double.Parse(txtTruncar.Text);
                    señal.truncar(umbral);
                }
                if ((bool)chbTruncar_segundaSeñal.IsChecked)
                {
                    double umbral = double.Parse(txtTruncar_segundaSeñal.Text);
                    segundaSeñal.truncar(umbral);
                }

                //actualizar amplitud maxima
                señal.actualizarAmplitudMaxima();
                segundaSeñal.actualizarAmplitudMaxima();

                amplitudMaxima = señal.amplitudMaxima;
                if (amplitudMaxima < segundaSeñal.amplitudMaxima)
                    amplitudMaxima = segundaSeñal.amplitudMaxima;                
   
                
                //recorrer una coleccion o arreglo
                foreach (Muestra muestra in señal.muestras)
                {
                    plnGrafica.Points.Add(new Point((muestra.x - tiempoInicial) * scrContenedor.Width, (muestra.y / amplitudMaxima * ((scrContenedor.Height / 2.0) - 30) * -1) + (scrContenedor.Height / 2)));
                }
                foreach (Muestra muestra in segundaSeñal.muestras)
                {
                    plnGrafica2.Points.Add(new Point((muestra.x - tiempoInicial) * scrContenedor.Width, (muestra.y / amplitudMaxima * ((scrContenedor.Height / 2.0) - 30) * -1) + (scrContenedor.Height / 2)));
                }

                lblAmplitudMaximaY.Text = amplitudMaxima.ToString();
                lblAmplitudMaximaNegativaY.Text = "-" + amplitudMaxima.ToString();
            }

           

            

            plnEjeX.Points.Clear();
            //punto del principio
            plnEjeX.Points.Add(new Point(0, scrContenedor.Height / 2));
            //punto del fin
            plnEjeX.Points.Add(new Point((tiempoFinal - tiempoInicial) * scrContenedor.Width, scrContenedor.Height / 2));

            plnEjeY.Points.Clear();
            //punto del principio
            plnEjeY.Points.Add(new Point((0 - tiempoInicial) * scrContenedor.Width, (1 * ((scrContenedor.Height / 2.0) - 30) * -1) + (scrContenedor.Height / 2)));
            //punto del fin
            plnEjeY.Points.Add(new Point((0 - tiempoInicial) * scrContenedor.Width, (-1 * ((scrContenedor.Height / 2.0) - 30) * -1) + (scrContenedor.Height / 2)));
        }

        private void btnGraficarRampa_Click(object sender, RoutedEventArgs e)
        {
            double tiempoInicial = double.Parse(txtTiempoInicial.Text);
            double tiempoFinal = double.Parse(txtTiempoFinal.Text);
            double frecuenciaMuestreo = double.Parse(txtFrecuenciaMuestreo.Text);

            Rampa rampa = new Rampa();
            plnGrafica.Points.Clear();

            double periodoMuestro = 1 / frecuenciaMuestreo;

            for (double i = tiempoInicial; i <= tiempoFinal; i += periodoMuestro)
            {
                double valorMuestra = rampa.evaluar(i);
                rampa.muestras.Add(new Muestra(i, valorMuestra));
            }
            foreach (Muestra muestra in rampa.muestras)
            {
                plnGrafica.Points.Add(new Point(muestra.x * scrContenedor.Width, (muestra.y * ((scrContenedor.Height / 2.0) - 30) * -1) + (scrContenedor.Height / 2)));
            }
        }

        private void cbTipoSeñal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (panelConfiguracion != null)
            {
                panelConfiguracion.Children.Clear();
                switch (cbTipoSeñal.SelectedIndex)
                {
                    case 0: //Senoidal
                        panelConfiguracion.Children.Add(new ConfiguracionSeñalSenoidal());
                        break;
                    case 1: //rampa
                        break;
                    case 2: //Exponencial
                        panelConfiguracion.Children.Add(new ConfiguracionSeñalExponencial());
                        break;
                    case 3: //Rectangular
                        break;
                    default:
                        break;
                }
            }
            
        }

        private void cbTipoSeñal_segundaSeñal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (panelConfiguracion_segundaSeñal != null)
            {
                panelConfiguracion_segundaSeñal.Children.Clear();
                switch (cbTipoSeñal_segundaSeñal.SelectedIndex)
                {
                    case 0: //Senoidal
                        panelConfiguracion_segundaSeñal.Children.Add(new ConfiguracionSeñalSenoidal());
                        break;
                    case 1: //rampa
                        break;
                    case 2: //Exponencial
                        panelConfiguracion_segundaSeñal.Children.Add(new ConfiguracionSeñalExponencial());
                        break;
                    default:
                        break;
                }
            }
        }

        private void btnRealizarOperacion_Click(object sender, RoutedEventArgs e)
        {
            señalResultado = null;
            switch (cbOperacion.SelectedIndex)
            {
                case 0: //suma
                    señalResultado = Señal.sumar(señal, segundaSeñal);
                    break;
                case 1: //Multiplicacion
                    señalResultado = Señal.multiplicar(señal, segundaSeñal);
                    break;
                case 2: //Convolucion
                    señalResultado = Señal.convolucionar(señal, segundaSeñal);
                    break;
                default:
                    break;
            }

            plnResultado.Points.Clear();

            //actualizar amplitud maxima
            señalResultado.actualizarAmplitudMaxima();

            if (señalResultado != null)
            {
                //recorrer una coleccion o arreglo
                foreach (Muestra muestra in señalResultado.muestras)
                {
                    plnResultado.Points.Add(new Point((muestra.x - señalResultado.tiempoInicial) * scrContenedorResultado.Width, (muestra.y / señalResultado.amplitudMaxima * ((scrContenedorResultado.Height / 2.0) - 30) * -1) + (scrContenedorResultado.Height / 2)));
                }
            }
            

            lblAmplitudMaximaY_Resultado.Text = señalResultado.amplitudMaxima.ToString();
            lblAmplitudMaximaNegativaY_Resultado.Text = "-" + señalResultado.amplitudMaxima.ToString();
        





        plnEjeXResultado.Points.Clear();
            //punto del principio
            plnEjeXResultado.Points.Add(new Point(0, scrContenedor.Height / 2));
            //punto del fin
            plnEjeXResultado.Points.Add(new Point((señalResultado.tiempoFinal - señalResultado.tiempoInicial) * scrContenedorResultado.Width, scrContenedorResultado.Height / 2));

            plnEjeYResultado.Points.Clear();
            //punto del principio
            plnEjeYResultado.Points.Add(new Point((0 - señalResultado.tiempoInicial) * scrContenedorResultado.Width, (1 * ((scrContenedorResultado.Height / 2.0) - 30) * -1) + (scrContenedorResultado.Height / 2)));
            //punto del fin
            plnEjeYResultado.Points.Add(new Point((0 - señalResultado.tiempoInicial) * scrContenedorResultado.Width, (-1 * ((scrContenedorResultado.Height / 2.0) - 30) * -1) + (scrContenedorResultado.Height / 2)));
        

        }
    }
}
