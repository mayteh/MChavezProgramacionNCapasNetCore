using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        // GET: api/<UsuarioController>
        [HttpGet("GetAll")]
        //public IEnumerable<string> Get()
        public IActionResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();
            ML.Result result = BL.Usuario.GetAll(usuario);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
            //return new string[] { "value1", "value2" };
        }

        [HttpPost("GetAll")]
        //public IEnumerable<string> Get()
        public IActionResult GetAll(string? Nombre, string? ApellidoPaterno, byte Rol)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();
            usuario.NombreUsuario = (Nombre == null) ? "" : Nombre;
            usuario.ApellidoPaternoU = (ApellidoPaterno == null) ? "" : ApellidoPaterno;
            usuario.Rol.IdRol = (byte)((Rol == null) ? 0 : Rol);
            ML.Result result = BL.Usuario.GetAll(usuario);
            

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
            //return new string[] { "value1", "value2" };
        }

        // GET api/<UsuarioController>/5
        [HttpGet("GetById/{idUsuario}")]
        public IActionResult GetById(int idUsuario)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();
            ML.Result result = BL.Usuario.GetById(idUsuario);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
            //return "value";
        }

        // POST api/<UsuarioController>
        [HttpPost("Add")]
        public IActionResult Post([FromBody] ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.Add(usuario);


            if(result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        // PUT api/<UsuarioController>/5
        [HttpPost("Update/{IdUsuario}")]
        public IActionResult Put(int IdUsuario, [FromBody]  ML.Usuario usuario)
        {
            usuario.IdUsuario = IdUsuario;
            //usuario.Rol = new ML.Rol();
            ML.Result result = BL.Usuario.Update(usuario);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        // DELETE api/<UsuarioController>/5
        [HttpDelete("Delete/{IdUsuario}")]
        public IActionResult Delete(int IdUsuario)
        { 
                ML.Result result = BL.Usuario.DelateEF(IdUsuario);
                if (result.Correct)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
        }
    }
}
