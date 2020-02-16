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
        public int MaxDamage { get; set; }
        public int MinDamage { get; set; }
        public short HitPointsToHealHP { get; set; }
        public short HitPointsToHealMP { get; set; }
        public GameItem(ItemCategory category, int itemID, string name, string imageName, string description)
        {
            Category = category;
            ItemID = itemID;
            Name = name;
            ImageName = imageName;
            Descrtiption = description;
        }
        public GameItem(ItemCategory category, int itemID, string name, string imageName, string description, short hitPointsToHealHP, short hitPointsToHealMP)
        {
            Category = category;
            ItemID = itemID;
            Name = name;
            ImageName = imageName;
            Descrtiption = description;
            HitPointsToHealHP = hitPointsToHealHP;
            HitPointsToHealMP = hitPointsToHealMP;
        }
        public GameItem(ItemCategory category, int itemID, string name, string imageName, string description, int maxDamage, int minDamage)
        {
            Category = category;
            ItemID = itemID;
            Name = name;
            ImageName = imageName;
            Descrtiption = description;
            MaxDamage = maxDamage;
            MinDamage = minDamage;
        }
        public GameItem()
        {

        }
    }
}
