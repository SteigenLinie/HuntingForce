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
    public class NPCFactory
    {
        private const string GAME_DATA_FILENAME = "./Data/NPC.xml";

        internal static readonly List<NPC> _npc = new List<NPC>();

        public NPCFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                string rootImagePath =
                    data.SelectSingleNode("/NPCs")
                        .AttributeAsString("RootImagePath");

                LoadItemsFromNodes(data.SelectNodes("/NPCs/NPC"), rootImagePath);

            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }
        private static void LoadItemsFromNodes(XmlNodeList nodes, string rootImagePath)
        {
            if (nodes == null)
                return;

            foreach (XmlNode node in nodes)
            {
                NPC.TypeOfNPC typeOfNPC = DetermineItemCategory(node.AttributeAsString("Type"));

                NPC NPC = null;
                Quest quest = null;

                if (node.SelectSingleNode("./Quests/Quest") != null)
                    quest = QuestFactory._allQuests.First(x => x.ID == node.SelectSingleNode("./Quests/Quest").AttributeAsInt("ID"));

                switch (typeOfNPC)
                {
                    case NPC.TypeOfNPC.Quest:
                        NPC = new NPC(node.AttributeAsInt("ID"),
                                      node.AttributeAsString("Name"),
                                      typeOfNPC,
                                      $".{rootImagePath}{node.AttributeAsString("ImageName")}",
                                      node.AttributeAsString("Description"),
                                      quest);
                        break;

                    case NPC.TypeOfNPC.Trader:
                        NPC = new NPC(node.AttributeAsInt("ID"),
                                      node.AttributeAsString("Name"),
                                      typeOfNPC,
                                      $".{rootImagePath}{node.AttributeAsString("ImageName")}",
                                      node.AttributeAsString("Description"),
                                      null);
                        break;
                }



                _npc.Add(NPC);
            }
        }

        private static NPC.TypeOfNPC DetermineItemCategory(string itemType)
        {
            return itemType switch
            {
                "Quest" => NPC.TypeOfNPC.Quest,
                "Trader" => NPC.TypeOfNPC.Trader,
                _ => NPC.TypeOfNPC.Quest
            };
        }
    }
}

