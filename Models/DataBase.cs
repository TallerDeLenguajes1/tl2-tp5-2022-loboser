using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tl2_tp4_2022_loboser.ViewModels;

#nullable disable

namespace tl2_tp4_2022_loboser.Models
{
    public class DataBase
    {
        static SQLiteConnection CrearConexion()
        {  
            SQLiteConnection Conexion = new SQLiteConnection("Data Source=PedidosDB.db");
         
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
            SQLiteConnection Conexion = CrearConexion();
            Cadeteria Cadeteria = new Cadeteria();

            SQLiteCommand Comando = Conexion.CreateCommand();
            Comando.CommandText = "Select nombreCadeteria, telefonoCadeteria From Cadeteria WHERE idCadeteria = 1";

            SQLiteDataReader Lector = Comando.ExecuteReader();

            while (Lector.Read())
            {
                Cadeteria.Nombre = Lector["nombreCadeteria"].ToString();
                Cadeteria.Telefono = Lector["nombreCadeteria"].ToString();
            }
            Cadeteria.Cadetes = CargarCadetesDB(Conexion);
            Conexion.Close();

            return Cadeteria;
        }

        static List<Cadete> CargarCadetesDB(SQLiteConnection Conexion){
            List<Cadete> Cadetes = new List<Cadete>();

            SQLiteCommand Comando = Conexion.CreateCommand();
            Comando.CommandText = "SELECT * FROM Cadete";

            SQLiteDataReader Lector = Comando.ExecuteReader();

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
        
        public static void AltaCadeteDB(Cadete Cadete)
        {
            SQLiteConnection Conexion = CrearConexion();

            SQLiteCommand Comando = Conexion.CreateCommand();
            Comando.CommandText = "INSERT INTO Cadete(nombreCadete, direccionCadete, telefonoCadete) VALUES ('" + Cadete.Nombre + "', '" + Cadete.Direccion + "' ,'" + Cadete.Telefono + "');";
            Comando.ExecuteNonQuery();

            Comando.CommandText = "SELECT * FROM Cadete WHERE nombreCadete='" + Cadete.Nombre + "' AND direccionCadete='" + Cadete.Direccion + "' AND telefonoCadete='" + Cadete.Telefono + "';";
            SQLiteDataReader Lector = Comando.ExecuteReader();

            SQLiteCommand Comando2 = Conexion.CreateCommand();


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
            SQLiteConnection Conexion = CrearConexion();

            SQLiteCommand Comando = Conexion.CreateCommand();
            Comando.CommandText = "DELETE FROM Cadete WHERE idCadete='" + id + "';";
            Comando.ExecuteNonQuery();
            Comando.CommandText = "DELETE FROM CadeteCadeteria WHERE idCadete='" + id + "';";
            Comando.ExecuteNonQuery();

            Conexion.Close();
        }

        public static void EditarCadeteDB(EditarCadeteViewModel Cadete){
            SQLiteConnection Conexion = CrearConexion();

            SQLiteCommand Comando = Conexion.CreateCommand();
            Comando.CommandText = "UPDATE Cadete SET nombreCadete='" + Cadete.Nombre + "', direccionCadete='" + Cadete.Direccion + "', telefonoCadete='" + Cadete.Telefono + "' WHERE idCadete='" + Cadete.Id + "';";
            Comando.ExecuteNonQuery();

            Conexion.Close();
        }
    }
}