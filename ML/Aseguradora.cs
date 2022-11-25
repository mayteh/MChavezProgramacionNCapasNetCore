using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Aseguradora
    {
        public int IdAseguradora { get; set; }

        public string NombreAseguradora { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public int IdUsuario { get; set; }

        
         //PROPIEDADES AGREGADAS
        public List<object> Aseguradoras { get; set; }


        public ML.Usuario Usuario { get; set; }

        public string NombreUsuario { get; set; }

        public string ApellidoPaternoU { get; set; }

        public string? ApellidoMaternoU { get; set; }


    }
}
