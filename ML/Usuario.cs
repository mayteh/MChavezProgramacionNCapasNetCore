using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "Es necesario llenar este campo")]
        public string? Curp { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }

        [Required(ErrorMessage = "Es necesario que el usuario tenga un nombre")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "Es necesario llenar este campo")]
        public string ApellidoPaternoU { get; set; }
        [Required(ErrorMessage = "Es necesario llenar este campo")]
        public string ApellidoMaternoU { get; set; }
        [Required(ErrorMessage = "Es necesario llenar este campo")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Es necesario llenar este campo")]
        public string? Telefono { get; set; }
        public string? Celular { get; set; }
        public int? Edad { get; set; }
        public float? Estatura { get; set; }
        public string? Genero { get; set; }
        public string? FechaNacimiento { get; set; }
        public List<object>? Usuarios { get; set; }
        public string? Imagen { get; set; }

        public bool Status { get; set; }

        public ML.Rol? Rol { get; set; }

        //Propiedades de navegacion
        public ML.Direccion? Direccion { get; set; }
    }
}
