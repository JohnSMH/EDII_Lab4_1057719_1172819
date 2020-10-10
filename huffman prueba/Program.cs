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
            //for (int i = 0; i < length; i++)
            //{
            //    huffman.Encode("Seccioni");
            //}
            List<char> decoding1 = huffman.Decode(encoding);
            var outString = new string(decoding1.ToArray());
            //Console.WriteLine(outString == "aaaabbbccd" ? "Encoding/decoding worked" : "Encoding/Decoding failed");
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
            Console.WriteLine(texto2);
            List<char> decoding = huffman.ArmarArbol(texto2);
            foreach (char item in decoding)
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
