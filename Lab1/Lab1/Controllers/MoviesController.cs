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

namespace Lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Mostrar()
        {
            return new JsonResult(new { BIENVENIDA = "LABORATORIO 1" }); 
        }

        [HttpGet("compressions")]
        public ActionResult Compressions([FromRoute] string compressions)
        {
            if (compressions=="compressions")
            {
                return Ok();
            }
            return Ok();
        }


        //Post /api/compress/{name} COMPRIMIR
        [HttpPost("compress/{name}")]
        public IActionResult Comprimir([FromRoute] string name, [FromForm]IFormFile file)
        {
            try
            {
                //armar el arbol
                Data.Instance.nombre = name;
                using var fileRead = new FileStream(name, FileMode.OpenOrCreate);
                using var reader = new BinaryReader(fileRead);
                var buffer = new byte[2000];
                while (fileRead.Position < fileRead.Length)
                {
                    buffer = reader.ReadBytes(2000);
                    foreach (var value in buffer)
                    {
                        Data.Instance.huffman.fill((char)value);
                    }
                }
                reader.Close();
                fileRead.Close();
                //encodificar el archivo
                String metadata = Data.Instance.huffman.Huff();

                using var fileRead2 = new FileStream(name, FileMode.OpenOrCreate);
                using var reader2 = new BinaryReader(fileRead);
                buffer = new byte[2000];
                String texto = "";
                while (fileRead2.Position < fileRead2.Length)
                {
                    List<int> intermedio= new List<int>();
                    buffer = reader.ReadBytes(2000);
                    foreach (var value in buffer)
                    {
                       intermedio = Data.Instance.huffman.Encode((char)value);
                    }
                    foreach (int item in intermedio)
                    {
                        texto += item;
                    }
                }
                reader2.Close();
                fileRead2.Close();

                byte[] data = Data.Instance.huffman.GetBytesFromBinaryString(texto);
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
                fileWrite.Close();
                writer.Close();


                //Data.Instance.huffman.Huff();
                //StreamWriter nuevoarchivo = new StreamWriter(name);
                //nuevoarchivo.Write(texto2);

                //name = name + ".huff";
                //TextWriter escritor = new StreamWriter(name, true);
                //string result;
                //using (var reader = new StreamReader(file.OpenReadStream()))
                //{
                //    result = reader.ReadToEnd();
                //}
                return Created("", name);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        // POST: api/descompress DESCOMPRIR
        [Route("descompress")]
        public IActionResult Decompresion([FromRoute] string descompress, [FromForm]IFormFile file)
        {
            using var content = new MemoryStream();
            try
            {
                if (descompress == "descompress")
                {
                    string result="";
                    //using (var reader = new StreamReader(file.OpenReadStream()))
                    //{
                    //    result = reader.ReadToEnd();
                    //}
                    List<char> decoding = new List<char>();
                    using var fileRead = new FileStream("output.huff", FileMode.OpenOrCreate);
                    using var reader = new BinaryReader(fileRead);
                    var buffer = new byte[2000];
                    
                    while (fileRead.Position < fileRead.Length)
                    {
                        buffer = reader.ReadBytes(2000);
                        foreach (var value in buffer)
                        {
                            result+=(char)value;
                        }
                    }
                    reader.Close();
                    fileRead.Close();
                    Data.Instance.huffman.ArmarArbol(result);
                    decoding = Data.Instance.huffman.Decodewometadata(result);
                    foreach (char item in decoding)
                    {
                        Data.Instance.texto += item;
                    }

                    StreamWriter archivo = new StreamWriter(Data.Instance.nombre);
                    archivo.Write(Data.Instance.texto);
                    archivo.Close();
                    return Ok();
                }
                else
                {
                    return BadRequest("No se coloco bien la ruta");
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
