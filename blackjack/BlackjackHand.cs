namespace CasinoMinigames
{
    public sealed class BlackjackHand
    {
        private readonly List<PlayingCard> _cards = new();

        public IReadOnlyList<PlayingCard> Cards => _cards.AsReadOnly();

        public int BestValue => CalculateBestValue();

        public bool IsBusted => CalculateMinimumValue() > 21;

        public bool IsBlackjack => _cards.Count == 2 && BestValue == 21;

        public bool IsSoft
        {
            get
            {
                if (_cards.All(c => c.Rank != CardRank.Ace))
                {
                    return false;
                }

                int min = CalculateMinimumValue();
                int best = BestValue;
                return best <= 21 && best != min;
            }
        }

        public void AddCard(PlayingCard card)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            _cards.Add(card);
        }

        public override string ToString() => string.Join(", ", _cards.Select(c => c.ToString()));

        public IEnumerable<int> GetAllTotals()
        {
            IEnumerable<int> totals = new[] { 0 };

            foreach (var card in _cards)
            {
                totals = AddCardToTotals(totals, card);
            }

            return totals.Distinct().OrderBy(t => t);
        }

        private static IEnumerable<int> AddCardToTotals(IEnumerable<int> totals, PlayingCard card)
        {
            if (card.Rank == CardRank.Ace)
            {
                return totals.Select(t => t + 1).Concat(totals.Select(t => t + 11));
            }

            int value = GetBlackjackValue(card.Rank);
            return totals.Select(t => t + value);
        }

        private static int GetBlackjackValue(CardRank rank)
        {
            if (rank == CardRank.Ace)
            {
                return 11;
            }

            if (rank == CardRank.Jack || rank == CardRank.Queen || rank == CardRank.King)
            {
                return 10;
            }

            return (int)rank;
        }

        private int CalculateBestValue()
        {
            var totals = GetAllTotals().ToArray();
            int bestUnderOrEqual21 = totals.Where(t => t <= 21).DefaultIfEmpty(-1).Max();
            if (bestUnderOrEqual21 != -1)
            {
                return bestUnderOrEqual21;
            }

            return totals.Min();
        }

        private int CalculateMinimumValue()
        {
            int sum = 0;
            foreach (var card in _cards)
            {
                if (card.Rank == CardRank.Ace)
                {
                    sum += 1;
                }
                else if (card.Rank == CardRank.Jack || card.Rank == CardRank.Queen || card.Rank == CardRank.King)
                {
                    sum += 10;
                }
                else
                {
                    sum += (int)card.Rank;
                }
            }

            return sum;
        }
    }
}
