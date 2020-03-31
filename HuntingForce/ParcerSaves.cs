using EngineHF;
using EngineHF.Model;
using EngineHF.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HuntingForce
{
    public class ParcerSaves
    {
        private GameSession _gameSession;
        private MainWindow _mainWindow;
        private MainWindowViewModel _mainWindowViewModel;
        private const string GAME_DATA_FILENAME = "./Saves/save.xml";
        public ParcerSaves(GameSession gameSession, MainWindow mainWindow, MainWindowViewModel mainWindowViewModel)
        {
            _gameSession = gameSession;
            _mainWindow = mainWindow;
            _mainWindowViewModel = mainWindowViewModel;
            GameSessionParce();
        }
        public void GameSessionParce()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));
                MainStatsParce(data.SelectSingleNode("/GameSession/MainStats"));

                WeaponParce(data.SelectSingleNode("/GameSession/Weapon"));
                ArmorParce(data.SelectSingleNode("/GameSession/Armor"));
                AccessoryParce(data.SelectSingleNode("/GameSession/Accessory"));

                QuestParce(data.SelectSingleNode("/GameSession/Quests"));
                ItemsParce(data.SelectSingleNode("/GameSession/Items"));
                SkillsParce(data.SelectSingleNode("/GameSession/Skills"));
                DialogParce(data.SelectSingleNode("/GameSession/DialogMapPos"));
            }
        }
        public void MainStatsParce(XmlNode xmlNode)
        {
            var x = xmlNode.AttributeAsInt("CurrentX");
            var y = xmlNode.AttributeAsInt("CurrentY");
            _mainWindowViewModel.Move(x, y);
            var HP = xmlNode.SelectSingleNode("HP");
            var MP = xmlNode.SelectSingleNode("MP");

            var SP = xmlNode.SelectSingleNode("SP");
            var XP = xmlNode.SelectSingleNode("XP");

            _gameSession.mainStats = new MainStats("gay",
                                                   HP.AttributeAsInt("MaxHP"),
                                                   HP.AttributeAsInt("CurrentHP"),
                                                   MP.AttributeAsInt("MaxMP"),
                                                   MP.AttributeAsInt("CurrentMP"),
                                                   SP.AttributeAsInt("MaxSP"),
                                                   SP.AttributeAsInt("CurrentSP"),
                                                   XP.AttributeAsInt("MaxXP"),
                                                   XP.AttributeAsInt("CurrentLevel"),
                                                   XP.AttributeAsInt("CurrentXP"),
                                                   XP.AttributeAsInt("SkillPoint"),
                                                   XP.AttributeAsInt("TempSkillPoint"),
                                                   xmlNode.AttributeAsInt("CurrentGold"));
        }
        public void WeaponParce(XmlNode xmlNode)
        { 
            if(xmlNode != null)
                _gameSession.mainStats.CurrentWeapon = _gameSession._standardGameItems.FirstOrDefault(x => x.ItemID == xmlNode.AttributeAsInt("ID"));
        }
        public void ArmorParce(XmlNode xmlNode)
        {
            if (xmlNode != null)
                _gameSession.mainStats.CurrentArmor = _gameSession._standardGameItems.FirstOrDefault(x => x.ItemID == xmlNode.AttributeAsInt("ID"));
        }
        public void AccessoryParce(XmlNode xmlNode)
        {
            if (xmlNode != null)
                _gameSession.mainStats.CurrentAccessory = _gameSession._standardGameItems.FirstOrDefault(x => x.ItemID == xmlNode.AttributeAsInt("ID")); 
        }

        public void QuestParce(XmlNode xmlNode)
        {
            if (xmlNode != null)
            {
                foreach (XmlNode quest in xmlNode.ChildNodes)
                {
                    _mainWindow.AddNewQuest(_gameSession._allQuest.FirstOrDefault(x => x.ID == quest.AttributeAsInt("ID")));
                }
            }
        }

        public void SkillsParce(XmlNode xmlNode)
        {

        }

        public void ItemsParce(XmlNode xmlNode)
        {
            if (xmlNode != null)
            {
                foreach (XmlNode item in xmlNode.ChildNodes)
                {
                    _mainWindow.AddNewItemInInventory(item.AttributeAsInt("ID"));
                }
            }
        }

        public void DialogParce(XmlNode xmlNode)
        {
            if (xmlNode != null)
            {
                foreach (XmlNode dialogs in xmlNode.ChildNodes)
                {
                    var location = _gameSession.CurrentWorld.LocationAt(dialogs.AttributeAsInt("X"), dialogs.AttributeAsInt("Y"));
                    foreach (XmlNode dialog in dialogs.ChildNodes)
                    {
                        location.Dialogs.FirstOrDefault(x => x.ID == dialog.AttributeAsInt("ID")).WasRead = dialog.AttributeAsBool("WasRead");
                    }

                }
            }
        }
    }
}
