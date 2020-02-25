using System.Windows.Controls;

namespace EngineHF.Model
{
    public class ItemInInventory
    {
        public string Name { get; set; }
        public int GridColumn { get; set; }
        public int GridRow { get; set; }
        public bool IsEmpty { get; set; }
        public Border Item { get; set; } 
        public ItemInInventory(Border item, string name, int gridRow, int gridColumn, bool isEmpty = true)
        {
            Item = item;
            Name = name;
            GridColumn = gridColumn;
            GridRow = gridRow;
            IsEmpty = isEmpty;
        }
    }
}
