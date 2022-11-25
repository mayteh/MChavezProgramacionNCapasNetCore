using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Empresa
    {
        public static ML.Result Add(ML.Empresa empresa)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MchavezProgramacionNcapasContext context = new DL.MchavezProgramacionNcapasContext())
                {
                    var usuarios = context.Database.ExecuteSqlRaw($"EmpresaAdd '{empresa.NombreEmpresa}', '{empresa.Telefono}', '{empresa.Email}', '{empresa.DireccionWeb}', '{empresa.Logo}'");

                    if (usuarios > 0)
                    {
                        result.Message = "La empresa se agrego correctamente";
                    }

                    result.Correct = true;
                }
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

        public static ML.Result Update(ML.Empresa empresa)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MchavezProgramacionNcapasContext context = new DL.MchavezProgramacionNcapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"EmpresaUpdate {empresa.IdEmpresa}, '{empresa.NombreEmpresa}', '{empresa.Telefono}', '{empresa.Email}', '{empresa.DireccionWeb}', '{empresa.Logo}'");

                    if (query > 0)
                    {
                        result.Message = "La empresa se modifico correctamente";
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

        public static ML.Result Delate(ML.Empresa empresa)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MchavezProgramacionNcapasContext context = new DL.MchavezProgramacionNcapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"EmpresaDelete {empresa.IdEmpresa}");
                    if (query >= 1)
                    {
                        result.Message = "EL dato se elimino correctamente";
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

        public static ML.Result GetAll(ML.Empresa empresa)
        {
            ML.Result result = new ML.Result();

            try
            {
                //Se obtiene la conexion a la base de datos
                using (DL.MchavezProgramacionNcapasContext context = new DL.MchavezProgramacionNcapasContext())
                {
                    var query = context.Empresas.FromSqlRaw($"EmpresaGetAll '{empresa.NombreEmpresa}'");
                    result.Objects = new List<object>(); //Instanciar la lista para dar a entender que se trata de una lista lo que va a traer

                    if (query != null)
                    {
                        foreach (var objeto in query)//Sacar los row de la tabla EmpresaTable
                        {
                            ML.Empresa empresaobj = new ML.Empresa();
                            empresaobj.IdEmpresa = objeto.IdEmpresa;
                            empresaobj.NombreEmpresa = objeto.Nombre;
                            empresaobj.Telefono = objeto.Telefono;
                            empresaobj.Email = objeto.Email;
                            empresaobj.DireccionWeb = objeto.DireccionWeb;

                            empresaobj.Logo = objeto.Logo;

                            result.Objects.Add(empresaobj);
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
            }//(Algún error en el programa)
            return result;
        }

        public static ML.Result GetById(int idEmpresa)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MchavezProgramacionNcapasContext context = new DL.MchavezProgramacionNcapasContext())
                {
                    var query = context.Empresas.FromSqlRaw($"EmpresaGetById {idEmpresa}").AsEnumerable().FirstOrDefault();
                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        ML.Empresa empresa = new ML.Empresa();

                        empresa.IdEmpresa = query.IdEmpresa;
                        empresa.NombreEmpresa = query.Nombre;
                        empresa.Telefono = query.Telefono;
                        empresa.Email = query.Email;
                        empresa.DireccionWeb = query.DireccionWeb;
                        empresa.Logo = query.Logo;

                        ///boxing
                        result.Object = empresa;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Ocurrio un error al cargar la información" + result.Ex;
                throw;
            }


        }
    }
}
