namespace CasinoMinigames
{
    public class BlackjackGame : GameBase
    {
        public override string Name => "Blackjack";

        protected override string Description => "Blackjack is coming soon. Good luck when it lands!";

        protected override GameOutcome RunGame(int bet)
        {
            Console.WriteLine("This is a placeholder for the Blackjack game.");
            return new GameOutcome(GameResult.Push);
        }
    }
}
