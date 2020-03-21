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
    internal static class WorldFactory
    {
        private const string GAME_DATA_FILENAME = @"./Data/Locations.xml";

        internal static World CreateWorld()
        {
            World world = new World();
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                string rootImagePath =
                    data.SelectSingleNode("/Locations")
                        .AttributeAsString("RootImagePath");

                LoadLocationsFromNodes(world,
                                       rootImagePath,
                                       data.SelectNodes("/Locations/Location"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }

            return world;
        }

        private static void LoadLocationsFromNodes(World world, string rootImagePath, XmlNodeList nodes)
        {
            if (nodes == null)
            {
                return;
            }

            foreach (XmlNode node in nodes)
            {
                Monster monster = null;
                Quest quest = null;
                NPC npc = null;
                if (node.SelectSingleNode("./Monsters/Monster") != null)
                    monster = MonsterFactory._baseMonsters.First(x => x.ID == node.SelectSingleNode("./Monsters/Monster").AttributeAsInt("ID"));
                if (node.SelectSingleNode("./Quests/Quest") != null)
                    quest = QuestFactory._allQuests.First(x => x.ID == node.SelectSingleNode("./Quests/Quest").AttributeAsInt("ID"));
                if (node.SelectSingleNode("./NPC") != null)
                    npc = NPCFactory._npc.First(x => x.NpcID == node.SelectSingleNode("./NPC").AttributeAsInt("ID"));
                Location location =
                    new Location(node.AttributeAsString("Name"),
                                 new CurrentPos(node.AttributeAsInt("X"),
                                                node.AttributeAsInt("Y")),
                                 $".{rootImagePath}{node.AttributeAsString("ImageName")}",
                                 node.SelectSingleNode("./Description")?.InnerText ?? "",
                                 monster,
                                 quest,
                                 npc);

                //1
                location.Dialogs = DialogFactory.dialogDict.FirstOrDefault(x => x.Key.CurrentX == location.CurrentCoordinate.CurrentX &&
                                                                                x.Key.CurrentY == location.CurrentCoordinate.CurrentY).Value;

                world.AddLocation(location);
            }
        }
    }
}
