using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tl2_tp4_2022_loboser.Models;
using tl2_tp4_2022_loboser.ViewModels;

#nullable disable

namespace tl2_tp4_2022_loboser.Repositories{
    public interface ICadeteriaRepository
    {
        Cadeteria GetCadeteria();
        List<Cadete> GetCadetes();
        void AltaCadete(AltaCadeteViewModel Cadete);
        void BajaCadete(int id);
        void EditarCadete(EditarCadeteViewModel Cadete);
    }

    public class CadeteriaRepository : ICadeteriaRepository
    {
        private readonly string _cadenaConexion;

        public CadeteriaRepository()
        {
            this._cadenaConexion = "Data Source=PedidosDB.db";
        }

        public Cadeteria GetCadeteria(){
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "SELECT nombreCadeteria, telefonoCadeteria FROM Cadeteria WHERE idCadeteria = 1";
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        Cadeteria Cadeteria = new Cadeteria();
                        while (Lector.Read())
                        {
                            Cadeteria.Nombre = Lector["nombreCadeteria"].ToString();
                            Cadeteria.Telefono = Lector["nombreCadeteria"].ToString();
                        }
                        Cadeteria.Cadetes = GetCadetes();

                        return Cadeteria;
                    }
                }
            }
        }

        public List<Cadete> GetCadetes(){
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                Conexion.Open();
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "SELECT * FROM Cadete";
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        List<Cadete> Cadetes = new List<Cadete>();
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
                }
            }
        }

        public void AltaCadete(AltaCadeteViewModel Cadete)
        {
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "INSERT INTO Cadete(nombreCadete, direccionCadete, telefonoCadete) VALUES ('" + Cadete.Nombre + "', '" + Cadete.Direccion + "' ,'" + Cadete.Telefono + "');";
                    Comando.ExecuteNonQuery();

                    Comando.CommandText = "SELECT * FROM Cadete WHERE nombreCadete='" + Cadete.Nombre + "' AND direccionCadete='" + Cadete.Direccion + "' AND telefonoCadete='" + Cadete.Telefono + "';";
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        using (SqliteCommand Comando2 = Conexion.CreateCommand())
                        {
                            if(Lector.Read())
                            {
                                int idCadeteria = 1;
                                int id = Convert.ToInt32(Lector["idCadete"].ToString());
                                Comando2.CommandText = "INSERT INTO CadeteCadeteria(idCadeteria, idCadete) VALUES ('" + idCadeteria + "', '" + id + "');";
                                Comando2.ExecuteNonQuery();   
                            }
                        }  
                    }
                }
            }
        }

        public void BajaCadete(int id)
        {
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "DELETE FROM Cadete WHERE idCadete='" + id + "';";
                    Comando.ExecuteNonQuery();
                    Comando.CommandText = "DELETE FROM CadeteCadeteria WHERE idCadete='" + id + "';";
                    Comando.ExecuteNonQuery();
                }
            }
        }

        public void EditarCadete(EditarCadeteViewModel Cadete){
            using(SqliteConnection Conexion = new SqliteConnection(_cadenaConexion)){
                using (SqliteCommand Comando = Conexion.CreateCommand())
                {
                    Comando.CommandText = "UPDATE Cadete SET nombreCadete='" + Cadete.Nombre + "', direccionCadete='" + Cadete.Direccion + "', telefonoCadete='" + Cadete.Telefono + "' WHERE idCadete='" + Cadete.Id + "';";
                    Comando.ExecuteNonQuery();
                }
            }
        }
    }
}