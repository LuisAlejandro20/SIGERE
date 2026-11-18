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
            if (string.IsNullOrWhiteSpace(usuario))
            {
                return (false, "El campo nombre está vacío", null);
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                return (false, "El campo contraseña está vacío", null);
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
                    return (false, "Nombre o contraseña incorrectos", null);
                }
            }
            catch (InvalidOperationException ex)
            {
                return (false, ex.Message, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public (bool Exito, string Mensaje) ValidarTelefonoRecuperacion(string telefonoTxt)
        {
            if (!int.TryParse(telefonoTxt, out int telefono) || !usuarioDAO.ValidarTelefonoExistente(telefono))
            {
                return (false, "Número de teléfono no reconocido");
            }
            return (true, "Número reconocido. Ingrese nueva contraseña.");
        }

        public (bool Exito, string Mensaje) ReestablecerPassword(string telefonoTxt, string pass1, string pass2)
        {
            if (string.IsNullOrWhiteSpace(pass1) || string.IsNullOrWhiteSpace(pass2) || pass1 != pass2)
            {
                return (false, "Las contraseñas no coinciden");
            }

            if (!int.TryParse(telefonoTxt, out int telefono))
            {
                return (false, "Número de teléfono no válido");
            }

            bool actualizada = usuarioDAO.ActualizarPasswordPorTelefono(telefono, pass1);
            if (actualizada)
            {
                return (true, "Contraseña reestablecida con éxito");
            }
            else
            {
                return (false, "Error al actualizar la contraseña en la base de datos");
            }
        }

        public (bool Exito, string Mensaje) EnviarCorreoVerificacion(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo) || !usuarioDAO.ValidarCorreoExistente(correo))
            {
                return (false, "Correo electrónico no reconocido");
            }
            return (true, "Se ha enviado un correo de verificación");
        }
    }
}