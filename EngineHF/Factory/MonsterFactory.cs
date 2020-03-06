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
    public class MonsterFactory
    {
        private const string GAME_DATA_FILENAME = "./Data/Monsters.xml";
        internal static readonly List<Monster> _baseMonsters = new List<Monster>();

        public MonsterFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                string rootImagePath =
                    data.SelectSingleNode("/Monsters")
                        .AttributeAsString("RootImagePath");

                LoadMonstersFromNodes(data.SelectNodes("/Monsters/Monster"), rootImagePath);
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }

        private static void LoadMonstersFromNodes(XmlNodeList nodes, string rootImagePath)
        {
            if (nodes == null)
            {
                return;
            }

            foreach (XmlNode node in nodes)
            {
                Monster monster =
                    new Monster(node.AttributeAsInt("ID"),
                                node.AttributeAsString("Name"),
                                $".{rootImagePath}{node.AttributeAsString("ImageName")}",
                                node.AttributeAsInt("MaxHP"),
                                node.AttributeAsInt("MaxHP"),
                                node.AttributeAsInt("Level"),
                                node.SelectSingleNode("./Drops").AttributeAsInt("Gold"),
                                node.AttributeAsInt("AttackMax"),
                                node.AttributeAsInt("AttackMin"),
                                Drops(node.SelectNodes("./Drops/Drop")),
                                Quests(node.SelectNodes("./Quests/Quest")));

                _baseMonsters.Add(monster);
            }
        }
        private static List<Drop> Drops(XmlNodeList nodes)
        {
            List<Drop> drop = new List<Drop>();
            foreach(XmlNode node in nodes)
                drop.Add(new Drop(node.AttributeAsInt("ID"),
                                  node.AttributeAsInt("Chance")));
            return drop;
        }
        private static List<int> Quests(XmlNodeList nodes)
        {
            List<int> quests = new List<int>();
            foreach (XmlNode node in nodes)
                quests.Add(node.AttributeAsInt("ID"));
            return quests;
        }
    }
}
