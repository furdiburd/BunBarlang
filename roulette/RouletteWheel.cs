namespace CasinoMinigames
{
    public sealed class RouletteWheel
    {
        private readonly Random _rng;
        private readonly IReadOnlyList<RoulettePocket> _pockets;

        public RouletteWheel(Random rng)
        {
            _rng = rng;
            _pockets = CreateEuropeanPockets();
        }

        public RoulettePocket Spin()
        {
            int index = _rng.Next(0, _pockets.Count);
            return _pockets[index];
        }

        private static IReadOnlyList<RoulettePocket> CreateEuropeanPockets()
        {
            var list = new List<RoulettePocket>(37);
            list.Add(new RoulettePocket(0, RouletteColor.Green));

            for (int number = 1; number <= 36; number++)
            {
                list.Add(new RoulettePocket(number, RouletteRules.GetColor(number)));
            }

            return list;
        }
    }
}
