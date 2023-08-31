using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeonBattle
{
    public enum ItemType { EQUIPMENT, CONSUMPTION }
    public enum ItemTarget { ToCharacter = 0, ToMonster }
    
	public abstract class Item
    {
        public ItemType ItemType { get; }
        public string ItemName { get; }
        public int Gold { get; }
        public string Description { get; }

        public Item(ItemType itemType, string itemName, int gold, string description)
        {
            ItemType = itemType;
            ItemName = itemName;
            Gold = gold;
            Description = description;
        }
    }

    public class Equipment : Item
    {
		// Equipment 고유 필드
		public int Attack { get; }
        public int Defense { get; }
        public bool OnEquipped { get; set; }
        
		public Equipment(string itemName, int gold, string description, int atk, int def) : base(ItemType.EQUIPMENT, itemName, gold, description)
        {
            Attack = atk;
            Defense = def;
            OnEquipped = false;
        }
    }
    public class Consumption : Item
    {
        public ItemTarget Target { get; }

        // Consumption 고유 메서드
        public void ItemFunc(Character? character, Monster? monster)
        {
            if (character != null) ItemToCharacter.Invoke(character);
            if (monster != null) ItemToMonster.Invoke(monster);
        }

        Action<Character>? ItemToCharacter;
		Action<Monster>? ItemToMonster;
        
		public Consumption(string itemName, int gold, string description, ItemTarget target) : base(ItemType.CONSUMPTION, itemName, gold, description)
        {
            Target = target;
        }

        public static Consumption MakeRedPotion()
        {
            Consumption potion = new Consumption("붉은 포션", 20, "HP를 20 회복시킵니다.", ItemTarget.ToCharacter);
            potion.ItemToCharacter += (Character character) => { character.HP += 20; if (character.HP > character.MaxHP) character.HP = character.MaxHP; };
            return potion;
        }

        public static Consumption MakeMpPotion()
        {
            Consumption potion = new Consumption("파란 포션 ", 20, "MP를 20 회복시킵니다.", ItemTarget.ToCharacter);
            potion.ItemToCharacter += (Character character) => { character.MP += 20; if (character.MP > character.MaxMP) character.MP = character.MaxMP; };

            return potion;
        }
    }
}
