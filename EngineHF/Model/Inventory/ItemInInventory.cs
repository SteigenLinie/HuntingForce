using EngineHF.Model.Inventory;
using System.Windows.Controls;

namespace EngineHF.Model
{
    public class ItemInInventory: BaseItem
    {
        public int GridRow { get; set; }
        public int GridColumn { get; set; }
        public bool IsEmpty { get; set; }
        public Border Item { get; set; } 
        public ItemInInventory(Border item, int gridRow, int gridColumn, bool isEmpty = true)
        {
            Item = item;
            GridColumn = gridColumn;
            GridRow = gridRow;
            IsEmpty = isEmpty;
        }
    }
}
