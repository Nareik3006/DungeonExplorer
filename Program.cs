using System;

namespace DungeonExplorer
{
    /// <summary>
    /// The main entry point for the Dungeon Explorer game.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The Main method initializes and starts the game.
        /// </summary>
        static void Main(string[] args)
        {
            // Runs predefined tests before the game starts
            Testing.RunTests(); // DEBUG TESTS

            // Create a new game instance
            Game game = new Game();

            // Start the game
            game.Start();
        }
    }
}
