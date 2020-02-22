using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineHF.Model.ItemCategory
{
    public class Accessory
    {
        public int PlusHP { get; set; }
        public int PlusMP { get; set; }
        public Accessory(int plusHP, int plusMP)
        {
            PlusHP = plusHP;
            PlusMP = plusMP;
        }
    }
}
