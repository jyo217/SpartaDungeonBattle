using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeonBattle
{
    [Serializable]
    public class Skill
    {
        public string SkillName { get; private set; }
        public int Cost { get; private set; }
        public string SkillDescription { get; private set; }

        /// <summary>
        /// 필요에 따라 편집
        /// </summary>
        public Skill() { }

        public void SkillFunc() { }

        public void SkillFunc(Character character) { }

        public void SkillFunc(Monster monster) { }
    }
}
