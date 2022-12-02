using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Empleado
    {
        public static ML.Result GetAll ()
        {
            ML.Result result = new ML.Result ();

            try
            {
                using (DL.MchavezProgramacionNcapasContext context = new DL.MchavezProgramacionNcapasContext())
                {
                    var query = context.Empleados.FromSqlRaw("EmpleadoGetAll");
                    result.Objects = new List<object>();

                    if(query!= null)
                    {
                        foreach (var obj in query) //Sacar los objetos de la lista
                        {
                            ML.Empleado objEmpleado = new ML.Empleado();

                            objEmpleado.NumeroEmpleado = obj.NumeroEmpleado;
                            objEmpleado.RFC = obj.Rfc;
                            objEmpleado.NombreEmpleado = obj.Nombre;
                            objEmpleado.ApellidoPaternoE = obj.ApellidoPaterno;
                            objEmpleado.ApellidoMaternoE = obj.ApellidoMaterno;
                            objEmpleado.Email = obj.Email;
                            objEmpleado.Telefono = obj.Telefono;
                            objEmpleado.FechaNacimiento = obj.FechaNacimiento.ToString("dd-MM-yyyy");
                            objEmpleado.NSS = obj.Nss;
                            objEmpleado.FechaIngreso = obj.FechaIngreso.ToString("dd-MM-yyyy");
                            objEmpleado.Foto = obj.Foto;

                            objEmpleado.Empresa = new ML.Empresa();
                            objEmpleado.Empresa.IdEmpresa = obj.IdEmpresa.Value;
                            objEmpleado.Empresa.NombreEmpresa = obj.NombreEmpresa;

            

                            result.Objects.Add(objEmpleado);
                        }
                    }
                }
                result.Correct = true;
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

        public static ML.Result Add(ML.Empleado empleado)
        {
            ML.Result result = new ML.Result(); //instancia de Result
            try
            {
                using (DL.MchavezProgramacionNcapasContext contex = new DL.MchavezProgramacionNcapasContext())
                {
                    int query = contex.Database.ExecuteSqlRaw($"EmpleadoAdd '{empleado.NumeroEmpleado}', '{empleado.RFC}', '{empleado.NombreEmpleado}', '{empleado.ApellidoPaternoE}', '{empleado.ApellidoMaternoE}', '{empleado.Email}', '{empleado.Telefono}', '{empleado.FechaNacimiento}', '{empleado.NSS}', '{empleado.FechaIngreso}', '{empleado.Foto}', {empleado.Empresa.IdEmpresa}");

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
                throw;
            }//(Algún error en el programa)
            return result;
        }

        public static ML.Result GetById(string NumeroEmpleado)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MchavezProgramacionNcapasContext context = new DL.MchavezProgramacionNcapasContext())
                {
                    //var tableUsuario = context.UsuarioGetById(idUsuario).SingleOrDefault();
                    var tableUsuario = context.Empleados.FromSqlRaw($"EmpleadoGetById {NumeroEmpleado}").AsEnumerable().FirstOrDefault(); //No olvidar el signo de pesos
                    result.Objects = new List<object>();
                    if (tableUsuario != null)
                    {
                        ML.Empleado empleado = new ML.Empleado();

                        empleado.NumeroEmpleado = tableUsuario.NumeroEmpleado;
                        empleado.RFC = tableUsuario.Rfc;
                        empleado.NombreEmpleado = tableUsuario.Nombre;
                        empleado.ApellidoPaternoE = tableUsuario.ApellidoMaterno;
                        empleado.ApellidoMaternoE = tableUsuario.ApellidoMaterno;
                        empleado.Email = tableUsuario.Email;
                        empleado.Telefono = tableUsuario.Telefono;
                        empleado.FechaNacimiento = tableUsuario.FechaNacimiento.ToString("dd-MM-yyyy");
                        empleado.NSS = tableUsuario.Nss;
                        empleado.FechaIngreso = tableUsuario.FechaIngreso.ToString("dd-MM-yyyy");
                        empleado.Foto = tableUsuario.Foto;

                        //Se tiene que instanciar el objeto para poderlo usarlo y asi almacenar
                        empleado.Empresa = new ML.Empresa();
                        empleado.Empresa.IdEmpresa = tableUsuario.IdEmpresa.Value;
                        //empleado.Empresa.NombreEmpresa = tableUsuario.NombreEmpresa.ToString();
                        
                        //boxing
                        //Almacenar cualquier tipo de dato en un dato object
                        result.Object = empleado;
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

        public static ML.Result Update(ML.Empleado empleado)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MchavezProgramacionNcapasContext context = new DL.MchavezProgramacionNcapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"EmpresaUpdate '{empleado.NumeroEmpleado}', '{empleado.RFC}', '{empleado.NombreEmpleado}', '{empleado.ApellidoPaternoE}', '{empleado.ApellidoMaternoE}', '{empleado.Email}', '{empleado.Telefono}', '{empleado.FechaNacimiento}', '{empleado.NSS}', '{empleado.FechaIngreso}','{empleado.Foto}', {empleado.Empresa.IdEmpresa}");

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

        public static ML.Result DelateEF(string NumeroEmpleado)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MchavezProgramacionNcapasContext contex = new DL.MchavezProgramacionNcapasContext())
                {
                    //ML.Empleado empleado = new ML.Empleado();
                    var query = contex.Database.ExecuteSqlRaw($"EmpleadoDelate '{NumeroEmpleado}'");

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
    }
}
