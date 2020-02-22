using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineHF.Model
{
    public class ItemInInventory
    {
        public string Name { get; set; }
        public int GridColumn { get; set; }
        public int GridRow { get; set; }
        public ItemInInventory(string name, int gridColumn, int gridRow)
        {
            Name = name;
            GridColumn = gridColumn;
            GridRow = gridRow;
        }
    }
}
