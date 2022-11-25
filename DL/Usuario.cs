using System;
using System.Collections.Generic;

namespace DL;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string? ApellidoMaterno { get; set; }

    public DateTime FechaNacimiento { get; set; }

    public string Sexo { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string? Celular { get; set; }

    public string? Curp { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public byte? IdRol { get; set; }

    public string? Imagen { get; set; }

    public virtual ICollection<Aseguradora> Aseguradoras { get; } = new List<Aseguradora>();

    public virtual ICollection<Direccion> Direccions { get; } = new List<Direccion>();

    public virtual Rol? IdRolNavigation { get; set; }

    //Agregadas para usar rol

    public string? NombreRol { get; set; }
    public int IdDireccion { get; set; }
    public string? Calle { get; set; }
    public string? NumeroInterior { get; set; }
    public string? NumeroExterior { get; set; }


    //Colonia
    public int IdColonia { get; set; }
    public string? NombreColonia { get; set; }
    public string? CodigoPostal { get; set; }


    //Municipio
    public int IdMunicipio { get; set; }
    public string NombreMunicipio { get; set; }

    //Estado
    public int IdEstado { get; set; }
    public string NombreEstado { get; set; }

    //Pais
    public int IdPais { get; set; }
    public string NombrePais { get; set; }

}
