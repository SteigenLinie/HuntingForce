using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingForce.Character
{
    class MainStats
    {
        public int MaxHP { get; set; } = 100;
        public int CurrentHP { get; set; } = 100;
        public int MaxMP { get; set; } = 100;
        public int CurrentMP { get; set; } = 100;
        public int MaxSP { get; set; } = 100;
        public int CurrentSP { get; set; } = 100;
        public int MaxXP { get; set; } = 20;
        public int CurrentXP { get; set; }
        public int SkillPoint { get; set; }
    }
}
