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
            char[] letras = new char[chars.Count];
            string texto = "";
            List<char> guardar = new List<char>();

            for (int i = 0; i < encoding.Count; i++)
            {
                texto += encoding[i];
            }
            var data = huffman.GetBytesFromBinaryString(texto);
            var text = System.Text.Encoding.UTF8.GetString(data);
            foreach (char item in text)
            {
                guardar.Add(item);
            }
            string texto2 = "";
            foreach (char x in text)
            {
                Console.Write("{0}", x);
            }
            Console.WriteLine("");
            
            texto2 = huffman.conoceri + texto;
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
