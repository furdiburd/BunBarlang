namespace CasinoMinigames
{
    /// <summary>
    /// Blackjack game implementation (placeholder).
    /// </summary>
    public class BlackjackGame : IGame
    {
        public string Name => "Blackjack";

        public void Play()
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("    BLACKJACK GAME STARTED       ");
            Console.WriteLine("=================================");
            Console.WriteLine();
            Console.WriteLine("Welcome to Blackjack!");
            Console.WriteLine("This is a placeholder for the Blackjack game.");
            Console.WriteLine();
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey(true);
        }
    }
}
