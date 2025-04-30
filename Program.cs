using System;

namespace DungeonExplorer
{
    /// <summary>
    /// The main entry point for the Dungeon Explorer game.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Launches the game by running tests and starting the main game loop.
        /// </summary>
        public static void Main(string[] args)
        {
            // Runs predefined automated tests to verify game logic
            Testing.RunTests();

            // Create a new game instance
            Game game = new Game();

            // Start the game with player setup and tutorial option
            game.Start();
        }
    }
}
