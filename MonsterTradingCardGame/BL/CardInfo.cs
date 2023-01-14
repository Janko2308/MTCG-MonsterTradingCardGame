using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.BL
{
    public class CardInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Element { get; set; }
        public int Damage { get; set; }
        public CardInfo(string id, string name, string type, string element, int damage)
        {
            this.Id = id;
            this.Name = name;
            this.Type = type;
            this.Element = element;
            this.Damage = damage;
        }
    }
}
