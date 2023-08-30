using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeonBattle
{
    public enum ItemType { Weapon = 0, Armor, Consumption }
​
	public abstract class Item
    {
        public ItemType _ItemType { get; }
        public string _ItemName { get; }
        public int _Gold { get; }
        public string _Description { get; }
    }

    public class Equipment : Item
    {
​
		// interface 공통 필드
		public ItemType _ItemType { get; }
        public string _ItemName { get; }
        public int _Gold { get; }
        public string _Description { get; }
​
		// Equipment 고유 필드
		public int Attack { get; }
        public int Defense { get; }
​
		public Equipment(ItemType itemType, string itemName, int gold, string description, int atk, int def)
        {
            _ItemType = itemType;
            _ItemName = itemName;
            _Gold = gold;
            _Description = description;
            Attack = atk;
            Defense = def;
        }
    }
    public class Consumption : Item
    {
​
		// interface 공통 필드
		public ItemType _ItemType { get; }
        public string _ItemName { get; }
        public int _Gold { get; }
        public string _Description { get; }
​
		// Consumption 고유 메서드
		public void ItemFunc()
        {
        }
​
		public void itemfunc(Monster monster)
        {
        }
​
		public void itemfunc(Character character)
        {
        }
​
		public Consumption(string itemName, int gold, string description)
        {
            _ItemType = ItemType.Consumption;
            _ItemName = itemName;
            _Gold = gold;
            _Description = description;
        }
    }
}
