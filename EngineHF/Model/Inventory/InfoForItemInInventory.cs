using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineHF.Model.Inventory
{
    public class InfoForItemInInventory
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public int? Count { get; set; }
        public int ID { get; set; }
        public ItemInInventory ItemInInventory { get; set; }
        public InfoForItemInInventory(string name, string category, int? count, int id, ItemInInventory itemInInventory)
        {
            Name = name;
            Category = category;
            Count = count;
            ID = id;
            ItemInInventory = itemInInventory;
        }
    }
}
