using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineHF.Model.TypesOfSkills
{
    public class Attack
    {
        public int MinBonusDamage { get; set; }
        public int MaxBonusDamage { get; set; }
        public int CountOfSlash { get; set; }
        public Attack(int minBonusDamage, int maxBonusDamage, int countOfSlash)
        {
            MinBonusDamage = minBonusDamage;
            MaxBonusDamage = maxBonusDamage;
            CountOfSlash = countOfSlash;
        }
    }
}
