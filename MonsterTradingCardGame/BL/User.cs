using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.BL
{
    public class User
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public int Coins { get; set; }
        public string? Name { get; set; }
        public string? Bio { get; set; }
        public string? Image { get; set; }
        //name bio image als konstruktor
        public User(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }
        

        
    }
}
