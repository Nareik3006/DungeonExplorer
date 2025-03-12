using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents the main game logic for Dungeon Explorer.
    /// Handles game initialization and player interactions.
    /// </summary>
    internal class Game
    {
        private Player player;
        private Room room;

        /// <summary>
        /// Initializes the game by creating a player and a starting room.
        /// </summary>
        public Game()
        {
            player = new Player("", 100);
            room = new Room();
        }

        /// <summary>
        /// Starts the game, prompts the player for their name, 
        /// and initiates the room interaction.
        /// </summary>
        public void Start()
        {
            Console.WriteLine("Dungeon Explorer");
            player.Name = GetPlayerName();
            Console.WriteLine("Would you like to start the game? (y/n)");

            if (Console.ReadLine()?.ToLower() == "y")
            {
                Console.WriteLine("Game Started...");
                Console.Clear();
                Console.WriteLine("You enter the first room.");
                room.GetDescription(player);
            }
            else
            {
                Console.WriteLine("Exiting game...");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Prompts the player to enter their name.
        /// Ensures the name is not empty.
        /// </summary>
        /// <returns>The player's name.</returns>
        private string GetPlayerName()
        {
            string name = "";
            while (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Enter your name:");
                name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Invalid name, please try again.");
                }
            }
            return name;
        }


    }
}
