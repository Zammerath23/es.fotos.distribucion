using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace es.fotos.distribucion
{
    class Program
    {
        static string directorioInicial = string.Empty;
        static string directorioDestino = string.Empty;
        static void Main(string[] args)
        {
            directorioInicial = ConfigurationManager.AppSettings["directorioOrigen"].ToString();
            directorioDestino = ConfigurationManager.AppSettings["directorioDestino"].ToString();
            TratarDirectorio(directorioInicial);
        }

        static void MoverFichero(string rutaFichero)
        {
            Console.WriteLine("Tratando fichero: " + rutaFichero);

            var infoFichero = new FileInfo(rutaFichero);
            var fechaCreacion = File.GetCreationTime(rutaFichero);

            var fechaFichero = fechaCreacion < infoFichero.LastWriteTime ? fechaCreacion : infoFichero.LastWriteTime;


            var rutaDestino = directorioDestino + "\\" + fechaFichero.Year +"\\" + ObtenerNombreMes(fechaFichero.Month);

            bool existe = System.IO.Directory.Exists(rutaDestino);

            if (!existe)
                System.IO.Directory.CreateDirectory(rutaDestino);

            var file = new FileInfo(rutaFichero);
            var rutaCompletaDestino = rutaDestino + "\\" + file.Name;

            if(!File.Exists(rutaCompletaDestino))
            {
                file.CopyTo(rutaDestino + "\\" + file.Name);
            }

            


        }
        static void TratarDirectorio(string rutaAbsolutaDirectorio)
        {
            Console.WriteLine("Tratando directorio: " + rutaAbsolutaDirectorio);
            string[] subDirectorios = Directory.GetDirectories(rutaAbsolutaDirectorio);
            foreach(string subdirectorio in subDirectorios)
            {
                TratarDirectorio(subdirectorio);
            }
            var ficheros = Directory.GetFiles(rutaAbsolutaDirectorio);

            foreach(string fichero in ficheros)
            {

                MoverFichero(fichero);
            }
        }

        static string ObtenerNombreMes(int numeroMes)
        {
            try
            {
                CultureInfo ci = new CultureInfo("es-ES");
                DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                string nombreMes = ci.TextInfo.ToTitleCase(formatoFecha.GetMonthName(numeroMes));
                return nombreMes;
            }
            catch
            {
                return "Desconocido";
            }
        }
        
    }
}
