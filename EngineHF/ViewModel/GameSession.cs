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
        private MonsterFactory monsterFactory;
        private ItemFactory itemFactory;
        private SkillsFactory skillsFactory;
        public List<Skills> _standartSkills = SkillsFactory._standardSkills;
        public List<GameItem> _standardGameItems = ItemFactory._standardGameItems;
        public World CurrentWorld { get; }

        public bool HasLocationToNorth =>
            CurrentWorld.LocationAt(currentPos.CurrentX, currentPos.CurrentY + 1) != null;

        public bool HasLocationToEast =>
            CurrentWorld.LocationAt(currentPos.CurrentX + 1, currentPos.CurrentY) != null;

        public bool HasLocationToSouth =>
            CurrentWorld.LocationAt(currentPos.CurrentX , currentPos.CurrentY - 1) != null;

        public bool HasLocationToWest =>
            CurrentWorld.LocationAt(currentPos.CurrentX - 1, currentPos.CurrentY ) != null;

        public GameSession()
        {
            monsterFactory = new MonsterFactory();
            itemFactory = new ItemFactory();
            //TODO parce xml to mainstats
            mainStats = new MainStats("3,14door", 100, 100, 100, 100, 100, 100, 20, 0, 0, 1, 1, 0, _standardGameItems[0], _standardGameItems[1]);
            skillsFactory = new SkillsFactory();
            CurrentWorld = WorldFactory.CreateWorld(); 
            currentPos = CurrentWorld.LocationAt(0,0);
        }
    }
}
