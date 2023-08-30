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
        public Inventory()
        {
            ItemList = new List<Item>();
            ItemList.Add(new Equipment(ItemType.Armor, "무쇠갑옷", 100, "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 5));
            ItemList.Add(new Equipment(ItemType.Weapon, "낡은 검", 100, "쉽게 볼 수 있는 낡은 검 입니다.", 2, 0));
            ItemList.Add(Consumption.MakePotion());
        }
        public Inventory(List<Item> itemList) { ItemList = itemList; }

        public Item this[int index]
        {
            get { return ItemList[index]; }
            set { ItemList[index] = value; }
        }

        public int Count { get { return ItemList.Count; } }
    }
}
