using Microsoft.AspNetCore.Mvc;
using ML;
using System.Drawing.Drawing2D;
using System.IO;
namespace PL.Controllers
{
    public class CargaMasiva : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public CargaMasiva(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }



        [HttpGet]
        public ActionResult UsuarioCargaMasiva()
        {
            ML.Result result = new ML.Result();
            return View(result);
        }

        [HttpPost]
        public ActionResult CargaTXT(ML.Usuario usuario)
        {
            IFormFile file = Request.Form.Files["archivoTXT"];

            if (file != null)//Si existe el archivo en la ruta
            {
                StreamReader textFile = new StreamReader(file.OpenReadStream());
                string line = textFile.ReadLine();
                //line = textFile.ReadLine();

                ML.Result resultEx = new ML.Result();
                resultEx.Objects = new List<object>();

                while ((line = textFile.ReadLine()) != null) //Mientras existan lineas por leer
                {
                    string[] lines = line.Split('|'); //Del archivo .txt Split va a quitar todos los '|' que existan

                    //ML.Usuario usuario = new ML.Usuario();

                    usuario.NombreUsuario = lines[1];
                    usuario.ApellidoPaternoU = lines[2];
                    usuario.ApellidoMaternoU = lines[3];
                    usuario.FechaNacimiento = lines[4];
                    usuario.Genero = lines[5];
                    usuario.Password = lines[6];
                    usuario.Telefono = lines[7];
                    usuario.Celular = lines[8];
                    usuario.Curp = lines[9];
                    usuario.UserName = lines[10];
                    usuario.Email = lines[11];

                    usuario.Rol = new ML.Rol(); //Se instancia o inicializa para poder utilizar sus propiedades
                    usuario.Rol.IdRol = byte.Parse(lines[12]);

                    usuario.Imagen = null;

                    usuario.Direccion = new ML.Direccion(); //Se instancia para poder usar sus propiedades

                    usuario.Direccion.Calle = lines[13];
                    usuario.Direccion.NumeroInterior = lines[14];
                    usuario.Direccion.NumeroExterior = lines[15];

                    usuario.Direccion.Colonia = new ML.Colonia();
                    usuario.Direccion.Colonia.IdColonia = int.Parse(lines[16]);

                    ML.Result result = BL.Usuario.Add(usuario);

                    if (!result.Correct)
                    {
                             
                            //resultErrores.Objects.Add(
                            //"No se inserto el Nombre " + usuario.NombreUsuario +
                            //"No se inserto el Apellido Paterno " + usuario.ApellidoPaternoU +
                            //"No se inserto el Apellido Materno" + usuario.ApellidoMaternoU +
                            //"No se inserto el Fecha de nacimiento" + usuario.FechaNacimiento +
                            //"No se inserto el Genero" + usuario.Genero +
                            //"No se inserto el Password" + usuario.Password +
                            //"No se inserto el Telefono" + usuario.Telefono +
                            //"No se inserto el Celular" + usuario.Celular +
                            //"No se inserto el Curp" + usuario.Curp +
                            //"No se inserto el UserName" + usuario.UserName +
                            //"No se inserto el Email" + usuario.Email +
                            //"No se inserto el Rol" + usuario.Rol.IdRol +
                            //"No se inserto el Calle" + usuario.Direccion.Calle +
                            //"No se inserto el Numero int" + usuario.Direccion.NumeroInterior +
                            //"No se inserto el Numero Ext" + usuario.Direccion.NumeroExterior +
                            //"No se inserto e Id de la colonia" + usuario.Direccion.Colonia.IdColonia);
                    }
                    if (resultEx.Objects != null)
                    {
                        ViewBag.Message = "Los datos han sido registrados correctamente";
                    }
                    TextWriter fileError = new StreamWriter(@"C:\Users\digis\OneDrive\Documents\Mayte Chavez Salazar\ErroresLayout.txt");

                    foreach (string error in resultEx.Objects)
                    {
                        fileError.WriteLine(error); //Se le concatenan todos los errores
                    }
                    fileError.Close(); //se cierra el archivo
                }
            }   //cierre del if
            return PartialView("Modal");
        } //cierre del metodo post


