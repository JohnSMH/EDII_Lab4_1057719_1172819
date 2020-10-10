using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API_HUFFMAN;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace API_HUFFMAN.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HuffmanController : ControllerBase
    {
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
                TextWriter escritor = new StreamWriter(name, true);
                string result;
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                     result = reader.ReadToEnd();
                }
                var huffman = new Huffman<char>(result);
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
        [Route("populate")]
        public IActionResult PostMovies([FromForm]IFormFile file)
        {
            using var content = new MemoryStream();
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
