using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineHF.Model
{
    public class CurrentPos
    {
        public int CurrentX { get; set; }
        public int CurrentY { get; set; }
        public CurrentPos(int currentX, int currentY)
        {
            CurrentX = currentX;
            CurrentY = currentY;
        }
    }
}
