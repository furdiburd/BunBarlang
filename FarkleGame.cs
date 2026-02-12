namespace CasinoMinigames
{
    public class FarkleGame : GameBase
    {
        public override string Name => "Farkle";

        protected override string Description =>
            "Dobj 6 kockával. Pontozás: sor (1-6) 1500, három pár 1500, két hármas iker 2500, " +
			"hat azonos 3000, öt azonos 2000, négy azonos 1000 (egyeseknél 2000), " +
			"három azonos egyes 1000 / egyéb számoknál érték*100, egy darab 1-es = 100, egy darab 5-ös = 50. " +
			"Nincs pontot érõ kocka = Farkle (0 pont a körre). Az nyer, aki elõször eléri a 2000 pontot.";

		private const int TargetScore = 2000;
        private readonly Random _rng = new();

        protected override GameOutcome RunGame(int bet)
        {
            int playerScore = 0;
            int dealerScore = 0;
            int turn = 1;

            while (playerScore < TargetScore && dealerScore < TargetScore)
            {
                Console.WriteLine($"-- Menet {turn} --");
                playerScore += TakeTurn("Te", isPlayer: true);
                DisplayScoreboard(playerScore, dealerScore);
                if (playerScore >= TargetScore)
                {
                    break;
                }

                dealerScore += TakeTurn("Osztó", isPlayer: false);
                DisplayScoreboard(playerScore, dealerScore);

                turn++;
                Console.WriteLine("Üss le egy billentyût a folytatáshoz...");
                Console.ReadKey(true);
                Console.Clear();
                RenderHeader();
                if (!string.IsNullOrWhiteSpace(Description))
                {
                    Console.WriteLine(Description);
                    Console.WriteLine();
                }
            }

            bool playerWins = playerScore >= TargetScore;
            Console.WriteLine(playerWins ? "Te nyertél!" : "Osztó nyert. Sok szerencsét a következõ menethez!");

            return new GameOutcome(playerWins ? GameResult.Win : GameResult.Lose);
        }

        private int TakeTurn(string name, bool isPlayer)
        {
            int diceToRoll = 6;
            int turnScore = 0;
            bool rolling = true;

            while (rolling)
            {
                Console.WriteLine($"{name} dob {diceToRoll} kockákat...");
                var dice = RollDice(diceToRoll);
                var result = ScoreRoll(dice);

                Console.WriteLine($"Dobott: {string.Join(", ", dice)}");

                if (result.IsFarkle)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Farkle! Nincsen pontot érõ kocka a dobásban.");
                    Console.ResetColor();
                    Console.WriteLine();
                    return 0;
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Pontok ebben a dobásban: {result.Score}");
                Console.ResetColor();

                if (!string.IsNullOrWhiteSpace(result.Breakdown))
                {
                    Console.WriteLine($"Részletek: {result.Breakdown}");
                }

                turnScore += result.Score;
                int remainingDice = diceToRoll - result.UsedDice;

                // all dice scored, roll all 6 again if continuing
                if (remainingDice == 0)
                {
                    remainingDice = 6;
                    Console.WriteLine("Forró kockák! Ha folytatod, újra dobhatsz mind a hattal.");
                }

                Console.WriteLine($"Fordulók eddig: {turnScore}");
                Console.WriteLine();

                if (isPlayer)
                {
                    rolling = PromptPlayerRollAgain(remainingDice);
                }
                else
                {
                    rolling = DealerRollAgain(turnScore, remainingDice);
                    Console.WriteLine(rolling
                        ? "Az osztó az újradobás mellett dönt..."
						: "Az osztó elteszi a pontokat.");
				}

                if (rolling)
                {
                    diceToRoll = remainingDice;
                    Console.WriteLine();
                    continue;
                }

                return turnScore;
            }

            return turnScore;
        }

        private int[] RollDice(int count) => Enumerable.Range(0, count).Select(_ => _rng.Next(1, 7)).ToArray();

        private RollResult ScoreRoll(int[] dice)
        {
            var counts = new int[7];
            foreach (var die in dice)
            {
                counts[die]++;
            }

            var breakdown = new List<string>();
            int score = 0;

            if (IsStraight(counts))
            {
                return new RollResult(dice, 1500, false, "Sor (1-6): 1500", dice.Length);
            }

            if (IsThreePairs(counts))
            {
                return new RollResult(dice, 1500, false, "Három pár: 1500", dice.Length);
            }

            if (IsTwoTriplets(counts))
            {
                return new RollResult(dice, 2500, false, "Kettõ tripla: 2500", dice.Length);
            }

            for (int face = 1; face <= 6; face++)
            {
                if (counts[face] == 6)
                {
                    score += 3000;
                    breakdown.Add($"Hat ugyanolyan ({face}s): 3000");
                    counts[face] = 0;
                    return FinalizeScore(dice, score, breakdown, counts);
                }
            }

            for (int face = 1; face <= 6; face++)
            {
                if (counts[face] == 5)
                {
                    score += 2000;
                    breakdown.Add($"Öt ugyanolyan ({face}s): 2000");
                    counts[face] = 0;
                }
            }

            for (int face = 1; face <= 6; face++)
            {
                if (counts[face] == 4)
                {
                    int fourScore = face == 1 ? 2000 : 1000;
                    score += fourScore;
                    breakdown.Add($"Négy ugyanabból ({face}s): {fourScore}");
                    counts[face] = 0;
                }
            }

            for (int face = 1; face <= 6; face++)
            {
                if (counts[face] >= 3)
                {
                    int tripleScore = face == 1 ? 1000 : face * 100;
                    score += tripleScore;
                    breakdown.Add($"Hármas pár ({face}s): {tripleScore}");
                    counts[face] -= 3;
                }
            }

            if (counts[1] > 0)
            {
                int onesScore = counts[1] * 100;
                score += onesScore;
                breakdown.Add($"Egyes egyesek: {onesScore}");
                counts[1] = 0;
            }

            if (counts[5] > 0)
            {
                int fivesScore = counts[5] * 50;
                score += fivesScore;
                breakdown.Add($"Egyes ötösök: {fivesScore}");
                counts[5] = 0;
            }

            return FinalizeScore(dice, score, breakdown, counts);
        }

        private static RollResult FinalizeScore(int[] dice, int score, List<string> breakdown, int[] counts)
        {
            string detail = breakdown.Count == 0 ? string.Empty : string.Join(", ", breakdown);
            int unscored = counts.Skip(1).Sum();
            int usedDice = dice.Length - unscored;
            return new RollResult(dice, score, score == 0, detail, usedDice);
        }

        private static bool IsStraight(int[] counts) => counts.Skip(1).All(c => c == 1);

        private static bool IsThreePairs(int[] counts) => counts.Skip(1).Count(c => c == 2) == 3;

        private static bool IsTwoTriplets(int[] counts) => counts.Skip(1).Count(c => c == 3) == 2;

        private void DisplayScoreboard(int playerScore, int dealerScore)
        {
            Console.WriteLine("-------------------------------");
            Console.WriteLine($"Te:    {playerScore} pts");
            Console.WriteLine($"Osztó: {dealerScore} pts");
            Console.WriteLine("-------------------------------");
            Console.WriteLine();
        }

        private bool PromptPlayerRollAgain(int remainingDice)
        {
            while (true)
            {
                Console.Write(remainingDice == 6
                    ? "Újradobod mind a hatot? (r = dobás, b = mentés): "
					: $"Dobod a maradék {remainingDice} kockát? (r = dobás, b = mentés): ");
				var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.R)
                {
                    return true;
                }

                if (key == ConsoleKey.B)
                {
                    return false;
                }

                Console.WriteLine("Érvénytelen választás. Nyomd meg az r gombot a dobáshoz, vagy a b gombot a mentéshez.");
            }
        }

        private bool DealerRollAgain(int turnScore, int remainingDice)
        {
            //  dealer banks at 300 or more, or if only one die remains
            if (turnScore >= 300 || remainingDice <= 1) // 400 was too reckeless, switched to 300
            {
                return false;
            }

            return true;
        }

        private record RollResult(int[] Dice, int Score, bool IsFarkle, string Breakdown, int UsedDice);
    }
}