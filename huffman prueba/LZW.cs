using System;
using System.Collections.Generic;
using System.Text;

namespace huffman_prueba
{
    class LZW
    {
        Dictionary<string, int> Diccionario;
        Dictionary<int, string> Diccionariosalida;
        string deco; 
        
        
        public LZW(){
            Diccionario = new Dictionary<string, int>();
            Diccionariosalida = new Dictionary<int, string>();
            for (int i = 0; i < 256; i++)
            {
                Diccionario.Add(((char)i) + "", i);
                Diccionariosalida.Add(i, (char)i + "");
            }
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

        public string Firstdeco(int entrada) {
            deco = Diccionariosalida[entrada];
            return Diccionariosalida[entrada];
        }
        public string Decode(int entrada) {
            string resultado = null;

            if (Diccionariosalida.ContainsKey(entrada)) {
                resultado = Diccionariosalida[entrada];            
            }
            else if (entrada == Diccionariosalida.Count)
                resultado =  deco + deco[0];

            

            // new sequence; add it to the dictionary
            Diccionariosalida.Add(Diccionariosalida.Count, deco + resultado[0]);

            deco = resultado;
            return resultado;
        }



    }
}
