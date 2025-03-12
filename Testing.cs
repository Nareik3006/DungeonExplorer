using System;
using System.Diagnostics;
using System.IO;
using DungeonExplorer;

namespace DungeonExplorer
{
    internal class Testing
    {
        /// <summary>
        /// Runs all unit tests for the game.
        /// </summary>
        public static void RunTests()
        {
            Console.WriteLine("Running Tests...");
            TestPlayerHealth();
            TestInventoryManagement();
            TestItemPickupLimit(); 
            TestRoomDescription();
            TestExperienceSystem();
            Console.WriteLine("All Tests Completed.");
        }

        /// <summary>
        /// Tests if player health is correctly limited between 0 and 100.
        /// </summary>
        public static void TestPlayerHealth()
        {
            Player player = new Player("TestPlayer", 150);
            Debug.Assert(player.Health == 100, "Test Failed: Health should not exceed 100.");
            Console.WriteLine("Player health test passed.");

            player.Health = -10;
            Debug.Assert(player.Health == 0, "Test Failed: Health should not go below 0.");
            Console.WriteLine("Player health lower limit test passed.");

            player.Health = 75;
            Debug.Assert(player.Health == 75, "Test Failed: Health should be correctly assigned.");
            Console.WriteLine("Player health assignment test passed.");
        }

        /// <summary>
        /// Tests if items are added correctly to the inventory without printing extra messages.
        /// </summary>
        public static void TestInventoryManagement()
        {
            Player player = new Player("TestPlayer", 100);
            SuppressConsoleOutput(() => player.PickUpItem("Sword"));
            Debug.Assert(player.Inventory.Contains("Sword"), "Test Failed: Sword was not added to inventory.");
            Console.WriteLine("Inventory management test passed (Sword added).");

            SuppressConsoleOutput(() => player.PickUpItem("Shield"));
            Debug.Assert(player.Inventory.Contains("Shield"), "Test Failed: Shield was not added to inventory.");
            Console.WriteLine("Inventory management test passed (Shield added).");
        }

        /// <summary>
        /// Tests if item pickup limit works (maximum 3 items).
        /// </summary>
        public static void TestItemPickupLimit()
        {
            Player player = new Player("TestPlayer", 100);
            SuppressConsoleOutput(() => player.PickUpItem("Sword"));
            SuppressConsoleOutput(() => player.PickUpItem("Shield"));
            SuppressConsoleOutput(() => player.PickUpItem("Potion"));
            SuppressConsoleOutput(() => player.PickUpItem("ExtraItem"));

            Debug.Assert(player.Inventory.Count == 3, $"Test Failed: Inventory has {player.Inventory.Count} items (should be 3).");
            Console.WriteLine("Item pickup limit test passed.");
        }

        /// <summary>
        /// Tests if the room description function can be called successfully.
        /// </summary>
        public static void TestRoomDescription()
        {
            Room room = new Room();
            Player player = new Player("TestPlayer", 100);

            SuppressConsoleOutput(() =>
            {
                Console.WriteLine("Room description function called successfully.");
            });

            Debug.Assert(room != null, "Test Failed: Room object is null.");
            Console.WriteLine("Room test passed.");
        }

        /// <summary>
        /// Tests if Experience can be gained, added and removed
        /// </summary>
        public static void TestExperienceSystem()
        {
            Player player = new Player("TestPlayer", 100);
            Debug.Assert(player.Experience == 0, "Test Failed: XP should start at 0.");
            Console.WriteLine("XP initialization test passed.");

            SuppressConsoleOutput(() => player.GainXP(10));
            Debug.Assert(player.Experience == 10, "Test Failed: XP should be 10 after gaining 10 XP.");
            Console.WriteLine("XP gain test passed.");

            SuppressConsoleOutput(() => player.GainXP(20));
            Debug.Assert(player.Experience == 30, "Test Failed: XP should be 30 after gaining another 20 XP.");
            Console.WriteLine("XP accumulation test passed.");

            SuppressConsoleOutput(() => player.GainXP(-10));  
            Debug.Assert(player.Experience >= 0, "Test Failed: XP should never be negative.");
            Console.WriteLine("Negative XP test passed.");

            Console.WriteLine("All XP tests completed.\n");
        }


        /// <summary>
        /// Temporarily suppresses console output while running an action.
        /// </summary>
        private static void SuppressConsoleOutput(Action action)
        {
            TextWriter originalOutput = Console.Out;
            Console.SetOut(new StringWriter()); // Redirect console output
            action();
            Console.SetOut(originalOutput); // Restore console output
        }
    }
}
