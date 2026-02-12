namespace CasinoMinigames
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var player = new Player(10_000);

            var games = new List<GameBase>
            {
                new FarkleGame(),
                new RouletteGame(),
                new BlackjackGame()
            };

            var menu = new Menu(games, player);
            menu.Run();
        }
    }
}
