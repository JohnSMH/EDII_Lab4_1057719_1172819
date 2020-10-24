using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Lab1.Models;
using Lab1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using huffman_prueba;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.AspNetCore.Routing;
using System.Collections;
using System.Data.SqlTypes;
using System.Security.Cryptography.X509Certificates;

namespace Lab1.Controllers
{
    [Route("api")]
    [ApiController]
    public class LzwController : ControllerBase
    {
        [HttpGet]
        public IActionResult Mostrar()
        {
            return new JsonResult(new { BIENVENIDA = "LABORATORIO 4" }); 
        }

        [HttpGet("compressions")]
        public ActionResult Compressions()
        {
            try
            {
                var result = Data.Instance.archivos.Select(x => new Datos { Nombredelarchivooriginal = x.Nombredelarchivooriginal, Nombreyrutadelarchivocomprimido = x.Nombreyrutadelarchivocomprimido, Razóndecompresión = x.Razóndecompresión, Factordecompresión = x.Factordecompresión, Porcentajedereducción = x.Porcentajedereducción });
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        //Post /api/compress/{name} COMPRIMIR
        [HttpPost("compress/{name}")]
        public IActionResult Comprimir([FromRoute] string name, [FromForm]IFormFile file)
        {
            using var fileRead = file.OpenReadStream();
            try
            {
                LZW testing = new LZW();
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
                        existe = testing.Encode(trabajo, encodificador);
                        if (existe != -1)
                        {
                            Intermedio.Add(existe);
                            encodificador = "" + (char)value;
                        }
                        else
                        {
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

                Datos obtener = new Datos();
                obtener.Razóndecompresión = (Convert.ToDouble(fileWrite.Length) / Convert.ToDouble(fileRead.Length));
                obtener.Factordecompresión = (Convert.ToDouble(fileRead.Length) / Convert.ToDouble(fileWrite.Length));
                obtener.Porcentajedereducción = (Convert.ToDouble(fileRead.Length) / Convert.ToDouble(fileWrite.Length)) * 100;
                obtener.Nombredelarchivooriginal = (file.FileName);
                obtener.Nombreyrutadelarchivocomprimido = (name + ".lzw");
                Data.Instance.archivos.Add(obtener);
                writer.Close();
                fileWrite.Close();
                reader.Close();
                fileRead.Close();

                var files = System.IO.File.OpenRead(name + ".lzw");
                return new FileStreamResult(files, "application/lzw")
                {
                    FileDownloadName = name + ".lzw"
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        // POST: api/descompress DESCOMPRIR
        [Route("descompress")]
        public IActionResult Decompresion([FromForm]IFormFile file)
        {
            try
            {
                string input= file.FileName;
                List<byte> result = new List<byte>();
                List<byte> decoding = new List<byte>();
                using var fileRead2 = new FileStream(input, FileMode.OpenOrCreate);
                using var reader2 = new BinaryReader(fileRead2);
                var buffer = new byte[2000];
                while (fileRead2.Position < fileRead2.Length)
                {
                    buffer = reader2.ReadBytes(2000);
                    foreach (byte value in buffer)
                    {
                        result.Add(value);
                    }
                }

                //DECODIFICAR
                List<byte> total;
                bool first = true;
                for ( int j = 0; j < result.Count; j = j + 4)
                {
                    byte[] plzwork = new byte[] { result[j], result[j + 1], result[j + 2], result[j + 3] };
                    if (first)
                    {
                        total = Data.Instance.acceder.Firstdeco(BitConverter.ToInt32(plzwork));
                        first = false;
                    }
                    else
                    {
                        total += Data.Instance.acceder.Decode(BitConverter.ToInt32(plzwork));
                    }


                }

                reader2.Close();
                fileRead2.Close();
                Data.Instance.LZW.ArmarArbol(result.ToArray());
                decoding = Data.Instance.LZW.Decodewometadata(result.ToArray());
                int i = 0;
                string output = "";
                foreach (Datos item in Data.Instance.archivos)
                {
                    if (item.Nombreyrutadelarchivocomprimido == input)
                    {
                        output = item.Nombredelarchivooriginal;
                        break;
                    }
                    i++;
                }
               
                //Buffer de escritura
                var archivo = new FileStream(output, FileMode.OpenOrCreate);
                var escritor = new BinaryWriter(archivo);

                escritor.Write(decoding.ToArray());
                escritor.Close();
                archivo.Close();
                var files = System.IO.File.OpenRead(output);
                return new FileStreamResult(files, "application/txt")
                {
                    FileDownloadName = output
                };

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
