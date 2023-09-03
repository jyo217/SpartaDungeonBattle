using SpartaDungeonBattle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Battle
{
    public static Battle CurrentBattle { get; private set; }
    public List<Monster> Monsters { get; private set; }
    private BattlePhase _battlePhase;
    public bool isBattleEnd { get { return _battlePhase == BattlePhase.BATTLE_OUT; } }
    bool isAllDead
    {
        get
        {
            foreach (Monster monster in Monsters)
                if (monster.isDead == false) return false;
            return true;
        }
    }

    public Battle()
    {
        CurrentBattle = this;
        _battlePhase = BattlePhase.PLAYER_BATTLE;
        Monsters = new List<Monster>();
        CreateMonsters();
    }

    public void Phase()
    {
        DisplayTop();
        switch (_battlePhase)
        {
            case BattlePhase.PLAYER_BATTLE:
                DisplayMonsters();
                DisplayCharacterInfo();
                Process_PlayerBattle();
                break;
            case BattlePhase.PLAYER_ATTACK:
                DisplayMonsters(true);
                DisplayCharacterInfo();
                Process_PlayerAttack();
                break;
            case BattlePhase.PLAYER_SKILL:
                Process_PlayerSkill();
                break;
            case BattlePhase.PLAYER_ITEM:
                Process_PlayerItem();
                break;
            case BattlePhase.MONSTER_ATTACK:
                Process_MonsterAttack();
                break;
            case BattlePhase.BATTLE_END:
                Process_BattleResult();
                break;
            case BattlePhase.BATTLE_OUT:
                return;
        }
    }

    private void DisplayTop()
    {
        Console.Clear();
        Console.WriteLine($"Battle!!{(_battlePhase != BattlePhase.BATTLE_END ? "" : " - Result")}\n");
    }

    private void DisplayMonsters(bool withNumbers = false)
    {
        int n = 0;
        foreach (Monster monster in Monsters)
        {
            Console.WriteLine($"{(withNumbers ? $"{++n} => " : "")}Lv.{monster.Level} {monster.Name}  {(monster.isDead ? "Dead" : $"HP {monster.HP}")}");
        }
        Console.WriteLine();
    }

    private void DisplayCharacterInfo()
    {
        Console.WriteLine("[내정보]");
        Console.WriteLine($"Lv.{Character.CurrentCharacter.Level}  {Character.CurrentCharacter.Name}  ({Character.CurrentCharacter.ClassToString()})");
        Console.WriteLine($"HP {Character.CurrentCharacter.HP}/{Character.CurrentCharacter.MaxHP}");
        Console.WriteLine($"MP {Character.CurrentCharacter.MP}/{Character.CurrentCharacter.MaxMP}");
        Console.WriteLine();
    }

    private void Process_PlayerBattle()
    {
        Console.WriteLine("1. 공격");
        Console.WriteLine("2. 스킬");
        Console.WriteLine("3. 아이템 사용");
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해주세요.");
        Console.Write(">> ");

        int input = StateManager.CheckValidInput(1, 3);
        switch (input)
        {
            case 1:
                _battlePhase = BattlePhase.PLAYER_ATTACK;
                break;
            case 2:
                _battlePhase = BattlePhase.PLAYER_SKILL;
                break;
            case 3:
                _battlePhase = BattlePhase.PLAYER_ITEM;
                break;
        }
    }

    private void Process_PlayerAttack()
    {
        Console.WriteLine("공격하려는 몬스터의 번호를 입력해주세요.");
        Console.WriteLine("0. 돌아가기\n");
        Console.Write(">> ");

        int input = -1;
        while (true)
        {
            input = StateManager.CheckValidInput(0, Monsters.Count) - 1;
            if (input == -1)
            {
                _battlePhase = BattlePhase.PLAYER_BATTLE;
                return;
            }
            if (Monsters[input].isDead) Console.WriteLine("잘못된 입력입니다.");
            else break;
        }

        //공격 함수
        DisplayTop();
        Character.CurrentCharacter.NormalAttack(Monsters[input]);

        Console.WriteLine("\n0.다음\n");
        Console.Write(">> ");
        StateManager.CheckValidInput(0, 0);
        if (isAllDead == true) _battlePhase = BattlePhase.BATTLE_END;
        else _battlePhase = BattlePhase.MONSTER_ATTACK;
    }

    private void Process_PlayerSkill()
    {
        while (true)
        {
            DisplayTop();
            DisplayMonsters();
            DisplayCharacterInfo();
            Console.WriteLine("[스킬]");
            Console.WriteLine("\n***    기술명    >>  소모 MP  >>    기술 설명  ***\n");
            int n = 0;
            Console.WriteLine($"{(++n).ToString()}. {Character.CurrentCharacter.MainSkill.SkillName}  >>  {Character.CurrentCharacter.MainSkill.Cost}   >>  {Character.CurrentCharacter.MainSkill.SkillDescription}");
            Console.WriteLine();

            Console.WriteLine("사용하려는 스킬의 번호를 입력해주세요.");
            Console.WriteLine("0. 돌아가기\n");
            Console.Write(">> ");

            int input = StateManager.CheckValidInput(0, n) - 1;
            if (input == -1)
            {
                _battlePhase = BattlePhase.PLAYER_BATTLE;
                return;
            }

            if (Character.CurrentCharacter.MainSkill.IsNeedTarget == false)
            {
                DisplayTop();
                // 몬스터 전체에게 스킬 사용
                Character.CurrentCharacter.UseSkill(Monsters);
                Console.WriteLine("\n0.다음\n");
                Console.Write(">> ");
                StateManager.CheckValidInput(0, 0);
                if (isAllDead == true) _battlePhase = BattlePhase.BATTLE_END;
                else _battlePhase = BattlePhase.MONSTER_ATTACK;
                return;
            }
            else if (Character.CurrentCharacter.MainSkill.IsNeedTarget == true)
            {
                bool isSuccess = Process_SkillToMonster(Character.CurrentCharacter.MainSkill);
                if (isSuccess)
                {
                    Console.WriteLine("\n0.다음\n");
                    Console.Write(">> ");
                    StateManager.CheckValidInput(0, 0);
                    if (isAllDead == true) _battlePhase = BattlePhase.BATTLE_END;
                    else _battlePhase = BattlePhase.MONSTER_ATTACK;
                    break;
                }
            }
        }
    }

    private bool Process_SkillToMonster(Skill skill)
    {
        DisplayTop();
        DisplayMonsters(true);

        // 몬스터에게 아이템 사용
        Console.WriteLine("대상이 되는 몬스터의 번호를 입력해주세요.");
        Console.WriteLine("0. 돌아가기\n");
        Console.Write(">> ");

        int input = -1;
        while (true)
        {
            input = StateManager.CheckValidInput(0, Monsters.Count) - 1;
            if (input == -1)
                return false;

            if (Monsters[input].isDead) Console.WriteLine("잘못된 입력입니다.");
            else break;
        }

        DisplayTop();
        Console.WriteLine($"{Character.CurrentCharacter.Name}은(는) {skill.SkillName}을(를) 사용했다!\n");
        Character.CurrentCharacter.UseSkill(Monsters[input]);
        return true;
    }

    private void Process_PlayerItem()
    {
        while (true)
        {
            DisplayTop();
            DisplayMonsters();
            DisplayCharacterInfo();
            Console.WriteLine("[아이템 목록]");
            Console.WriteLine("\n***아이템 이름  >>         아이템 설명     ***\n");
            int n = 0;
            List<Consumption> ItemList = new List<Consumption>();
            for (int i = 0; i < Character.CurrentCharacter.Inventory.Count; i++)
            {
                Item item = Character.CurrentCharacter.Inventory[i];
                if (item.ItemType == ItemType.CONSUMPTION)
                {
                    Consumption consumption = item as Consumption;
                    Console.WriteLine((++n).ToString() + "    " + consumption.ItemName + "  >>  " + consumption.Description);
                    ItemList.Add(consumption);
                }
            }

            Console.WriteLine("사용하려는 아이템의 번호를 입력해주세요.");
            Console.WriteLine("0. 돌아가기\n");
            Console.Write(">> ");

            int input = StateManager.CheckValidInput(0, n) - 1;
            if (input == -1)
            {
                _battlePhase = BattlePhase.PLAYER_BATTLE;
                return;
            }

            if (ItemList[input].Target == ItemTarget.ToCharacter)
            {
                // 자신에게 아이템 사용
                DisplayTop();
                Console.WriteLine($"{Character.CurrentCharacter.Name}은(는) {ItemList[input].ItemName}을(를) 사용했다!\n");
                ItemList[input].ItemFunc(Character.CurrentCharacter, null);
                Character.CurrentCharacter.Inventory.ItemList.Remove((Item)ItemList[input]);

                Console.WriteLine("\n0.다음\n");
                Console.Write(">> ");
                StateManager.CheckValidInput(0, 0);
                _battlePhase = BattlePhase.MONSTER_ATTACK;
                return;
            }
            else if (ItemList[input].Target == ItemTarget.ToMonster)
            {
                bool isSuccess = Process_ItemToMonster(ItemList[input]);
                if (isSuccess)
                {
                    Character.CurrentCharacter.Inventory.ItemList.Remove((Item)ItemList[input]);
                    Console.WriteLine("\n0.다음\n");
                    Console.Write(">> ");
                    StateManager.CheckValidInput(0, 0);
                    if (isAllDead == true) _battlePhase = BattlePhase.BATTLE_END;
                    else _battlePhase = BattlePhase.MONSTER_ATTACK;
                    break;
                }
            }

        }
    }

    private bool Process_ItemToMonster(Consumption consumption)
    {
        DisplayTop();
        DisplayMonsters(true);

        // 몬스터에게 아이템 사용
        Console.WriteLine("대상이 되는 몬스터의 번호를 입력해주세요.");
        Console.WriteLine("0. 돌아가기\n");
        Console.Write(">> ");

        int input = -1;
        while (true)
        {
            input = StateManager.CheckValidInput(0, Monsters.Count) - 1;
            if (input == -1)
                return false;

            if (Monsters[input].isDead) Console.WriteLine("잘못된 입력입니다.");
            else break;
        }

        DisplayTop();
        Console.WriteLine($"{Character.CurrentCharacter.Name}은(는) {consumption.ItemName}을(를) 사용했다!\n");
        consumption.ItemFunc(null, Monsters[input]);
        return true;
    }

    private void Process_MonsterAttack()
    {
        foreach (Monster monster in Monsters)
        {
            if (monster.isDead) continue;
            DisplayTop();
            monster.NormalAttack(Character.CurrentCharacter);
            Console.WriteLine("\n0.다음\n");
            Console.Write(">> ");
            StateManager.CheckValidInput(0, 0);
            if (Character.CurrentCharacter.isDead)
            {
                _battlePhase = BattlePhase.BATTLE_END;
                return;
            }
        }
        _battlePhase = BattlePhase.PLAYER_BATTLE;
    }

    private void Process_BattleResult()
    {
        if (Character.CurrentCharacter.isDead == false) // 승리한 경우
        {
            int earnedExp = 0;
            int earnedGold = 0;

            //승리 보상 경험치와 골드 계산
            for (int i = 0; i < Monsters.Count; i++) 
            {
                earnedExp += Monsters[i].Level;
                earnedGold += (Monsters[i].Level * 20);
            }

            Console.WriteLine("Victory\n");
            Console.WriteLine($"던전에서 몬스터 {Monsters.Count}마리를 잡았습니다.\n");
            Console.WriteLine("[캐릭터 정보]");
            Console.WriteLine($"현재 체력 : {Character.CurrentCharacter.HP}");
            Console.Write($"경험치 : Lv.{Character.CurrentCharacter.Level} (exp : {Character.CurrentCharacter.Exp})");
            Character.CurrentCharacter.IncreaseExp(earnedExp);
            Console.WriteLine($" -> Lv.{Character.CurrentCharacter.Level} (exp : {Character.CurrentCharacter.Exp})");
            Character.CurrentCharacter.EarnGold(earnedGold);
            Console.WriteLine($"드롭된 골드 : {Character.CurrentCharacter.Gold}");

            Character character = Character.CurrentCharacter;

            //포션 획득. 포션 드랍 랜덤값에 따라 HP 포션은 1~2개, MP 포션은 0~1개 인벤토리에 추가, 획득 메시지 출력
            Random random = new Random();
            int potionDrop = random.Next(0,4); 
            int earnedHpPotion = potionDrop > 1 ? 2 : 1; 
            int earnedMpPotion = potionDrop > 1 ? 1 : 0;
            string earnedPotionMsg = "포션 획득! : ";

            if (earnedHpPotion > 0) { earnedPotionMsg += $"{Consumption.MakeHpPotion().ItemName} X {earnedHpPotion}   "; }
            for (int i = 0; i < earnedHpPotion; i++) { character.Inventory.ItemList.Add(Consumption.MakeHpPotion()); }//HP 포션 인벤토리에 추가
           
            if (earnedMpPotion > 0) { earnedPotionMsg += $"{Consumption.MakeMpPotion().ItemName} X {earnedHpPotion}   "; }
            for (int i = 0; i < earnedMpPotion; i++) { character.Inventory.ItemList.Add(Consumption.MakeMpPotion()); }//MP 포션 인벤토리에 추가

            if(earnedHpPotion > 0 || earnedMpPotion > 0){Console.WriteLine(earnedPotionMsg);}//획득한 포션이 존재한다면 메시지 출력

            //장비 획득. 장비 드랍 랜덤값에 따라 해당 장비를 인벤토리에 추가, 획득 메시지 출력
            Equipment broken_straight_sword = new Equipment("부러진 직검  ", 10, "정상인이라면 이것을 무기로 사용하지 않으려 할 것입니다.", 1, 0);
            Equipment fundoshi = new Equipment("낡은 치부가리개?", 5, "속옷 '이었던 것' 으로 추정됩니다. 낡았고, 냄새가 나는 것만 같습니다.", 0, 1);
            Equipment fined_sword = new Equipment("쓸만한 검", 500, "쓸만해 보이지만, 어딘가 만듦새가 아쉬워 보입니다.", 5, 0);
            Equipment chain_male = new Equipment("사슬 갑옷", 350, "작은 사슬을 엮어 만든 갑옷. 생각보다 상태가 양호한 것 같습니다.", 5, 0);
            Equipment Excution = new Equipment("집행", 10000, "투박해 보이는 방패? 로 추정됩니다. 예사롭지 않은 기운이 흘러나옵니다. 도대체 왜 이런 곳에...?", 50, 30);
            
            //0~5 면 드랍 없음, 6~15 부러진 직검, 10~23 낡은 치부가리개, 20~25 사슬 갑옷, 25~28 쓸만한 검, 29 집행
            int equipmentDrop = random.Next(0, 30);
            string earnedEquipmentMsg = "장비 획득! : ";
            if (equipmentDrop >= 6 && equipmentDrop <= 15) { character.Inventory.ItemList.Add(broken_straight_sword); earnedEquipmentMsg += $"{broken_straight_sword.ItemName}   "; }
            else if (equipmentDrop >= 10 && equipmentDrop <= 23) { character.Inventory.ItemList.Add(fundoshi); earnedEquipmentMsg += $"{fundoshi.ItemName}   "; }
            else if (equipmentDrop >= 20 && equipmentDrop <= 25) { character.Inventory.ItemList.Add(chain_male); earnedEquipmentMsg += $"{chain_male.ItemName}   "; }
            else if (equipmentDrop >= 25 && equipmentDrop <= 28) { character.Inventory.ItemList.Add(fined_sword); earnedEquipmentMsg += $"{fined_sword.ItemName}   "; }
            else if(equipmentDrop >= 29) { character.Inventory.ItemList.Add(Excution); earnedEquipmentMsg += $"{Excution.ItemName}   "; }

            if (equipmentDrop >= 6) { Console.WriteLine(earnedEquipmentMsg); }
        }
        else // 패배한 경우
        {
            Console.WriteLine("You Lose\n");
            Console.WriteLine($"Lv.{Character.CurrentCharacter.Level} {Character.CurrentCharacter.Name}");
            Character.CurrentCharacter.HP = 10;
        }
        Console.WriteLine("\n0.다음\n");
        Console.Write(">> ");
        StateManager.CheckValidInput(0, 0);
        _battlePhase = BattlePhase.BATTLE_OUT;
    }

    private void CreateMonsters()
    {
        Random seed = new Random();
        int monsterNum = seed.Next(1, 4);
        for (int i = 0; i < monsterNum; ++i)
        {
            int randMonsterSeed = seed.Next(0, 100);
            if (randMonsterSeed < 50)
                Monsters.Add(new Monster(2, "미니언", 5, 15));
            else if (randMonsterSeed < 80)
                Monsters.Add(new Monster(3, "공허충", 9, 10));
            else
                Monsters.Add(new Monster(5, "대포미니언", 8, 25));
        }
    }
}

enum BattlePhase
{
    PLAYER_BATTLE,
    PLAYER_ATTACK,
    PLAYER_SKILL,
    PLAYER_ITEM,
    MONSTER_ATTACK,
    BATTLE_END,
    BATTLE_OUT,
}