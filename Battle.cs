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

                break;
            case BattlePhase.PLAYER_ITEM:

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

            if (earnedHpPotion > 0) { earnedPotionMsg += $"{Consumption.MakeRedPotion().ItemName} X {earnedHpPotion}   ,   "; }
            for (int i = 0; i < earnedHpPotion; i++) { character.Inventory.ItemList.Add(Consumption.MakeRedPotion()); }//HP 포션 인벤토리에 추가
           
            if (earnedMpPotion > 0) { earnedPotionMsg += $"{Consumption.MakeBluePotion().ItemName} X {earnedHpPotion}   "; }
            for (int i = 0; i < earnedMpPotion; i++) { character.Inventory.ItemList.Add(Consumption.MakeBluePotion()); }//MP 포션 인벤토리에 추가

            Console.WriteLine(earnedPotionMsg);

            //장비 획득. 장비 드랍 랜덤값에 따라 해당 장비를 인벤토리에 추가, 획득 메시지 출력
            int equipmentDrop = random.Next(0, 5);
            
            /*
            string earnedEquipmentMsg = "장비 획득! : ";
            if (earnedHpPotion > 0) { earnedPotionMsg += $"{Consumption.MakePotion().ItemName} X {earnedHpPotion}   ,   "; }
            for (int i = 0; i < earnedHpPotion; i++) { character.Inventory.ItemList.Add(Consumption.MakePotion()); }//HP 포션 인벤토리에 추가

            if (earnedMpPotion > 0) { earnedPotionMsg += $"{Consumption.MakeMpPotion().ItemName} X {earnedMpPotion}   "; }
            for (int i = 0; i < earnedMpPotion; i++) { character.Inventory.ItemList.Add(Consumption.MakeMpPotion()); }//MP 포션 인벤토리에 추가

            Console.WriteLine(earnedPotionMsg);
            */
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