namespace CasinoMinigames
{
    public class PokerGame : GameBase
    {
        public override string Name => "Poker";

        protected override string Description => "Poker - To be implemented";

        protected override GameOutcome RunGame(int bet)
        {
            Console.WriteLine("This is a placeholder for the Poker game.");
            return new GameOutcome(GameResult.Push);
        }
    }
}
