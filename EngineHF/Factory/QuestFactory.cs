using EngineHF.Model;
using EngineHF.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;

namespace EngineHF.Factory
{
    public class QuestFactory
    {
        private const string GAME_DATA_FILENAME = "./Data/Quests.xml";

        internal static readonly List<Quest> _allQuests= new List<Quest>();

        public QuestFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                LoadItemsFromNodes(data.SelectNodes("/Quests/Quest"));
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
                var questItem = new Quest(node.AttributeAsInt("ID"),
                                          node.AttributeAsString("Name"),
                                          node.AttributeAsString("Description"),
                                          node.AttributeAsString("Mission"),
                                          node.AttributeAsString("IsDone"));

                _allQuests.Add(questItem);
            }
        }
    }
}
