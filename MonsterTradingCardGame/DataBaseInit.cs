using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame
{
    internal class DataBaseInit
    {
        public static void init()
        {
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = @"CREATE TABLE IF NOT EXISTS USERS (
	                                    USERNAME VARCHAR(20) PRIMARY KEY,
	                                    PASSWORD VARCHAR(100) NOT NULL,
	                                    COINS INT NOT NULL,
	                                    NAME VARCHAR(20),
	                                    BIO VARCHAR(500),
	                                    IMAGE VARCHAR(50));";


            command.ExecuteNonQuery();
            DataBaseConnectionService.closeConnectionToDataBase(connection);
        }
        //TODO: DROP ALL TABLES 
    }
}
