using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class EmpresaController : Controller
    {
        // GET: Empresa
        [HttpGet]
            public ActionResult GetAll()
            {
                ML.Empresa empresa = new ML.Empresa();
                ML.Result result = BL.Empresa.GetAll(empresa);

            if (result.Correct)
                {
                    empresa.Empresas = result.Objects;
                }
                else
                {
                    ViewBag.Message = "Error al cargar la informacion";
                }


                return View(empresa);
            }


        [HttpPost]
        public ActionResult GetAll(ML.Empresa empresa)
        {
            ML.Result result = BL.Empresa.GetAll(empresa);

            //ML.Result result = BL.Usuario.GetAll(usuario);
            if (result.Correct)
            {
                empresa.Empresas = result.Objects;
            }
            else
            {
                ViewBag.Message = "Error al cargar la informacion";
            }


            return View(empresa);
        }



        [HttpGet]
            public ActionResult Form(int? IdEmpresa) //se usa el int? para que acepte valores null
            {
                ML.Empresa empresa = new ML.Empresa();
                if (IdEmpresa == null)
                {
                return View(empresa);
                }

                else
                {
                    ML.Result result = BL.Empresa.GetById(IdEmpresa.Value);

                    //ML.Empresa empresa = new ML.Empresa();

                    if (result.Correct)
                    {
                        //Se tiene que hacer un unboxing del objeto que nos trajo el metodo de GEYBYID
                        empresa = (ML.Empresa)result.Object; //Se usa el object porque solo es 1 objeto 
                    }
                    else
                    {
                        ViewBag.Message = "Error al cargar la informacion";
                    }
                    return View(empresa);
                }
            }

            [HttpPost]
            public ActionResult Form(ML.Empresa empresa) //se usa el int? para que acepte valores null
            {
            
            IFormFile image = Request.Form.Files["ImagenData"];


            //valido si traigo imagen
            if (image != null)
            {
                //llamar al metodo que convierte a bytes la imagen
                byte[] LogoBytes = ConvertToBytes(image);
                //convierto a base 64 la imagen y la guardo en la propiedad de imagen en el objeto alumno
                empresa.Logo = Convert.ToBase64String(LogoBytes);
            }

            if (empresa.IdEmpresa == 0)
                {
                    ML.Result result = BL.Empresa.Add(empresa);
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
                    ML.Result result = BL.Empresa.Update(empresa);

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

            public ActionResult Delete(ML.Empresa empresa)
            {
                ML.Result result = BL.Empresa.Delate(empresa);
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




        //JSON//

        public byte[] ConvertToBytes(IFormFile Imagen)
        {
            using var fileStream = Imagen.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);

            return bytes;
        }  //COnvertir en un arreglo de bytes

    }
}

