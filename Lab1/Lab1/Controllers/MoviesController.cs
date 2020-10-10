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

        [HttpGet("{traversal}")]
        public ActionResult GetByOrder([FromRoute] string traversal)
        {
            return NotFound();
        }


        //Post /api/compress/{name} COMPRIMIR
        [HttpPost("{name}")]
        public IActionResult Comprimir([FromRoute] string name, [FromForm]IFormFile file)
        {
            try
            {
                name = name + ".huff";
                TextWriter escritor = new StreamWriter(name, true);
                string result;
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    result = reader.ReadToEnd();
                }
                
                List<int> encoding = huffman.Encode(result);

                var chars = new HashSet<char>(result);
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

                string texto2 = huffman.conoceri + texto;
                StreamWriter nuevoarchivo = new StreamWriter(name);
                nuevoarchivo.Write(texto2);
                return Created("", name);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        // POST: api/movies/populate/<Peliculas>
        [Route("descompress")]
        public IActionResult Decompresion([FromRoute] string descompress, [FromForm]IFormFile file)
        {
            using var content = new MemoryStream();
            try
            {
                if (descompress == "descompress")
                {
                    string result;
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        result = reader.ReadToEnd();
                    }
                    var huffman = new Huffman<char>(result);
                    List<char> decoding = huffman.ArmarArbol(result);
                    foreach (char item in decoding)
                    {
                        Console.Write(item);
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
