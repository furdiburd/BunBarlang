namespace CasinoMinigames
{
    public sealed class Deck
    {
        private readonly List<PlayingCard> _cards;
        private readonly Random _rng;

        public int Count => _cards.Count;

        public Deck(Random? rng = null, int deckCount = 1)
        {
            if (deckCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(deckCount), "Deck count must be positive.");
            }

            _rng = rng ?? new Random();
            _cards = new List<PlayingCard>(52 * deckCount);

            for (int i = 0; i < deckCount; i++)
            {
                AddStandard52CardSet();
            }

            Shuffle();
        }

        public PlayingCard Draw()
        {
            if (_cards.Count == 0)
            {
                throw new InvalidOperationException("Deck is empty.");
            }

            int lastIndex = _cards.Count - 1;
            var card = _cards[lastIndex];
            _cards.RemoveAt(lastIndex);
            return card;
        }

        public void Shuffle()
        {
            for (int i = _cards.Count - 1; i > 0; i--)
            {
                int j = _rng.Next(i + 1);
                (_cards[i], _cards[j]) = (_cards[j], _cards[i]);
            }
        }

        private void AddStandard52CardSet()
        {
            foreach (CardSuit suit in Enum.GetValues<CardSuit>())
            {
                foreach (CardRank rank in Enum.GetValues<CardRank>())
                {
                    _cards.Add(new PlayingCard(suit, rank));
                }
            }
        }
    }
}
