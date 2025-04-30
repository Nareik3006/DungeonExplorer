using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DungeonExplorer
{
    /// <summary>
    /// Main class responsible for generating the dungeon, managing player input, navigation, and triggering the boss fight.
    /// </summary>
    internal class Game
    {
        private Player player;
        private GameMap currentRoom;
        private Random rand;
        private List<GameMap> allRooms;
        private GameMap[,] grid;
        private GameMap trapdoorRoom;

        /// <summary>
        /// Initializes the game and builds the dungeon map.
        /// </summary>
        public Game()
        {
            player = new Player("", 100);
            rand = new Random();
            allRooms = new List<GameMap>();
            GenerateMaze(3, 3); // Create a 3x3 dungeon grid
        }

        /// <summary>
        /// Generates the maze layout and ensures one chest contains the trapdoor key.
        /// </summary>
        private void GenerateMaze(int width, int height)
        {
            grid = new GameMap[width, height];

            // Create rooms and add to allRooms list
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = new GameMap(rand);
                    allRooms.Add(grid[x, y]);
                }

            // Link rooms to each other
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    GameMap room = grid[x, y];
                    if (x > 0) room.Exits["west"] = grid[x - 1, y];
                    if (x < width - 1) room.Exits["east"] = grid[x + 1, y];
                    if (y > 0) room.Exits["north"] = grid[x, y - 1];
                    if (y < height - 1) room.Exits["south"] = grid[x, y + 1];
                }

            currentRoom = grid[0, 0];

            // Randomly choose a room to contain the trapdoor
            trapdoorRoom = grid[rand.Next(width), rand.Next(height)];
            trapdoorRoom.HasTrapdoor = true;

            // Ensure one chest contains a trapdoor key
            List<Chest> allChests = new List<Chest>();
            foreach (GameMap room in allRooms)
                allChests.AddRange(room.GetChests());

            if (allChests.Count > 0)
            {
                Chest randomChest = allChests[rand.Next(allChests.Count)];
                randomChest.ForceKey();
            }
        }

        /// <summary>
        /// Begins the game, prompts for tutorial, and starts exploration.
        /// </summary>
        public void Start()
        {
            Console.WriteLine("Dungeon Explorer");
            player.Name = GetPlayerName();

            Console.WriteLine();
            Console.WriteLine("Would you like to read the tutorial? (y/n)");
            string input = Console.ReadLine()?.Trim().ToLower();

            if (input == "y")
            {
                ShowTutorial();
            }
            else if (input == "n")
            {
                Console.WriteLine("Skipping tutorial...");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Invalid input. Skipping tutorial by default.");
                Console.ReadLine();
            }

            Console.Clear();
            Console.WriteLine("Game Started...");
            EnterRoom();
        }

        /// <summary>
        /// Displays the tutorial with basic instructions for the game.
        /// </summary>
        private void ShowTutorial()
        {
            Console.Clear();
            Console.WriteLine("TUTORIAL");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Navigate through rooms using 'north', 'south', 'east', 'west'.");

            Console.Write("Open ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("chests ");
            Console.ResetColor();
            Console.Write("to find ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("items ");
            Console.ResetColor();
            Console.Write("like ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("potions");
            Console.ResetColor();
            Console.Write(", ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("weapons");
            Console.ResetColor();
            Console.Write(", and ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("shields");
            Console.ResetColor();
            Console.WriteLine(".");

            Console.Write("Keep an eye out for ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Special Features");
            Console.ResetColor();
            Console.Write(" of rooms to learn new ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("spells");
            Console.ResetColor();
            Console.WriteLine("!");

            Console.Write("Fight ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("monsters");
            Console.ResetColor();
            Console.Write(" by choosing ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("attack");
            Console.ResetColor();
            Console.Write(", ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("magic");
            Console.ResetColor();
            Console.Write(", or ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("items");
            Console.ResetColor();
            Console.WriteLine(".");

            Console.WriteLine("Earn experience (XP) to level up and grow stronger!");

            Console.Write("Look for the trapdoor and use a ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("key");
            Console.ResetColor();
            Console.Write(" to reach the ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("boss room");
            Console.ResetColor();
            Console.WriteLine(".");

            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Press Enter to begin your adventure!");
            Console.ReadLine();
        }

        /// <summary>
        /// Manages the main exploration loop and player movement.
        /// </summary>
        private void EnterRoom()
        {
            bool exploring = true;

            while (exploring)
            {
                currentRoom.IsVisited = true;
                Console.Clear();

                // Trapdoor logic
                if (currentRoom == trapdoorRoom)
                {
                    if (player.Inventory.Exists(item => item is Key))
                    {
                        Console.WriteLine("You find a locked trapdoor. Your key fits perfectly...");
                        Console.WriteLine("Would you like to use the key to descend? (y/n)");
                        string input = Console.ReadLine()?.Trim().ToLower();

                        if (input == "y")
                        {
                            var keyItem = player.Inventory.FirstOrDefault(item => item is Key);
                            if (keyItem != null)
                                player.Inventory.Remove(keyItem);

                            StartBossFight();
                            return;
                        }
                        else
                        {
                            Console.WriteLine("You decide to hold onto the key for now.");
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("You find a locked trapdoor... but you have no key.");
                        Console.ReadLine();
                    }
                }

                currentRoom.GetDescription(player, ShowMinimap);
                ShowMinimap(); // Show again before movement

                Console.WriteLine("Available directions:");
                foreach (var exit in currentRoom.Exits.Keys)
                    Console.WriteLine($"- {exit}");

                Console.WriteLine("Type a direction to move or 'exit' to leave game.");

                string nav = Console.ReadLine()?.ToLower();

                if (nav == "exit")
                {
                    exploring = false;
                    Console.WriteLine("Thanks for playing!");
                    Console.ReadLine();
                }
                else if (currentRoom.Exits.ContainsKey(nav))
                {
                    Console.WriteLine("You leave the room...");
                    Console.ReadLine();
                    currentRoom = currentRoom.Exits[nav];
                }
                else
                {
                    Console.WriteLine("You can't go that way.");
                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// Handles the final boss fight sequence when the player uses the trapdoor key.
        /// </summary>
        private void StartBossFight()
        {
            Console.Clear();
            Console.WriteLine("You descend through the trapdoor into a hidden chamber...");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("An enormous Troll Boss stands ready to crush you!");
            Console.ResetColor();
            Console.ReadLine();

            Monster boss = new Monster("Troll Boss", 150, 12, 20);
            int turnCount = 1;

            while (boss.IsAlive && player.Health > 0)
            {
                Console.Clear();
                UIHelper.ShowBattleHUD(player, boss, turnCount, true);

                string input = Console.ReadLine();

                if (input == "1")
                {
                    player.Attack(boss);
                    Console.ReadLine();
                    if (boss.IsAlive)
                    {
                        boss.TakeTurn(player);
                        Console.ReadLine();
                    }
                    turnCount++;
                }
                else if (input == "2")
                {
                    if (Magic.ChooseSpell(player, boss))
                    {
                        if (boss.IsAlive)
                        {
                            boss.TakeTurn(player);
                            Console.ReadLine();
                        }
                        turnCount++;
                    }
                }
                else if (input == "3")
                {
                    if (player.OpenItemMenu(true))
                    {
                        if (boss.IsAlive)
                        {
                            boss.TakeTurn(player);
                            Console.ReadLine();
                        }
                        turnCount++;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                    Console.ReadLine();
                }
            }

            if (player.Health > 0)
            {
                Console.WriteLine("You have defeated the Troll Boss and completed the dungeon!");
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("Would you like to play again? (y/n)");
                string input = Console.ReadLine()?.Trim().ToLower();

                if (input == "y")
                {
                    Console.Clear();
                    Program.Main(null); // Restarts the game
                }
                else
                {
                    Console.WriteLine("Thanks for playing!");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
            else
            {
                player.HandleDeath();
            }
        }

        /// <summary>
        /// Displays the minimap based on the player's visited rooms.
        /// </summary>
        private void ShowMinimap()
        {
            Console.WriteLine("Minimap:");
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    GameMap room = grid[x, y];
                    if (room == currentRoom)
                        Console.Write(" X ");
                    else if (room.IsVisited)
                        Console.Write(" O ");
                    else
                        Console.Write(" . ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Prompts the player to enter a valid name.
        /// </summary>
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
