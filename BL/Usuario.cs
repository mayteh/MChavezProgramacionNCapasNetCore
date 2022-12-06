using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {
        public static ML.Result GetAll(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MchavezProgramacionNcapasContext contex = new DL.MchavezProgramacionNcapasContext())//Se obtiene la conexion
                {
                    /*usuario.Rol.IdRol = (usuario.Rol.IdRol == null) ? 0 : usuario.Rol.IdRol;*/ //operador ternario

                    var usuarios = contex.Usuarios.FromSqlRaw($"UsuarioGetAll '{usuario.NombreUsuario}', '{usuario.ApellidoPaternoU}', {usuario.Rol.IdRol}").ToList();

                    result.Objects = new List<object>();
                    //result.Objects = new List<object>();

                    if (usuarios != null)//Si las rows dentro de nuestra lista query es diferente de null
                    {
                        foreach (var objeto in usuarios)// Sacar los row de nuestra lista
                        {

                            ML.Usuario usuarioobj = new ML.Usuario();

                            usuarioobj.IdUsuario = objeto.IdUsuario;
                            usuarioobj.NombreUsuario = objeto.NombreUsuario;
                            usuarioobj.ApellidoPaternoU = objeto.ApellidoPaterno;
                            usuarioobj.ApellidoMaternoU = objeto.ApellidoMaterno;
                            usuarioobj.FechaNacimiento = objeto.FechaNacimiento.ToString("dd-MM-yyyy");
                            usuarioobj.Genero = objeto.Sexo;
                            usuarioobj.Password = objeto.Password;
                            usuarioobj.Telefono = objeto.Telefono;
                            usuarioobj.Celular = objeto.Celular;
                            usuarioobj.Curp = objeto.Curp;
                            usuarioobj.UserName = objeto.UserName;
                            usuarioobj.Email = objeto.Email;


                            usuarioobj.Rol = new ML.Rol();
                            usuarioobj.Rol.IdRol = byte.Parse(objeto.IdRol.ToString());
                            usuarioobj.Rol.NombreRol = objeto.NombreRol;

                            usuarioobj.Imagen = objeto.Imagen;

                            //usuario.Imagen = byte[].Parse(objeto.Imagen.ToString());
                            //DIRECCION//
                            usuarioobj.Direccion = new ML.Direccion(); //Se inicializa
                            usuarioobj.Direccion.IdDireccion = objeto.IdDireccion;
                            usuarioobj.Direccion.Calle = objeto.Calle;
                            usuarioobj.Direccion.NumeroInterior = objeto.NumeroInterior;
                            usuarioobj.Direccion.NumeroExterior = objeto.NumeroExterior;
                            ////COLONIA//
                            usuarioobj.Direccion.Colonia = new ML.Colonia();
                            usuarioobj.Direccion.Colonia.IdColonia = objeto.IdColonia;
                            usuarioobj.Direccion.Colonia.NombreColonia = objeto.NombreColonia;
                            usuarioobj.Direccion.Colonia.CodigoPostal = objeto.CodigoPostal;
                            ////MUNICIPIO//
                            usuarioobj.Direccion.Colonia.Municipio = new ML.Municipio();
                            usuarioobj.Direccion.Colonia.Municipio.IdMunicipio = objeto.IdMunicipio;
                            usuarioobj.Direccion.Colonia.Municipio.NombreMunicipio = objeto.NombreMunicipio;
                            ////ESTADO//
                            usuarioobj.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                            usuarioobj.Direccion.Colonia.Municipio.Estado.IdEstado = objeto.IdEstado;
                            usuarioobj.Direccion.Colonia.Municipio.Estado.NombreEstado = objeto.NombreEstado;
                            ////PAIS// 
                            usuarioobj.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                            usuarioobj.Direccion.Colonia.Municipio.Estado.Pais.IdPais = objeto.IdPais;
                            usuarioobj.Direccion.Colonia.Municipio.Estado.Pais.NombrePais = objeto.NombrePais;

                            usuarioobj.Status = objeto.Status.Value;

                            result.Objects.Add(usuarioobj);//Agrega todos los datos a usuario
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No existen registros en la tabla de usuario";
                    }

                }
                result.Correct = true;
            }// cierre de try

            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Ocurrio un error al cargar la información" + result.Ex;
                throw;
            }//(Algún error en el programa)
            return result;
        }

        public static ML.Result GetById(int idUsuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MchavezProgramacionNcapasContext context = new DL.MchavezProgramacionNcapasContext())
                {
                    //var tableUsuario = context.UsuarioGetById(idUsuario).SingleOrDefault();
                    var tableUsuario = context.Usuarios.FromSqlRaw($"UsuarioGetById {idUsuario}").AsEnumerable().FirstOrDefault(); //No olvidar el signo de pesos
                    result.Objects = new List<object>();
                    if (tableUsuario != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();

                        usuario.IdUsuario = tableUsuario.IdUsuario;
                        usuario.NombreUsuario = tableUsuario.NombreUsuario;
                        usuario.ApellidoPaternoU = tableUsuario.ApellidoPaterno;
                        usuario.ApellidoMaternoU = tableUsuario.ApellidoMaterno;
                        usuario.FechaNacimiento = tableUsuario.FechaNacimiento.ToString("dd-MM-yyyy");
                        usuario.Genero = tableUsuario.Sexo;
                        usuario.Password = tableUsuario.Password;
                        usuario.Telefono = tableUsuario.Telefono;
                        usuario.Celular = tableUsuario.Telefono;
                        usuario.Curp = tableUsuario.Curp;
                        usuario.UserName = tableUsuario.UserName;
                        usuario.Email = tableUsuario.Email;
                        //Se tiene que instanciar el objeto para poderlo usarlo y asi almacenar
                        usuario.Rol = new ML.Rol();
                        usuario.Rol.IdRol = byte.Parse(tableUsuario.IdRol.ToString());

                        usuario.Imagen = tableUsuario.Imagen;

                        usuario.Direccion = new ML.Direccion();
                        usuario.Direccion.IdDireccion = tableUsuario.IdDireccion;
                        usuario.Direccion.Calle = tableUsuario.Calle;
                        usuario.Direccion.NumeroInterior = tableUsuario.NumeroInterior;
                        usuario.Direccion.NumeroExterior = tableUsuario.NumeroExterior;
                        //COLONIA//
                        usuario.Direccion.Colonia = new ML.Colonia();
                        usuario.Direccion.Colonia.IdColonia = tableUsuario.IdColonia;
                        usuario.Direccion.Colonia.NombreColonia = tableUsuario.NombreColonia;
                        usuario.Direccion.Colonia.CodigoPostal = tableUsuario.CodigoPostal;
                        //MUNICIPIO//
                        usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                        usuario.Direccion.Colonia.Municipio.IdMunicipio = tableUsuario.IdMunicipio;
                        usuario.Direccion.Colonia.Municipio.NombreMunicipio = tableUsuario.NombreMunicipio;
                        //ESTADO//
                        usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                        usuario.Direccion.Colonia.Municipio.Estado.IdEstado = tableUsuario.IdEstado;
                        usuario.Direccion.Colonia.Municipio.Estado.NombreEstado = tableUsuario.NombreEstado;
                        //PAIS// 
                        usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                        usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais = tableUsuario.IdPais;
                        usuario.Direccion.Colonia.Municipio.Estado.Pais.NombrePais = tableUsuario.NombrePais;

                        usuario.Status = tableUsuario.Status.Value;

                        //boxing
                        //Almacenar cualquier tipo de dato en un dato object
                        result.Object = usuario;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Ocurrio un error al cargar la información" + result.Ex;
                throw;
            }
            return result;
        }

        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result(); //instancia de Result
            try
            {
                using (DL.MchavezProgramacionNcapasContext contex = new DL.MchavezProgramacionNcapasContext())
                {
                    int query = contex.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.NombreUsuario}', '{usuario.ApellidoPaternoU}', '{usuario.ApellidoMaternoU}','{usuario.FechaNacimiento}', '{usuario.Genero}', '{usuario.Password}', '{usuario.Telefono}', '{usuario.Celular}', '{usuario.Curp}', '{usuario.UserName}','{usuario.Email}',{usuario.Rol.IdRol}, '{usuario.Imagen}', {usuario.Status},'{usuario.Direccion.Calle}', '{usuario.Direccion.NumeroInterior}', '{usuario.Direccion.NumeroExterior}', {usuario.Direccion.Colonia.IdColonia}");

                    if (query > 0)
                    {
                        result.Message = "El usuario se agrego con exito";
                    }
                }
                result.Correct = true;
            }

            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Ocurrio un error al cargar la información" + result.Ex;
            }//(Algún error en el programa)
            return result;
        }

        public static ML.Result Update(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MchavezProgramacionNcapasContext context = new DL.MchavezProgramacionNcapasContext())
                {
                    //int query = context.Database.ExecuteSqlRaw($"UsuarioUpdate {usuario.IdUsuario}, '{usuario.NombreUsuario}', '{usuario.ApellidoPaternoU}', '{usuario.ApellidoMaternoU}', '{usuario.FechaNacimiento}', '{usuario.Genero}', '{usuario.Password}', '{usuario.Telefono}', '{usuario.Celular}', '{usuario.Curp}', '{usuario.UserName}','{usuario.Email}', {usuario.Rol.IdRol}, '{usuario.Direccion.Calle}', '{usuario.Direccion.NumeroInterior}', '{usuario.Direccion.NumeroExterior}', {usuario.Direccion.Colonia.IdColonia}");
                    int query = context.Database.ExecuteSqlRaw($"UsuarioUpdate {usuario.IdUsuario}, '{usuario.NombreUsuario}', '{usuario.ApellidoPaternoU}', '{usuario.ApellidoMaternoU}', '{usuario.FechaNacimiento}', '{usuario.Genero}', '{usuario.Password}', '{usuario.Telefono}', '{usuario.Celular}', '{usuario.Curp}', '{usuario.UserName}','{usuario.Email}', {usuario.Rol.IdRol}, '{usuario.Imagen}', '{usuario.Direccion.Calle}', '{usuario.Direccion.NumeroInterior}', '{usuario.Direccion.NumeroExterior}', {usuario.Direccion.Colonia.IdColonia}");

                    if (query > 0)
                    {
                        result.Message = "EL dato se modifico correctamente";
                    }

                }
                result.Correct = true;
            }// cierre de try
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Ocurrio un error al cargar la información" + result.Ex;
                throw;
            }//(Algún error en el programa)
            return result;


        }

        public static ML.Result DelateEF(int idUsuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MchavezProgramacionNcapasContext contex = new DL.MchavezProgramacionNcapasContext())
                {
                    int query = contex.Database.ExecuteSqlRaw($"UsuarioDelate {idUsuario}");

                    if (query >= 1)
                    {
                        result.Message = "EL dato se elimino correctamente";
                    }

                }
                result.Correct = true;
            }// cierre de try
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Ocurrio un error al cargar la información" + result.Ex;
                throw;
            }//(Algún error en el programa)
            return result;

        }

        public static ML.Result ConvertirExceltoDataTable(string connString)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (OleDbConnection context = new OleDbConnection(connString))
                {
                    string query = "SELECT * FROM [Sheet1$]"; //query toma el valor del comando o consulta a ejecutar
                    using (OleDbCommand cmd = new OleDbCommand())
                    {
                        cmd.CommandText = query; //cmd almacena el valor de query
                        cmd.Connection = context; //cmd almacena ell valor de la conexion


                        OleDbDataAdapter da = new OleDbDataAdapter(); //DataAdapter convierte el archivo a algo que pueda leer
                        da.SelectCommand = cmd; //Se le asigna la conexion y la sentencia

                        DataTable tableUsuario = new DataTable(); //se crea una tavlamateria de tipo DataTable

                        da.Fill(tableUsuario); //Llenado de informacion

                        if (tableUsuario.Rows.Count > 0)
                        {
                            result.Objects = new List<object>();

                            foreach (DataRow row in tableUsuario.Rows)
                            {
                                ML.Usuario usuario = new ML.Usuario();
                                usuario.NombreUsuario = row[0].ToString();
                                usuario.ApellidoPaternoU = row[1].ToString();
                                usuario.ApellidoMaternoU = row[2].ToString();
                                usuario.FechaNacimiento = row[3].ToString();
                                usuario.Genero = row[4].ToString();
                                usuario.Password = row[5].ToString();
                                usuario.Telefono = row[6].ToString();
                                usuario.Celular = row[7].ToString();
                                usuario.Curp = row[8].ToString();
                                usuario.UserName = row[9].ToString();
                                usuario.Email = row[10].ToString();


                                usuario.Rol = new ML.Rol();
                                usuario.Rol.IdRol = byte.Parse(row[11].ToString());

                                //usuario.Imagen = row.Imagen;

                                //DIRECCION//
                                usuario.Direccion = new ML.Direccion(); //Se inicializa 
                                usuario.Direccion.Calle = row[12].ToString();
                                usuario.Direccion.NumeroInterior = row[13].ToString();
                                usuario.Direccion.NumeroExterior = row[14].ToString();

                                usuario.Direccion.Colonia = new ML.Colonia();
                                usuario.Direccion.Colonia.IdColonia = int.Parse(row[15].ToString());


                                result.Objects.Add(usuario);


                            }

                            result.Correct = true;

                        }

                        if (tableUsuario.Rows.Count > 1)
                        {
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.Message = "No existen registros en el excel";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;

            }

            return result;

        }

        public static ML.Result ValidarExcel(List<object> Object)
        {
            ML.Result result = new ML.Result();

            try
            {
                result.Objects = new List<object>();
                //DataTable  //Rows //Columns
                int i = 1;
                foreach (ML.Usuario usuario in Object)
                {
                    ML.ErrorExcel error = new ML.ErrorExcel();
                    error.IdRegistro = i++; //contador de errores


                    usuario.NombreUsuario = (usuario.NombreUsuario == " ") ? error.Mensaje += "Ingresar el nombre  " : usuario.NombreUsuario; //operador ternario
                    usuario.ApellidoPaternoU = (usuario.ApellidoPaternoU == " ") ? error.Mensaje += "Ingresar el apellido paterno  " : usuario.ApellidoPaternoU; 
                    usuario.ApellidoMaternoU = (usuario.ApellidoMaternoU == " ") ? error.Mensaje += "Ingresar el apellido materno  " : usuario.ApellidoMaternoU; 
                    usuario.FechaNacimiento = (usuario.FechaNacimiento == " ") ? error.Mensaje += "Ingresar la fecha de nacimiento  " : usuario.FechaNacimiento; 
                    usuario.Genero = (usuario.Genero== " ") ? error.Mensaje += "Ingresar el genero  " : usuario.Genero; 
                    usuario.Password = (usuario.Password== " ") ? error.Mensaje += "Ingresar el password  " : usuario.Password; 
                    usuario.Telefono = (usuario.Telefono== " ") ? error.Mensaje += "Ingresar el telefono  " : usuario.Telefono; 
                    usuario.Celular = (usuario.Celular== " ") ? error.Mensaje += "Ingresar el celular  " : usuario.Celular; 
                    usuario.Curp = (usuario.Curp== " ") ? error.Mensaje += "Ingresar el curp  " : usuario.Curp; 
                    usuario.UserName = (usuario.UserName== " ") ? error.Mensaje += "Ingresar el username  " : usuario.UserName; 
                    usuario.Email = (usuario.Email== " ") ? error.Mensaje += "Ingresar el email  " : usuario.Email;

                    //usuario.Rol.IdRol = (usuario.Rol.IdRol == null) ? error.Mensaje += "Ingresar el nombre  " : usuario.Rol.IdRol;
                    usuario.Direccion.Calle = (usuario.Direccion.Calle== "") ? error.Mensaje += "Ingresar el nombre  " : usuario.Direccion.Calle; 



                    if (error.Mensaje != null)
                    {
                        result.Objects.Add(error); //se agregar los errores a la lista de objects
                    }


                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;

            }

            return result;
        }


        public static ML.Result ChangeStatus(int idUsuario, bool status)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.MchavezProgramacionNcapasContext context = new DL.MchavezProgramacionNcapasContext())
                {
                    //int query = context.Database.ExecuteSqlRaw($"UsuarioUpdate {usuario.IdUsuario}, '{usuario.NombreUsuario}', '{usuario.ApellidoPaternoU}', '{usuario.ApellidoMaternoU}', '{usuario.FechaNacimiento}', '{usuario.Genero}', '{usuario.Password}', '{usuario.Telefono}', '{usuario.Celular}', '{usuario.Curp}', '{usuario.UserName}','{usuario.Email}', {usuario.Rol.IdRol}, '{usuario.Direccion.Calle}', '{usuario.Direccion.NumeroInterior}', '{usuario.Direccion.NumeroExterior}', {usuario.Direccion.Colonia.IdColonia}");
                    int query = context.Database.ExecuteSqlRaw($"UsuarioChangeStatus {idUsuario}, {status}");

                    if (query > 0)
                    {
                        result.Message = "EL dato se modifico correctamente";
                    }

                }
                result.Correct = true;
            }// cierre de try
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Ocurrio un error al cargar la información" + result.Ex;
                throw;
            }//(Algún error en el programa)
            return result;


        }

    }
}
