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
        public int MaxDamage { get; set; }
        public Weapon(int minDamage, int maxDamage)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }
    }
}
