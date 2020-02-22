using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineHF.Model
{
    public class Drop
    {
        public int DropID { get; set; }
        public int Chance { get; set; }
        public Drop(int dropID, int chance)
        {
            DropID = dropID;
            Chance = chance;
        }
    }
}
