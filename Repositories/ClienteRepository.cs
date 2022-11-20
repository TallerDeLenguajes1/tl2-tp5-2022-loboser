using tl2_tp4_2022_loboser.Models;
using Microsoft.Data.Sqlite;

#nullable disable

namespace tl2_tp4_2022_loboser.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly string _cadenaConexion;
        public ClienteRepository(IConexionRepository conexion)
        {
            this._cadenaConexion = conexion.GetConnectionString();
        }

        public Cliente GetCliente(int id)
        {
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "SELECT * FROM Cliente WHERE idCliente=" + id + ";";
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        Cliente Cliente = new Cliente();

                        if (Lector.Read())
                        {
                            Cliente.Id = Convert.ToInt32(Lector["idCliente"].ToString());
                            Cliente.Nombre = Lector["nombreCliente"].ToString();
                            Cliente.Direccion = Lector["direccionCliente"].ToString();
                            Cliente.Telefono = Lector["telefonoCliente"].ToString();
                            Cliente.DatosReferenciaDireccion = Lector["datosReferenciaDireccion"].ToString();
                        }

                        Conexion.Close();
                        return Cliente;
                    }
                }
            }
        }

        public void AltaCliente(Cliente cliente)
        {
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "INSERT INTO Cliente(nombreCliente, direccionCliente, telefonoCliente, datosReferenciaDireccion) VALUES('" + cliente.Nombre + "', '" + cliente.Direccion + "', '" + cliente.Telefono + "', '" + cliente.DatosReferenciaDireccion + "');";
                    Comando.ExecuteNonQuery();
                }
                Conexion.Close();
            }
        }

        public void EditarCliente(Cliente cliente)
        {
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "UPDATE Cliente SET direccionCliente='" + cliente.Direccion + "', datosReferenciaDireccion='" + cliente.DatosReferenciaDireccion + "' WHERE telefonoCliente='" + cliente.Telefono + "';";
                    Comando.ExecuteNonQuery();
                }
                Conexion.Close();
            }
        }
    }
}