using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Aseguradora
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MchavezProgramacionNcapasContext contex = new DL.MchavezProgramacionNcapasContext())//Se obtiene la conexion
                {
                    var aseguradoras = contex.Aseguradoras.FromSqlRaw("AseguradoraGetAll");

                    result.Objects = new List<object>();

                    if (aseguradoras != null)//Si las rows dentro de nuestra lista query es diferente de null
                    {
                        foreach (var objeto in aseguradoras)// Sacar los row de nuestra lista
                        {

                            ML.Aseguradora aseguradora = new ML.Aseguradora();

                            aseguradora.IdAseguradora = objeto.IdAseguradora;
                            aseguradora.NombreAseguradora = objeto.NombreAseguradora;
                            aseguradora.FechaCreacion = objeto.FechaCreacion;
                            aseguradora.FechaModificacion = objeto.FechaModificacion;


                            aseguradora.Usuario = new ML.Usuario();
                            aseguradora.IdUsuario = objeto.IdUsuario;
                            aseguradora.NombreUsuario = objeto.NombreUsuario;
                            aseguradora.ApellidoPaternoU = objeto.ApellidoPaterno;
                            aseguradora.ApellidoMaternoU = objeto.ApellidoMaterno;

                            result.Objects.Add(aseguradora);//Agrega todos los datos a usuario
                        }
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

        public static ML.Result Add(ML.Aseguradora aseguradora)
        {
            ML.Result result = new ML.Result(); //instancia de Result
            try
            {
                using (DL.MchavezProgramacionNcapasContext contex = new DL.MchavezProgramacionNcapasContext())
                {
                    int query = contex.Database.ExecuteSqlRaw($"AseguradoraAdd '{aseguradora.NombreAseguradora}', {aseguradora.IdUsuario}");

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

        public static ML.Result GetById(int idAseguradora)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MchavezProgramacionNcapasContext context = new DL.MchavezProgramacionNcapasContext())
                {
                    var query = context.Aseguradoras.FromSqlRaw($"AseguradoraGetById {idAseguradora}").AsEnumerable().FirstOrDefault();
                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        ML.Aseguradora aseguradora = new ML.Aseguradora();

                        aseguradora.IdAseguradora = query.IdAseguradora;
                        aseguradora.NombreAseguradora = query.NombreAseguradora;
                        aseguradora.FechaCreacion = query.FechaCreacion;
                        aseguradora.FechaModificacion = query.FechaModificacion;
                        aseguradora.IdUsuario = query.IdUsuario;

                        aseguradora.Usuario = new ML.Usuario();
                        aseguradora.IdUsuario = query.IdUsuario;
                        aseguradora.NombreUsuario = query.NombreUsuario;
                        aseguradora.ApellidoPaternoU = query.ApellidoPaterno;
                        aseguradora.ApellidoMaternoU = query.ApellidoMaterno;


                        ///boxing
                        result.Object = aseguradora;
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
        public static ML.Result Update(ML.Aseguradora aseguradora)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MchavezProgramacionNcapasContext context = new DL.MchavezProgramacionNcapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"AseguradoraUpdate {aseguradora.IdAseguradora}, '{aseguradora.NombreAseguradora}', {aseguradora.IdUsuario}");

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

        public static ML.Result Delate(ML.Aseguradora aseguradora)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MchavezProgramacionNcapasContext context = new DL.MchavezProgramacionNcapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"AseguradoraDelete {aseguradora.IdAseguradora}");
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
    }
}
