using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace EngineHF.Model
{
    public class Quest: BindableBase
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Mission { get; set; }
        public bool IsDone { get; set; } 

        private string _progress;
        public string Progress
        {
            get => _progress;
            set
            {
                if (value == "5/5")
                {
                    SetProperty(ref _progress, "done");
                    IsDone = true;
                }
                else
                    SetProperty(ref _progress, value);
            }
        }
        public string CurrentState { get; set; }
        public Quest(int id, string name, string description, string mission, string progress, bool isDone = false)
        {
            ID = id;
            Name = name;
            Description = description;
            Mission = mission;
            Progress = progress;
            IsDone = isDone;
        }
    }
}
