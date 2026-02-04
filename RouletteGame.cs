namespace CasinoMinigames
{

    public class RouletteGame : GameBase
    {
        public override string Name => "Roulette";

        protected override string Description => "Roulette is coming soon. Place your bets later!";

        protected override GameOutcome RunGame(int bet)
        {
            Console.WriteLine("This is a placeholder for the Roulette game.");
            return new GameOutcome(GameResult.Push);
        }
    }
}
