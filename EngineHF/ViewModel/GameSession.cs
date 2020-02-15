using EngineHF.Factory;
using EngineHF.Model;
using EngineHF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EngineHF
{
    public class GameSession
    {

        public MainStats mainStats;
        public Location currentPos;
        public MonsterFactory monsterFactory;

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
            mainStats = new MainStats(100, 100, 100, 100, 100, 100, 20, 0, 0);
            monsterFactory = new MonsterFactory();
            CurrentWorld = WorldFactory.CreateWorld(); 
            currentPos = CurrentWorld.LocationAt(0,0);
        }
    }
}
