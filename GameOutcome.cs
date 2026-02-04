namespace CasinoMinigames
{
    public enum GameResult
    {
        Win,
        Lose,
        Push
    }

    public record GameOutcome(GameResult Result, int Payout = 0);
}
