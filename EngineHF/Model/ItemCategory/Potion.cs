using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineHF.Model.ItemCategory
{
    public class Potion
    {
        public int HitPointsToHealHP { get; set; }
        public int HitPointsToHealMP { get; set; }
        public Potion (int hitPointToHealHP, int hitPointToHealMP)
        {
            HitPointsToHealHP = hitPointToHealHP;
            HitPointsToHealMP = hitPointToHealMP;
        }
    }
}
