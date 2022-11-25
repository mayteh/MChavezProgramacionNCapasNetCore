using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class AseguradoraController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        
        {
            ML.Result result = BL.Aseguradora.GetAll();
            ML.Aseguradora aseguradora = new ML.Aseguradora();

            if (result.Correct)
            {
                aseguradora.Aseguradoras = result.Objects;
            }
            else
            {
                ViewBag.Message = "Error al cargar la informacion";
            }


            return View(aseguradora);
        }

        [HttpGet]
        public ActionResult Form(int? IdAseguradora) //se usa el int? para que acepte valores null
        {
            ML.Aseguradora aseguradora = new ML.Aseguradora(); //Se instancia aseguradora para poder asignar los usuarios
            aseguradora.Usuario = new ML.Usuario(); //Se inicializa aseguradora.Usuario

            ML.Usuario usuario = new ML.Usuario();

            ML.Result resultUsuarios = BL.Usuario.GetAll(usuario); //Se manda a llamar al metodo de GetAll Usuario para que tome en cuenta las opciones de la lista


            if (IdAseguradora == null)
            {
                aseguradora.Usuario.Usuarios = resultUsuarios.Objects;
                return View(aseguradora);
            }

            else
            {
                ML.Result result = BL.Aseguradora.GetById(IdAseguradora.Value);

              
                if (result.Correct)
                {
                    //Se tiene que hacer un unboxing del objeto que nos trajo el metodo de GEYBYID
                    aseguradora = (ML.Aseguradora)result.Object; //Se usa el object porque solo es 1 objeto 

                    aseguradora.Usuario.Usuarios = resultUsuarios.Objects; //ResultUsuarios trae todos los Usuarios es necesario especificar la ruta: aseguradora.Usuario.Usuarios


                }
                else
                {
                    ViewBag.Message = "Error al cargar la informacion";
                }
                return View(aseguradora);
            }
            return View(aseguradora);
        }

        [HttpPost]
        public ActionResult Form(ML.Aseguradora aseguradora) //se usa el int? para que acepte valores null
        {
            //ML.Usuario usuario = new ML.Usuario();

            if (aseguradora.IdAseguradora == 0)
            {
                ML.Result result = BL.Aseguradora.Add(aseguradora);
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
                ML.Result result = BL.Aseguradora.Update(aseguradora);

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

        public ActionResult Delete(ML.Aseguradora aseguradora)
        {
            ML.Result result = BL.Aseguradora.Delate(aseguradora);
            if (result.Correct)
            {
                ViewBag.Message = result.Message;
            }
            else
            {
                ViewBag.Message = "Error:" + result.Message;
            }
            return PartialView("Modal");
        }
    }
}
