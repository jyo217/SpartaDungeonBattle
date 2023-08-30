namespace SpartaDungeonBattle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Initialize();
            StateManager.State();
        }

        static void Initialize()
        {
            StateManager.Initialize();
            new Character();
        }
    }
}