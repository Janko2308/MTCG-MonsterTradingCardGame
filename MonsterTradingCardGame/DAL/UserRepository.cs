using MonsterTradingCardGame.BL;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.DAL
{
    internal class UserRepository
    {
        public bool CheckIfUserExists(string username)
        {
           
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM USERS WHERE username = @username";

            IDbDataParameter pUSERNAME = command.CreateParameter();
            pUSERNAME.ParameterName = "username";
            pUSERNAME.DbType = DbType.String;
            pUSERNAME.Size = 20;
            pUSERNAME.Value = username;
            command.Parameters.Add(pUSERNAME);

            using (IDataReader reader = command.ExecuteReader())
            {

                if (reader.Read())
                {
                    reader.Close();
                    DataBaseConnectionService.closeConnectionToDataBase(connection);
                    return true;
                }
                reader.Close();
                DataBaseConnectionService.closeConnectionToDataBase(connection);
                return false;
            }
        }
        public void RegisterUser(string username, string password)
        {
            int role = checkIfEmpty();
            
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO USERS(username, password, coins, role, elo, wins, losses) values (@username,@password,@coins,@role,@elo,@wins,@losses)";

            NpgsqlCommand c = (command as NpgsqlCommand)!;
            c.Parameters.AddWithValue("username", username);
            c.Parameters.AddWithValue("password", password);
            c.Parameters.AddWithValue("coins", 20);
            c.Parameters.AddWithValue("role", role);
            c.Parameters.AddWithValue("elo", 100);
            c.Parameters.AddWithValue("wins", 0);
            c.Parameters.AddWithValue("losses", 0);
            c.Prepare();
            command.ExecuteNonQuery();

            DataBaseConnectionService.closeConnectionToDataBase(connection);
        }

        public void DeleteUser(string username, string password)
        {

            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM USERS WHERE username = @username AND password = @password";

            IDbDataParameter pUSERNAME = command.CreateParameter();
            pUSERNAME.ParameterName = "username";
            pUSERNAME.DbType = DbType.String;
            pUSERNAME.Size = 20;
            pUSERNAME.Value = username;
            command.Parameters.Add(pUSERNAME);

            IDbDataParameter pPASSWORD = command.CreateParameter();
            pPASSWORD.ParameterName = "password";
            pPASSWORD.DbType = DbType.String;
            pPASSWORD.Size = 100;
            pPASSWORD.Value = password;
            command.Parameters.Add(pPASSWORD);

            command.ExecuteNonQuery();
            DataBaseConnectionService.closeConnectionToDataBase(connection);
        }
    

        public int checkIfEmpty()
        {
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM USERS";
            
            var reader = command.ExecuteReader();
            int role = 1;
            while (reader.Read())
            {
                role = 0;
            }
            reader.Close();
            DataBaseConnectionService.closeConnectionToDataBase(connection);
            return role;
        }

        public UserInfo GetUser(string username)
        {
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();

            command.CommandText = "SELECT name, bio, image FROM USERS WHERE username = @username";

            IDbDataParameter usernameParameter = command.CreateParameter();
            usernameParameter.ParameterName = "username";
            usernameParameter.DbType = DbType.String;
            usernameParameter.Value = username;
            command.Parameters.Add(usernameParameter);
            UserInfo user = new();
            string? name = null;
            string? bio = null;
            string? image = null;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    name = reader.GetString(0);
                }
                else
                {
                    name = "no name";
                }
                if (!reader.IsDBNull(1))
                {
                    bio = reader.GetString(1);
                }
                else
                {
                    bio = "no bio";
                }
                if (!reader.IsDBNull(2))
                {
                    image = reader.GetString(2);
                }
                else
                {
                    image = "no image";
                }
                user.Name = name;
                user.Bio = bio;
                user.Image = image;
            }
            reader.Close();
            DataBaseConnectionService.closeConnectionToDataBase(connection);
            
            return user;
        }
        public void UpdateUserInfo(string username, string content)
        {
            UserInfo user = JsonSerializer.Deserialize<UserInfo>(content)!;
            if(user == null)
            {
                throw new ArgumentNullException();
            }
            string name = user.Name!;
            string bio = user.Bio!;
            string image = user.Image!;
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE USERS SET name = @name, bio = @bio, image = @image WHERE username = @username";

            if(user.Name != null)
            {
                IDbDataParameter pName = command.CreateParameter();
                pName.ParameterName = "name";
                pName.DbType = DbType.String;
                pName.Value = name;
                pName.Size = 20;
                command.Parameters.Add(pName);
            }

            if(user.Bio != null)
            {
                IDbDataParameter pBio = command.CreateParameter();
                pBio.ParameterName = "bio";
                pBio.DbType = DbType.String;
                pBio.Value = bio;
                pBio.Size = 500;
                command.Parameters.Add(pBio);
            }

            if(user.Image != null)
            {
                IDbDataParameter pImage = command.CreateParameter();
                pImage.ParameterName = "image";
                pImage.DbType = DbType.String;
                pImage.Value = image;
                pImage.Size = 50;
                command.Parameters.Add(pImage);
            }

            IDbDataParameter pUsername = command.CreateParameter();
            pUsername.ParameterName = "username";
            pUsername.DbType = DbType.String;
            pUsername.Value = username;
            pUsername.Size = 20;
            command.Parameters.Add(pUsername);

            command.ExecuteNonQuery();
            DataBaseConnectionService.closeConnectionToDataBase(connection);
        }

        public bool CheckIfAdmin(string token)
        {
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = "SELECT ROLE FROM USERS WHERE token = @token";
            IDbDataParameter pToken = command.CreateParameter();
            pToken.ParameterName = "token";
            pToken.DbType = DbType.String;
            pToken.Value = token;
            pToken.Size = 40;
            command.Parameters.Add(pToken);

            var reader = command.ExecuteReader();
            bool isAdmin = false;
            while (reader.Read())
            {
                isAdmin = true;
            }
            reader.Close();
            DataBaseConnectionService.closeConnectionToDataBase(connection);
            return isAdmin;
        }
        public string GetUserNameFromToken(string token)
        {
            IDbConnection connection = DataBaseConnectionService.connectToDataBase();
            IDbCommand command = connection.CreateCommand();
            string username = "";
            command.CommandText = @"SELECT USERNAME FROM USERS WHERE token = @token";
            NpgsqlCommand c = (command as NpgsqlCommand)!;
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
    }
}
