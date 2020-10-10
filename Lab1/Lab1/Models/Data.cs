using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab1.Models;

namespace Lab1.Models
{
    public class Data
    {
        private static Data _instance = null;

        public static Data Instance
        {
            get
            {
                if (_instance == null) _instance = new Data();
                return _instance;
            }

        }

        public int Size = 0;
        public string nombre = "";
        public string texto = "";
    }
}
