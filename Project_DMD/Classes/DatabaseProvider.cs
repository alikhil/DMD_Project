using Npgsql;
using Project_DMD.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Project_DMD.Classes
{
    public abstract class DatabaseProvider
    {
        public static NpgsqlConnection CreateConnection()
        {
            var conn = new NpgsqlConnection(DatabaseConstants.ConnectionString);
            conn.Open();
            conn.RegisterEnum<ActionType>("actiontype");
            return conn;
        }

        public void Log(string message)
        {
            TextWriter writer = Console.Out;
            //Writer = new StreamWriter(String.Format("log_{0}_.txt", DateTime.Now.ToShortDateString()));
            writer.WriteLine(message);
        }
    }
}