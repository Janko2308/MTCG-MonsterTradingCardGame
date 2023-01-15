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
    internal class CardRepository
    {
        public List<CardInfo> GetCards(string username)
        {
            List<CardInfo> cards = new();

            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();

            command.CommandText = "SELECT id, name, type, element, damage FROM CARD WHERE owner = @owner";

            NpgsqlCommand c = (command as NpgsqlCommand)!;
            c.Parameters.AddWithValue("owner", username);
            c.Prepare();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                CardInfo? card = new(reader.GetString(0), reader.GetString(1),
                                    reader.GetString(2), reader.GetString(3), 
                                    reader.GetInt32(4));
                cards.Add(card);
            }
            reader.Close();
            DataBaseConnectionService.closeConnectionToDataBase(connection);

            return cards;
        }
        
    }
}
