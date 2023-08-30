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

    private Action resultCallback;

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
            case BattlePhase.PLAYER_ITEMSELECT:

                break;
            case BattlePhase.PLAYER_ITEMUSE:

                break;
            case BattlePhase.PLAYER_ACTIONRESULT:

                break;
            case BattlePhase.MONSTER_ACTION:

                break;
            case BattlePhase.BATTLE_END:

                break;
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
                _battlePhase = BattlePhase.PLAYER_ITEMSELECT;
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
        _battlePhase = BattlePhase.MONSTER_ACTION;
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
    PLAYER_ITEMSELECT,
    PLAYER_ITEMUSE,
    PLAYER_ACTIONRESULT,
    MONSTER_ACTION,
    BATTLE_END,
    BATTLE_OUT,
}