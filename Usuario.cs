using System;

namespace SIGERE.Models
{
    public class Usuario
    {
        // Propiedades mapeadas según la Vista Lógica de la Base de Datos
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string NombreUsuario { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
        public string CorreoElectronico { get; set; }
        public int Telefono { get; set; }
        public string Estatus { get; set; }
        public DateTime FechaAlta { get; set; }
    }
}