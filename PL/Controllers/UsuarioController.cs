using Microsoft.AspNetCore.Mvc;
using ML;
using System.Drawing.Drawing2D;
using System.Net.Http;
using System.Net.Http.Json;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public UsuarioController(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Result resultRol = BL.Rol.GetAll(); //mostrar roles
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();
            try
            {
                string urlAPI = _configuration["UrlAPI"]; //Se le asigna la direccion a URLAPI
                using (var client = new HttpClient()) //HTTPCLIENT es una clase para hacer llamadas a traves del protocolo HTTP se usa cada que se envia o consulta una informacion de una API
                {
                    client.BaseAddress = new Uri(urlAPI); //Se crea un punto de referencia

                    var responseTask = client.GetAsync("Usuario/GetAll");

                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        foreach (var resultItem in readTask.Result.Objects)
                        {
                            ML.Usuario resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(resultItem.ToString());
                            result.Objects.Add(resultItemList);
                        }
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Ocurrio un error al cargar la información" + result.Ex;
            }

            if (result.Correct)
            {
                usuario.Rol.Roles = resultRol.Objects;
                usuario.Usuarios = result.Objects;
            }
            else
            {
                ViewBag.Message = "Error al cargar la informacion";
            }
            return View (usuario);
        }

        [HttpPost]
        public ActionResult GetAll(string? Nombre, string? ApellidoPaterno, byte? Rol)
        {
            ML.Result resultRol = BL.Rol.GetAll();
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();

            try
            {
                string urlAPI = _configuration["UrlAPI"]; //Se le asigna la direccion a URLAPI
                using (var client = new HttpClient()) //HTTPCLIENT es una clase para hacer llamadas a traves del protocolo HTTP se usa cada que se envia o consulta una informacion de una API
                {
                    client.BaseAddress = new Uri(urlAPI); //Se crea un punto de referencia

                    var responseTask = client.PostAsJsonAsync("Usuario/GetAll" + Nombre + ApellidoPaterno, Rol);

                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        foreach (var resultItem in readTask.Result.Objects)
                        {
                            ML.Usuario resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(resultItem.ToString());
                            result.Objects.Add(resultItemList);
                        }
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Ocurrio un error al cargar la información" + result.Ex;
            }

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
                //ML.Result result = BL.Usuario.GetById(IdUsuario.Value);
                ML.Result result = new ML.Result();
                try
                {
                    string urlAPI = _configuration["UrlAPI"]; //Se le asigna la direccion a URLAPI
                    using (var client = new HttpClient()) //HTTPCLIENT es una clase para hacer llamadas a traves del protocolo HTTP se usa cada que se envia o consulta una informacion de una API
                    {
                        client.BaseAddress = new Uri(urlAPI); //Se crea un punto de referencia

                        var responseTask = client.GetAsync("Usuario/GetById/"+IdUsuario);

                        responseTask.Wait();

                        var resultServicio = responseTask.Result;

                        if (resultServicio.IsSuccessStatusCode)
                        {
                            var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                            readTask.Wait();
                            //ML.Departamento resultItemList = new ML.Departamento();
                            ML.Usuario resultItemList = new ML.Usuario();
                            resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(readTask.Result.Object.ToString());
                            result.Object = resultItemList;
                        }
                        result.Correct = true;
                    }
                }
                catch (Exception ex)
                {
                    result.Correct = false;
                    result.Ex = ex;
                    result.Message = "Ocurrio un error al cargar la información" + result.Ex;
                }



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
                    //return View(usuario);
                }
                return View(usuario);
            }
        }

        [HttpPost]
        public ActionResult Form(ML.Usuario usuario) //se usa el int? para que acepte valores null
        {
            //result.Objects = new List<object>();
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
            //ADD
            if (usuario.IdUsuario == 0)
            {
                ML.Result result = new ML.Result();

                try
                {
                    string urlAPI = _configuration["UrlAPI"]; //Se le asigna la direccion a URLAPI
                    using (var client = new HttpClient()) //HTTPCLIENT es una clase para hacer llamadas a traves del protocolo HTTP se usa cada que se envia o consulta una informacion de una API
                    {
                        client.BaseAddress = new Uri(urlAPI); //Se crea un punto de referencia

                        var postTask = client.PostAsJsonAsync("Usuario/Add", usuario);

                        postTask.Wait();

                        var resultServicio = postTask.Result;

                        if (resultServicio.IsSuccessStatusCode)
                        {
                            //return RedirectToAction("GetAll");
                            result.Correct = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Correct = false;
                    result.Ex = ex;
                    result.Message = "Ocurrio un error al cargar la información" + result.Ex;
                }
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
                ML.Result result = new ML.Result();
                try
                {
                    string urlAPI = _configuration["UrlAPI"]; //Se le asigna la direccion a URLAPI
                    using (var client = new HttpClient()) //HTTPCLIENT es una clase para hacer llamadas a traves del protocolo HTTP se usa cada que se envia o consulta una informacion de una API
                    {
                        client.BaseAddress = new Uri(urlAPI); //Se crea un punto de referencia

                        //var postTask = client.PostAsJsonAsync<ML.Usuario>("Usuario/Update", usuario);
                        var postTask = client.PutAsJsonAsync<ML.Usuario>("Usuario/Update", usuario);

                        postTask.Wait();

                        var resultServicio = postTask.Result;

                        if (resultServicio.IsSuccessStatusCode)
                        {
                            //return RedirectToAction("GetAll");
                            result.Correct = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Correct = false;
                    result.Ex = ex;
                    result.Message = "Ocurrio un error al cargar la información" + result.Ex;
                }
                //return View(usuario);
                if (result.Correct)
                {
                    ViewBag.Message = result.Message;
                }
                else
                {
                    ViewBag.Message = "Error:" + result.Message;
                }
            }

            //if (ModelState.IsValid)
            //{
            //    if (usuario.IdUsuario == 0)
            //    {

            //        ML.Result result = BL.Usuario.Add(usuario);
            //        if (result.Correct)
            //        {
            //            ViewBag.Message = result.Message;
            //        }
            //        else
            //        {
            //            ViewBag.Message = "Error:" + result.Message;
            //        }

            //    }

            //    else
            //    {
            //        ML.Result result = BL.Usuario.Update(usuario);

            //        if (result.Correct)
            //        {
            //            ViewBag.Message = result.Message;
            //        }
            //        else
            //        {
            //            ViewBag.Message = "Error: " + result.Message;
            //        }
            //    }

            //    return PartialView("Modal");
            //}
            //else
            //{
            //    ML.Result result = BL.Rol.GetAll();
            //    ML.Result resultPais = BL.Pais.GetAll();

            //    usuario.Rol = new ML.Rol();

            //    usuario.Direccion = new ML.Direccion();
            //    usuario.Direccion.Colonia = new ML.Colonia();
            //    usuario.Direccion.Colonia.Municipio = new ML.Municipio();
            //    usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
            //    usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();

            //    usuario.Rol.Roles = result.Objects;
            //    usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;
            //    return View(usuario);
            //}

            return PartialView("Modal");
        }

        public ActionResult Delete(int IdUsuario)
        {
            ML.Result result = new ML.Result();
            ML.Usuario usuario = new ML.Usuario();
            usuario.IdUsuario = IdUsuario;
            if (IdUsuario >= 1)
            {

                try
                {
                    string urlAPI = _configuration["UrlAPI"]; //Se le asigna la direccion a URLAPI
                    using (var client = new HttpClient()) //HTTPCLIENT es una clase para hacer llamadas a traves del protocolo HTTP se usa cada que se envia o consulta una informacion de una API
                    {
                        client.BaseAddress = new Uri(urlAPI); //Se crea un punto de referencia

                        //var postTask = client.PostAsJsonAsync<ML.Usuario>("Usuario/Update/" + usuario.IdUsuario, usuario);
                        var postTask = client.DeleteAsync("Usuario/Delete/" + IdUsuario);

                        postTask.Wait();

                        var resultServicio = postTask.Result;

                        if (resultServicio.IsSuccessStatusCode)
                        {
                            //return RedirectToAction("GetAll");
                            result.Correct = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Correct = false;
                    result.Ex = ex;
                    result.Message = "Ocurrio un error al cargar la información" + result.Ex;
                }



                //ML.Result result = BL.Usuario.DelateEF(idUsuario);
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
