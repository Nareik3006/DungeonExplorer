namespace DungeonExplorer
{
    /// <summary>
    /// Abstract base class for all collectible items in the game.
    /// Implements the ICollectible interface.
    /// </summary>
    internal abstract class Item : ICollectible
    {
        /// <summary>
        /// Name of the item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Constructs an item with a given name.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        public Item(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Defines how the item is collected by the player.
        /// Default behavior adds the item to the player's inventory.
        /// </summary>
        /// <param name="player">The player collecting the item.</param>
        public virtual void Collect(Player player)
        {
            player.PickUpItem(this);
        }

        /// <summary>
        /// Abstract method that defines how the item is used.
        /// Must be implemented by subclasses.
        /// </summary>
        /// <param name="player">The player using the item.</param>
        public abstract void Use(Player player);

        /// <summary>
        /// Provides a description of the item.
        /// Can be overridden by subclasses.
        /// </summary>
        public virtual string Description => "No description available.";
    }
}