        [HttpPost]
        public ActionResult CargaEXCEL(ML.Usuario usuario)
        {

            IFormFile archivoExcel = Request.Form.Files["fileExc"]; //Crea la sesion se necesita el nombre en la vista "fileExc"
            //Session 

            if (HttpContext.Session.GetString("PathArchivo") == null) //Verifica que la sesion sea nula o que no exista
            {
                if (archivoExcel.Length > 0)
                {
                    //que sea .xlsx
                    string fileName = Path.GetFileName(archivoExcel.FileName);  //obtenemos el nombre del archivo
                    string folderPath = _configuration["PathFolder:value"];     //Obtenemos la ruta
                    string extensionArchivo = Path.GetExtension(archivoExcel.FileName).ToLower(); //Se obtiene la extension .xlsx
                    string extensionModulo = _configuration["TipoExcel"]; // Se define el tipo de archivo que es

                    if (extensionArchivo == extensionModulo)
                    {
                        //crear copia
                        string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, folderPath, Path.GetFileNameWithoutExtension(fileName)) + '-' + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                        if (!System.IO.File.Exists(filePath))
                        {
                            using (FileStream stream = new FileStream(filePath, FileMode.Create)) // Crea una copia del archivo
                            {
                                archivoExcel.CopyTo(stream); //Copia los datos del archivo original a la copia
                            }
                            //leer

                            //A connectionString se le agrega el valor de ConnectionStringExcel Que tenemos en Appsettings y el path = direccion del archivo
                            string connectionString = _configuration["ConnectionStringExcel:value"] + filePath;

                            ML.Result resultConvertExcel = BL.Usuario.ConvertirExceltoDataTable(connectionString); //Se manda a llamar al metodos en la var resultData
                            if (resultConvertExcel.Correct)
                            {
                                ML.Result resultValidacion = BL.Usuario.ValidarExcel(resultConvertExcel.Objects);
                                if (resultValidacion.Objects.Count == 0)
                                {
                                    resultValidacion.Correct = true;
                                    HttpContext.Session.SetString("PathArchivo", filePath);
                                }

                                return View("UsuarioCargaMasiva",resultValidacion);
                            }
                            else
                            {
                                //error al leer el archivo
                                ViewBag.Message = "Ocurrio un error al leer el arhivo";
                                return View("Modal");
                            }
                        }
                    }
                }
         
            }

            else
            {
                string rutaArchivoExcel = HttpContext.Session.GetString("PathArchivo");
                string connectionString = _configuration["ConnectionStringExcel:value"] + rutaArchivoExcel;

                ML.Result resultData = BL.Usuario.ConvertirExceltoDataTable(connectionString);
                if (resultData.Correct)
                {
                    ML.Result resultErrores = new ML.Result();
                    resultErrores.Objects = new List<object>();

                    foreach (ML.Usuario usuarioItem in resultData.Objects)
                    {

                        ML.Result resultAdd = BL.Usuario.Add(usuarioItem);
                        if (!resultAdd.Correct)
                        {
                            resultErrores.Objects.Add("No se insertó el usuario con nombre: " + usuarioItem.NombreUsuario + " Error: " + resultAdd.Message);
                        }
                    }
                    if (resultErrores.Objects.Count > 0)
                    {

                        //string fileError = Path.Combine(_hostingEnvironment.WebRootPath, @"~\Files\logErrores.txt");
                        string fileError = _hostingEnvironment.WebRootPath + @"\Files\logErrores.txt";
                        using (StreamWriter writer = new StreamWriter(fileError))
                        {
                            foreach (string ln in resultErrores.Objects)
                            {
                                writer.WriteLine(ln);
                            }
                        }
                        ViewBag.Message = "Los usuarios No han sido registrados correctamente";
                    }
                    else
                    {
                        ViewBag.Message = "Los usuarios han sido registrados correctamente";

                    }

                }

            }
            return PartialView("Modal");
        }

    }   //cierre de la clase
}  //cierre de namespace
