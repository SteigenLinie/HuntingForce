using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineHF.Model
{
    public class Logging
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public Logging(string type, string message)
        {
            Type = type;
            Message = message;
        }
    }
}
