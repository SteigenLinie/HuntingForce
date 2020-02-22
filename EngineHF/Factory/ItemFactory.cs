using EngineHF.Model;
using EngineHF.Model.ItemCategory;
using EngineHF.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EngineHF.Factory
{
    public class ItemFactory
    {
        private const string GAME_DATA_FILENAME = "./Data/GameItems.xml";

        internal static readonly List<GameItem> _standardGameItems = new List<GameItem>();

        public ItemFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                string rootImagePath =
                    data.SelectSingleNode("/GameItems")
                        .AttributeAsString("RootImagePath");

                LoadItemsFromNodes(data.SelectNodes("/GameItems/GameItem"), rootImagePath);

            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }
        private static void LoadItemsFromNodes(XmlNodeList nodes, string rootImagePath)
        {
            if (nodes == null)
            {
                return;
            }

            foreach (XmlNode node in nodes)
            {
                GameItem.ItemCategory itemCategory = DetermineItemCategory(node.AttributeAsString("Type"));
                GameItem gameItem = new GameItem();
                switch(itemCategory)
                {
                    case GameItem.ItemCategory.Weapon:
                        gameItem = new GameItem(itemCategory,
                                                node.AttributeAsInt("ID"),
                                                node.AttributeAsString("Name"),
                                                $".{rootImagePath}{node.AttributeAsString("ImageName")}",
                                                node.AttributeAsString("Description"),
                                                Weapon(node.SelectSingleNode("./Weapon")));
                        break;
                    case GameItem.ItemCategory.Armor:
                        gameItem = new GameItem(itemCategory,
                                                node.AttributeAsInt("ID"),
                                                node.AttributeAsString("Name"),
                                                $".{rootImagePath}{node.AttributeAsString("ImageName")}",
                                                node.AttributeAsString("Description"),
                                                null,
                                                Armor(node.SelectSingleNode("./Armor")));
                        break;
                    case GameItem.ItemCategory.Accessory:
                        gameItem = new GameItem(itemCategory,
                                                node.AttributeAsInt("ID"),
                                                node.AttributeAsString("Name"),
                                                $".{rootImagePath}{node.AttributeAsString("ImageName")}",
                                                node.AttributeAsString("Description"),
                                                null,
                                                null,
                                                Accessory(node.SelectSingleNode("./Accessory")));
                        break;
                    case GameItem.ItemCategory.Potion:
                        gameItem = new GameItem(itemCategory,
                                                node.AttributeAsInt("ID"),
                                                node.AttributeAsString("Name"),
                                                $".{rootImagePath}{node.AttributeAsString("ImageName")}",
                                                node.AttributeAsString("Description"),
                                                null,
                                                null,
                                                null,
                                                Potion(node.SelectSingleNode("./Potion")));
                        break;
                    case GameItem.ItemCategory.Miscellaneous:
                        gameItem = new GameItem(itemCategory,
                                                node.AttributeAsInt("ID"),
                                                node.AttributeAsString("Name"),
                                                $".{rootImagePath}{node.AttributeAsString("ImageName")}",
                                                node.AttributeAsString("Description"));
                        break;
                }
                


                _standardGameItems.Add(gameItem);
            }
        }

        private static GameItem.ItemCategory DetermineItemCategory(string itemType)
        {
            switch (itemType)
            {
                case "Weapon":
                    return GameItem.ItemCategory.Weapon;
                case "Potion":
                    return GameItem.ItemCategory.Potion;
                case "Armor":
                    return GameItem.ItemCategory.Armor;
                case "Accessory":
                    return GameItem.ItemCategory.Accessory;
                default:
                    return GameItem.ItemCategory.Miscellaneous;
            }
        }
        public static Weapon Weapon(XmlNode node) => new Weapon(node.AttributeAsInt("MinDamage"),
                                                                node.AttributeAsInt("MaxDamage"));
        public static Armor Armor(XmlNode node) => new Armor(node.AttributeAsInt("PlusArmor"));
        public static Potion Potion(XmlNode node) => new Potion(node.AttributeAsInt("HitPointsToHealHP"),
                                                                node.AttributeAsInt("HitPointsToHealMP"));
        public static Accessory Accessory(XmlNode node) => new Accessory(node.AttributeAsInt("PlusHP"),
                                                                         node.AttributeAsInt("PlusMP"));
    }
}
