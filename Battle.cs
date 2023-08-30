using SpartaDungeonBattle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Battle
{
    public static Battle? CurrentBattle { get; private set; }
    public List<Monster> Monsters { get; private set; }
    private BattlePhase _battlePhase;
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
        Console.WriteLine($"Battle!!{(_battlePhase == BattlePhase.BATTLE_END ? "" : " - Result")}");
        switch (_battlePhase)
        {
            case BattlePhase.PLAYER_BATTLE:

                break;
            case BattlePhase.PLAYER_ATTACK:

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

    private void DisplayMonsters()
    {
        int n = 0;
        foreach (Monster monster in Monsters)
        {
            Console.WriteLine($"{++n} Lv.{monster.Level} {monster.Name}  {(monster.isDead ? "Dead" : $"HP {monster.HP}")}");
        }
        Console.WriteLine();
    }

    private void DisplayCharacterInfo()
    {
        Console.WriteLine("[내정보]");
        Console.WriteLine($"Lv.{Character.CurrentCharacter.Level}  {Character.CurrentCharacter.Name}  ({Character.CurrentCharacter.ClassToString()})");
        Console.WriteLine($"{Character.CurrentCharacter.HP}/{Character.CurrentCharacter.MaxHP}");
        Console.WriteLine($"{Character.CurrentCharacter.MP}/{Character.CurrentCharacter.MaxMP}");
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
}