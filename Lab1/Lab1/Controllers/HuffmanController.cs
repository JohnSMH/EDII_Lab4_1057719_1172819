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
    public class HuffmanController : ControllerBase
    {
        [HttpGet]
        public IActionResult Mostrar()
        {
            return new JsonResult(new { BIENVENIDA = "LABORATORIO 3" }); 
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
                //using var fileRead = new FileStream(file.FileName, FileMode.OpenOrCreate);
                using var reader = new BinaryReader(fileRead);
                var buffer = new byte[2000];
                while (fileRead.Position < fileRead.Length)
                {
                    buffer = reader.ReadBytes(2000);
                    foreach (var value in buffer)
                    {
                        Data.Instance.huffman.fill(value);
                    }
                }

                //encodificar el archivo
                byte[] metadata = Data.Instance.huffman.Huff();
                fileRead.Position = 0;
                buffer = new byte[2000];
                List<bool> intermedio = new List<bool>();
                while (fileRead.Position < fileRead.Length)
                {

                    buffer = reader.ReadBytes(2000);
                    foreach (var value in buffer)
                    {
                        intermedio.AddRange(Data.Instance.huffman.Encode(value));
                    }
                }


                if (intermedio.Count % 8 != 0)
                {
                    for (int i = intermedio.Count % 8; i < 8; i++)
                    {
                        intermedio.Add(false);
                    }
                }

                BitArray bits = new BitArray(intermedio.ToArray());
                byte[] data = new byte[bits.Length / 8];

                int contador = 0;
                for (int i = 0; i < bits.Length; i = i + 8)
                {
                    BitArray bitscambio = new BitArray(8);
                    bitscambio[0] = bits[i + 7];
                    bitscambio[1] = bits[i + 6];
                    bitscambio[2] = bits[i + 5];
                    bitscambio[3] = bits[i + 4];
                    bitscambio[4] = bits[i + 3];
                    bitscambio[5] = bits[i + 2];
                    bitscambio[6] = bits[i + 1];
                    bitscambio[7] = bits[i];
                    byte[] convert = new byte[1];
                    bitscambio.CopyTo(convert, 0);
                    data[contador] = convert[0];
                    contador++;
                }


                List<byte> bytesfinal = new List<byte>();

                bytesfinal.AddRange(metadata);
                bytesfinal.AddRange(data);

                using var fileWrite = new FileStream(name + ".huff", FileMode.OpenOrCreate);
                var writer = new BinaryWriter(fileWrite);

                writer.Write(bytesfinal.ToArray());

                Datos obtener = new Datos();
                obtener.Razóndecompresión = (Convert.ToDouble(fileWrite.Length) / Convert.ToDouble(fileRead.Length));
                obtener.Factordecompresión = (Convert.ToDouble(fileRead.Length) / Convert.ToDouble(fileWrite.Length));
                obtener.Porcentajedereducción = (Convert.ToDouble(fileRead.Length) / Convert.ToDouble(fileWrite.Length)) * 100;
                obtener.Nombredelarchivooriginal = (file.FileName);
                obtener.Nombreyrutadelarchivocomprimido = (name + ".huff");
                Data.Instance.archivos.Add(obtener);
                writer.Close();
                fileWrite.Close();
                reader.Close();
                fileRead.Close();

                var files = System.IO.File.OpenRead(name + ".huff");
                return new FileStreamResult(files, "application/huff")
                {
                    FileDownloadName = name + ".huff"
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
                Data.Instance.huffman.ArmarArbol(result.ToArray());
                decoding = Data.Instance.huffman.Decodewometadata(result.ToArray());
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
