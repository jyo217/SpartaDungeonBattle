using SpartaDungeonBattle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class StateManager
{
    public static Character? CurrentCharacter { get; set; }
    public static GameState PresentState { get; private set; }

    public static void State()
    {
        switch (PresentState)
        {
            case GameState.GAMESTART_NICKNAME:
                Display_GameStart_Nickname();
                string input = Console.ReadLine();
                CurrentCharacter.Name = input;
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

    static void Display_Error()
    {
        Console.Clear();
        Console.WriteLine("[ERROR] 분기에 맞는 콘솔 출력 설정이 되어 있지 않습니다.");
    }

    static void Display_GameStart_Nickname()
    {
        Console.Clear();
        Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
        Console.WriteLine("원하시는 이름을 설정해주세요.");
        Console.Write(">> ");
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


