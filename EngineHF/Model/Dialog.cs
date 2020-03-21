using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineHF.Model
{
    public class Dialog
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public int QuestID { get; set; }
        public bool WasRead { get; set; } = false;
        public Dialog(int id, string name, string text, int questID = 0)
        {
            ID = id;
            Name = name;
            Text = text;
            QuestID = questID;
        }
    }
}
