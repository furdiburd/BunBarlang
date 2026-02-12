namespace CasinoMinigames
{
    public class RouletteGame : GameBase
    {
        public override string Name => "Roulette";

        protected override string Description =>
            "European roulette (0-36). Choose ONE bet and spin the wheel. " +
            "Payouts: number 35:1, red/black 1:1, even/odd 1:1, low/high 1:1, dozen 2:1, column 2:1.";

        private readonly Random _rng = new();

        protected override GameOutcome RunGame(int bet)
        {
            var wheel = new RouletteWheel(_rng);

            RouletteBet? placedBet = PromptForBetSelection();
            if (placedBet == null)
            {
                Console.WriteLine("Bet selection cancelled.");
                return new GameOutcome(GameResult.Push);
            }

            Console.WriteLine($"Your bet: {placedBet}");
            Console.WriteLine();
            Console.WriteLine("Spinning...");

            var pocket = wheel.Spin();
            RenderPocket(pocket);

            if (placedBet.TryGetProfit(bet, pocket, out int profit))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"You win! Profit: {profit}");
                Console.ResetColor();
                return new GameOutcome(GameResult.Win, profit);
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You lose.");
            Console.ResetColor();
            return new GameOutcome(GameResult.Lose);
        }

        private static void RenderPocket(RoulettePocket pocket)
        {
            Console.Write("Result: ");
            ConsoleColor color = pocket.Color switch
            {
                RouletteColor.Red => ConsoleColor.Red,
                RouletteColor.Black => ConsoleColor.DarkGray,
                _ => ConsoleColor.Green
            };

            Console.ForegroundColor = color;
            Console.WriteLine(pocket);
            Console.ResetColor();
            Console.WriteLine();
        }

        private static RouletteBet? PromptForBetSelection()
        {
            while (true)
            {
                Console.WriteLine("Choose bet type:");
                Console.WriteLine("  1) Number (0-36)       payout 35:1");
                Console.WriteLine("  2) Red / Black         payout 1:1");
                Console.WriteLine("  3) Even / Odd          payout 1:1 (0 loses)");
                Console.WriteLine("  4) Low / High          payout 1:1 (0 loses)");
                Console.WriteLine("  5) Dozen (1-12/13-24/25-36) payout 2:1 (0 loses)");
                Console.WriteLine("  6) Column (1st/2nd/3rd) payout 2:1 (0 loses)");
                Console.WriteLine("  0) Cancel");
                Console.Write("Select: ");

                var input = (Console.ReadLine() ?? string.Empty).Trim();
                Console.WriteLine();

                if (input == "0")
                {
                    return null;
                }

                if (!int.TryParse(input, out int option) || option < 1 || option > 6)
                {
                    Console.WriteLine("Invalid option. Try again.\n");
                    continue;
                }

                switch (option)
                {
                    case 1:
                        {
                            int number = PromptInt("Number (0-36): ", 0, 36);
                            return RouletteBet.StraightUp(number);
                        }

                    case 2:
                        {
                            var color = PromptRedBlack();
                            return RouletteBet.Color(color);
                        }

                    case 3:
                        {
                            var eo = PromptEnum("Even or Odd? (e/o): ",
                                ("e", RouletteEvenOdd.Even),
                                ("o", RouletteEvenOdd.Odd));
                            return RouletteBet.EvenOdd(eo);
                        }

                    case 4:
                        {
                            var hl = PromptEnum("Low or High? (l/h): ",
                                ("l", RouletteHighLow.Low),
                                ("h", RouletteHighLow.High));
                            return RouletteBet.HighLow(hl);
                        }

                    case 5:
                        {
                            var dozen = PromptEnum("Dozen? (1/2/3): ",
                                ("1", RouletteDozen.First),
                                ("2", RouletteDozen.Second),
                                ("3", RouletteDozen.Third));
                            return RouletteBet.Dozen(dozen);
                        }

                    case 6:
                        {
                            var column = PromptEnum("Column? (1/2/3): ",
                                ("1", RouletteColumn.First),
                                ("2", RouletteColumn.Second),
                                ("3", RouletteColumn.Third));
                            return RouletteBet.Column(column);
                        }
                }
            }
        }

        private static RouletteColor PromptRedBlack()
        {
            while (true)
            {
                Console.Write("Red or Black? (r/b): ");
                var input = (Console.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();

                if (input == "r") return RouletteColor.Red;
                if (input == "b") return RouletteColor.Black;

                Console.WriteLine("Invalid choice. Enter r or b.\n");
            }
        }

        private static int PromptInt(string prompt, int minInclusive, int maxInclusive)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (!int.TryParse(input, out int value) || value < minInclusive || value > maxInclusive)
                {
                    Console.WriteLine($"Invalid number. Enter {minInclusive}-{maxInclusive}.\n");
                    continue;
                }

                return value;
            }
        }

        private static T PromptEnum<T>(string prompt, params (string Key, T Value)[] map)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = (Console.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();

                foreach (var (key, value) in map)
                {
                    if (input == key)
                    {
                        Console.WriteLine();
                        return value;
                    }
                }

                Console.WriteLine("Invalid choice. Try again.\n");
            }
        }
    }
}
