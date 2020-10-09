﻿using System;
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
            //var outString = new string(decoding.ToArray());
            //Console.WriteLine(outString == "aaaabbbccd" ? "Encoding/decoding worked" : "Encoding/Decoding failed");
            var chars = new HashSet<char>("aaaabbbccd");
            char[] letras = new char[chars.Count];
            string texto = "";

            for (int i = 0; i < encoding.Count; i++)
            {
                texto += encoding[i];
            }
            var data = huffman.GetBytesFromBinaryString(texto);
            var text = Encoding.ASCII.GetString(data);
            string texto2 = "";
            foreach (char x in text)
            {
                Console.Write("{0}", x);
            }
            Console.WriteLine("");
            texto2 = huffman.conoceri + texto;
            List<char> decoding = huffman.decodificar(texto2);
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
