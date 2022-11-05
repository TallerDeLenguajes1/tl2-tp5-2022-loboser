using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tl2_tp4_2022_loboser.Models
{
    class SQLite
    {
        static SQLiteConnection CrearConexion()
      {  
         SQLiteConnection Conexion = new SQLiteConnection("Data Source=../PedidosDB.db;Version=3,New=True;Compress=True;");
         
         try
         {
            Conexion.Open();
         }
         catch (Exception ex)
         {

         }
         return Conexion;
      }
      static void InsertData(SQLiteConnection Conexion)
      {
         SQLiteCommand Comando;
         Comando = Conexion.CreateCommand();
         Comando.ExecuteNonQuery();
      }

      static void ReadData(SQLiteConnection Conexion)
      {
         SQLiteDataReader Lector;
         SQLiteCommand Comando;
         Comando = Conexion.CreateCommand();
         Comando.CommandText = "SELECT * FROM SampleTable";

         Lector = Comando.ExecuteReader();
         while (Lector.Read())
         {
            string myreader = Lector.GetString(0);
            Console.WriteLine(myreader);
         }
         Conexion.Close();
      }
    }
}