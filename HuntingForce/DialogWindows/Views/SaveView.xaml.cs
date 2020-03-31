using EngineHF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace HuntingForce.DialogWindows.Views
{
    /// <summary>
    /// Логика взаимодействия для SaveView.xaml
    /// </summary>
    public partial class SaveView : Window
    {
        private GameSession _gameSession;
        public SaveView(GameSession gameSession)
        {
            InitializeComponent();
            _gameSession = gameSession;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var currDir = Directory.GetCurrentDirectory();
            if (!Directory.Exists(currDir + "/Saves"))
                Directory.CreateDirectory(currDir + "/Saves");
            XDocument xdoc = new XDocument();
            var gameSession = new XElement("GameSession");

            #region MainStats
            var mainStats = new XElement("MainStats");
            mainStats.Add(new XAttribute("CurrentX", _gameSession.currentPos.CurrentCoordinate.CurrentX));
            mainStats.Add(new XAttribute("CurrentY", _gameSession.currentPos.CurrentCoordinate.CurrentY));
            mainStats.Add(new XAttribute("CurrentGold", _gameSession.mainStats.CurrentGold));

            var HP = new XElement("HP");
            HP.Add(new XAttribute("MaxHP",_gameSession.mainStats.MaxHP));
            HP.Add(new XAttribute("CurrentHP", _gameSession.mainStats.CurrentHP));
            mainStats.Add(HP);

            var MP = new XElement("MP");
            MP.Add(new XAttribute("MaxMP", _gameSession.mainStats.MaxMP));
            MP.Add(new XAttribute("CurrentMP", _gameSession.mainStats.CurrentMP));
            mainStats.Add(MP);

            var SP = new XElement("SP");
            SP.Add(new XAttribute("MaxSP", _gameSession.mainStats.MaxSP));
            SP.Add(new XAttribute("CurrentSP", _gameSession.mainStats.CurrentSP));
            mainStats.Add(SP);

            var XP = new XElement("XP");
            XP.Add(new XAttribute("MaxXP", _gameSession.mainStats.MaxXP));
            XP.Add(new XAttribute("CurrentXP", _gameSession.mainStats.CurrentXP));
            XP.Add(new XAttribute("CurrentLevel", _gameSession.mainStats.CurrentLevel));
            XP.Add(new XAttribute("SkillPoint", _gameSession.mainStats.SkillPoint));
            XP.Add(new XAttribute("TempSkillPoint", _gameSession.mainStats.TempSkillPoint));
            mainStats.Add(XP);

            gameSession.Add(mainStats);

            if (_gameSession.mainStats.CurrentWeapon != null)
            {
                var Weapon = new XElement("Weapon");
                Weapon.Add(new XAttribute("ID", _gameSession.mainStats.CurrentWeapon.ItemID));
                gameSession.Add(Weapon);
            }

            if (_gameSession.mainStats.CurrentArmor != null)
            {
                var Armor = new XElement("Armor");
                Armor.Add(new XAttribute("ID", _gameSession.mainStats.CurrentArmor.ItemID));
                gameSession.Add(Armor);
            }

            if (_gameSession.mainStats.CurrentAccessory != null)
            {
                var Accessory = new XElement("Accessory");
                Accessory.Add(new XAttribute("ID", _gameSession.mainStats.CurrentAccessory.ItemID));
                gameSession.Add(Accessory);
            }
            #endregion

            #region World
            if (_gameSession._questOnPlayer.Count > 0)
            {
                var Quests = new XElement("Quests");
                foreach(var quest in _gameSession._questOnPlayer)
                {
                    var Quest = new XElement("Quest");
                    Quest.Add(new XAttribute("ID", quest.ID));
                    Quests.Add(Quest);
                }
                gameSession.Add(Quests);
            }

            if (_gameSession._skillsOnPlayer.Count > 0)
            {
                var Skills = new XElement("Skills");
                foreach (var skill in _gameSession._skillsOnPlayer)
                {
                    var Skill = new XElement("Skill");
                    Skill.Add(new XAttribute("ID", skill.ID));
                    Skills.Add(Skill);
                }
                gameSession.Add(Skills);
            }

            if (_gameSession._itemInInventory.Count > 0)
            {
                var Items = new XElement("Items");
                foreach (var item in _gameSession._itemInInventory)
                {
                    var Item = new XElement("Item");
                    Item.Add(new XAttribute("ID", item.ItemID));
                    Item.Add(new XAttribute("Count", item.Count));
                    Items.Add(Item);
                }

                gameSession.Add(Items);
            }

            var DialogMapPos = new XElement("DialogMapPos");
            foreach (var elm in _gameSession._dialogDict.Keys)
            {
                var Dialogs = new XElement("Dialogs");
                Dialogs.Add(new XAttribute("X", elm.CurrentX));
                Dialogs.Add(new XAttribute("Y", elm.CurrentY));
                foreach(var dialog in _gameSession.CurrentWorld.LocationAt(elm.CurrentX, elm.CurrentY).Dialogs)
                {
                    var Dialog = new XElement("Dialog");
                    Dialog.Add(new XAttribute("ID", dialog.ID));
                    Dialog.Add(new XAttribute("WasRead", dialog.WasRead));
                    Dialogs.Add(Dialog);
                }
                DialogMapPos.Add(Dialogs);
            }
            gameSession.Add(DialogMapPos);
            xdoc.Add(gameSession);
            #endregion
            xdoc.Save(currDir + "/Saves/save.xml");

        }
    }
}
