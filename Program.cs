namespace ConsoleApp1
{
    /// <summary>
    /// Main entry point for the Casino Minigame Collection application.
    /// Keeps the main function minimal by delegating to modular classes.
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initialize all available games
            var games = new List<IGame>
            {
                new PokerGame(),
                new RouletteGame(),
                new BlackjackGame()
            };

            // Create and run the main menu
            var menu = new Menu(games);
            menu.Run();
        }
    }
}
