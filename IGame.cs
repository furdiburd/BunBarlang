namespace CasinoMinigames
{
    /// <summary>
    /// Interface for all casino games. Implement this to add a new game to the menu.
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// Display name shown in the menu.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Runs the game loop when selected from the menu.
        /// </summary>
        void Play();
    }
}
