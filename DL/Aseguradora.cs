﻿using System;
using System.Collections.Generic;

namespace DL;

public partial class Aseguradora
{
    public int IdAseguradora { get; set; }

    public string NombreAseguradora { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int IdUsuario { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    //AGREGADAS PARA USO USUARIO//
    public string NombreUsuario { get; set; }

    public string ApellidoPaterno { get; set; }

    public string? ApellidoMaterno { get; set; }


}