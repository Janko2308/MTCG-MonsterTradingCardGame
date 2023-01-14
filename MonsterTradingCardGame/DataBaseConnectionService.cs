using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Npgsql;

namespace MonsterTradingCardGame
{
    public class DataBaseConnectionService
    {
        //private string connstring = "Server=localhost;Port=5432;UserId=swe1user;Password=swe1pw;Database=mtcg;";
        //public IDbConnection connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=swe1pw;Database=postgres");
        public static IDbConnection connectToDataBase()
        {
            IDbConnection connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=swe1pw;Database=postgres");
            connection.Open();
            return connection;
        }

        public static void closeConnectionToDataBase(IDbConnection connection)
        {
            connection.Close();
        }
    }
}
