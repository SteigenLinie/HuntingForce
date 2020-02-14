using HuntingForce.Character;
using HuntingForce.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HuntingForce.Factory
{
    class ItemFactory
    {
        private const string GAME_DATA_FILENAME = @"C:\Users\SteigenLinie\source\repos\HuntingForce\HuntingForce\GameData\GameItems.xml";

        public static readonly List<GameItem> _standardGameItems = new List<GameItem>();

        static ItemFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));
                LoadItemsFromNodes(data.SelectNodes("/GameItems/Weapons/Weapon"));
            }
        }
        private static void LoadItemsFromNodes(XmlNodeList nodes)
        {
            if (nodes == null)
            {
                return;
            }

            foreach (XmlNode node in nodes)
            {
                GameItem.ItemCategory itemCategory = DetermineItemCategory(node.Name);

                GameItem gameItem =
                    new GameItem(itemCategory,
                                 node.AttributeAsString("Name"),
                                 node.AttributeAsInt("MinimumDamage"),
                                 node.AttributeAsInt("MaximumDamage"));

                _standardGameItems.Add(gameItem);
            }
        }
        private static GameItem.ItemCategory DetermineItemCategory(string itemType)
        {
            switch (itemType)
            {
                case "Weapon":
                    return GameItem.ItemCategory.Weapon;
                case "HealingItem":
                    return GameItem.ItemCategory.Armor;
                default:
                    return GameItem.ItemCategory.Potion;
            }
        }
    }
}
