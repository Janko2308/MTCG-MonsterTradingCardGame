using MonsterTradingCardGame.BL;
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
    internal class DeckRepository
    {
        public List<CardInfo> GetDeck(string username)
        {
            List<CardInfo> deck = new();

            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();

            command.CommandText = "SELECT id, name, type, element, damage FROM CARD WHERE owner = @owner AND deck = true";

            NpgsqlCommand c = (command as NpgsqlCommand)!;
            c.Parameters.AddWithValue("owner", username);
            c.Prepare();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                CardInfo? card = new(reader.GetString(0), reader.GetString(1),
                                    reader.GetString(2), reader.GetString(3),
                                    reader.GetInt32(4));
                deck.Add(card);
            }
            reader.Close();
            DataBaseConnectionService.closeConnectionToDataBase(connection);

            return deck;

        }
        public void ConfigureDeck(string username, List<string> cardIds)
        {
            if(!CheckIfCardIsYours(username, cardIds))
            {
                throw new NoItemAvaiableException("Card is not yours");
            }
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();

            command.CommandText = "UPDATE CARD SET deck = false WHERE owner = @owner";
            NpgsqlCommand c = (command as NpgsqlCommand)!;
            c.Parameters.AddWithValue("owner", username);
            c.Prepare();
            command.ExecuteNonQuery();
            
            foreach (string cardId in cardIds)
            {
                command.CommandText = "UPDATE CARD SET deck = true WHERE id = @id";
                NpgsqlCommand c2 = (command as NpgsqlCommand)!;
                c2.Parameters.Clear();
                c2.Parameters.AddWithValue("id", cardId);
                c2.Prepare();
                command.ExecuteNonQuery();
            }
            DataBaseConnectionService.closeConnectionToDataBase(connection);
        }
        public bool CheckIfCardIsYours(string username, List<string> cardIds)
        {
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            int counter = 0;
            foreach (string cardId in cardIds)
            {
                command.CommandText = "SELECT id FROM CARD WHERE owner = @owner";
                NpgsqlCommand c = (command as NpgsqlCommand)!;
                c.Parameters.AddWithValue("owner", username);
                c.Prepare();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {

                    string id = reader.GetString(0);
                    if (id == cardId)
                    {

                        counter++;
                    }
                }
                reader.Close();

            }
            DataBaseConnectionService.closeConnectionToDataBase(connection);
            if(counter == 4)
            {
                return true;
            }
            return false;
        }
        
        public bool CheckIfDeckExists(string username)
        {
            
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id FROM CARD WHERE owner = @owner AND deck = true";
            NpgsqlCommand c = (command as NpgsqlCommand)!;
            c.Parameters.AddWithValue("owner", username);
            c.Prepare();
            int counter = 0;
            var reader = command.ExecuteReader();
            while(reader.Read())
            {
                counter++;
            }
            reader.Close();
            DataBaseConnectionService.closeConnectionToDataBase(connection);
            return counter == 4;
            
        }

    }
}
