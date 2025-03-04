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
            Testing.RunTests(); //DEBUG TESTS
            Game game = new Game();
            game.Start();
        }
    }
}
