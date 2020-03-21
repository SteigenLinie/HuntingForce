using EngineHF.Model.TypesOfSkills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineHF.Model
{
    public class Skills
    {
        public enum TypeOfSkill
        {
            Attack,
            Heal,
            Passiv
        }
        public TypeOfSkill Type { get; set; }
        public int ID { get; set; }
        public int GridRow { get; set; }
        public int GridColumn { get; set; }
        public string Name { get; set; }
        public int CostSkillPoint { get; set; }
        public string ImageName { get; set; }
        public Attack Attack { get; set; }
        public Heal Heal { get; set; }
        public Passiv Passiv { get; set; }

        public Skills(TypeOfSkill type, int id, int gridRow, int gridColumn, string name, int costSkillPoint, string imageName, Attack attack = null, Heal heal = null, Passiv passiv = null)
        {
            Type = type;
            ID = id;
            GridRow = gridRow;
            GridColumn = gridColumn;
            Name = name;
            CostSkillPoint = costSkillPoint;
            ImageName = imageName;
            Attack = attack;
            Heal = heal;
            Passiv = passiv;
        }
        public Skills(){ }
    }
}
