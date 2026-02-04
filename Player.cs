namespace CasinoMinigames
{
    // Player class to manage credits and betting
    public class Player
    {
        public int Credits { get; private set; }

        public Player(int startingCredits)
        {
            Credits = startingCredits;
        }

        public bool CanBet(int amount) => amount > 0 && amount <= Credits;

        public void AddCredits(int amount)
        {
            if (amount < 0) return;
            Credits += amount;
        }

        public void DeductCredits(int amount)
        {
            if (amount < 0) return;
            Credits = Math.Max(0, Credits - amount);
        }
    }
}
