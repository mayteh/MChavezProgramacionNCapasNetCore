using Microsoft.AspNetCore.Mvc;

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
        public ActionResult Form(string? NumeroEmpleado) //se usa el int? para que acepte valores null
        {
            ML.Empleado empleado = new ML.Empleado(); //Se crea un nuevo empleado
            ML.Empresa empresa = new ML.Empresa();
            ML.Result resultEmpresa = BL.Empresa.GetAll(empresa);
            
            empleado.Empresa = new ML.Empresa();

            

            if (NumeroEmpleado == null)
            {
                empleado.Action = "Add";
                empleado.Empresa.Empresas = resultEmpresa.Objects;
                return View(empleado);
            }

            else
            {
                empleado.Action = "Update";
                //GET BY ID
                ML.Result result = BL.Empleado.GetById(NumeroEmpleado);

                if (result.Correct)
                {
                    //Se tiene que hacer un unboxing del objeto que nos trajo el metodo de GEYBYID este a su vez los muestra en la vista
                    empleado = (ML.Empleado)result.Object;// Unboxing
                    empleado.Empresa.Empresas = resultEmpresa.Objects; //ResultRol trae todos los Roles es necesario especificar la ruta: usuario.Rol.Roles

                    return View(empleado);
                }
                else
                {
                    ViewBag.Message = "Error al cargar la informacion";
                }
                
            }
            return View(empleado);
        }
        

        [HttpPost]
        public ActionResult Form(ML.Empleado empleado) //se usa el int? para que acepte valores null
        {
            IFormFile image = Request.Form.Files["ImagenData"];
            /*ML.Usuario usuariov = new ML.Usuario();*/ //Se instancia la clase usuario para poder asignar el Rol

            //valido si traigo imagen
            if (image != null)
            {
                //llamar al metodo que convierte a bytes la imagen
                byte[] ImagenBytes = ConvertToBytes(image);
                //convierto a base 64 la imagen y la guardo en la propiedad de imagen en el objeto alumno
                empleado.Foto = Convert.ToBase64String(ImagenBytes);
            }
            //empleado.NumeroEmpleado == "0"

            
                if (empleado.Action == "Add")
                {

                    ML.Result result = BL.Empleado.Add(empleado);

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
                    ML.Result result = BL.Empleado.Update(empleado);

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

        public ActionResult Delete(string NumeroEmpleado)
        {
            if (NumeroEmpleado != null)
            {   /*ML.Usuario usuario = new ML.Usuario();*/
                ML.Result result = BL.Empleado.DelateEF(NumeroEmpleado);
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

        public byte[] ConvertToBytes(IFormFile Imagen)
        {
            using var fileStream = Imagen.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);

            return bytes;
        }
    }
}
