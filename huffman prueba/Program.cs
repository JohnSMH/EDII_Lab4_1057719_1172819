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
    
            LZW testing = new LZW();
            using var fileRead = new FileStream("cuento.txt", FileMode.OpenOrCreate);
            using var reader = new BinaryReader(fileRead);
            var buffer = new byte[2000];
            int existe = 0;
            string encodificador = "";
            List<int> Intermedio = new List<int>();
            while (fileRead.Position < fileRead.Length)
            {
                buffer = reader.ReadBytes(2000);
                foreach (var value in buffer)
                {
                    string trabajo = encodificador + (char)value;
                    existe = testing.Encode(trabajo,encodificador);
                    if (existe != -1)
                    {
                        Intermedio.Add(existe);
                        encodificador = "" + (char)value;
                    }
                    else {
                        encodificador = trabajo;
                    }
                }
            }

            //INTERMEDIO A BYTES
            List<byte> Aescribir = new List<byte>();

            foreach (int item in Intermedio)
            {
                Aescribir.AddRange(BitConverter.GetBytes(item));
            }

            //ESCRIBIR COMPRIMIDO

            using var fileWrite = new FileStream("LZWtest.txt", FileMode.OpenOrCreate);
            var writer = new BinaryWriter(fileWrite);

            writer.Write(Aescribir.ToArray());
            writer.Close();
            

            //LEER DOCUMENTO CODIFICADO
            List<byte> result = new List<byte>();

            
            using var fileRead2 = new FileStream("LZWtest.txt", FileMode.OpenOrCreate);
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

            //DECODIFICAR
            String total = "";
            bool first = true;
            for (int i = 0; i < result.Count; i=i+4)
            {
                byte[] plzwork = new byte[] { result[i], result[i + 1], result[i + 2], result[i + 3] };
                if (first)
                {
                    total= testing.Firstdeco(BitConverter.ToInt32(plzwork));
                    first = false;
                }
                else {
                    total += testing.Decode(BitConverter.ToInt32(plzwork));
                }
                
                
            }
            Console.WriteLine(total);

            Console.ReadKey();
        }

    }

}
