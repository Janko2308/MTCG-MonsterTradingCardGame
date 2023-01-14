using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.BL
{
    public class Card
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Element { get; set; }
        public int Damage { get; set; }

        public Card(string ID, string Name, string Type, string Element, int Damage)
        {
            this.ID = ID;
            this.Name = Name;
            this.Type = Type;
            this.Element = Element;
            this.Damage = Damage;
        }
    }
}
