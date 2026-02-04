namespace CasinoMinigames
{
    /// <summary>
    /// Kingdom Come: Deliverance style Farkle.
    /// First to 2000 points wins. Each roll is auto-scored.
    /// </summary>
    public class FarkleGame : GameBase
    {
        public override string Name => "Farkle";

        protected override string Description =>
            "Roll 6 dice. Scoring: straight (1-6) 1500, three pairs 1500, two triplets 2500, " +
            "six of a kind 3000, five of a kind 2000, four of a kind 1000 (ones 2000), " +
            "three of a kind ones 1000 / others face*100, single 1 = 100, single 5 = 50. " +
            "No scoring dice = Farkle (0 for the turn). First to 2000 points wins.";

        private const int TargetScore = 2000;
        private readonly Random _rng = new();

        protected override void RunGame()
        {
            int playerScore = 0;
            int dealerScore = 0;
            int turn = 1;

            while (playerScore < TargetScore && dealerScore < TargetScore)
            {
                Console.WriteLine($"-- Turn {turn} --");
                playerScore += TakeTurn("You");
                DisplayScoreboard(playerScore, dealerScore);
                if (playerScore >= TargetScore)
                {
                    break;
                }

                dealerScore += TakeTurn("Dealer");
                DisplayScoreboard(playerScore, dealerScore);

                turn++;
                Console.WriteLine("Press any key for the next turn...");
                Console.ReadKey(true);
                Console.Clear();
                RenderHeader();
                if (!string.IsNullOrWhiteSpace(Description))
                {
                    Console.WriteLine(Description);
                    Console.WriteLine();
                }
            }

            Console.WriteLine(playerScore >= TargetScore ? "You win! ??" : "Dealer wins. Better luck next time!");
        }

        private int TakeTurn(string name)
        {
            Console.WriteLine($"{name} rolls...");
            var dice = RollDice(6);
            var result = ScoreRoll(dice);

            Console.WriteLine($"Rolled: {string.Join(", ", dice)}");

            if (result.IsFarkle)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Farkle! No scoring dice this turn.");
                Console.ResetColor();
                Console.WriteLine();
                return 0;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Score this roll: {result.Score}");
            Console.ResetColor();

            if (!string.IsNullOrWhiteSpace(result.Breakdown))
            {
                Console.WriteLine($"Details: {result.Breakdown}");
            }

            Console.WriteLine();
            return result.Score;
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
                return new RollResult(dice, 1500, false, "Straight (1-6): 1500");
            }

            if (IsThreePairs(counts))
            {
                return new RollResult(dice, 1500, false, "Three pairs: 1500");
            }

            if (IsTwoTriplets(counts))
            {
                return new RollResult(dice, 2500, false, "Two triplets: 2500");
            }

            for (int face = 1; face <= 6; face++)
            {
                if (counts[face] == 6)
                {
                    score += 3000;
                    breakdown.Add($"Six of a kind ({face}s): 3000");
                    counts[face] = 0;
                    return FinalizeScore(dice, score, breakdown);
                }
            }

            for (int face = 1; face <= 6; face++)
            {
                if (counts[face] == 5)
                {
                    score += 2000;
                    breakdown.Add($"Five of a kind ({face}s): 2000");
                    counts[face] = 0;
                }
            }

            for (int face = 1; face <= 6; face++)
            {
                if (counts[face] == 4)
                {
                    int fourScore = face == 1 ? 2000 : 1000;
                    score += fourScore;
                    breakdown.Add($"Four of a kind ({face}s): {fourScore}");
                    counts[face] = 0;
                }
            }

            for (int face = 1; face <= 6; face++)
            {
                if (counts[face] >= 3)
                {
                    int tripleScore = face == 1 ? 1000 : face * 100;
                    score += tripleScore;
                    breakdown.Add($"Three of a kind ({face}s): {tripleScore}");
                    counts[face] -= 3;
                }
            }

            if (counts[1] > 0)
            {
                int onesScore = counts[1] * 100;
                score += onesScore;
                breakdown.Add($"Single 1s: {onesScore}");
                counts[1] = 0;
            }

            if (counts[5] > 0)
            {
                int fivesScore = counts[5] * 50;
                score += fivesScore;
                breakdown.Add($"Single 5s: {fivesScore}");
                counts[5] = 0;
            }

            return FinalizeScore(dice, score, breakdown);
        }

        private static RollResult FinalizeScore(int[] dice, int score, List<string> breakdown)
        {
            string detail = breakdown.Count == 0 ? string.Empty : string.Join(", ", breakdown);
            return new RollResult(dice, score, score == 0, detail);
        }

        private static bool IsStraight(int[] counts) => counts.Skip(1).All(c => c == 1);

        private static bool IsThreePairs(int[] counts) => counts.Skip(1).Count(c => c == 2) == 3;

        private static bool IsTwoTriplets(int[] counts) => counts.Skip(1).Count(c => c == 3) == 2;

        private void DisplayScoreboard(int playerScore, int dealerScore)
        {
            Console.WriteLine("-------------------------------");
            Console.WriteLine($"You:    {playerScore} pts");
            Console.WriteLine($"Dealer: {dealerScore} pts");
            Console.WriteLine("-------------------------------");
            Console.WriteLine();
        }

        private record RollResult(int[] Dice, int Score, bool IsFarkle, string Breakdown);
    }
}