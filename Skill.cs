using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpartaDungeonBattle;

namespace SpartaDungeonBattle
{
    [Serializable]
    public class Skill
    {
        public string SkillName { get; private set; }
        public int Cost { get; private set; }
        public string SkillDescription { get; private set; }

        public bool IsNeedTarget { get; private set; }

        /// <summary>
        /// 필요에 따라 편집
        /// </summary>
        public Skill(SkillName skillName) 
        { 
            switch(skillName) 
            {
                // 파이어볼
                case SpartaDungeonBattle.SkillName.FIREBALL:
                    skillName = "파이어볼"
                    Cost = 15;
                    SkillDescription = "단일의 적에게 캐릭터의 MaxMP의 절반의 데미지를 가한다"
                    IsNeedTarget = true;
                    break;
                case SpartaDungeonBattle.SkillName.MULTISHOT:
                    skillName = "다중사격"
                    Cost = 15;
                    SkillDescription = "모든 적에게 캐릭터의 공격력의 데미지를 가한다"
                    IsNeedTarget = false;
                    break
                case SpartaDungeonBattle.SkillName.MULTISHOT:
                    skillName = "강한타격"
                    Cost = 10;
                    SkillDescription = "단일의 적에게 캐릭터의 공격력의 2배의 데미지를 가한다"
                    IsNeedTarget = true;
                    break;
                default:
                    // 예외 처리
                    break;
            }
        }

        public void SkillFunc() { }

        public void SkillFunc(Character character) { }

        public void SkillFunc(Monster monster) { }
    }

    public enum SkillName
    {
        FIREBALL = 0,
        MULTISHOT,
        POWERSTRIKE
    }
}
