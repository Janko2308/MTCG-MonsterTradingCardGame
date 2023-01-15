using MonsterTradingCardGame.BL.Exceptions;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.DAL
{
    internal class StatsRepository
    {
        
        public BL.Stats GetStats(string username)
        {
            BL.Stats? stats = null;
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = "SELECT username,elo,wins,losses FROM USERS WHERE Username = @username";
            NpgsqlCommand c = (command as NpgsqlCommand)!;
            c.Parameters.AddWithValue("username", username);
            c.Prepare();
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                stats = new(reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3));
            }
            else
            {
                throw new UserNotFoundException("User not found");
            }
            reader.Close();
            DataBaseConnectionService.closeConnectionToDataBase(connection);
            return stats;
        }
    }
}
