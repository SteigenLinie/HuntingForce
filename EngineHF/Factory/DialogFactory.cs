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
    class DialogFactory
    {
        private const string GAME_DATA_FILENAME = "./Data/Dialogs.xml";
        internal static readonly Dictionary<CurrentPos, List<Dialog>> dialogDict = new Dictionary<CurrentPos, List<Dialog>>();

        public DialogFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                LoadItemsFromNodes(data.SelectNodes("/AllDialogs/Dialogs"));
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
                Dialog dialog;
                var currentPos = new CurrentPos(node.AttributeAsInt("X"),
                                                node.AttributeAsInt("Y"));
                List<Dialog> dialogList = new List<Dialog>();
                foreach(XmlNode xmlNode in node.SelectNodes("Dialog"))
                {
                    dialog = new Dialog(xmlNode.AttributeAsInt("ID"),
                                        xmlNode.AttributeAsString("Name"),
                                        xmlNode.AttributeAsString("Text"));
                    //TODO
                    if (xmlNode.Attributes["QuestID"] != null)
                        dialog.QuestID = xmlNode.AttributeAsInt("QuestID");

                    dialogList.Add(dialog);
                }
                dialogDict.Add(currentPos, dialogList);
            }
        }
    }
}
