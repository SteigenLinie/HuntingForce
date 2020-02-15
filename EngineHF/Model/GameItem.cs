using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineHF.Model
{
    public class GameItem
    {
        public enum ItemCategory
        {
            Miscellaneous,
            Weapon,
            Potion
        }
        public ItemCategory Category { get; set; }
        public int ItemID { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }
        public string Descrtiption { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public GameItem(ItemCategory category, int itemID, string name, string imageName, string description, int minDamage, int maxDamage)
        {
            Category = category;
            ItemID = itemID;
            Name = name;
            ImageName = imageName;
            Descrtiption = description;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }
    }
}
