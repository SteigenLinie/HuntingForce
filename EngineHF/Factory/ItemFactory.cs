using EngineHF.Model;
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

                LoadItemsFromNodes(data.SelectNodes("/GameItems/Weapons/Weapon"), rootImagePath);
                LoadItemsFromNodes(data.SelectNodes("/GameItems/Potions/Potion"), rootImagePath);
                LoadItemsFromNodes(data.SelectNodes("/GameItems/MiscellaneousItems/MiscellaneousItem"), rootImagePath);
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
                GameItem.ItemCategory itemCategory = DetermineItemCategory(node.Name);
                GameItem gameItem = new GameItem();
                switch(itemCategory)
                {
                    case GameItem.ItemCategory.Weapon:
                        gameItem = new GameItem(itemCategory,
                                                node.AttributeAsInt("ID"),
                                                node.AttributeAsString("Name"),
                                                $".{rootImagePath}{node.AttributeAsString("ImageName")}",
                                                node.AttributeAsString("Description"),
                                                node.AttributeAsInt("MaxDamage"),
                                                node.AttributeAsInt("MinDamage"));
                        break;
                    case GameItem.ItemCategory.Potion:
                        gameItem = new GameItem(itemCategory,
                                                node.AttributeAsInt("ID"),
                                                node.AttributeAsString("Name"),
                                                $".{rootImagePath}{node.AttributeAsString("ImageName")}",
                                                node.AttributeAsString("Description"),
                                                node.AttributeAsInt("HitPointsToHealHP"),
                                                node.AttributeAsInt("HitPointsToHealMP"));
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
                default:
                    return GameItem.ItemCategory.Miscellaneous;
            }
        }
    }
}
