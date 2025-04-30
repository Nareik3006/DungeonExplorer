using System;

namespace DungeonExplorer
{
    /// <summary>
    /// Interface that defines collectible behavior for items.
    /// Any item that can be picked up by the player should implement this.
    /// </summary>
    internal interface ICollectible
    {
        /// <summary>
        /// Method to define how the item is collected by the player.
        /// </summary>
        /// <param name="player">The player collecting the item.</param>
        void Collect(Player player);
    }
}
