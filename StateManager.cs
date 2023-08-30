using SpartaDungeonBattle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

public static class StateManager
{
    public static GameState CurrentState { get; private set; }

    public static void Initialize()
    {
        CurrentState = GameState.GAMESTART_NICKNAME;
    }

    public static void State()
    {
        while(true)
        {
            switch (CurrentState)
            {
                case GameState.GAMESTART_NICKNAME:
                    Display_GameStart_Nickname();
                    Process_GameStart_Nickname();
                    break;
                case GameState.GAMESTART_CLASS:
                    Display_GameStart_Class();
                    Process_GameStart_Class();
                    break;
                case GameState.LOBBY:
                    Display_Lobby();
                    Process_Lobby();
                    break;
                case GameState.VIEW_STATUS:
                    Display_View_Status();
                    Process_View_Status();
                    break;
                case GameState.VIEW_INVENTORY:
                    Display_View_Inventory();
                    Process_View_Inventory();
                    break;
                case GameState.MANAGE_EQUIPMENT:
                    Display_Manage_Equipment();
                    Process_Manage_Equipment();
                    break;
                case GameState.BATTLE:

                    break;
                default:
                    Console.WriteLine("State에 맞는 함수를 설정해주지 않았습니다.");
                    break;
            }
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

    static void Display_GameStart_Class()
    {
        Console.Clear();
        Console.WriteLine("캐릭터의 직업을 결정할 수 있습니다.");
    }

    static void Display_Lobby()
    {
        Console.Clear();
        Console.WriteLine("[LOBBY]");
        Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
        Console.WriteLine("이제 전투를 시작할 수 있습니다.\n");
    }

    static void Display_View_Status()
    {
        Console.Clear();
        Console.WriteLine("[상태 보기]");
        Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
        Console.WriteLine($"Lv. {Character.CurrentCharacter.Level.ToString("D2")}");
        Console.WriteLine($"{Character.CurrentCharacter.Name} ( {Character.CurrentCharacter.ClassToString()} )");
        Console.WriteLine($"공격력 : {Character.CurrentCharacter.Attack}");
        Console.WriteLine($"방어력 : {Character.CurrentCharacter.Defense}");
        Console.WriteLine($"체 력 : {Character.CurrentCharacter.HP}");
        Console.WriteLine($"Gold : {Character.CurrentCharacter.Gold}\n");
    }

    static void Display_View_Inventory()
    {
        Console.Clear();
        Console.WriteLine("[인벤토리]");
        Console.WriteLine("보유중인 장비,포션을 관리할 수 있습니다.\n");
        Console.WriteLine("[아이템 목록]");
        Console.WriteLine("    아이템 이름          효과                     설명               ");
        for (int i = 0; i < Character.CurrentCharacter.Inventory.Count; i++)
        {
            Item item = Character.CurrentCharacter.Inventory[i];
            switch (item.ItemType) 
            {
                case ItemType.CONSUMPTION :
                    Consumption consumption = item as Consumption;
                    Console.WriteLine(" " + consumption.ItemName.PadLeft(10, ' ') + "|" + consumption.Description);
                    break;
                case ItemType.EQUIPMENT :
                    Equipment equipment = item as Equipment;
                    Console.WriteLine(" " + equipment.ItemName.PadLeft(10, ' ') + "| 공격력 : "+ equipment.Attack.ToString().PadLeft(5, ' ') 
                                        +" 방어력 : " + equipment.Defense.ToString().PadLeft(5, ' ') + "|" + item.Description);
                    break;
                default:
                    break;
            }
            
        }
        Console.WriteLine();
    }

    static void Display_Manage_Equipment()
    {
        Console.Clear();
        Console.WriteLine("[장착 관리]");
        Console.WriteLine("장착 또는 장착 해제할 아이템을 선택해주세요.\n");
        Console.WriteLine("[아이템 목록]");
        Console.WriteLine("    아이템 이름          효과                     설명               ");
        for (int i = 0; i < Character.CurrentCharacter.Inventory.Count; i++)
        {
            //아이템 출력함수
        }
        Console.WriteLine();
    }

    static void Process_GameStart_Nickname()
    {
        string inputName = "";
        do
        {
            Console.WriteLine("원하시는 이름을 설정해주세요.");
            Console.Write(">> ");
            inputName = Console.ReadLine();
        } while (Character.CurrentCharacter.SetNickname(inputName) == false);
        CurrentState = GameState.GAMESTART_CLASS;
    }

    static void Process_GameStart_Class()
    {
        Console.WriteLine("1. 전사 / 2. 궁수 / 3. 마법사 / 4. 클레릭");
        Console.WriteLine("원하시는 직업을 선택해주세요.");
        Console.Write(">> ");
        int input = CheckValidInput(1, Enum.GetValues(typeof(ClassType)).Length);
        Character.CurrentCharacter.SetClass(((ClassType)input));

        CurrentState = GameState.LOBBY;
    }

    static void Process_Lobby()
    {
        Console.WriteLine("1. 상태 보기");
        Console.WriteLine("2. 인벤토리");
        Console.WriteLine("3. 전투 시작");
        Console.WriteLine("\n0. 게임 종료\n");

        Console.WriteLine("원하시는 행동을 입력해주세요.");
        Console.Write(">> ");
        int input = CheckValidInput(0, 3);

        switch (input)
        {
            case 0:
                Environment.Exit(0);
                break;
            case 1:
                CurrentState = GameState.VIEW_STATUS;
                break;
            case 2:
                CurrentState = GameState.VIEW_INVENTORY;
                break;
            case 3:
                CurrentState = GameState.BATTLE;
                break;
        }
    }

    static void Process_View_Status()
    {
        Console.WriteLine("\n0. 로비로 나가기\n");

        Console.WriteLine("원하시는 행동을 입력해주세요.");
        Console.Write(">> ");
        int input = CheckValidInput(0, 0);
        switch (input)
        {
            case 0:
                CurrentState = GameState.LOBBY;
                break;
        }
    }

    static void Process_View_Inventory()
    {
        Console.WriteLine("1. 장착 관리");
        Console.WriteLine("\n0. 로비로 나가기\n");

        Console.WriteLine("원하시는 행동을 입력해주세요.");
        Console.Write(">> ");

        int input = CheckValidInput(0, 1);

        switch (input)
        {
            case 0:
                CurrentState = GameState.LOBBY;
                break;
            case 1:
                CurrentState = GameState.MANAGE_EQUIPMENT;
                break;
        }
    }

    static void Process_Manage_Equipment()
    {

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