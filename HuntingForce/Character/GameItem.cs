using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingForce.Character
{
    class GameItem
    {
        public enum ItemCategory
        {
            Weapon,
            Armor,
            Potion
        }
        public ItemCategory Category { get; }
        public string Name { get; }
        public int MinDamage { get; }
        public int MaxDamage { get; }
        public GameItem(ItemCategory category,string name, int mindamage, int maxdamage)
        {
            Category = category;
            Name = name;
            MinDamage = mindamage;
            MaxDamage = maxdamage;
        }
    }
}
