using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
namespace Lab1.Models
{
    public class Datos
    {
        public string Nombredelarchivooriginal { get; set; }
        public string Nombreyrutadelarchivocomprimido { get; set; }
        public double Razóndecompresión { get; set; }
        public double Factordecompresión { get; set; }
        public double Porcentajedereducción { get; set; }

        public Datos(string nombre1, string nombre2, double razon, double factor, double porcentaje)
        {

            Nombredelarchivooriginal = nombre1;
            Nombreyrutadelarchivocomprimido = nombre2;
            Razóndecompresión = razon;
            Factordecompresión = factor;
            Porcentajedereducción = porcentaje;
        }
        public Datos() { }


    }
}
