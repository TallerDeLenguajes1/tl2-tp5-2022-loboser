using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Threading.Tasks;
using tl2_tp4_2022_loboser.Models;
using tl2_tp4_2022_loboser.ViewModels;

#nullable disable

namespace tl2_tp4_2022_loboser.Repositories
{
    public interface IPedidoRepository
    {
        List<Pedido> GetPedidos();
        void AltaPedido(AltaPedidoViewModel Pedido);
        void BajaPedido(int nro);
        void EditarPedido(EditarPedidoViewModel Pedido);
    }
    public class PedidoRepository : IPedidoRepository
    {
        private readonly string _cadenaConexion;

        public PedidoRepository()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            this._cadenaConexion = configuration.GetConnectionString("Default");
        }

        public List<Pedido> GetPedidos(){
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "SELECT * FROM Pedido;";
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        List<Pedido> Pedidos = new List<Pedido>();
                        while (Lector.Read())
                        {
                            Pedido NuevoPedido = new Pedido();

                            NuevoPedido.Nro = Convert.ToInt32(Lector["nroPedido"].ToString());
                            NuevoPedido.Obs = Lector["Obs"].ToString();
                            NuevoPedido.Estado = Lector["Estado"].ToString();

                            NuevoPedido.Cliente = GetCliente(Convert.ToInt32(Lector["idCliente"].ToString()));

                            Pedidos.Add(NuevoPedido);
                        }

                        Conexion.Close();
                        return Pedidos;
                    }
                }
            }
        }

        private Cliente GetCliente(int idCliente){
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "SELECT * FROM Cliente WHERE idCliente=" + idCliente + ";";
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

        public void AltaPedido(AltaPedidoViewModel Pedido){
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
            {
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "SELECT idCliente FROM Cliente WHERE telefonoCliente='" + Pedido.Cliente.Telefono + "';";
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        if (Lector.Read() == false)
                        {
                            Lector.Close();
                            Comando.CommandText = "INSERT INTO Cliente(nombreCliente, direccionCliente, telefonoCliente, datosReferenciaDireccion) VALUES('" + Pedido.Cliente.Nombre + "', '" + Pedido.Cliente.Direccion + "', '" + Pedido.Cliente.Telefono + "', '" + Pedido.Cliente.DatosReferenciaDireccion + "');";
                            Comando.ExecuteNonQuery();
                        }
                    }
                }
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "SELECT idCliente FROM Cliente WHERE telefonoCliente='" + Pedido.Cliente.Telefono + "';";
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        if (Lector.Read())
                        {
                            using (SqliteCommand Comando2 = Conexion.CreateCommand())
                            {
                                Comando2.CommandText = "INSERT INTO Pedido(Obs, Estado, idCliente) VALUES('" + Pedido.Obs + "', 'En Proceso', '" + Convert.ToInt32(Lector["idCliente"].ToString()) + "');";
                                Comando2.ExecuteNonQuery();
                            }
                        }
                    }  
                }
                Conexion.Close();
            }
        }

        public void BajaPedido(int nro)
        {
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "DELETE FROM Pedido WHERE nroPedido='" + nro + "';";
                    Comando.ExecuteNonQuery();

                    Conexion.Close();
                }
            }
        }

        public void EditarPedido(EditarPedidoViewModel Pedido)
        {
            using (SqliteConnection Conexion = new SqliteConnection(_cadenaConexion))
            {
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "SELECT * FROM Cliente WHERE telefonoCliente='" + Pedido.Cliente.Telefono + "';";
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        if (Lector.Read() == false)
                        {
                            Lector.Close();
                            Comando.CommandText = "INSERT INTO Cliente(nombreCliente, direccionCliente, telefonoCliente, datosReferenciaDireccion) VALUES('" + Pedido.Cliente.Nombre + "', '" + Pedido.Cliente.Direccion + "', '" + Pedido.Cliente.Telefono + "', '" + Pedido.Cliente.DatosReferenciaDireccion + "');";
                            Comando.ExecuteNonQuery();
                        }else
                        {
                            Lector.Close();
                            Comando.CommandText = "UPDATE Cliente SET direccionCliente='" + Pedido.Cliente.Direccion + "', datosReferenciaDireccion='" + Pedido.Cliente.DatosReferenciaDireccion + "' WHERE telefonoCliente='" + Pedido.Cliente.Telefono + "';";
                            Comando.ExecuteNonQuery();
                        }
                    }
                }
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {     
                    Comando.CommandText = "SELECT idCliente FROM Cliente WHERE telefonoCliente='" + Pedido.Cliente.Telefono + "';";
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        if (Lector.Read())
                        {
                            using (SqliteCommand Comando2 = Conexion.CreateCommand())
                            {
                                Comando2.CommandText = "UPDATE Pedido SET Obs='" + Pedido.Obs + "', idCliente ='" + Convert.ToInt32(Lector["idCliente"].ToString()) + "' WHERE nroPedido='" + Pedido.Nro + "';";
                                Comando2.ExecuteNonQuery();
                            }
                        }
                    }
                    Conexion.Close();
                }
            }
        }
    }
}