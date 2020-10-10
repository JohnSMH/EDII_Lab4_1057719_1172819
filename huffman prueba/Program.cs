using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace huffman_prueba
{
    class Program
    {
        static void Main(string[] args)
        {
            //armar el arbol
            Huffman<char> huffman = new Huffman<char>(); 
            using var fileRead = new FileStream("cuento.txt", FileMode.OpenOrCreate);
            using var reader = new BinaryReader(fileRead);
            var buffer = new byte[2000];
            while (fileRead.Position < fileRead.Length)
            {
                buffer = reader.ReadBytes(2000);
                foreach (var value in buffer)
                {
                    huffman.fill((char)value);
                }
            }
            
            //encodificar el archivo
            String metadata = huffman.Huff();
            fileRead.Position = 0;
            buffer = new byte[2000];
            String texto = "";
            while (fileRead.Position < fileRead.Length)
            {
                List<int> intermedio = new List<int>();
                buffer = reader.ReadBytes(2000);
                foreach (var value in buffer)
                {
                    intermedio.AddRange(huffman.Encode((char)value));
                }
                foreach (int item in intermedio)
                {
                    texto += item;
                }
            }
            reader.Close();
            fileRead.Close();

            byte[] data = huffman.GetBytesFromBinaryString(texto);
            string valor = "";
            foreach (var item in data)
            {
                valor += (char)item;
            }
            String textofinal = "";

            textofinal = metadata + valor;

            using var fileWrite = new FileStream("output.huff", FileMode.OpenOrCreate);
            var writer = new StreamWriter(fileWrite);
            writer.Write(textofinal);
            writer.Close();
            fileWrite.Close();




            string result = "";
            //using (var reader = new StreamReader(file.OpenReadStream()))
            //{
            //    result = reader.ReadToEnd();
            //}
            List<char> decoding = new List<char>();
            using var fileRead2 = new FileStream("output1.huff", FileMode.OpenOrCreate);
            using var reader2 = new BinaryReader(fileRead2);
            buffer = new byte[2000];

            while (fileRead2.Position < fileRead2.Length)
            {
                buffer = reader2.ReadBytes(2000);
                foreach (var value in buffer)
                {
                    result += (char)value;
                }
            }
            reader2.Close();
            fileRead2.Close();
            huffman.ArmarArbol(result);
            decoding = huffman.Decodewometadata(result);
            string deregreso = "";
            foreach (char item in decoding)
            {
                deregreso += item;
            }

            var archivo = new FileStream("output2", FileMode.OpenOrCreate);
            var escritor = new StreamWriter(archivo);
            escritor.Write(deregreso);
            escritor.Close();
            archivo.Close();



            //var huffman = new Huffman<char>(fulltext);
            //List<byte> encoding = huffman.Encode(fulltext);
            //List<char> decoding1 = huffman.Decode(encoding);
            //var outString = new string(decoding1.ToArray());
            //var chars = new HashSet<char>("aaaabbbccd");
            //string texto = "";
            //List<char> guardar = new List<char>();

            //for (int i = 0; i < encoding.Count; i++)
            //{
            //    texto += encoding[i];
            //}
            //var data = huffman.GetBytesFromBinaryString(texto);
            //string valor = "";
            //foreach (var item in data)
            //{
            //    valor += (char)item;
            //}

            //string texto2 = "";


            //texto2 = huffman.conoceri + valor;
            //string bits = huffman.Regresar(texto2);
            //List<int> vs = new List<int>();
            //foreach (var item in bits.ToCharArray())
            //{
            //    vs.Add(int.Parse(item.ToString()));
            //}
            //List<char> test = huffman.Decode(vs);
            //Console.WriteLine(texto2);

            //huffman.ArmarArbol(texto2);


            //foreach (char item in decoding1)
            //{
            //    Console.Write(item);
            //}


            //foreach (char c in chars)
            //{
            //    encoding = huffman.Encode(c);
            //    Console.Write("{0}:  ", c);
            //    int[] code = new int[chars.Count];
            //    foreach (int bit in encoding)
            //    {
            //        string codigo = bit.ToString();
            //        byte[] bytes = BitConverter.GetBytes(bit);
            //        Console.Write("{0}", codigo);
            //    }
            //    Console.WriteLine();
            //}

            Console.ReadKey();
        }

    }

}
