using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tl2_tp4_2022_loboser.ViewModels;

#nullable disable

namespace tl2_tp4_2022_loboser.Models
{
    public class DataBase
    {
        static SqliteConnection CrearConexion()
        {  
            SqliteConnection Conexion = new SqliteConnection("Data Source=PedidosDB.db");
         
            try
            {
                Conexion.Open();
            }
            catch (Exception)
            {

            }
            return Conexion;
        }
        public static Cadeteria CargarCadeteriaDB()
        {
            SqliteConnection Conexion = CrearConexion();
            Cadeteria Cadeteria = new Cadeteria();

            SqliteCommand Comando = Conexion.CreateCommand();
            Comando.CommandText = "SELECT nombreCadeteria, telefonoCadeteria FROM Cadeteria WHERE idCadeteria = 1";

            SqliteDataReader Lector = Comando.ExecuteReader();

            while (Lector.Read())
            {
                Cadeteria.Nombre = Lector["nombreCadeteria"].ToString();
                Cadeteria.Telefono = Lector["nombreCadeteria"].ToString();
            }
            Cadeteria.Cadetes = CargarCadetesDB(Conexion);
            Conexion.Close();

            return Cadeteria;
        }

        static List<Cadete> CargarCadetesDB(SqliteConnection Conexion){
            List<Cadete> Cadetes = new List<Cadete>();

            SqliteCommand Comando = Conexion.CreateCommand();
            Comando.CommandText = "SELECT * FROM Cadete";

            SqliteDataReader Lector = Comando.ExecuteReader();

            while (Lector.Read())
            {
                Cadete NuevoCadete = new Cadete();

                NuevoCadete.Id = Convert.ToInt32(Lector["idCadete"].ToString());
                NuevoCadete.Nombre = Lector["nombreCadete"].ToString();
                NuevoCadete.Direccion = Lector["direccionCadete"].ToString();
                NuevoCadete.Telefono = Lector["telefonoCadete"].ToString();
                NuevoCadete.Pedidos = new List<Pedido>();

                Cadetes.Add(NuevoCadete);
            }

            return Cadetes;
        }
        
        public static void AltaCadeteDB(AltaCadeteViewModel Cadete)
        {
            SqliteConnection Conexion = CrearConexion();

            SqliteCommand Comando = Conexion.CreateCommand();
            Comando.CommandText = "INSERT INTO Cadete(nombreCadete, direccionCadete, telefonoCadete) VALUES ('" + Cadete.Nombre + "', '" + Cadete.Direccion + "' ,'" + Cadete.Telefono + "');";
            Comando.ExecuteNonQuery();

            Comando.CommandText = "SELECT * FROM Cadete WHERE nombreCadete='" + Cadete.Nombre + "' AND direccionCadete='" + Cadete.Direccion + "' AND telefonoCadete='" + Cadete.Telefono + "';";
            SqliteDataReader Lector = Comando.ExecuteReader();

            SqliteCommand Comando2 = Conexion.CreateCommand();


            if(Lector.Read())
            {
                int idCadeteria = 1;
                int id = Convert.ToInt32(Lector["idCadete"].ToString());
                Comando2.CommandText = "INSERT INTO CadeteCadeteria(idCadeteria, idCadete) VALUES ('" + idCadeteria + "', '" + id + "');";
                Comando2.ExecuteNonQuery();   
            }

            Conexion.Close();
        }

        public static void BajaCadeteDB(int id)
        {
            SqliteConnection Conexion = CrearConexion();

            SqliteCommand Comando = Conexion.CreateCommand();
            Comando.CommandText = "DELETE FROM Cadete WHERE idCadete='" + id + "';";
            Comando.ExecuteNonQuery();
            Comando.CommandText = "DELETE FROM CadeteCadeteria WHERE idCadete='" + id + "';";
            Comando.ExecuteNonQuery();

            Conexion.Close();
        }

        public static void EditarCadeteDB(EditarCadeteViewModel Cadete){
            SqliteConnection Conexion = CrearConexion();

            SqliteCommand Comando = Conexion.CreateCommand();
            Comando.CommandText = "UPDATE Cadete SET nombreCadete='" + Cadete.Nombre + "', direccionCadete='" + Cadete.Direccion + "', telefonoCadete='" + Cadete.Telefono + "' WHERE idCadete='" + Cadete.Id + "';";
            Comando.ExecuteNonQuery();

            Conexion.Close();
        }
        public static List<Pedido> CargarPedidosDB(){
            SqliteConnection Conexion = CrearConexion();

            List<Pedido> Pedidos = new List<Pedido>();

            SqliteCommand Comando = Conexion.CreateCommand();
            Comando.CommandText = "SELECT * FROM Pedido;";

            SqliteDataReader Lector = Comando.ExecuteReader();

            while (Lector.Read())
            {
                Pedido NuevoPedido = new Pedido();

                NuevoPedido.Nro = Convert.ToInt32(Lector["nroPedido"].ToString());
                NuevoPedido.Obs = Lector["Obs"].ToString();
                NuevoPedido.Estado = Lector["Estado"].ToString();

                NuevoPedido.Cliente = CargarClienteDB(Conexion, Convert.ToInt32(Lector["idCliente"].ToString()));

                Pedidos.Add(NuevoPedido);
            }

            Conexion.Close();
            return Pedidos;
        }

