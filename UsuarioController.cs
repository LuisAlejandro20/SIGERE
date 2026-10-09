using System;
using SIGERE.DataAccess;
using SIGERE.Models;

namespace SIGERE.Controllers
{
    public class UsuarioController
    {
        private readonly UsuarioDAO usuarioDAO;

        public UsuarioController()
        {
            this.usuarioDAO = new UsuarioDAO();
        }

        public (bool Exito, string Mensaje, Usuario Usuario) IniciarSesion(string usuario, string password)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
            {
                return (false, "Por favor ingrese usuario y contraseña.", null);
            }

            try
            {
                Usuario user = usuarioDAO.AutenticarUsuario(usuario, password);

                if (user != null)
                {
                    return (true, $"¡Bienvenido {user.Nombre}! (Rol: {user.Rol})", user);
                }
                else
                {
                    return (false, "Usuario o contraseña incorrectos.", null);
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error al autenticar: {ex.Message}", null);
            }
        }
    }
}