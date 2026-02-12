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
				throw new ArgumentOutOfRangeException(nameof(number), "A rulett számnak 0 és 36 között kell lennie.");
			}

			if (number == 0 && color != RouletteColor.Green)
			{
				throw new ArgumentException("A 0-s zsebnek zöldnek kell lennie.", nameof(color));
			}

			if (number != 0 && color == RouletteColor.Green)
			{
				throw new ArgumentException("Csak a 0-s zseb lehet zöld.", nameof(color));
			}


			Number = number;
            Color = color;
        }

        public override string ToString() => $"{Number} ({Color})";
    }
}
