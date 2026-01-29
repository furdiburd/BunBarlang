namespace ConsoleApp1
{
    /// <summary>
    /// Interface for all casino games.
    /// Implement this interface to add new games to the casino.
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// Gets the name of the game as displayed in the menu.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Starts the game. This method is called when the user selects the game from the menu.
        /// </summary>
        void Play();
    }
}
