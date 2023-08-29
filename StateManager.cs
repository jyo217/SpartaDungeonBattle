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
        string _inputName = Console.ReadLine();

        if (_inputName != null)
        {
            Console.Clear();
            VIEW_STATUS = new VIEW_STATUS($"{_inputName}", "전사", 1, 10, 5, 100, 1500);
        }
        else
        {
            Console.WriteLine("이름을 입력해주세요!");
        }
    }

    public class VIEW_STATUS
    {
        public string Name;
        public string PlayerClass;
        public int Level;
        public int AtkValue;
        public int DefValue;
        public int HpValue;
        public int Gold;
        public int BaseAtkValue;
        public int BaseDefValue;

        // BaseAtkValue 는 장비를 착용 / 착용 해제 시
        //기본 플레이어의 데이터를 저장하기 위해 설정했다.
        //장비를 착용하면, 기존의 플레이어의 스탯에 + - 되는 구조다.

        public VIEW_STATUS(string _name, string _playerClass, int _level, int _atkValue, int _defValue, int _hpValue, int _gold)
        {
            Name = _name;
            PlayerClass = _playerClass;
            Level = _level;
            AtkValue = _atkValue;
            DefValue = _defValue;
            HpValue = _hpValue;
            Gold = _gold;

            BaseAtkValue = _atkValue;
            BaseDefValue = _defValue;
        }
    }


    static void LOBBY()
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
        int _input = CheckValidAction(0, 3);

        switch (_input)
        {
            case 0:
                Environment.Exit(0);
                break;
            case 1:
                VIEW_STATUS();
                break;
            case 2:
                VIEW_INVENTORY();
                break;
            case 3:
                BATTLE();
                break;
        }
    }
        static int CheckValidAction(int _min, int _max)
        {
            while (true)
            {
                Console.WriteLine(" ");
                Console.WriteLine(" 원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                string _input = Console.ReadLine();

                bool _parseSuccess = int.TryParse(_input, out var _ret);
                if (_parseSuccess)
                {
                    if (_ret >= _min && _ret <= _max)
                        return _ret;
                }
                Console.WriteLine("잘못된 입력입니다.");
            }
        }

    static void VIEW_INVENTORY()
    {
        Console.Clear();
        Console.WriteLine("인벤토리");
        Console.WriteLine("보유중인 장비,포션을 관리할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

    }

    static void BATTLE()
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