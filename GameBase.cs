namespace CasinoMinigames
{
    /// <summary>
    /// Provides a common template for all games.
    /// </summary>
    public abstract class GameBase : IGame
    {
        public abstract string Name { get; }

        /// <summary>
        /// Short description shown before gameplay starts.
        /// </summary>
        protected virtual string Description => string.Empty;

        public void Play()
        {
            Console.Clear();
            RenderHeader();

            if (!string.IsNullOrWhiteSpace(Description))
            {
                Console.WriteLine(Description);
                Console.WriteLine();
            }

            RunGame();

            Console.WriteLine();
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Implement the core game flow here.
        /// </summary>
        protected abstract void RunGame();

        protected void RenderHeader()
        {
            Console.WriteLine("=================================");
            Console.WriteLine($"        {Name.ToUpper()} GAME        ");
            Console.WriteLine("=================================");
            Console.WriteLine();
        }
    }
}