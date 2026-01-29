namespace ConsoleApp1
{
    /// <summary>
    /// Poker game implementation (placeholder).
    /// </summary>
    public class PokerGame : IGame
    {
        public string Name => "Poker";

        public void Play()
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("       POKER GAME STARTED        ");
            Console.WriteLine("=================================");
            Console.WriteLine();
            Console.WriteLine("Welcome to Poker!");
            Console.WriteLine("This is a placeholder for the Poker game.");
            Console.WriteLine();
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey(true);
        }
    }
}
