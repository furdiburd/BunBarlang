namespace CasinoMinigames
{
    public class RouletteGame : GameBase
    {
        public override string Name => "Roulette";

        protected override string Description =>
            "Európai rulett (0-36). Válassz EGY tétet, és pörgesd meg a kereket. " +
			"Kifizetések: szám 35:1, piros/fekete 1:1, páros/páratlan 1:1, kicsi/nagy 1:1, tucat 2:1, oszlop 2:1.";

		private readonly Random _rng = new();

        protected override GameOutcome RunGame(int bet)
        {
            var wheel = new RouletteWheel(_rng);

            RouletteBet? placedBet = PromptForBetSelection();
            if (placedBet == null)
            {
				Console.WriteLine("Tét kiválasztása megszakítva.");
				return new GameOutcome(GameResult.Push);
			}

			Console.WriteLine($"Téted: {placedBet}");
			Console.WriteLine();
			Console.WriteLine("Pörgetés...");

			var pocket = wheel.Spin();
            RenderPocket(pocket);

            if (placedBet.TryGetProfit(bet, pocket, out int profit))
            {
                Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine($"Nyertél! Profit: {profit}");
				Console.ResetColor();
                return new GameOutcome(GameResult.Win, profit);
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Vesztettél.");
            Console.ResetColor();
            return new GameOutcome(GameResult.Lose);
        }

        private static void RenderPocket(RoulettePocket pocket)
        {
            Console.Write("Eredméy: ");
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
				Console.WriteLine("Válasszon fogadási típust:");
				Console.WriteLine("  1) Szám (0-36)         kifizetés 35:1");
				Console.WriteLine("  2) Piros / Fekete      kifizetés 1:1");
				Console.WriteLine("  3) Páros / Páratlan    kifizetés 1:1 (a 0 veszít)");
				Console.WriteLine("  4) Alacsony / Magas    kifizetés 1:1 (a 0 veszít)");
				Console.WriteLine("  5) Tucat (1-12/13-24/25-36) kifizetés 2:1 (a 0 veszít)");
				Console.WriteLine("  6) Oszlop (1./2./3.)   kifizetés 2:1 (a 0 veszít)");
				Console.WriteLine("  0) Mégse");
				Console.Write("Választás: ");

				var input = (Console.ReadLine() ?? string.Empty).Trim();
                Console.WriteLine();

                if (input == "0")
                {
                    return null;
                }

                if (!int.TryParse(input, out int option) || option < 1 || option > 6)
                {
					Console.WriteLine("Érvénytelen opció. Próbálja újra.\n");
					continue;
                }

                switch (option)
                {
                    case 1:
                        {
							int szam = PromptInt("Szám (0-36): ", 0, 36);
							return RouletteBet.StraightUp(szam);
						}

					case 2:
						{
							var szin = PromptRedBlack();
							return RouletteBet.Color(szin);
						}

					case 3:
						{
							var ps = PromptEnum("Páros vagy páratlan? (p/t): ",
								("p", RouletteEvenOdd.Even),
								("t", RouletteEvenOdd.Odd));
							return RouletteBet.EvenOdd(ps);
						}

					case 4:
						{
							var am = PromptEnum("Alacsony vagy magas? (a/m): ",
								("a", RouletteHighLow.Low),
								("m", RouletteHighLow.High));
							return RouletteBet.HighLow(am);
						}

					case 5:
						{
							var tucat = PromptEnum("Tucat? (1/2/3): ",
								("1", RouletteDozen.First),
								("2", RouletteDozen.Second),
								("3", RouletteDozen.Third));
							return RouletteBet.Dozen(tucat);
						}

					case 6:
						{
							var oszlop = PromptEnum("Oszlop? (1/2/3): ",
								("1", RouletteColumn.First),
								("2", RouletteColumn.Second),
								("3", RouletteColumn.Third));
							return RouletteBet.Column(oszlop);
						}

				}
			}
        }

        private static RouletteColor PromptRedBlack()
        {
            while (true)
			{
				Console.Write("Piros vagy fekete? (p/f): ");
				var input = (Console.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();

				if (input == "p") return RouletteColor.Red;
				if (input == "f") return RouletteColor.Black;

				Console.WriteLine("Érvénytelen választás. Adj meg p-t vagy f-et.\n");
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
                    Console.WriteLine($"Érvénytelen szám. Adjon meg egy értéket {minInclusive} és {maxInclusive} között.\n");
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

                Console.WriteLine("Érvénytelen választás.\n");
            }
        }
    }
}
