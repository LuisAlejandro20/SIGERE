using System;
using System.Collections.Generic;
using SIGERE.DataAccess;
using SIGERE.Models;

namespace SIGERE.Controllers
{
    public class InventarioControl
    {
        private readonly Inventario inventarioDAO;

        public InventarioControl()
        {
            this.inventarioDAO = new Inventario();
        }

        public (bool Exito, string Mensaje, List<Producto> Productos) ConsultarProductos(string criterio)
        {
            if (string.IsNullOrWhiteSpace(criterio))
            {
                return (false, "Ingrese el nombre o la clave del producto a buscar.", new List<Producto>());
            }

            try
            {
                List<Producto> lista = inventarioDAO.BuscarDisponibilidad(criterio);

                if (lista.Count == 0)
                {
                    // Escenario alterno 3a y caso TDD DIS-03
                    return (false, "Producto no encontrado.", lista);
                }

                return (true, $"Se encontraron {lista.Count} coincidencia(s).", lista);
            }
            catch (Exception ex)
            {
                return (false, "Ocurrió un error al realizar la consulta: " + ex.Message, new List<Producto>());
            }
        }

        public (bool Exito, string Mensaje, List<Producto> Productos) CargarTodosLosProductos()
        {
            try
            {
                List<Producto> lista = inventarioDAO.ObtenerCatalogoCompleto();
                return (true, $"Catálogo cargado ({lista.Count} productos).", lista);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, new List<Producto>());
            }
        }
    }
}