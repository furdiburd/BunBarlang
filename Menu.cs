namespace CasinoMinigames
{
    public class Menu
    {
        private readonly List<GameBase> _games;
        private readonly Player _player;
        private int _selectedIndex;
        private const string QuitOption = "Kilépés";

        public Menu(List<GameBase> games, Player player)
        {
            if (games == null)
            {
                throw new ArgumentNullException(nameof(games), "A játék lista nem lehet null.");
            }

            if (games.Count == 0)
            {
                throw new ArgumentException("A játlk lista nem lehet üres, legalább 1 kell.", nameof(games));
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

        private void DisplayMenu()
        {
			Console.Clear();
			Console.WriteLine("=================================");
			Console.WriteLine("      KASZINÓ MINIJÁTÉK MENÜ     ");
			Console.WriteLine("=================================");
			Console.WriteLine();
			Console.WriteLine($"Egyenleg: {_player.Credits}");
			Console.WriteLine("Használja a ↑/↓ nyilakat a navigáláshoz");
			Console.WriteLine("Nyomjon Entert a kiválasztáshoz");
			Console.WriteLine();

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
				Console.WriteLine("Köszönjük a játékot! Viszlát!");
				return false;
            }

            // play the selected game
            _games[_selectedIndex].Play(_player);
            return true;
        }

        private void ShowBankruptScreen()
        {
			Console.Clear();
			Console.WriteLine("=================================");
			Console.WriteLine("         CSŐDBE MENTÉL           ");
			Console.WriteLine("=================================");
			Console.WriteLine();
			Console.WriteLine("Elfogyott az összes kredited. Köszönjük a játékot!");
			Console.WriteLine();
			Console.WriteLine("Nyomj meg egy gombot a kilépéshez..."); // nem szép ,de jobb, mint az azonnali kilépés
			Console.ReadKey(true);

		}
	}
}
