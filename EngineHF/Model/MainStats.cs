using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineHF.Model
{
    public class MainStats
    {
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; } 
        public int MaxMP { get; set; } 
        public int CurrentMP { get; set; } 
        public int MaxSP { get; set; } 
        public int CurrentSP { get; set; } 
        public int MaxXP { get; set; } 
        public int CurrentXP { get; set; }
        public int SkillPoint { get; set; }
        public GameItem CurrentWeapon { get; set; }
        public MainStats(int maxHP, int currentHP, int maxMP, int currentMP, int maxSP,
            int currentSP, int maxXP, int currentXp, int skillPoint)
        {
            MaxHP = maxHP;
            CurrentHP = currentHP;
            MaxMP = maxMP;
            CurrentMP = currentMP;
            MaxSP = maxSP;
            CurrentSP = currentSP;
            MaxXP = maxXP;
            CurrentXP = currentXp;
            SkillPoint = skillPoint;
        }
    }
}
