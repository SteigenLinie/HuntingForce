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
        public string Name { get; set; }
        public string ImageName { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int Level { get; set; }
        public int Gold { get; set; }
        public int AttackMax { get; set; }
        public int AttackMin { get; set; }
        public List<Drop> DropList { get; set; }
        public List<int> QuestProgress { get; set; }
        public Monster(int id, string name, string imageName, int maxHP, int currentHP, int level, int gold,
            int attackMax, int attackMin, List<Drop> dropList = null, List<int> questProgress = null)
        {
            ID = id;
            Name = name;
            ImageName = imageName;
            MaxHP = maxHP;
            CurrentHP = currentHP;
            Level = level;
            Gold = gold;
            AttackMax = attackMax;
            AttackMin = attackMin;
            DropList = dropList;
            QuestProgress = questProgress;
        }
        public Monster()
        {

        }
    }
}
