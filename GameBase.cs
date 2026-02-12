namespace CasinoMinigames
{
    // for common game logic and flow control

    public abstract class GameBase
    {
        public abstract string Name { get; }

        protected virtual string Description => string.Empty;

        public void Play(Player player)
        {

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

            Console.WriteLine($"Tét értéke: {bet} kredit");
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
                    Console.WriteLine($"Ön nyert {winAmount} kreditet!");
                    Console.ResetColor();
                    break;

                case GameResult.Lose:
                    player.DeductCredits(bet);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Ön elvesztett {bet} kreditet.");
                    Console.ResetColor();
                    break;

                case GameResult.Push:
                    Console.WriteLine("Döntetlen, nincs kredit változás.");
                    break;
            }

            Console.WriteLine($"Jelenlegi kreditek: {player.Credits}");
            Console.WriteLine();
            Console.WriteLine("Nyomjon meg egy tetszõleges billentyût a fõmenübe való visszatéréshez...");
            Console.ReadKey(true);
        }

        protected abstract GameOutcome RunGame(int bet);

        protected void RenderHeader()
        {
            Console.WriteLine("=================================");
            Console.WriteLine($"        {Name.ToUpper()} JÁTÉK        ");
            Console.WriteLine("=================================");
            Console.WriteLine();
        }

        private int PromptForBet(Player player)
        {
            while (true)
            {
                Console.Clear();
                RenderHeader();
                Console.WriteLine($"Kreditek: {player.Credits}");
                Console.Write("Adja meg a tétet (vagy 0 a kilépéshez): ");

                var input = Console.ReadLine();
                if (!int.TryParse(input, out int bet))
                {
                    Console.WriteLine("Érvénytelen szám. A folytatáshoz nyomjon meg egy tetszõleges billentyût...");
                    Console.ReadKey(true);
                    continue;
                }

                if (bet == 0)
                {
                    return 0;
                }

                if (!player.CanBet(bet))
                {
                    Console.WriteLine("A tétnek pozitívnak kell lennie, és nem haladhatja meg a jelenlegi egyenlegedet.");
                    Console.WriteLine("Nyomjon meg egy tetszõleges billentyût az újrapróbálkozáshoz...");
                    Console.ReadKey(true);
                    continue;
                }

                return bet;
            }
        }
    }
}