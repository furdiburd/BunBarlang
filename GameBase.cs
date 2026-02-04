namespace CasinoMinigames
{
    // for common game logic and flow control

    public abstract class GameBase
    {
        public abstract string Name { get; }

        protected virtual string Description => string.Empty;

        public void Play(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            if (player.Credits <= 0)
            {
                Console.Clear();
                RenderHeader();
                Console.WriteLine("You are out of credits. Add more to keep playing.");
                Console.WriteLine();
                Console.WriteLine("Press any key to return to the main menu...");
                Console.ReadKey(true);
                return;
            }

            int bet = PromptForBet(player);
            if (bet == 0)
            {
                return;
            }

            Console.Clear();
            RenderHeader();

            if (!string.IsNullOrWhiteSpace(Description))
            {
                Console.WriteLine(Description);
                Console.WriteLine();
            }

            Console.WriteLine($"Bet placed: {bet} credits");
            Console.WriteLine();

            var outcome = RunGame(bet);

            // the player receives at least their bet when marked as a win
            // larger payout can be specified via GameOutcome.Payout.
            int winAmount = outcome.Result == GameResult.Win
                ? Math.Max(outcome.Payout, bet)
                : 0;

            switch (outcome.Result)
            {
                case GameResult.Win:
                    player.AddCredits(winAmount);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"You won {winAmount} credits!");
                    Console.ResetColor();
                    break;

                case GameResult.Lose:
                    player.DeductCredits(bet);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"You lost {bet} credits.");
                    Console.ResetColor();
                    break;

                case GameResult.Push:
                    Console.WriteLine("Push. No credits exchanged.");
                    break;
            }

            Console.WriteLine($"Current credits: {player.Credits}");
            Console.WriteLine();
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey(true);
        }

        protected abstract GameOutcome RunGame(int bet);

        protected void RenderHeader()
        {
            Console.WriteLine("=================================");
            Console.WriteLine($"        {Name.ToUpper()} GAME        ");
            Console.WriteLine("=================================");
            Console.WriteLine();
        }

        private int PromptForBet(Player player)
        {
            while (true)
            {
                Console.Clear();
                RenderHeader();
                Console.WriteLine($"Credits: {player.Credits}");
                Console.Write("Enter your bet (or 0 to cancel): ");

                var input = Console.ReadLine();
                if (!int.TryParse(input, out int bet))
                {
                    Console.WriteLine("Invalid number. Press any key to retry...");
                    Console.ReadKey(true);
                    continue;
                }

                if (bet == 0)
                {
                    return 0;
                }

                if (!player.CanBet(bet))
                {
                    Console.WriteLine("Bet must be positive and no more than your current credits.");
                    Console.WriteLine("Press any key to retry...");
                    Console.ReadKey(true);
                    continue;
                }

                return bet;
            }
        }
    }
}