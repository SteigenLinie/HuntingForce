using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineHF.Model.ItemCategory
{
    public class Armor
    {
        public int PlusArmor { get; set; }
        public Armor(int plusArmor)
        {
            PlusArmor = plusArmor;
        }
    }
}