        static Cliente CargarClienteDB(SqliteConnection Conexion, int idCliente){
            Cliente Cliente = new Cliente();

            SqliteCommand Comando = Conexion.CreateCommand();
            Comando.CommandText = "SELECT * FROM Cliente WHERE idCliente=" + idCliente + ";";

            SqliteDataReader Lector = Comando.ExecuteReader();

            if (Lector.Read())
            {
                Cliente.Id = Convert.ToInt32(Lector["idCliente"].ToString());
                Cliente.Nombre = Lector["nombreCliente"].ToString();
                Cliente.Direccion = Lector["direccionCliente"].ToString();
                Cliente.Telefono = Lector["telefonoCliente"].ToString();
                Cliente.DatosReferenciaDireccion = Lector["datosReferenciaDireccion"].ToString();
            }

            return Cliente;
        }

        public static void AltaPedidoDB(AltaPedidoViewModel Pedido){
            SqliteConnection Conexion = CrearConexion();

            SqliteCommand Comando = Conexion.CreateCommand();
            Comando.CommandText = "SELECT idCliente FROM Cliente WHERE telefonoCliente=" + Pedido.Cliente.Telefono + " ;";
            SqliteDataReader Lector = Comando.ExecuteReader();


            if (Lector.Read() == false)
            {
                Lector.Close();

                Comando.CommandText = "INSERT INTO Cliente(nombreCliente, direccionCliente, telefonoCliente, datosReferenciaDireccion) VALUES('" + Pedido.Cliente.Nombre + "', '" + Pedido.Cliente.Direccion + "', '" + Pedido.Cliente.Telefono + "', '" + Pedido.Cliente.DatosReferenciaDireccion + "');";
                Comando.ExecuteNonQuery(); 

                Comando.CommandText = "SELECT idCliente FROM Cliente WHERE telefonoCliente=" + Pedido.Cliente.Telefono + ";";
                Lector = Comando.ExecuteReader(); 
            }

            SqliteCommand Comando2 = Conexion.CreateCommand();
            Comando2.CommandText = "INSERT INTO Pedido(Obs, Estado, idCliente) VALUES('" + Pedido.Obs + "', 'En Proceso', '" + Convert.ToInt32(Lector["idCliente"].ToString()) + "');";
            Comando2.ExecuteNonQuery(); 

            Conexion.Close();
        }

        public static void BajaPedidoDB(int nro)
        {
            SqliteConnection Conexion = CrearConexion();

            SqliteCommand Comando = Conexion.CreateCommand();
            Comando.CommandText = "DELETE FROM Pedido WHERE nroPedido='" + nro + "';";
            Comando.ExecuteNonQuery();

            Conexion.Close();
        }

        public static void EditarPedidoDB(EditarPedidoViewModel Pedido){
            SqliteConnection Conexion = CrearConexion();

            SqliteCommand Comando = Conexion.CreateCommand();
            Comando.CommandText = "SELECT * FROM Cliente WHERE telefonoCliente=" + Pedido.Cliente.Telefono + " ;";
            SqliteDataReader Lector = Comando.ExecuteReader();

            SqliteCommand Comando2 = Conexion.CreateCommand();

            if (Lector.Read())
            {
                Comando2.CommandText = "UPDATE Pedido SET Obs='" + Pedido.Obs + "' WHERE nroPedido='" + Pedido.Nro + "';";
                Comando2.ExecuteNonQuery(); 
            }else
            {
                Comando2.CommandText = "INSERT INTO Cliente(nombreCliente, direccionCliente, telefonoCliente, datosReferenciaDireccion) VALUES('" + Pedido.Cliente.Nombre + "', '" + Pedido.Cliente.Direccion + "', '" + Pedido.Cliente.Telefono + "', '" + Pedido.Cliente.DatosReferenciaDireccion + "');";
                Comando2.ExecuteNonQuery(); 

                Comando2.CommandText = "SELECT idCliente FROM Cliente WHERE telefonoCliente=" + Pedido.Cliente.Telefono + ";";
                SqliteDataReader Lector2 = Comando.ExecuteReader();

                SqliteCommand Comando3 = Conexion.CreateCommand();
                Comando3.CommandText = "UPDATE Pedido SET Obs='" + Pedido.Obs + "', idCliente ='" + Convert.ToInt32(Lector2["idCliente"].ToString()) + "' WHERE nroPedido='" + Pedido.Nro + "';";
                Comando3.ExecuteNonQuery();
            }

            Conexion.Close();
        }
    }
}