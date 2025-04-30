using System;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents a special key item used to unlock the trapdoor and access the boss room.
    /// Inherits from the base Item class.
    /// </summary>
    internal class Key : Item
    {
        /// <summary>
        /// Constructs a Key with a specific name.
        /// </summary>
        /// <param name="name">The name of the key (e.g., \"Trapdoor Key\").</param>
        public Key(string name) : base(name) { }

        /// <summary>
        /// Defines the behavior when the player uses the key.
        /// Typically, keys aren't directly used from inventory.
        /// </summary>
        /// <param name="player">The player using the key.</param>
        public override void Use(Player player)
        {
            Console.WriteLine("You hold the mysterious key. It seems to resonate with something nearby...");
        }

        /// <summary>
        /// Describes what the key is for.
        /// </summary>
        public override string Description => "A mysterious key that unlocks something important.";
    }
}
