namespace CasinoMinigames
{
    public class BlackjackGame : GameBase
    {
        public override string Name => "Blackjack";

        protected override string Description =>
            "Goal: get as close to 21 as possible without going over. " +
            "Number cards = face value, J/Q/K = 10, Ace = 1 or 11. " +
            "You can Hit (take a card) or Stand. Dealer hits until 17+. " +
            "Blackjack (Ace + 10-value) pays 3:2.";

        private const int Blackjack = 21;
        private readonly Random _rng = new();

        protected override GameOutcome RunGame(int bet)
        {
            var deck = new Deck(_rng, deckCount: 6);
            var playerHand = new BlackjackHand();
            var dealerHand = new BlackjackHand();

            DealStartingHands(deck, playerHand, dealerHand);

            Console.WriteLine("-- Starting hands --");
            RenderHands(playerHand, dealerHand, revealDealerHoleCard: false);

            var immediate = TryResolveImmediateBlackjack(bet, playerHand, dealerHand);
            if (immediate != null)
            {
                RenderHands(playerHand, dealerHand, revealDealerHoleCard: true);
                return immediate;
            }

            while (true)
            {
                if (playerHand.IsBusted)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You busted.");
                    Console.ResetColor();
                    RenderHands(playerHand, dealerHand, revealDealerHoleCard: true);
                    return new GameOutcome(GameResult.Lose);
                }

                var action = PromptPlayerAction(playerHand);
                if (action == PlayerAction.Stand)
                {
                    break;
                }

                var card = deck.Draw();
                playerHand.AddCard(card);
                Console.WriteLine($"You draw: {card}");
                RenderHands(playerHand, dealerHand, revealDealerHoleCard: false);
            }

            Console.WriteLine();
            Console.WriteLine("-- Dealer turn --");
            RenderHands(playerHand, dealerHand, revealDealerHoleCard: true);

            while (ShouldDealerHit(dealerHand))
            {
                var card = deck.Draw();
                dealerHand.AddCard(card);
                Console.WriteLine($"Dealer draws: {card}");
                RenderHands(playerHand, dealerHand, revealDealerHoleCard: true);
            }

            return ResolveOutcome(bet, playerHand, dealerHand);
        }

        private static void DealStartingHands(Deck deck, BlackjackHand playerHand, BlackjackHand dealerHand)
        {
            playerHand.AddCard(deck.Draw());
            dealerHand.AddCard(deck.Draw());
            playerHand.AddCard(deck.Draw());
            dealerHand.AddCard(deck.Draw());
        }
        private static GameOutcome? TryResolveImmediateBlackjack(int bet, BlackjackHand playerHand, BlackjackHand dealerHand)
        {
            bool playerBj = playerHand.IsBlackjack;
            bool dealerBj = dealerHand.IsBlackjack;

            if (!playerBj && !dealerBj)
            {
                return null;
            }

            if (playerBj && dealerBj)
            {
                Console.WriteLine("Both have Blackjack!");
                return new GameOutcome(GameResult.Push);
            }

            if (playerBj)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Blackjack! You win.");
                Console.ResetColor();
                int payout = (bet * 3) / 2;
                return new GameOutcome(GameResult.Win, payout);
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Dealer has Blackjack. You lose.");
            Console.ResetColor();
            return new GameOutcome(GameResult.Lose);
        }

        private static bool ShouldDealerHit(BlackjackHand dealerHand)
        {
            // dealer logic: hit until 17 or more (stands on soft 17 as well)
            return dealerHand.BestValue < 17;
        }

        private static GameOutcome ResolveOutcome(int bet, BlackjackHand playerHand, BlackjackHand dealerHand)
        {
            if (dealerHand.IsBusted)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Dealer busted. You win!");
                Console.ResetColor();
                return new GameOutcome(GameResult.Win, bet);
            }

            int playerValue = playerHand.BestValue;
            int dealerValue = dealerHand.BestValue;

            Console.WriteLine($"Final totals -> You: {playerValue}, Dealer: {dealerValue}");

            if (playerValue > dealerValue)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You win!");
                Console.ResetColor();
                return new GameOutcome(GameResult.Win, bet);
            }

            if (playerValue < dealerValue)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You lose.");
                Console.ResetColor();
                return new GameOutcome(GameResult.Lose);
            }

            Console.WriteLine("Push.");
            return new GameOutcome(GameResult.Push);
        }

        private static void RenderHands(BlackjackHand playerHand, BlackjackHand dealerHand, bool revealDealerHoleCard)
        {
            Console.WriteLine();

            Console.WriteLine($"Your hand ({FormatTotals(playerHand)}): {playerHand}");

            if (revealDealerHoleCard)
            {
                Console.WriteLine($"Dealer hand ({FormatTotals(dealerHand)}): {dealerHand}");
            }
            else
            {
                string dealerShown = dealerHand.Cards.Count == 0 ? "(no cards)" : dealerHand.Cards[0].ToString();
                Console.WriteLine($"Dealer shows: {dealerShown} + [hidden]");
            }

            Console.WriteLine();
        }

        private static string FormatTotals(BlackjackHand hand)
        {
            var totals = hand.GetAllTotals().Where(t => t <= Blackjack).ToArray();
            if (totals.Length == 0)
            {
                return hand.BestValue.ToString();
            }

            return totals.Length == 1 ? totals[0].ToString() : string.Join("/", totals);
        }

        private static PlayerAction PromptPlayerAction(BlackjackHand playerHand)
        {
            while (true)
            {
                Console.Write($"Hit or Stand? (h/s) Current: {playerHand.BestValue} : ");
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.H)
                {
                    return PlayerAction.Hit;
                }

                if (key == ConsoleKey.S)
                {
                    return PlayerAction.Stand;
                }

                Console.WriteLine("Invalid choice. Press 'h' or 's'.");
            }
        }

        private enum PlayerAction
        {
            Hit,
            Stand
        }
    }
}
