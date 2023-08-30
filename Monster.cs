using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeonBattle
{
    [Serializable]
    public class Monster
    {
        public int Level { get; private set; }
        public string Name { get; private set; }
        public int Attack { get; private set; }
        public int HP { get; private set; }

        public bool isDead { get { return HP <= 0; } }

        /// <summary>
        /// 필요에 따라 편집
        /// </summary>
        public Monster(int level, string name, int attack, int hp)
        {
            Level = level;
            Name = name;
            Attack = attack;
            HP = hp;
        }


        public event Action<Character>? OnHit;

        public event Action? OnDead;

        public event Action<Character>? OnAttack;
    }
}
