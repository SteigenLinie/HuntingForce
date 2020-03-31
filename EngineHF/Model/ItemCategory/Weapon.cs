using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineHF.Model.ItemCategory
{
    public class Weapon
    {
        public int MinDamage { get; set; }
        public int MinMinDamage { get; set; }
        public int CurrentMinDamage { get; set; }

        public int MaxDamage { get; set; }      
        public int MaxMaxDamage { get; set; }
        public int CurrentMaxDamage { get; set; }
        public Weapon(int minDamage, int maxDamage, int minMinDamage, int maxMaxDamage)
        {
            MinDamage = minDamage;
            MinMinDamage = minMinDamage;
            MaxDamage = maxDamage;
            MaxMaxDamage = maxMaxDamage;
        }
    }
}
