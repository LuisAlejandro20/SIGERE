using System;

namespace SIGERE.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string NombreUsuario { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
        public string CorreoElectronico { get; set; }
        public int Telefono { get; set; }
        public string Estatus { get; set; }
        public DateTime FechaAlta { get; set; }

        /// <summary>
        /// INVARIANTE DE CLASE (Diseño por Contrato):
        /// El estatus de un usuario siempre debe mantenerse en un estado válido.
        /// </summary>
        public bool EsInvarianteValido()
        {
            return Estatus == "Activo" || Estatus == "Inactivo";
        }
    }
}