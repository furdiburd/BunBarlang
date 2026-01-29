namespace ConsoleApp1
{
    /// <summary>
    /// Handles the main menu display and navigation.
    /// Supports arrow key navigation and game selection.
    /// </summary>
    public class Menu
    {
        private readonly List<IGame> _games;
        private int _selectedIndex;
        private const string QuitOption = "Quit";

        /// <summary>
        /// Initializes a new instance of the Menu class.
        /// </summary>
        /// <param name="games">List of games to display in the menu.</param>
        public Menu(List<IGame> games)
        {
            _games = games ?? new List<IGame>();
            _selectedIndex = 0;
        }

        /// <summary>
        /// Runs the main menu loop, handling user input and game selection.
        /// </summary>
        public void Run()
        {
            bool running = true;

            while (running)
            {
                DisplayMenu();
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        MoveSelectionUp();
                        break;

                    case ConsoleKey.DownArrow:
                        MoveSelectionDown();
                        break;

                    case ConsoleKey.Enter:
                        running = HandleSelection();
                        break;
                }
            }
        }

        /// <summary>
        /// Displays the menu with the current selection highlighted.
        /// </summary>
        private void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("      CASINO MINIGAME MENU       ");
            Console.WriteLine("=================================");
            Console.WriteLine();
            Console.WriteLine("Use ↑/↓ arrow keys to navigate");
            Console.WriteLine("Press Enter to select");
            Console.WriteLine();

            // Display game options
            for (int i = 0; i < _games.Count; i++)
            {
                if (i == _selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"  ► {_games[i].Name}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"    {_games[i].Name}");
                }
            }

            // Display quit option
            if (_selectedIndex == _games.Count)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"  ► {QuitOption}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"    {QuitOption}");
            }
        }

        /// <summary>
        /// Moves the selection up in the menu.
        /// </summary>
        private void MoveSelectionUp()
        {
            _selectedIndex--;
            if (_selectedIndex < 0)
            {
                _selectedIndex = _games.Count; // Wrap to quit option
            }
        }

        /// <summary>
        /// Moves the selection down in the menu.
        /// </summary>
        private void MoveSelectionDown()
        {
            _selectedIndex++;
            if (_selectedIndex > _games.Count)
            {
                _selectedIndex = 0; // Wrap to first game
            }
        }

        /// <summary>
        /// Handles the user's menu selection.
        /// </summary>
        /// <returns>True if the menu should continue running, false to exit.</returns>
        private bool HandleSelection()
        {
            // Check if quit option is selected
            if (_selectedIndex == _games.Count)
            {
                Console.Clear();
                Console.WriteLine("Thank you for playing! Goodbye!");
                return false;
            }

            // Play the selected game
            _games[_selectedIndex].Play();
            return true;
        }
    }
}
