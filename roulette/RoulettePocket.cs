namespace CasinoMinigames
{
    public sealed class RoulettePocket
    {
        public int Number { get; }
        public RouletteColor Color { get; }

        public RoulettePocket(int number, RouletteColor color)
        {
            if (number < 0 || number > 36)
            {
                throw new ArgumentOutOfRangeException(nameof(number), "Roulette number must be between 0 and 36.");
            }

            if (number == 0 && color != RouletteColor.Green)
            {
                throw new ArgumentException("Pocket 0 must be Green.", nameof(color));
            }

            if (number != 0 && color == RouletteColor.Green)
            {
                throw new ArgumentException("Only pocket 0 can be Green.", nameof(color));
            }

            Number = number;
            Color = color;
        }

        public override string ToString() => $"{Number} ({Color})";
    }
}
