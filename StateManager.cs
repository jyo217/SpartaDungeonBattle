using SpartaDungeonBattle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class StateManager
{
    public static Character? CurrentCharacter { get; set; }
    public static GameState CurrentState { get; private set; }

    public static void State()
    {
        switch (CurrentState)
        {
            case GameState.GAMESTART_NICKNAME:
                Display_GameStart_Nickname();
                Process_GameStart_Nickname();
                break;
            case GameState.GAMESTART_CLASS:

                break;
            case GameState.LOBBY:

                break;
            case GameState.VIEW_STATUS:

                break;
            case GameState.VIEW_INVENTORY:

                break;
            case GameState.MANAGE_EQUIPMENT:

                break;
            case GameState.BATTLE:

                break;
            default:
                Console.WriteLine("State에 맞는 함수를 설정해주지 않았습니다.");
                break;
        }
    }

    static void Display_Error()
    {
        Console.Clear();
        Console.WriteLine("[ERROR] 분기에 맞는 콘솔 출력 설정이 되어 있지 않습니다.");
    }

    static void Display_GameStart_Nickname()
    {
        Console.Clear();
        Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
    }

    static void Display_Lobby()
    {
        Console.Title = "SpartaDungeonBattle";
        Console.WriteLine("Sparta Dungeon Game!");
        Console.WriteLine("님, 스파르타 마을에 오신것을 환영합니다!");
        Console.WriteLine("이곳에서 던전으로 돌아가기 전 활동을 할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("0. 게임 종료 ");
        Console.WriteLine("1. 상태 보기");
        Console.WriteLine("2. 인벤토리");
        Console.WriteLine("3. 전투시작");
        Console.WriteLine(" ");
        int _input = CheckValidInput(0, 3);

        switch (_input)
        {
            case 0:
                Environment.Exit(0);
                break;
            case 1:
                Display_View_Status();
                break;
            case 2:
                Display_View_Inventory();
                break;
            case 3:
                Display_Battle();
                break;
        }
    }

    static void Display_View_Status()
    {
        Console.Clear();
        Console.WriteLine("상태 보기");
        Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
        Console.WriteLine($"Lv. {CurrentCharacter.Level.ToString("D2")}");
        Console.WriteLine($"{CurrentCharacter.Name} ( {CurrentCharacter.ClassToString()} )");
        Console.WriteLine($"공격력 : {CurrentCharacter.Attack}");
        Console.WriteLine($"방어력 : {CurrentCharacter.Defense}");
        Console.WriteLine($"체 력 : {CurrentCharacter.HP}");
        Console.WriteLine($"Gold : {CurrentCharacter.Gold}");
    }

    static void Display_View_Inventory()
    {
        Console.Clear();
        Console.WriteLine("인벤토리");
        Console.WriteLine("보유중인 장비,포션을 관리할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

    }

    static void Display_Battle()
    {
        Console.Clear();
        Console.WriteLine("전투시작");
        Console.WriteLine("전투를 시작하면 1~4마리의 몬스터가 랜덤하게 등장합니다.");
        Console.WriteLine("표시되는 순서는 랜덤입니다.");
        Console.WriteLine("몬스터는 3종류 있습니다.");
        Console.WriteLine("중복해서 나타날 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[몬스터 정보]");
    }

    static void Process_GameStart_Nickname()
    {
        string inputName = "";
        do
        {
            Console.WriteLine("원하시는 이름을 설정해주세요.");
            Console.Write(">> ");
            inputName = Console.ReadLine();
        } while (CurrentCharacter.SetNickname(inputName) == false);
        CurrentState = GameState.GAMESTART_CLASS;
    }

    static int CheckValidInput(int min, int max)
    {
        while (true)
        {
            string input = Console.ReadLine();

            bool parseSuccess = int.TryParse(input, out var ret);
            if (parseSuccess)
            {
                if (ret >= min && ret <= max)
                    return ret;
            }

            Console.WriteLine("잘못된 입력입니다.");
        }
    }

    public enum GameState
    {
        GAMESTART_NICKNAME,
        GAMESTART_CLASS,
        LOBBY,
        VIEW_STATUS,
        VIEW_INVENTORY,
        MANAGE_EQUIPMENT,
        BATTLE,
    }
}