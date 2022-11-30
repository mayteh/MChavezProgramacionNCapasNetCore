﻿using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class EmpleadoController : Controller
    {
            [HttpGet]
            public ActionResult GetAll()
            {
                ML.Empleado empleado = new ML.Empleado();
                ML.Result result = BL.Empleado.GetAll();

                if (result.Correct)
                {
                    empleado.Empleados = result.Objects;
                }
                else
                {
                    ViewBag.Message = "Error al cargar la informacion";
                }


                return View(empleado);
            }
        [HttpGet] //Verifica si el usuario viene null o con informacion
        public ActionResult Form(string? NumeroUsuario) //se usa el int? para que acepte valores null
        {
            ML.Empresa empresa = new ML.Empresa();
            ML.Empleado empleado = new ML.Empleado(); //Se instancia la clase usuario para poder asignar el Rol
            empleado.Empresa = new ML.Empresa();

            ML.Result resultEmpresa = BL.Empresa.GetAll(empresa);

            if (NumeroUsuario == null)
            {
                empleado.Empresa.Empresas = resultEmpresa.Objects;
                return View(empleado);
            }

            else
            {
                ViewBag.Message = "Error al cargar la informacion";
            }
            return View(empleado);
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



        public byte[] ConvertToBytes(IFormFile Imagen)
        {
            using var fileStream = Imagen.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);

            return bytes;
        }
    }
}
