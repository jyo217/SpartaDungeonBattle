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
            ItemList.Add(new Equipment("무쇠갑옷", 100, "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 5));
            ItemList.Add(new Equipment("낡은 검", 100, "쉽게 볼 수 있는 낡은 검 입니다.", 2, 0));
            ItemList.Add(Consumption.MakePotion());
        }
        public Inventory(List<Item> itemList) { ItemList = itemList; }

        public Item this[int index]
        {
            get { return ItemList[index]; }
            set { ItemList[index] = value; }
        }

        public int Count { get { return ItemList.Count; } }

        /// <summary>
        /// 장비 장착, 탈착 기능
        /// </summary>
        public void ManageEquipment(int index)
        {
            Character character = Character.CurrentCharacter;
            //이걸 장비 타입으로 캐스팅 시도
            Equipment equipment = ItemList[index] as Equipment;

            //캐스팅 성공 시 장비 장착/탈착 수행
            if (equipment != null)
            {
                //장착 중인 아이템이라면 장착 상태 변경 및 장착 아이템 리스트에서 제거
                if(equipment.OnEquipped)
                {
                    character.EquipItem(equipment);
                    equipment.OnEquipped = false;
                }
                //미장착 아이템이라면 장착 상태 변경 및 장착 아이템 리스트에 추가
                else
                {
                    character.EquipItem(equipment);
                    equipment.OnEquipped = true;
                }
            }
            else { Console.WriteLine("Item => Equipment 타입 캐스팅 실패!!!"); }
        }
    }
}
