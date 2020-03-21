using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineHF.Model
{
    public class NPC
    {
        public enum TypeOfNPC
        {
            Trader,
            Quest,
            Story
        }
        public int NpcID { get; set; }
        public string Name { get; set; }
        public TypeOfNPC Type { get; set; }  
        public string ImageName { get; set; }
        public string Description { get; set; }
        public Quest Quest { get; set; }
        public string Story { get; set; }
        public NPC(int npcID, string name, TypeOfNPC typeOfNPC, string imageName, string description, Quest quest = null, string story = null)
        {
            NpcID = npcID;
            Name = name;
            Type = typeOfNPC;
            ImageName = imageName;
            Description = description;
            Quest = quest;
            Story = story;
        }
    }
}
