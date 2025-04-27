using System;
using System.Collections.Generic;
using System.Threading;

namespace DungeonExplorer
{
    internal class Game
    {
        private Player player;
        private Room currentRoom;
        private Random rand;
        private List<Room> allRooms;
        private Room[,] grid;
        private Room trapdoorRoom;

        public Game()
        {
            player = new Player("", 100);
            rand = new Random();
            allRooms = new List<Room>();
            GenerateMaze(3, 3);
        }

        private void GenerateMaze(int width, int height)
        {
            grid = new Room[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = new Room(rand);
                    allRooms.Add(grid[x, y]);
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Room room = grid[x, y];
                    if (x > 0) room.Exits["west"] = grid[x - 1, y];
                    if (x < width - 1) room.Exits["east"] = grid[x + 1, y];
                    if (y > 0) room.Exits["north"] = grid[x, y - 1];
                    if (y < height - 1) room.Exits["south"] = grid[x, y + 1];
                }
            }

            currentRoom = grid[0, 0];

            // Select trapdoor room randomly
            trapdoorRoom = grid[rand.Next(width), rand.Next(height)];
            trapdoorRoom.HasTrapdoor = true;

            // ✅ Guarantee that one Chest has the Trapdoor Key
            List<Chest> allChests = new List<Chest>();

            foreach (Room room in allRooms)
            {
                allChests.AddRange(room.GetChests());
            }

            if (allChests.Count > 0)
            {
                Chest randomChest = allChests[rand.Next(allChests.Count)];
                randomChest.ForceKey();
            }
        }


        public void Start()
        {
            Console.WriteLine("Dungeon Explorer");
            player.Name = GetPlayerName();

            string input = "";
            while (input != "y" && input != "n")
            {
                Console.WriteLine("Would you like to start the game? (y/n)");
                input = Console.ReadLine()?.Trim().ToLower();

                if (input == "y")
                {
                    Console.Clear();
                    Console.WriteLine("Game Started...");
                    EnterRoom();
                }
                else if (input == "n")
                {
                    Console.WriteLine("Exiting game...");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                }
            }
        }

        private void EnterRoom()
        {
            bool exploring = true;

            while (exploring)
            {
                currentRoom.IsVisited = true;
                Console.Clear();

                if (currentRoom == trapdoorRoom)
                {
                    if (player.Inventory.Exists(item => item is Key))
                    {
                        Console.WriteLine("You find a locked trapdoor. Your key fits perfectly...");
                        Console.ReadLine();
                        StartBossFight();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("You find a locked trapdoor... but you have no key.");
                        Console.ReadLine();
                    }
                }

                currentRoom.GetDescription(player, ShowMinimap);

                //Show the Minimap again when choosing movement
                ShowMinimap();

                Console.WriteLine("Available directions:");
                foreach (var exit in currentRoom.Exits.Keys)
                {
                    Console.WriteLine($"- {exit}");
                }
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

                    turnCount++; // ✅ Only after an attack
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

                        turnCount++; // ✅ Only if a spell was cast
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

                        turnCount++; // ✅ Only if an item was used
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                    Console.ReadLine();
                    // ❌ No turn increment here
                }
            }

            if (player.Health > 0)
            {
                Console.WriteLine("You have defeated the Troll Boss and completed the dungeon!");
                Console.ReadLine();
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("You were defeated...");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        private void ShowMinimap()
        {
            Console.WriteLine("Minimap:");
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    Room room = grid[x, y];
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
