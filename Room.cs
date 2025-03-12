using System;
using System.Collections.Generic;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents a room in the dungeon.
    /// Contains interactable elements such as chests.
    /// </summary>
    internal class Room
    {
        private string firstChestItem;
        private string secondChestItem;
        private bool firstChestLooted = false;
        private bool secondChestLooted = false;
        private List<string> chestItems = new List<string> { "Potion", "Sword", "Shield", "Key" };

        /// <summary>
        /// Initializes the room with randomly generated chest items.
        /// </summary>
        public Room()
        {
            Random random = new Random();
            firstChestItem = chestItems[random.Next(chestItems.Count)];
            secondChestItem = chestItems[random.Next(chestItems.Count)];
        }

        /// <summary>
        /// Displays the room description and allows the player to interact with objects.
        /// </summary>
        /// <param name="player">The player currently in the room.</param>
        public void GetDescription(Player player)
        {
            Console.WriteLine("The room is dark and eerie...\nA skeleton lays in one corner, while two golden chests glimmer in another.");

            while (true)
            {
                // Display player's current stats
                player.Stats();
                Console.WriteLine("====================");

                // Provide interaction options
                Console.WriteLine("What would you like to inspect?\n" +
                                  "1. The Skeleton\n" +
                                  "2. Open Chest #1\n" +
                                  "3. Open Chest #2\n" +
                                  "4. Move to the next room");
                string choice = Console.ReadLine();
                Console.WriteLine("====================");

                switch (choice)
                {
                    case "1":
                        // The player inspects the skeleton, but it has no useful items.
                        Console.WriteLine("The skeleton holds nothing of value.");
                        Console.ReadLine();
                        Console.Clear();
                        break;

                    case "2":
                        // The player attempts to open Chest #1.
                        if (!firstChestLooted)
                        {
                            Console.WriteLine($"You open Chest #1 and find: {firstChestItem}");
                            // Add item to inventory
                            player.PickUpItem(firstChestItem);
                            player.GainXP(10);  
                            // Mark the chest as looted
                            firstChestLooted = true; 
                            Console.ReadLine();
                            Console.Clear();
                        }
                        else
                        {
                            // If the chest has already been looted, inform the player.
                            Console.WriteLine("Chest #1 is empty.");
                            Console.ReadLine();
                            Console.Clear();
                        }
                        break;

                    case "3":
                        // The player attempts to open Chest #2.
                        if (!secondChestLooted)
                        {
                            Console.WriteLine($"You open Chest #2 and find: {secondChestItem}");
                            // Add item to inventory
                            player.PickUpItem(secondChestItem);
                            player.GainXP(10);  
                            // Mark the chest as looted
                            secondChestLooted = true; 
                            Console.ReadLine();
                            Console.Clear();
                        }
                        else
                        {
                            // If the chest has already been looted, inform the player.
                            Console.WriteLine("Chest #2 is empty.");
                            Console.ReadLine();
                            Console.Clear();
                        }
                        break;

                    case "4":
                        // The player decides to leave the room.
                        Console.WriteLine("You leave the room.");
                        Console.ReadLine();
                        Console.WriteLine("Game End");
                        Console.ReadLine();
                        Console.Clear();
                        return;

                    default:
                        // If the player enters an invalid choice, prompt them to try again.
                        Console.WriteLine("Invalid input, try again.");
                        break;
                }
            }
        }
    }
}
