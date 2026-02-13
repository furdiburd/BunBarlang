namespace CasinoMinigames
{
    public sealed class RoulettePocket
    {
        public int Number { get; }
        public RouletteColor Color { get; }

        public RoulettePocket(int number, RouletteColor color)
        {
			Number = number;
            Color = color;
        }

        public override string ToString() => $"{Number} ({Color})";
    }
}
