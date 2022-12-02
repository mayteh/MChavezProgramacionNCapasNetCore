using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Empleado
    {
        public string NumeroEmpleado { get; set; }
        public string RFC { get; set; }
        public string NombreEmpleado { get; set; }
        public string ApellidoPaternoE { get; set; }
        public string ApellidoMaternoE  { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string? FechaNacimiento { get; set; }
        public string NSS { get; set; }
        public string? FechaIngreso { get; set; }
        public string? Foto { get; set; }

        public string Action { get; set; }

        // //PROPIEDAD DE NAVEGACION

        public List<object> Empleados { get; set; }
        public ML.Empresa Empresa { get; set; }
        public string NombreEmpresa { get; set; }
    }
}
