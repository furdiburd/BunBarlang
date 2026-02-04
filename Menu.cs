namespace CasinoMinigames
{
    public class Menu
    {
        private readonly List<GameBase> _games;
        private readonly Player _player;
        private int _selectedIndex;
        private const string QuitOption = "Quit";

        // Initializes a new instance of the Menu class.
        public Menu(List<GameBase> games, Player player)
        {
            if (games == null)
            {
                throw new ArgumentNullException(nameof(games), "Games list cannot be null.");
            }

            if (games.Count == 0)
            {
                throw new ArgumentException("Games list cannot be empty. At least one game must be provided.", nameof(games));
            }

            _games = games;
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _selectedIndex = 0;
        }

        public void Run()
        {
            bool running = true;

            while (running)
            {
                if (_player.Credits <= 0)
                {
                    ShowBankruptScreen();
                    return;
                }

                DisplayMenu();
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        AdjustSelection(-1);
                        break;

                    case ConsoleKey.DownArrow:
                        AdjustSelection(1);
                        break;

                    case ConsoleKey.Enter:
                        running = HandleSelection();
                        break;
                }
            }
        }

        // Display the menu with the current selection highlighted.
        private void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("      CASINO MINIGAME MENU       ");
            Console.WriteLine("=================================");
            Console.WriteLine();
            Console.WriteLine($"Credits: {_player.Credits}");
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

        // Moves the selection up or down, wrapping across game entries and the quit option
        private void AdjustSelection(int delta)
        {
            int totalOptions = _games.Count + 1; // games + Quit
            _selectedIndex = (_selectedIndex + delta + totalOptions) % totalOptions;
        }

        private bool HandleSelection()
        {
            if (_selectedIndex == _games.Count)
            {
                Console.Clear();
                Console.WriteLine("Thank you for playing! Goodbye!");
                return false;
            }

            // Play the selected game
            _games[_selectedIndex].Play(_player);
            return true;
        }

        private void ShowBankruptScreen()
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("       YOU WENT BANKRUPT         ");
            Console.WriteLine("=================================");
            Console.WriteLine();
            Console.WriteLine("You have no credits left. Thanks for playing!");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit..."); // Better than instant exit
            Console.ReadKey(true);
        }
    }
}
