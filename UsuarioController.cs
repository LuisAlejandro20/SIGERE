using System;
using System.Collections.Generic;
using SIGERE.DAO;
using SIGERE.Models;

namespace SIGERE.Controllers
{
    public class UsuarioController
    {
        private readonly UsuarioDAO _usuarioDao;

        public UsuarioController()
        {
            _usuarioDao = UsuarioDAO.Instancia;
        }

        public int CrearUsuario(string nombre, string correo, string passwordPlano, string rol)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del usuario es obligatorio.", nameof(nombre));
            if (string.IsNullOrWhiteSpace(correo) || !correo.Contains("@"))
                throw new ArgumentException("El correo electrónico no es válido.", nameof(correo));
            if (string.IsNullOrWhiteSpace(passwordPlano) || passwordPlano.Length < 8)
                throw new ArgumentException("La contraseña debe tener al menos 8 caracteres.", nameof(passwordPlano));
            if (string.IsNullOrWhiteSpace(rol))
                throw new ArgumentException("El rol del usuario es obligatorio.", nameof(rol));

            var nuevoUsuario = new Usuario
            {
                Nombre = nombre.Trim(),
                Correo = correo.Trim().ToLowerInvariant(),
                Rol = rol.Trim(),
                Activo = true
            };

            int idCreado = _usuarioDao.Crear(nuevoUsuario, passwordPlano);
            if (idCreado <= 0)
                throw new InvalidOperationException("No se pudo registrar el usuario en la base de datos.");

            return idCreado;
        }

        public List<Usuario> ListarUsuarios() => _usuarioDao.ObtenerTodos();

        public Usuario BuscarUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentOutOfRangeException(nameof(idUsuario), "El identificador debe ser mayor a cero.");

            return _usuarioDao.ObtenerPorId(idUsuario);
        }

        public Usuario IniciarSesion(string correo, string passwordPlano)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(passwordPlano))
                throw new ArgumentException("Debe ingresar correo y contraseña.");

            return _usuarioDao.Autenticar(correo.Trim().ToLowerInvariant(), passwordPlano);
        }

        public bool EditarUsuario(Usuario usuario, string nuevaPasswordPlano = null)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));
            if (usuario.IdUsuario <= 0)
                throw new ArgumentException("IdUsuario inválido.");
            if (string.IsNullOrWhiteSpace(usuario.Nombre) || string.IsNullOrWhiteSpace(usuario.Correo))
                throw new ArgumentException("Nombre y correo son obligatorios.");
            if (!string.IsNullOrWhiteSpace(nuevaPasswordPlano) && nuevaPasswordPlano.Length < 8)
                throw new ArgumentException("La nueva contraseña debe tener al menos 8 caracteres.");

            usuario.Nombre = usuario.Nombre.Trim();
            usuario.Correo = usuario.Correo.Trim().ToLowerInvariant();
            return _usuarioDao.Actualizar(usuario, nuevaPasswordPlano);
        }

        public bool DesactivarUsuario(int idUsuarioObjetivo, int idUsuarioSesionActual)
        {
            if (idUsuarioObjetivo <= 0)
                throw new ArgumentOutOfRangeException(nameof(idUsuarioObjetivo));
            if (idUsuarioObjetivo == idUsuarioSesionActual)
                throw new InvalidOperationException("Un usuario no puede desactivar su propia cuenta activa.");

            return _usuarioDao.Eliminar(idUsuarioObjetivo);
        }
    }
}