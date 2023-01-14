using MonsterTradingCardGame.BL.Exceptions;
using MonsterTradingCardGame.Http;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.DAL
{
    internal class TransactionsRepository
    {
        public void BuyPackage(string token)
        {
            int id = 0;
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            
            command.CommandText = "SELECT MIN(ID) FROM PACKAGES";

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    id = reader.GetInt32(0);
                }
            }
            reader.Close();
            
            if(id == 0)
            {
                throw new NoItemAvaiableException("No package avaiable");

            }
            if (!CheckMoneyAndPay(token))
            {
                throw new NotEnoughMoneyException("Not enough money");
            }

            IDbCommand command2 = connection.CreateCommand();
            command2.CommandText = "SELECT cardid1, cardid2, cardid3, cardid4, cardid5 FROM PACKAGES WHERE ID = @id";

            NpgsqlCommand c = command2 as NpgsqlCommand;
            c.Parameters.AddWithValue("id", id);
            c.Prepare();
            var reader2 = command2.ExecuteReader();
            string[] cardIds = new string[5];

            while (reader2.Read())
            {
                cardIds[0] = reader2.GetString(0);
                cardIds[1] = reader2.GetString(1);
                cardIds[2] = reader2.GetString(2);
                cardIds[3] = reader2.GetString(3);
                cardIds[4] = reader2.GetString(4);
            }
            reader2.Close();


            string username = GetUserNameFromToken(token);
            for(int i = 0; i < 5; i++)
            {
                AddCards(username, cardIds[i]);
            }
            
            DeletePackage(id);
            DataBaseConnectionService.closeConnectionToDataBase(connection);

        }
        
        public bool CheckMoneyAndPay(string token)
        {
            bool hasMoney = false;
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();

            command.CommandText = @"SELECT COINS FROM USERS WHERE token = @token";
            NpgsqlCommand c = command as NpgsqlCommand;
            c.Parameters.AddWithValue("token", token);
            c.Prepare();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    int coins = reader.GetInt32(0);
                    if (coins >= 5)
                    {
                        hasMoney =  true;
                    }
                }
            }
            reader.Close();
            if (hasMoney)
            {
                command.CommandText = @"UPDATE USERS SET coins = coins - 5 WHERE token = @token";
                c = command as NpgsqlCommand;
                c.Parameters.AddWithValue("token", token);
                c.Prepare();
                command.ExecuteNonQuery();
            }
            DataBaseConnectionService.closeConnectionToDataBase(connection);
            return hasMoney;
        }

        public void AddCards(string username, string cardid)
        {
            
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = @"UPDATE CARD SET owner = @owner WHERE id = @id";
            NpgsqlCommand c = command as NpgsqlCommand;
            c.Parameters.AddWithValue("owner", username);
            c.Parameters.AddWithValue("id", cardid);
            c.Prepare();
            command.ExecuteNonQuery();
           
            DataBaseConnectionService.closeConnectionToDataBase(connection);

        }

        public string GetUserNameFromToken(string token)
        {
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            string username = "";
            command.CommandText = @"SELECT USERNAME FROM USERS WHERE token = @token";
            NpgsqlCommand c = command as NpgsqlCommand;
            c.Parameters.AddWithValue("token", token);
            c.Prepare();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    username = reader.GetString(0);
                }
            }
            reader.Close();
            DataBaseConnectionService.closeConnectionToDataBase(connection);
            return username;
        }

        public void DeletePackage(int id)
        {
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM PACKAGES WHERE ID = @id";
            NpgsqlCommand c = command as NpgsqlCommand;
            c.Parameters.AddWithValue("id", id);
            c.Prepare();
            command.ExecuteNonQuery();

            DataBaseConnectionService.closeConnectionToDataBase(connection);

        }
    }
}
