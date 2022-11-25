using System;
using System.Collections.Generic;

namespace DL;

public partial class Colonium
{
    public int IdColonia { get; set; }

    public string NombreColonia { get; set; } = null!;

    public int? IdMunicipio { get; set; }

    public string CodigoPostal { get; set; } = null!;

    public virtual ICollection<Direccion> Direccions { get; } = new List<Direccion>();

    public virtual Municipio? IdMunicipioNavigation { get; set; }
}
