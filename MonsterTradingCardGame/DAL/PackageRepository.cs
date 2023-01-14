using MonsterTradingCardGame.BL;
using MonsterTradingCardGame.Http;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.DAL
{
    internal class PackageRepository
    {
        public void AddPackage(List<BL.Card> cardList)
        {
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            foreach(Card card in cardList)
            {
                IDbCommand command = connection.CreateCommand();
                string id = card.ID;
                string name = card.Name;
                string type = card.Type;
                string element = card.Element;
                int damage = card.Damage;
                bool deck = false;

                command.CommandText = @"INSERT INTO CARD(id, name, type, element, damage, deck) values (@id, @name, @type, @element, @damage, @deck)";

                NpgsqlCommand c = command as NpgsqlCommand;
                c.Parameters.AddWithValue("id", id);
                c.Parameters.AddWithValue("name", name);
                c.Parameters.AddWithValue("type", type);
                c.Parameters.AddWithValue("element", element);
                c.Parameters.AddWithValue("damage", damage);
                c.Parameters.AddWithValue("deck", deck);
                c.Prepare();
                command.ExecuteNonQuery();
            }
            IDbCommand command2 = connection.CreateCommand();
            string cardid1 = cardList[0].ID;
            string cardid2 = cardList[1].ID;
            string cardid3 = cardList[2].ID;
            string cardid4 = cardList[3].ID;
            string cardid5 = cardList[4].ID;

            command2.CommandText = @"INSERT INTO PACKAGES(cardid1, cardid2, cardid3, cardid4, cardid5) values (@cardid1, @cardid2, @cardid3, @cardid4, @cardid5)";

            NpgsqlCommand c2 = command2 as NpgsqlCommand;
            c2.Parameters.AddWithValue("cardid1", cardid1);
            c2.Parameters.AddWithValue("cardid2", cardid2);
            c2.Parameters.AddWithValue("cardid3", cardid3);
            c2.Parameters.AddWithValue("cardid4", cardid4);
            c2.Parameters.AddWithValue("cardid5", cardid5);
            c2.Prepare();
            command2.ExecuteNonQuery();

            DataBaseConnectionService.closeConnectionToDataBase(connection);
        }

        public bool CheckIfCardExists(List<BL.Card> cardList)
        {
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            bool found = false;
            foreach(Card card in cardList)
            {
                string id = card.ID;
                IDbCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT FROM CARD ID WHERE ID = @id";
                NpgsqlCommand c = command as NpgsqlCommand;
                c.Parameters.AddWithValue("id", id);
                c.Prepare();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    found = true;
                }
                reader.Close();
            }
            //wenn es die karten noch nicht gibt
            DataBaseConnectionService.closeConnectionToDataBase(connection);
            return found;
        }
    }
}
