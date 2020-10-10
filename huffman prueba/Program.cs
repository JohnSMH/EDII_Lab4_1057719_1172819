using System;
using System.Collections.Generic;
using System.Text;

namespace huffman_prueba
{
    class Program
    {
        static void Main(string[] args)
        {
            var huffman = new Huffman<char>("aaaabbbccd");
            List<int> encoding = huffman.Encode("aaaabbbccd");
            List<char> decoding1 = huffman.Decode(encoding,10);
            var outString = new string(decoding1.ToArray());
            var chars = new HashSet<char>("aaaabbbccd");
            string texto = "";
            List<char> guardar = new List<char>();

            for (int i = 0; i < encoding.Count; i++)
            {
                texto += encoding[i];
            }
            var data = huffman.GetBytesFromBinaryString(texto);
            string valor = "";
            foreach (var item in data)
            {
                valor += (char)item;
            }

            string texto2 = "";
           
            
            texto2 = huffman.conoceri + valor;
            string bits = huffman.Regresar(texto2);
            List<int> vs = new List<int>();
            foreach (var item in bits.ToCharArray())
            {
                vs.Add(int.Parse(item.ToString()));
            }
            List<char> test = huffman.Decode(vs,10);
            Console.WriteLine(texto2);
           
            huffman.ArmarArbol(texto2);

            
            foreach (char item in decoding1)
            {
                Console.Write(item);
            }
           

            foreach (char c in chars)
            {
                encoding = huffman.Encode(c);
                Console.Write("{0}:  ", c);
                int[] code = new int[chars.Count];
                foreach (int bit in encoding)
                {
                    string codigo = bit.ToString();
                    byte[] bytes = BitConverter.GetBytes(bit);
                    Console.Write("{0}", codigo);
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }

    }

}
