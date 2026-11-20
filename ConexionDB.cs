using System;
using System.Configuration;
using System.Data.SqlClient;

namespace SIGERE.DAO
{
    public sealed class ConexionDB
    {
        private static readonly Lazy<ConexionDB> _instancia =
            new Lazy<ConexionDB>(() => new ConexionDB());

        private readonly string _cadenaConexion;

        private ConexionDB()
        {
            _cadenaConexion = ConfigurationManager.ConnectionStrings["SIGEREConnection"]?.ConnectionString
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'SIGEREConnection'.");
        }

        public static ConexionDB Instancia => _instancia.Value;

        public SqlConnection CrearConexion()
        {
            return new SqlConnection(_cadenaConexion);
        }
    }
}