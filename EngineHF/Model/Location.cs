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
        public CurrentPos CurrentCoordinate { get; set; }
        public string ImageName { get; set; }
        public string Description { get; set; }
        public Monster Monster { get; set; }
        public Quest Quest { get; set; } 
        public NPC NPC { get; set; }
        public List<Dialog> Dialogs { get; set; }
        public Location(string name, CurrentPos currentPos, string imageName, string description = null, Monster monster = null, Quest quest = null, NPC nPC = null)
        {
            Name = name;
            CurrentCoordinate = currentPos;
            ImageName = imageName;
            Description = description;
            Monster = monster;
            Quest = quest;
            NPC = nPC;
        }
    }
}
