using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeonBattle
{
    [Serializable]
    public class Character
    {
        public int Level { get; private set; }
        public string Name { get; set; }
        public ClassType Class { get; private set; }
        public List<Equipment> ItemOnEquipped { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int HP { get; set; }
        public int MaxHP { get; private set; }
        public int MP { get; set; }
        public int MaxMP { get; private set; }
        public int Gold { get; private set; }
        public int Exp { get; private set; }
        public Inventory Inventory { get; private set; }
        public List<Skill> Skills { get; private set; }

        /// <summary>
        /// 필요에 따라 편집
        /// </summary>
        public Character(string name, ClassType classType, int gold, int exp) 
        {
            this.Name = name;
            Class = classType;
            SetClass(classType);
            this.Gold = gold;
            this.Exp = exp;
        }

        public void UseSkill(Skill skill) { }

        public void UseItem(Consumption consumption) { }

        public void EquipItem(Equipment equipment) { }

        /// <summary>
        /// 경험치 증가 및 레벨업 메소드.  
        /// 레벨업 시 공격력 1, 방어력 1 상승
        /// </summary>
        /// <param name="exp">죽인 적 레벨 1 당 1 exp</param>
        public void IncreaseExp(int exp) 
        {
            int[] max_exp = new int[4] {10, 35, 60, 100};

            this.Exp += exp;

            //5레벨부터는 레벨업 없이 경험치만 쌓임.
            if(this.Level < 5)
            {
                //현재 레벨의 경험치 상한에 도달했다면 레벨업
                if (this.Exp >= max_exp[this.Level - 1]) 
                {
                    this.Exp -= max_exp[this.Level - 1];
                    this.Level++;

                    this.Attack += 1;
                    this.Defense += 1;
                    this.MaxHP += 10;
                    this.MaxMP += 10;
                    this.HP = MaxHP;
                    this.MaxMP = MaxMP;
                }
            }
        }

        /// <summary>
        /// 입력받은 클래스에 따른 시작 ATK, DEF, HP, MP 설정
        /// </summary>
        private void SetClass(ClassType classType)
        {
            switch (classType)
            {
                case ClassType.WARRIOR:
                    {
                        Attack = 10;
                        Defense = 10;
                        MaxHP = 100;
                        HP = MaxHP;
                        MaxMP = 50;
                        MP = MaxMP;
                        break;
                    }
                case ClassType.ARCHER:
                    {
                        Attack = 10;
                        Defense = 6;
                        MaxHP = 80;
                        HP = MaxHP;
                        MaxMP = 60;
                        MP = MaxMP;
                        break;
                    }
                case ClassType.MAGICIAN:
                    {
                        Attack = 6;
                        Defense = 4;
                        MaxHP = 60;
                        HP = MaxHP;
                        MaxMP = 90;
                        MP = MaxMP;
                        break;
                    }
                case ClassType.CLERIC:
                    {
                        Attack = 8;
                        Defense = 8;
                        MaxHP = 80;
                        HP = MaxHP;
                        MaxMP = 70;
                        MP = MaxMP;
                        break;
                    }
            }
        }

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
        WARRIOR = 1,
        ARCHER,
        MAGICIAN,
        CLERIC
    }
}

