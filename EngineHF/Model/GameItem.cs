using EngineHF.Model.ItemCategory;
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
            Armor,
            Accessory,
            Potion
        }
        public ItemCategory Category { get; set; }
        public int ItemID { get; set; }
        public string Name { get; set; }
        public int Count { get; set; } = 1;
        public string ImageName { get; set; }
        public string Descrtiption { get; set; }
        public Weapon Weapon { get; set; }
        public Armor Armor { get; set; }
        public Accessory Accessory { get; set; }
        public Potion Potion { get; set; }
      

        public GameItem(ItemCategory category, int itemID, string name, string imageName, string description,
            Weapon weapon = null, Armor armor = null, Accessory accessory = null, Potion potion = null)
        {
            Category = category;
            ItemID = itemID;
            Name = name;
            ImageName = imageName;
            Descrtiption = description;
            Weapon = weapon;
            Armor = armor;
            Accessory = accessory;
            Potion = potion;
        }
        public GameItem()
        {

        }
    }
}
