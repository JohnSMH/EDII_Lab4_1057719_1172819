using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace huffman_prueba
{
    class Program
    {
        static void Main(string[] args)
        {
            //armar el arbol
            
            Huffman<byte> huffman = new Huffman<byte>();
            using var fileRead = new FileStream("easy test.txt", FileMode.OpenOrCreate);
            using var reader = new BinaryReader(fileRead);
            var buffer = new byte[2000];
            while (fileRead.Position < fileRead.Length)
            {
                buffer = reader.ReadBytes(2000);
                foreach (var value in buffer)
                {
                    huffman.fill(value);
                }
            }

            //encodificar el archivo
            byte[] metadata = huffman.Huff();
            fileRead.Position = 0;
            buffer = new byte[2000];
            List<bool> intermedio = new List<bool>();
            while (fileRead.Position < fileRead.Length)
            {
                
                buffer = reader.ReadBytes(2000);
                foreach (var value in buffer)
                {
                    intermedio.AddRange(huffman.Encode(value));
                }
                //System.List.int
                //foreach (int item in intermedio)
                //{

                //    texto += item;
                //}
            }
            reader.Close();
            fileRead.Close();

            if (intermedio.Count%8!=0)
            {
                for (int i = intermedio.Count%8; i < 8; i++)
                {
                    intermedio.Add(false);
                }
            }
            
            BitArray bits = new BitArray(intermedio.ToArray());
            byte[] data = new byte[bits.Length / 8];

            int contador = 0;
            for (int i = 0; i < bits.Length; i=i+8)
            {
                BitArray bitscambio = new BitArray(8);
                bitscambio[0] = bits[i+7];
                bitscambio[1] = bits[i+6];
                bitscambio[2] = bits[i+5];
                bitscambio[3] = bits[i+4];
                bitscambio[4] = bits[i+3];
                bitscambio[5] = bits[i+2];
                bitscambio[6] = bits[i+1];
                bitscambio[7] = bits[i];
                byte[] convert = new byte[1];
                bitscambio.CopyTo(convert,0);
                data[contador] = convert[0];
                contador++;
            }



           


            List<byte> bytesfinal = new List<byte>();

            bytesfinal.AddRange(metadata);
            bytesfinal.AddRange(data);

            //YA NO
            //IMPLEMENTAR BUFFER

            using var fileWrite = new FileStream("output3.huff", FileMode.OpenOrCreate);
            var writer = new BinaryWriter(fileWrite);

            writer.Write(bytesfinal.ToArray());
            writer.Close();
            fileWrite.Close();




            List<byte> result = new List<byte>();
            //using (var reader = new StreamReader(file.OpenReadStream()))
            //{
            //    result = reader.ReadToEnd();
            //}
            List<byte> decoding = new List<byte>();
            using var fileRead2 = new FileStream("output1.huff", FileMode.OpenOrCreate);
            using var reader2 = new BinaryReader(fileRead2);
            buffer = new byte[2000];

            while (fileRead2.Position < fileRead2.Length)
            {
                buffer = reader2.ReadBytes(2000);
                foreach (byte value in buffer)
                {
                    result.Add(value);
                }
            }
            reader2.Close();
            fileRead2.Close();
            huffman.ArmarArbol(result.ToArray());
            decoding = huffman.Decodewometadata(result.ToArray());


            //Buffer de escritura
            var archivo = new FileStream("output2.txt", FileMode.OpenOrCreate);
            var escritor = new BinaryWriter(archivo);

            escritor.Write(decoding.ToArray());
            escritor.Close();
            archivo.Close();
            //var archivo = new FileStream("output2", FileMode.OpenOrCreate);
            //var escritor = new StreamWriter(archivo);
            //escritor.Write(deregreso);
            //escritor.Close();
            //archivo.Close();



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
