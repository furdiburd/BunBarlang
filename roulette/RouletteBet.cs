namespace CasinoMinigames
{
    public sealed class RouletteBet
    {
        private readonly RouletteBetKind _kind;
        private readonly int? _number;
        private readonly RouletteColor? _color;
        private readonly RouletteEvenOdd? _evenOdd;
        private readonly RouletteHighLow? _highLow;
        private readonly RouletteDozen? _dozen;
        private readonly RouletteColumn? _column;

        public RouletteBetKind Kind => _kind;

        private RouletteBet(
            RouletteBetKind kind,
            int? number = null,
            RouletteColor? color = null,
            RouletteEvenOdd? evenOdd = null,
            RouletteHighLow? highLow = null,
            RouletteDozen? dozen = null,
            RouletteColumn? column = null)
        {
            _kind = kind;
            _number = number;
            _color = color;
            _evenOdd = evenOdd;
            _highLow = highLow;
            _dozen = dozen;
            _column = column;
        }

        public static RouletteBet StraightUp(int number)
        {
            return new RouletteBet(RouletteBetKind.StraightUp, number: number);
        }

        public static RouletteBet Color(RouletteColor color)
        {
            return new RouletteBet(RouletteBetKind.Color, color: color);
        }

        public static RouletteBet EvenOdd(RouletteEvenOdd selection) => new(RouletteBetKind.EvenOdd, evenOdd: selection);

        public static RouletteBet HighLow(RouletteHighLow selection) => new(RouletteBetKind.HighLow, highLow: selection);

        public static RouletteBet Dozen(RouletteDozen selection) => new(RouletteBetKind.Dozen, dozen: selection);

        public static RouletteBet Column(RouletteColumn selection) => new(RouletteBetKind.Column, column: selection);

        public bool TryGetProfit(int betAmount, RoulettePocket pocket, out int profit)
        {
            profit = 0;

            switch (_kind)
            {
                case RouletteBetKind.StraightUp:
                    if (pocket.Number == _number)
                    {
                        profit = betAmount * 35;
                        return true;
                    }
                    return false;

                case RouletteBetKind.Color:
                    if (pocket.Number != 0 && pocket.Color == _color)
                    {
                        profit = betAmount;
                        return true;
                    }
                    return false;

                case RouletteBetKind.EvenOdd:
                    if (pocket.Number != 0)
                    {
                        bool isEven = pocket.Number % 2 == 0;
                        bool wantEven = _evenOdd == RouletteEvenOdd.Even;
                        if (isEven == wantEven)
                        {
                            profit = betAmount;
                            return true;
                        }
                    }
                    return false;

                case RouletteBetKind.HighLow:
                    if (pocket.Number != 0)
                    {
                        bool isLow = RouletteRules.IsLow(pocket.Number);
                        bool wantLow = _highLow == RouletteHighLow.Low;
                        if (isLow == wantLow)
                        {
                            profit = betAmount;
                            return true;
                        }
                    }
                    return false;

                case RouletteBetKind.Dozen:
                    if (pocket.Number != 0 && RouletteRules.GetDozen(pocket.Number) == _dozen)
                    {
                        profit = betAmount * 2;
                        return true;
                    }
                    return false;

                case RouletteBetKind.Column:
                    if (pocket.Number != 0 && RouletteRules.GetColumn(pocket.Number) == _column)
                    {
                        profit = betAmount * 2;
                        return true;
                    }
                    return false;

                default:
                    return false;
            }
        }

        public override string ToString()
        {
            return _kind switch
            {
				RouletteBetKind.StraightUp => $"Szám: {_number}",
				RouletteBetKind.Color => $"Szín: {_color}",
				RouletteBetKind.EvenOdd => $"Páros/Páratlan: {_evenOdd}",
				RouletteBetKind.HighLow => $"Alacsony/Magas: {_highLow}",
				RouletteBetKind.Dozen => $"Tucat: {_dozen}",
				RouletteBetKind.Column => $"Oszlop: {_column}",
				_ => _kind.ToString()
			};
        }
    }
}
