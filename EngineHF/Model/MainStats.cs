using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineHF.Model
{
    public class MainStats
    {
        public string Name { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; } 
        public int MaxMP { get; set; } 
        public int CurrentMP { get; set; } 
        public int MaxSP { get; set; } 
        public int CurrentSP { get; set; } 
        public int MaxXP { get; set; } 
        public int CurrentLevel { get; set; }
        public int CurrentXP { get; set; }
        public int SkillPoint { get; set; }
        public int TempSkillPoint { get; set; }
        public int CurrentGold { get; set; }
        public GameItem CurrentWeapon { get; set; }
        public GameItem CurrentArmor { get; set; }
        public GameItem CurrentAccessory { get; set; }
        public List<Quest> QuestOnPlayer { get; set; } = new List<Quest>();
        public MainStats(string name, int maxHP, int currentHP, int maxMP, int currentMP, int maxSP,
            int currentSP, int maxXP, int currentLevel, int currentXp, int skillPoint, int tempSkillPoint,
            int currentGold, GameItem currentWeapon, GameItem currentArmor = null, GameItem currentAccessory = null)
        {
            Name = name;
            MaxHP = maxHP;
            CurrentHP = currentHP;
            MaxMP = maxMP;
            CurrentMP = currentMP;
            MaxSP = maxSP;
            CurrentSP = currentSP;
            MaxXP = maxXP;
            CurrentLevel = currentLevel;
            CurrentXP = currentXp;
            SkillPoint = skillPoint;
            TempSkillPoint = tempSkillPoint;
            CurrentGold = currentGold;
            CurrentWeapon = currentWeapon;
            CurrentArmor = currentArmor;
            CurrentAccessory = currentAccessory;
        }
    }
}
