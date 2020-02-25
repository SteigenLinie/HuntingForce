using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EngineHF.Model
{
    public class ItemInInventory
    {
        public string Name { get; set; }
        public int GridColumn { get; set; }
        public int GridRow { get; set; }
        public Border Item { get; set; } 
        public ItemInInventory(Border item, string name, int gridColumn, int gridRow)
        {
            Item = item;
            Name = name;
            GridColumn = gridColumn;
            GridRow = gridRow;
        }
    }
}
