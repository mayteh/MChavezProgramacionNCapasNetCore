using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();
            ML.Result result = new ML.Result();

            usuario.Rol = new ML.Rol();

            ML.Result resultRol = BL.Rol.GetAll();
            result = BL.Usuario.GetAll(usuario);
           

            if (result.Correct)
            {
                usuario.Usuarios = result.Objects;
                usuario.Rol.Roles = resultRol.Objects;
            }
            else
            {
                ViewBag.Message = "Error al cargar la informacion";
            }


            return View(usuario);
        }

        [HttpPost]
        public ActionResult GetAll(ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.GetAll(usuario);
            usuario.Rol = new ML.Rol();

            ML.Result resultRol = BL.Rol.GetAll();

            //ML.Result result = BL.Usuario.GetAll(usuario);
            if (result.Correct)
            {
                usuario.Usuarios = result.Objects;
                usuario.Rol.Roles = resultRol.Objects;
            }
            else
            {
                ViewBag.Message = "Error al cargar la informacion";
            }


            return View(usuario);
        }



        [HttpGet] //Verifica si el usuario viene null o con informacion
        public ActionResult Form(int? IdUsuario) //se usa el int? para que acepte valores null
        {
            ML.Usuario usuario = new ML.Usuario(); //Se instancia la clase usuario para poder asignar el Rol
            usuario.Rol = new ML.Rol();

            usuario.Direccion = new ML.Direccion();
            usuario.Direccion.Colonia = new ML.Colonia();
            usuario.Direccion.Colonia.Municipio = new ML.Municipio();
            usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
            usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();

            ML.Result resultPais = BL.Pais.GetAll();
            ML.Result resultRol = BL.Rol.GetAll(); //Se manda a llamar al metodo de GetAll Rol para que tome en cuenta las opciones de la lista



            if (IdUsuario == null)
            {
                usuario.Rol.Roles = resultRol.Objects;
                usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;
                return View(usuario);
            }

            else
            {
                //GET BY ID
                ML.Result result = BL.Usuario.GetById(IdUsuario.Value);

                if (result.Correct)
                {
                    //Se tiene que hacer un unboxing del objeto que nos trajo el metodo de GEYBYID este a su vez los muestra en la vista
                    usuario = (ML.Usuario)result.Object;// Unboxing
                    usuario.Rol.Roles = resultRol.Objects; //ResultRol trae todos los Roles es necesario especificar la ruta: usuario.Rol.Roles


                    //Para mostrar direccion en el update
                    usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;
                    ML.Result resultEstados = BL.Estado.EstadoGetByIdPais(usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais);

                    usuario.Direccion.Colonia.Municipio.Estado.Estados = resultEstados.Objects;//unboxing

                    ML.Result resultMunicipios = BL.Municipio.MunicipioGetByIdEstado(usuario.Direccion.Colonia.Municipio.Estado.IdEstado);
                    usuario.Direccion.Colonia.Municipio.Municipios = resultMunicipios.Objects;//unboxing

                    ML.Result resultColonias = BL.Colonia.ColoniaGetBYIdMunicipio(usuario.Direccion.Colonia.Municipio.IdMunicipio);
                    usuario.Direccion.Colonia.Colonias = resultColonias.Objects;//unboxing

                    return View(usuario);
                }
                else
                {
                    ViewBag.Message = "Error al cargar la informacion";
                }
                return View(usuario);
            }
        }

        [HttpPost]
        public ActionResult Form(ML.Usuario usuario) //se usa el int? para que acepte valores null
        {
            IFormFile image = Request.Form.Files["ImagenData"];
            /*ML.Usuario usuariov = new ML.Usuario();*/ //Se instancia la clase usuario para poder asignar el Rol

            //valido si traigo imagen
            if (image != null)
            {
                //llamar al metodo que convierte a bytes la imagen
                byte[] ImagenBytes = ConvertToBytes(image);
                //convierto a base 64 la imagen y la guardo en la propiedad de imagen en el objeto alumno
                usuario.Imagen = Convert.ToBase64String(ImagenBytes);
            }

            if(ModelState.IsValid)
            {
                if (usuario.IdUsuario == 0)
                {

                    ML.Result result = BL.Usuario.Add(usuario);
                    if (result.Correct)
                    {
                        ViewBag.Message = result.Message;
                    }
                    else
                    {
                        ViewBag.Message = "Error:" + result.Message;
                    }

                }

                else
                {
                    ML.Result result = BL.Usuario.Update(usuario);

                    if (result.Correct)
                    {
                        ViewBag.Message = result.Message;
                    }
                    else
                    {
                        ViewBag.Message = "Error: " + result.Message;
                    }
                }

                return PartialView("Modal");
            }
            else
            {
                ML.Result result = BL.Rol.GetAll();
                ML.Result resultPais = BL.Pais.GetAll();

                usuario.Rol = new ML.Rol();

                usuario.Direccion = new ML.Direccion();
                usuario.Direccion.Colonia = new ML.Colonia();
                usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();

                usuario.Rol.Roles = result.Objects;
                usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;
                return View(usuario);
            }
        }

        public ActionResult Delete(int idUsuario)
        {
            if (idUsuario >= 1)
            {   /*ML.Usuario usuario = new ML.Usuario();*/
                ML.Result result = BL.Usuario.DelateEF(idUsuario);
                if (result.Correct)
                {
                    ViewBag.Message = result.Message;
                }
                else
                {
                    ViewBag.Message = "Error:" + result.Message;
                }

            }

            return PartialView("Modal");
        }

        //JSON

        public JsonResult GetEstado(int IdPais)
        {
            var result = BL.Estado.EstadoGetByIdPais(IdPais);   //Se inicializa el result con el metodo EstadoGetByIdPais
            return Json(result.Objects);
        }

        public JsonResult GetMunicipio(int IdEstado)
        {
            var result = BL.Municipio.MunicipioGetByIdEstado(IdEstado);   //Se inicializa el result con el metodo EstadoGetByIdPais
            return Json(result.Objects);
        }

        public JsonResult GetColonia(int IdMunicipio)
        {
            var result = BL.Colonia.ColoniaGetBYIdMunicipio(IdMunicipio);   //Se inicializa el result con el metodo EstadoGetByIdPais
            return Json(result.Objects);
        }

        public byte[] ConvertToBytes(IFormFile Imagen)
        {
            using var fileStream = Imagen.OpenReadStream();
            
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);

            return bytes;
        }  //COnvertir en un arreglo de bytes


        //PARA STATUS

        public JsonResult ActualizarEstatus(int idUsuario, bool status)
        {
            var result = BL.Usuario.ChangeStatus(idUsuario, status);   //Se inicializa el result con el metodo ChangeStatus
            return Json(result.Object);
        }
    }
}
