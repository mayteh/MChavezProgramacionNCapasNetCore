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
    }
}
