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

    }
}
