using EngineHF.Model;
using EngineHF.Model.TypesOfSkills;
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
    public class SkillsFactory
    {
        private const string GAME_DATA_FILENAME = "./Data/Skills.xml";

        internal static readonly List<Skills> _standardSkills = new List<Skills>();

        public SkillsFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                string rootImagePath =
                    data.SelectSingleNode("/Skills")
                        .AttributeAsString("RootImagePath");

                LoadItemsFromNodes(data.SelectNodes("/Skills/Skill"), rootImagePath);
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
                Skills.TypeOfSkill type = DetermineTypeOfSkill(node.AttributeAsString("Type"));
                Skills skill = new Skills();
                switch (type)
                {
                    case Skills.TypeOfSkill.Attack:
                        
                        skill = new Skills(type,
                                            node.AttributeAsInt("ID"),
                                            node.AttributeAsInt("GridRow"),
                                            node.AttributeAsInt("GridColumn"),
                                            node.AttributeAsString("Name"),
                                            $".{rootImagePath}{node.AttributeAsString("ImageName")}",
                                            NewAttackSkill(node.SelectSingleNode("./Attack")),
                                            null,
                                            null);
                        break;
                    case Skills.TypeOfSkill.Heal:
                        skill = new Skills(type,
                                            node.AttributeAsInt("ID"),
                                            node.AttributeAsInt("GridRow"),
                                            node.AttributeAsInt("GridColumn"),
                                            node.AttributeAsString("Name"),
                                            $".{rootImagePath}{node.AttributeAsString("ImageName")}");
                        break;
                   
                }
                _standardSkills.Add(skill);
            }
        }
        public static Attack NewAttackSkill(XmlNode node) => new Attack(node.AttributeAsInt("MinBonusDamage"),
                                                                 node.AttributeAsInt("MaxBonusDamage"),
                                                                 node.AttributeAsInt("CountOfSlash"));

        private static Skills.TypeOfSkill DetermineTypeOfSkill(string itemType)
        {
            switch (itemType)
            {
                case "Attack":
                    return Skills.TypeOfSkill.Attack;
                case "Heal":
                    return Skills.TypeOfSkill.Heal;
                default:
                    return Skills.TypeOfSkill.Heal;
            }
        }
    }
}
