using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
        public class Empresa //Se crea la clase empresa
        {
            public int IdEmpresa { get; set; }
            public string NombreEmpresa { get; set; }
            public string Telefono { get; set; }
            public string Email { get; set; }
            public string DireccionWeb { get; set; }
            public List<object> Empresas { get; set; }

        public string? Logo { get; set; }
    }
}
