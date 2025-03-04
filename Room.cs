using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_ASSIGNMENT_TEST2
{
    using System;
    using System.Collections.Generic;

    namespace OOP_ASSIGNMENT_TEST
    {
        internal class Room
        {
            private string firstChestItem;
            private string secondChestItem;
            private bool firstChestLooted = false;
            private bool secondChestLooted = false;

            private List<string> chestItems = new List<string> { "Potion", "Sword", "Shield", "Key" };

            public Room()
            {
                Random random = new Random();
                firstChestItem = chestItems[random.Next(chestItems.Count)];
                secondChestItem = chestItems[random.Next(chestItems.Count)];
            }

            public void GetDescription(Player player)
            {
                Console.WriteLine("The room is dark and eerie...\nA skeleton lays in one corner, while two golden chests glimmer in another.");

                while (true)
                {
                    player.Stats();
                    Console.WriteLine("====================");
                    Console.WriteLine("What would you like to inspect?\n" +
                                      "1. The Skeleton\n" +
                                      "2. Open Chest #1\n" +
                                      "3. Open Chest #2\n" +
                                      "4. Move to the next room");
                    string choice = Console.ReadLine();
                    Console.WriteLine("====================");

                    if (choice == "1")
                    {
                        Console.WriteLine("The skeleton holds nothing of value.");
                    }
                    else if (choice == "2")
                    {
                        if (!firstChestLooted)
                        {
                            Console.WriteLine($"You open Chest #1 and find: {firstChestItem}");
                            player.PickUpItem(firstChestItem);
                            firstChestLooted = true;
                        }
                        else
                        {
                            Console.WriteLine("Chest #1 is empty.");
                        }
                    }
                    else if (choice == "3")
                    {
                        if (!secondChestLooted)
                        {
                            Console.WriteLine($"You open Chest #2 and find: {secondChestItem}");
                            player.PickUpItem(secondChestItem);
                            secondChestLooted = true;
                        }
                        else
                        {
                            Console.WriteLine("Chest #2 is empty.");
                        }
                    }
                    else if (choice == "4")
                    {
                        Console.WriteLine("You leave the room.");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input, try again.");
                    }

                }
            }
        }
    }
}
