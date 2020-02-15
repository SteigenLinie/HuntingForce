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
    public static class ItemFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\GameItems.xml";

        private static readonly List<GameItem> _standardGameItems = new List<GameItem>();

        static ItemFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                LoadItemsFromNodes(data.SelectNodes("/GameItems/Weapons/Weapon"));
                LoadItemsFromNodes(data.SelectNodes("/GameItems/HealingItems/HealingItem"));
                LoadItemsFromNodes(data.SelectNodes("/GameItems/MiscellaneousItems/MiscellaneousItem"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
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

                switch(itemCategory)
                {
                    case GameItem.ItemCategory.Weapon:

                        break;
                    case GameItem.ItemCategory.Potion:

                        break;
                    case GameItem.ItemCategory.Miscellaneous:

                        break;
                }
                GameItem gameItem =
                    new GameItem(itemCategory,
                                 node.AttributeAsInt("ID"),
                                 node.AttributeAsString("Name"),
                                 node.AttributeAsInt("ImageName"),
                                 itemCategory == GameItem.ItemCategory.Weapon);


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
