using MonsterTradingCardGame.BL;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.DAL
{
    internal class ScoreRepository
    {
        
        public List<BL.Stats> GetScoreBoard()
        {

            List<BL.Stats> scoreBoard = new();

            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = "SELECT username,elo,wins,losses FROM USERS ORDER BY elo DESC";
            
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
            
                BL.Stats? score = new(reader.GetString(0), reader.GetInt32(1),
                                    reader.GetInt32(2), reader.GetInt32(3));
                
                scoreBoard.Add(score);
            }
            reader.Close();
            DataBaseConnectionService.closeConnectionToDataBase(connection);
            return scoreBoard;
        }
    }
}
