namespace CasinoMinigames
{
    public static class RouletteRules
    {
        private static readonly HashSet<int> RedNumbers = new()
        {
            1, 3, 5, 7, 9,
            12, 14, 16, 18,
            19, 21, 23, 25, 27,
            30, 32, 34, 36
        };

        public static RouletteColor GetColor(int number)
        {
            if (number == 0)
            {
                return RouletteColor.Green;
            }

            return RedNumbers.Contains(number) ? RouletteColor.Red : RouletteColor.Black;
        }

        public static bool IsLow(int number) => number >= 1 && number <= 18;

        public static bool IsHigh(int number) => number >= 19 && number <= 36;

        public static RouletteDozen GetDozen(int number)
        {
            if (number < 1 || number > 36)
            {
                throw new ArgumentOutOfRangeException(nameof(number));
            }

            if (number <= 12) return RouletteDozen.First;
            if (number <= 24) return RouletteDozen.Second;
            return RouletteDozen.Third;
        }

        public static RouletteColumn GetColumn(int number)
        {
            if (number < 1 || number > 36)
            {
                throw new ArgumentOutOfRangeException(nameof(number));
            }

            int mod = number % 3;
            return mod switch
            {
                1 => RouletteColumn.First,
                2 => RouletteColumn.Second,
                _ => RouletteColumn.Third
            };
        }
    }
}
