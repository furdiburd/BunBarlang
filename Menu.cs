namespace CasinoMinigames
{
     // Rábai Miklós
    public class Menu
    {
        public void Run(List<GameBase> games, Player player)
        {
            if (games == null || games.Count == 0)
            {
                Console.WriteLine("Nincs elérhető játék.");
                return;
            }

            if (player == null)
            {
                Console.WriteLine("Hiányzik a játékos.");
                return;
            }

            while (true)
            {
                if (player.Credits <= 0)
                {
                    Console.WriteLine("Nincs több kredited. Köszönjük a játékot!");
                    Console.ReadKey(true);
                    return;
                }

                int choice = PromptSelection(games, player.Credits);
                if (choice == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Köszönjük a játékot! Viszlát!");
                    return;
                }

                games[choice - 1].Play(player);
            }
        }

        private static int PromptSelection(List<GameBase> games, int credits)
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("        KASZINÓ JÁTÉKMENÜ        ");
            Console.WriteLine("=================================");
            Console.WriteLine();
            Console.WriteLine($"Kreditek: {credits}");
            Console.WriteLine();

            for (int i = 0; i < games.Count; i++)
            {
                Console.WriteLine($"  {i + 1}) {games[i].Name}");
            }

            Console.WriteLine("  0) Kilépés");
            Console.WriteLine();
            Console.Write("Válassz: ");

            while (true)
            {
                var input = Console.ReadLine();
                if (int.TryParse(input, out int value) && value >= 0 && value <= games.Count)
                {
                    return value;
                }

                Console.Write($"Érvénytelen. Add meg 0-{games.Count}: ");
            }
        }
    }
}
