using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Rol
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MchavezProgramacionNcapasContext context = new DL.MchavezProgramacionNcapasContext())
                {
                    var query = context.Rols.FromSqlRaw("RolGetAll").ToList();
                    result.Objects = new List<object>(); //Se inicializa la lista
                    if (query != null)
                    {
                        foreach (var objeto in query)
                        {
                            ML.Rol rol = new ML.Rol(); //tiene que ir inicializado dentro  del foreach para que los guarde en un nuevo obj rol

                            rol.IdRol = (byte)objeto.IdRol;
                            rol.NombreRol = objeto.NombreRol;

                            result.Objects.Add(rol);
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
    }
}
