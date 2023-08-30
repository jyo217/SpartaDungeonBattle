using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeonBattle
{
    public class Character
    {
        public int Level { get; private set; }
        public string Name { get; set; }
        public ClassType Class { get; private set; }
        public List<Equipment> ItemOnEquipped { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int HP { get; private set; }
        public int MP { get; private set; }
        public int Gold { get; private set; }
        public int Exp { get; private set; }
        public Inventory Inventory { get; private set; }
        public List<Skill> Skills { get; private set; }

        /// <summary>
        /// 필요에 따라 편집
        /// </summary>
        public Character() { }

        public void UseSkill(Skill skill) { }

        public void UseItem(Consumption consumption) { }

        public void EquipItem(Equipment equipment) { }

        public void IncreaseExp(int exp) { }

        public event Action<Monster> OnHit;

        public event Action OnDead;

        public event Action<Monster> OnAttack;

        public string ClassToString()
        {
            switch (Class) {
                case ClassType.WARRIOR:
                    return "전사";
                case ClassType.ARCHER:
                    return "궁수";
                case ClassType.MAGICIAN:
                    return "마법사";
                case ClassType.CLERIC:
                    return "클레릭";
                default:
                    return "";
            }
        }

        public bool SetNickname(string nickname)
        {
            if (nickname != "") return false;
            Name = nickname;
            return true;
        }
    }

    public enum ClassType
    {
        WARRIOR,
        ARCHER,
        MAGICIAN,
        CLERIC
    }
}

