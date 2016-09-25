using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace DatabaseCMD
{
    class Program
    {
        
        
        static void Main(string[] args)
        {
            OdbcConnection conn = new OdbcConnection("DRIVER=SQLite3 ODBC Driver;Database=YoutubeDB;LongNames=0;Timeout=1000;NoTXN=0;SyncPragma = NORMAL; StepAPI = 0; ");

            conn.Open();

            //SQLiteConnection connection = new SQLiteConnection("Data Source=c:\\lucas.db;Version=3;");
            //connection.Open();

            //SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'Video'", connection);
            
            //SQLiteDataReader reader = command.ExecuteReader();

            //while (reader.Read())
            //{
            //    Console.WriteLine("item: {0}", reader[0]);
            //}

            //reader.Close();
            //connection.Close();

            //Console.Read();

        }
    }
}
