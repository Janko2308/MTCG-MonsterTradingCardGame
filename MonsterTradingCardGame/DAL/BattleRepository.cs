using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.DAL
{
    internal class BattleRepository
    {
        public List<BL.Card> GetDeck(string player)
        {
            List<BL.Card> deck = new();
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id,name,type,element,damage FROM CARD WHERE owner = @owner AND deck = true";
            NpgsqlCommand c = command as NpgsqlCommand;
            c.Parameters.AddWithValue("owner", player);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                BL.Card? card = new(reader.GetString(0), reader.GetString(1),
                                    reader.GetString(2), reader.GetString(3),
                                    reader.GetInt32(4));
                deck.Add(card);
            }
            reader.Close();
            DataBaseConnectionService.closeConnectionToDataBase(connection);
            return deck; 
        } 

        public void SetScore(string winner, string loser)
        {
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            
            IDbCommand command = connection.CreateCommand();
            command.CommandText = "SELECT elo,wins FROM USERS WHERE username = @username";
            NpgsqlCommand c = command as NpgsqlCommand;
            c.Parameters.AddWithValue("username", winner);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                int elo = reader.GetInt32(0);
                int wins = reader.GetInt32(1);
                wins++;
                elo += 3;
                UpdateScoreWinner(winner, elo, wins);
            }
            reader.Close();
            
            IDbCommand command2 = connection.CreateCommand();
            command2.CommandText = "SELECT elo,losses FROM USERS WHERE username = @username";
            NpgsqlCommand c2 = command2 as NpgsqlCommand;
            c2.Parameters.AddWithValue("username", loser);
            var reader2 = command2.ExecuteReader();
            while (reader2.Read())
            {
                int elo = reader2.GetInt32(0);
                int losses = reader2.GetInt32(1);
                losses++;
                elo -= 5;
                if(elo <= 0)
                {
                    elo = 0;
                }
                UpdateScoreLoser(loser, elo, losses);
            }
            reader2.Close();
            DataBaseConnectionService.closeConnectionToDataBase(connection);
        
        }

        public void UpdateScoreWinner(string winner, int elo, int wins)
        {
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE USERS SET elo = @elo, wins = @wins WHERE username = @username";
            NpgsqlCommand c = command as NpgsqlCommand;
            c.Parameters.AddWithValue("username", winner);
            c.Parameters.AddWithValue("elo", elo);
            c.Parameters.AddWithValue("wins", wins);
            c.Prepare();
            command.ExecuteNonQuery();
            DataBaseConnectionService.closeConnectionToDataBase(connection);
        }
        public void UpdateScoreLoser(string loser, int elo, int losses)
        {
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE USERS SET elo = @elo, losses = @losses WHERE username = @username";
            NpgsqlCommand c = command as NpgsqlCommand;
            c.Parameters.AddWithValue("username", loser);
            c.Parameters.AddWithValue("elo", elo);
            c.Parameters.AddWithValue("losses", losses);
            c.Prepare();
            command.ExecuteNonQuery();
            DataBaseConnectionService.closeConnectionToDataBase(connection);
        }
    }
}
