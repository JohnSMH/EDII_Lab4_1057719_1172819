using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
namespace Lab1.Models
{
    public class Peliculas:IComparable<Peliculas>
    {
        public string director { get; set; }
        public double imdbRating { get; set; }
        public string genre { get; set; }
        public string releaseDate { get; set; }
        public int rottenTomatoesRating { get; set; }
        public string title { get; set; }

        

        int IComparable<Peliculas>.CompareTo(Peliculas other)
        {
            throw new NotImplementedException();
        }
    }
}
