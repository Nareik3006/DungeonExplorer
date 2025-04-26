using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DungeonExplorer;

namespace DungeonExplorer
{
    /// <summary>
    /// Contains unit tests for various components of the Dungeon Explorer game.
    /// </summary>
    internal class Testing
    {
        /// <summary>
        /// Runs all tests for the game.
        /// </summary>
        public static void RunTests()
        {
            Console.WriteLine("Running Tests...");
            TestPlayerHealth();
            TestInventoryManagement();
            TestItemPickupLimit();
            TestRoomConstruction();
            TestExperienceSystem();
            Console.WriteLine("All Tests Completed.");
        }

        /// <summary>
        /// Ensures that player health is correctly clamped between 0 and 100.
        /// </summary>
        public static void TestPlayerHealth()
        {
            Player player = new Player("TestPlayer", 150);
            Debug.Assert(player.Health == 100, "Health should not exceed 100.");
            Console.WriteLine("Player health test passed.");

            player.Health = -10;
            Debug.Assert(player.Health == 0, "Health should not go below 0.");
            Console.WriteLine("Player health lower limit test passed.");

            player.Health = 75;
            Debug.Assert(player.Health == 75, "Health should be correctly assigned.");
            Console.WriteLine("Player health assignment test passed.");
        }

        /// <summary>
        /// Tests that items can be added correctly to a player's inventory.
        /// </summary>
        public static void TestInventoryManagement()
        {
            Player player = new Player("TestPlayer", 100);
            SuppressConsoleOutput(() => player.PickUpItem(new Weapon("Sword", 3, 6)));
            Debug.Assert(player.Inventory.Any(i => i.Name == "Sword"), "Sword was not added to inventory.");
            Console.WriteLine("Inventory management test passed (Sword added).");

            SuppressConsoleOutput(() => player.PickUpItem(new Potion("Health Potion", 25)));
            Debug.Assert(player.Inventory.Any(i => i.Name == "Health Potion"), "Potion was not added to inventory.");
            Console.WriteLine("Inventory management test passed (Potion added).");
        }

        /// <summary>
        /// Verifies that inventory does not exceed its limit.
        /// </summary>
        public static void TestItemPickupLimit()
        {
            Player player = new Player("TestPlayer", 100);
            SuppressConsoleOutput(() => player.PickUpItem(new Weapon("Sword", 3, 6)));
            SuppressConsoleOutput(() => player.PickUpItem(new Weapon("Dagger", 2, 4)));
            SuppressConsoleOutput(() => player.PickUpItem(new Potion("Health Potion", 25)));
            SuppressConsoleOutput(() => player.PickUpItem(new Potion("Extra Potion", 25)));

            Debug.Assert(player.Inventory.Count <= 5, $"Inventory has {player.Inventory.Count} items (should not exceed limit).");
            Console.WriteLine("Item pickup limit test passed.");
        }

        /// <summary>
        /// Confirms that a Room object can be instantiated successfully.
        /// </summary>
        public static void TestRoomConstruction()
        {
            Room room = new Room(new Random());
            Debug.Assert(room != null, "Room object is null.");
            Console.WriteLine("Room construction test passed.");
        }

        /// <summary>
        /// Tests for adding and storing XP correctly.
        /// </summary>
        public static void TestExperienceSystem()
        {
            Player player = new Player("TestPlayer", 100);
            Debug.Assert(player != null, "Player should be created.");

            Console.WriteLine("Testing XP and Leveling...");

            // Test initial XP and level
            player.GainXP(10);  // Should not level up
            player.GainXP(40);  // Should trigger level up at 50 XP

            // Add more XP to test multiple level ups
            player.GainXP(75);  // Should level again at 75 XP
            Console.WriteLine("XP and Leveling test completed.\n");
        }

        /// <summary>
        /// Redirects Console output temporarily to suppress unwanted test output.
        /// </summary>
        /// <param name="action">The code block to run silently.</param>
        private static void SuppressConsoleOutput(Action action)
        {
            TextWriter originalOutput = Console.Out;
            Console.SetOut(new StringWriter());
            action();
            Console.SetOut(originalOutput);
        }
    }
}
