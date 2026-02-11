namespace CasinoMinigames
{
    public sealed class PlayingCard
    {
        private readonly CardSuit _suit;
        private readonly CardRank _rank;

        public CardSuit Suit => _suit;
        public CardRank Rank => _rank;

        public PlayingCard(CardSuit suit, CardRank rank)
        {
            _suit = suit;
            _rank = rank;
        }

        public override string ToString()
        {
            return $"{RankToDisplay(_rank)} of {SuitToDisplay(_suit)}";
        }

        private static string SuitToDisplay(CardSuit suit) => suit switch
        {
            CardSuit.Clubs => "Clubs",
            CardSuit.Diamonds => "Diamonds",
            CardSuit.Hearts => "Hearts",
            CardSuit.Spades => "Spades",
            _ => suit.ToString()
        };

        private static string RankToDisplay(CardRank rank) => rank switch
        {
            CardRank.Jack => "Jack",
            CardRank.Queen => "Queen",
            CardRank.King => "King",
            CardRank.Ace => "Ace",
            _ => ((int)rank).ToString()
        };
    }
}
