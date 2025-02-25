using System;
using System.Media;

namespace DungeonExplorer
{
    internal class Game
    {
        private Player player;
        private Room room;

        public Game()
        {
            player = new Player("", 100);
            room = new Room();
        }

        public void Start()
        {
            Console.WriteLine("Dungeon Explorer");
            player.Name = GetPlayerName();
            Console.WriteLine("Would you like to start the game? (y/n)");

            if (Console.ReadLine()?.ToLower() == "y")
            {
                Console.WriteLine("Game Started...");
                room.GetDescription(player);
            }
            else
            {
                Console.WriteLine("Exiting game...");
            }
        }

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