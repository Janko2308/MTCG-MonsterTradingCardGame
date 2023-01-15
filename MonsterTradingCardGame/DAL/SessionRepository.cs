using MonsterTradingCardGame.BL;
using MonsterTradingCardGame.BL.Exceptions;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.DAL
{
    public class SessionRepository
    {
        public bool CheckIfPasswordIsCorrect(string username, string password)
        {
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM USERS WHERE username = @username and password = @password";

            IDbDataParameter usernameParameter = command.CreateParameter();
            usernameParameter.ParameterName = "username";
            usernameParameter.DbType = DbType.String;
            usernameParameter.Value = username;
            command.Parameters.Add(usernameParameter);

            var pPassword = command.CreateParameter();
            pPassword.ParameterName = "password";
            pPassword.DbType = DbType.String;
            pPassword.Value = password;
            command.Parameters.Add(pPassword);

            var reader = command.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {
                count++;
            }
            reader.Close();
            DataBaseConnectionService.closeConnectionToDataBase(connection);
            return count == 1 ? true : false;
        }
        public string CreateSession(string username)
        {
            string token = username + "-mtcgtoken";
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE USERS SET token = @token WHERE username = @username";

            IDbDataParameter pUsername = command.CreateParameter();
            pUsername.ParameterName = "username";
            pUsername.DbType = DbType.String;
            pUsername.Size = 20;
            pUsername.Value = username;
            command.Parameters.Add(pUsername);

            var pToken = command.CreateParameter();
            pToken.ParameterName = "token";
            pToken.DbType = DbType.String;
            pToken.Size = 40;
            pToken.Value = token;
            command.Parameters.Add(pToken);

            var reader = command.ExecuteReader();
            reader.Read();
            reader.Close();
            DataBaseConnectionService.closeConnectionToDataBase(connection);
            return token;

        }
        public bool CheckIfSession(string username, string authorizationToken)
        {
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = "SELECT token FROM USERS WHERE username = @username";

            IDbDataParameter pUsername = command.CreateParameter();
            pUsername.ParameterName = "username";
            pUsername.DbType = DbType.String;
            pUsername.Size = 20;
            pUsername.Value = username;
            command.Parameters.Add(pUsername);

            var reader = command.ExecuteReader();
            string token = "";
            while (reader.Read())
            {

                if (reader.IsDBNull(0))
                {
                    reader.Close();
                    DataBaseConnectionService.closeConnectionToDataBase(connection);
                    throw new AuthenticateTokenException("Database is empty");
                }
                token = reader.GetString(0);
            }
            reader.Close();
            DataBaseConnectionService.closeConnectionToDataBase(connection);

            token = token.ToLower();
            authorizationToken = authorizationToken.ToLower();
            return token == username + "-mtcgtoken" && token == authorizationToken;
        }
        public void RemoveSession(string token)
        {
            string[] parts = token.Split('-');
            string firstPart = parts[0];
            token = firstPart + "-mtcgtoken";

            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE USERS SET token = null WHERE token = @token";

            IDbDataParameter pToken = command.CreateParameter();
            pToken.ParameterName = "token";
            pToken.DbType = DbType.String;
            pToken.Size = 40;
            pToken.Value = token;
            command.Parameters.Add(pToken);

            command.ExecuteNonQuery();

            DataBaseConnectionService.closeConnectionToDataBase(connection);
            
        }
    }
}
