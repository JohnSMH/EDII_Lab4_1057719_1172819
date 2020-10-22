using System;
using System.Collections.Generic;
using System.Text;

namespace huffman_prueba
{
    class LZW
    {
        Dictionary<string, int> Diccionario;
        Dictionary<int, string> Diccionariosalida;
        
        
        public LZW(){
            Diccionario = new Dictionary<string, int>();
            for (int i = 0; i < 256; i++)
                Diccionario.Add(((char)i).ToString(), i);
        }

        public int Encode(string entrada,string encodificador) {
            //revisar si existe en el diccionario
            if (Diccionario.ContainsKey(entrada))
            {
                //Si existe devolver algo para significar que solo añada el siguiente byte
                return -1;
            }
            else {
                //Si no existe Insertar y devolver valor en int a bytes
                Diccionario.Add(entrada,Diccionario.Count);
                return Diccionario[encodificador];
            }
        }
        public void Fill(byte entrada) {
            string insert = ""+(char)entrada;
            if (!Diccionario.ContainsKey(insert)) {
                Diccionario.Add(insert,Diccionario.Count);
                
            }
        }
        public void Decode() { }

    }
}
