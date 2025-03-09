using System;
using System.Collections.Generic;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents a player in the Dungeon Explorer game.
    /// Stores player's name, health, and inventory.
    /// </summary>
    internal class Player
    {
        private string name;
        private int health;
        private List<string> inventory;

        /// <summary>
        /// Gets or sets the player's name. If an empty name is given, defaults to "Unknown".
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = string.IsNullOrWhiteSpace(value) ? "Unknown" : value; }
        }

        /// <summary>
        /// Gets or sets the player's health, ensuring it stays between 0 and 100.
        /// </summary>
        public int Health
        {
            get { return health; }
            set { health = Math.Max(0, Math.Min(100, value)); }
        }

        /// <summary>
        /// Gets the player's inventory, which contains a list of items.
        /// </summary>
        public List<string> Inventory { get { return inventory; } }

        /// <summary>
        /// Initializes a new player with a given name and health.
        /// </summary>
        /// <param name="name">The player's name.</param>
        /// <param name="health">The player's initial health (0-100).</param>
        public Player(string name, int health)
        {
            Name = name;
            Health = health;
            inventory = new List<string>();
        }

        /// <summary>
        /// Allows the player to pick up an item and add it to their inventory.
        /// Limits inventory size to 3 items.
        /// </summary>
        /// <param name="item">The item to be added.</param>
        public void PickUpItem(string item)
        {
            if (inventory.Count < 3)
            {
                inventory.Add(item);
                Console.WriteLine($"{item} added to inventory.");
            }
            else
            {
                Console.WriteLine("Inventory full! Cannot pick up more items.");
            }
        }

        /// <summary>
        /// Displays the player's current stats, including health and inventory.
        /// </summary>
        public void Stats()
        {
            Console.WriteLine("====================");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Health: {Health}/100");
            Console.WriteLine("Items: " + (inventory.Count > 0 ? string.Join(", ", inventory) : "No items"));
        }
    }
}
