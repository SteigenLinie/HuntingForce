using EngineHF.Factory;
using EngineHF.Model;
using EngineHF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineHF.Model.ItemCategory;

namespace EngineHF
{
    public class GameSession
    {

        public MainStats mainStats;
        public Location currentPos;
        public List<Skills> _standartSkills = SkillsFactory._standardSkills;
        public List<GameItem> _standardGameItems = ItemFactory._standardGameItems;
        public List<Quest> _allQuest = QuestFactory._allQuests;
        public Dictionary<CurrentPos, List<Dialog>> _dialogDict = DialogFactory.dialogDict;


        public List<Quest> _questOnPlayer = new List<Quest>();
        public List<Skills> _skillsOnPlayer = new List<Skills>();
        public List<GameItem> _itemInInventory = new List<GameItem>();

        public World CurrentWorld { get; }

        public bool HasLocationToNorth =>
            CurrentWorld.LocationAt(currentPos.CurrentCoordinate.CurrentX, currentPos.CurrentCoordinate.CurrentY + 1) != null;

        public bool HasLocationToEast =>
            CurrentWorld.LocationAt(currentPos.CurrentCoordinate.CurrentX + 1, currentPos.CurrentCoordinate.CurrentY) != null;

        public bool HasLocationToSouth =>
            CurrentWorld.LocationAt(currentPos.CurrentCoordinate.CurrentX , currentPos.CurrentCoordinate.CurrentY - 1) != null;

        public bool HasLocationToWest =>
            CurrentWorld.LocationAt(currentPos.CurrentCoordinate.CurrentX - 1, currentPos.CurrentCoordinate.CurrentY ) != null;

        public GameSession()
        {
            new MonsterFactory();
            new ItemFactory();
            new QuestFactory();
            new NPCFactory();
            new DialogFactory();
            new SkillsFactory();
            CurrentWorld = WorldFactory.CreateWorld(); 
            currentPos = CurrentWorld.LocationAt(0,0);
        }
    }
}
