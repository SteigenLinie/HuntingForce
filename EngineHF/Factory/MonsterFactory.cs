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
                                $".{rootImagePath}{node.AttributeAsString("ImageName")}",
                                node.AttributeAsInt("MaxHP"),
                                node.AttributeAsInt("CurrentHP"),
                                node.AttributeAsInt("GiveXP"),
                                node.AttributeAsInt("AttackMax"),
                                node.AttributeAsInt("AttackMin"));

                _baseMonsters.Add(monster);
            }
        }
    }
}
