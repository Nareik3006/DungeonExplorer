using System;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents a chest that can contain a random item for the player to loot.
    /// </summary>
    internal class Chest
    {
        public Item Item { get; private set; }
        public bool IsLooted { get; private set; }
        private static readonly Random rand = new Random();

        /// <summary>
        /// Creates an empty chest. Item will be generated when opened.
        /// </summary>
        public Chest()
        {
            // Item will be generated later when the chest is opened
        }

        /// <summary>
        /// Allows the player to loot the chest and collect the item.
        /// </summary>
        /// <param name="player">The player looting the chest.</param>
        public void Loot(Player player)
        {
            if (!IsLooted)
            {
                if (Item == null)
                {
                    GenerateItem(player); // 🛠 Generate loot only when opening
                }

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"You open the chest and find: {Item.Name}");
                Console.ResetColor();

                if (player.Inventory.Count < 5)
                {
                    Item.Collect(player); // Adds item to inventory
                    player.GainXP(10);    // Rewards XP for looting
                    IsLooted = true;
                }
                else
                {
                    Console.WriteLine("Your inventory is full! You cannot pick up the item.");
                }
            }
            else
            {
                Console.WriteLine("The chest is empty.");
            }

            Console.ReadLine();
            Console.Clear();
        }

        /// <summary>
        /// Forces the chest to contain a Trapdoor Key instead of random loot.
        /// </summary>
        public void ForceKey()
        {
            Item = new Key("Trapdoor Key");
        }

        /// <summary>
        /// Randomly generates an item inside the chest when it is first opened.
        /// </summary>
        /// <param name="player">The player opening the chest (used to check for key duplication).</param>
        private void GenerateItem(Player player)
        {
            int roll = rand.Next(0, 5); // 0 = potion, 1 = weapon, 2 = shield, 3 = mana potion, 4 = key

            if (roll == 0)
                Item = new Potion("Health Potion", 25);
            else if (roll == 1)
                Item = new Weapon("Iron Sword", 3, 6);
            else if (roll == 2)
                Item = new Shield("Wooden Shield", 3, 2);
            else if (roll == 3)
                Item = new ManaPotion("Mana Potion", 25);
            else
            {
                // Only give a key if the player doesn't already have one
                if (!player.Inventory.Exists(item => item is Key))
                    Item = new Key("Trapdoor Key");
                else
                    Item = new Potion("Health Potion", 25); // fallback item
            }
        }
    }
}
