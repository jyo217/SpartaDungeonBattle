using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeonBattle
{
    [Serializable]
    public class Inventory
    {
        public List<Item> ItemList { get; private set; }

        /// <summary>
        /// 필요에 따라 편집
        /// </summary>
        public Inventory(List<Item> itemList) { ItemList = ItemList; }
    }
}
