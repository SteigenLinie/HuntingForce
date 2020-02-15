using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineHF.Model
{
    public class Monster
    {
        public int ID { get; set; }
        public string ImageName { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int GiveXP { get; set; }
        public int AttackMax { get; set; }
        public int AttackMin { get; set; }
        public Monster(int id, string imageName, int maxHP, int currentHP, int giveXP, int attackMax, int attackMin)
        {
            ID = id;
            ImageName = imageName;
            MaxHP = maxHP;
            CurrentHP = currentHP;
            GiveXP = giveXP;
            AttackMax = attackMax;
            AttackMin = attackMin;
        }
        public Monster()
        {

        }
    }
}
