# Casino Minigame Collection

A modular, object-oriented C# console application for casino minigames.

## Features

- **Modular Design**: Uses object-oriented principles with clear separation of concerns
- **Extensible Architecture**: Easy to add new games via the `IGame` interface
- **Interactive Menu**: Text-based menu with arrow key navigation (↑/↓)
- **Multiple Games**: Includes placeholders for Poker, Roulette, and Blackjack
- **Clean Code**: Minimal main function that delegates to specialized classes

## Architecture

### Classes

- **`Program.cs`**: Entry point - initializes games and starts the menu (minimal code)
- **`IGame`**: Interface that all games must implement for consistency
- **`Menu`**: Handles menu display, navigation, and game selection
- **`PokerGame`**: Poker game implementation (placeholder)
- **`RouletteGame`**: Roulette game implementation (placeholder)
- **`BlackjackGame`**: Blackjack game implementation (placeholder)

### Class Diagram

```
IGame (interface)
  ├── PokerGame
  ├── RouletteGame
  └── BlackjackGame

Menu
  └── Uses List<IGame>

Program (entry point)
  └── Creates games and Menu
```

## How to Use

### Building

```bash
dotnet build
```

### Running

```bash
dotnet run
```

### Navigation

- Use **↑/↓ arrow keys** to navigate the menu
- Press **Enter** to select a game or quit
- When in a game, press **any key** to return to the main menu

## Adding New Games

To add a new casino game:

1. Create a new class that implements `IGame`:
   ```csharp
   public class MyNewGame : IGame
   {
       public string Name => "My New Game";
       
       public void Play()
       {
           // Game implementation
       }
   }
   ```

2. Add the game to the list in `Program.cs`:
   ```csharp
   var games = new List<IGame>
   {
       new PokerGame(),
       new RouletteGame(),
       new BlackjackGame(),
       new MyNewGame()  // Add your game here
   };
   ```

That's it! The menu will automatically include your new game.

## Requirements Met

✅ Base menu with game selection  
✅ Arrow key navigation (up/down)  
✅ Placeholder options for Poker, Roulette, and Blackjack  
✅ Quit option  
✅ Object-oriented design with separate classes  
✅ IGame interface for extensibility  
✅ Minimal main function (just entry point)  
✅ Games return to main menu after completion  
✅ Clear comments and documentation  

## Technical Details

- **Framework**: .NET 8.0
- **Language**: C# with nullable reference types enabled
- **Design Pattern**: Strategy pattern via IGame interface
- **User Input**: Console.ReadKey() for responsive arrow key navigation
