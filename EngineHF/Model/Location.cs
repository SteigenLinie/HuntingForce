using EngineHF.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineHF.Model
{
    public class Location
    {
        public string Name { get; set; }
        public int CurrentX { get; set; }
        public int CurrentY { get; set; }
        public string ImageName { get; set; }
        public string Description { get; set; }
        public Monster Monster { get; set; }
        public Location(string name, int currentX, int currentY, string imageName, string description = null, Monster monster = null)
        {
            Name = name;
            CurrentX = currentX;
            CurrentY = currentY;
            ImageName = imageName;
            Description = description;
            Monster = monster;
        }
    }
}
