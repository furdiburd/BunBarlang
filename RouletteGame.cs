namespace ConsoleApp1
{
    /// <summary>
    /// Roulette game implementation (placeholder).
    /// </summary>
    public class RouletteGame : IGame
    {
        public string Name => "Roulette";

        public void Play()
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("     ROULETTE GAME STARTED       ");
            Console.WriteLine("=================================");
            Console.WriteLine();
            Console.WriteLine("Welcome to Roulette!");
            Console.WriteLine("This is a placeholder for the Roulette game.");
            Console.WriteLine();
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey(true);
        }
    }
}
