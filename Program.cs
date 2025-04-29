using System;

namespace DungeonExplorer
{
    /// <summary>
    /// The main entry point for the Dungeon Explorer game.
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            //Runs predefined tests before the game starts
            Testing.RunTests();

            //Create a new game instance
            Game game = new Game();

            //Start the game
            game.Start();
        }
    }
}
