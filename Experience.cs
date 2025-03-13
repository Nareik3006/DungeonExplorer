using System;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents the experience system for the player.
    /// Manages the player's XP and provides methods for gaining XP.
    /// </summary>
    internal class Experience
    {
        /// <summary>
        /// Stores the player's current experience points.
        /// Kept private to prevent unintended modifications.
        /// </summary>
        private int xp;

        /// <summary>
        /// Gets the current XP value.
        /// Read-only to ensure XP is modified only through controlled methods.
        /// </summary>
        public int XP
        {
            get { return xp; }
        }

        /// <summary>
        /// Initializes the Experience object with zero XP.
        /// </summary>
        public Experience()
        {
            // Start with 0 XP
            xp = 0;  
        }

        /// <summary>
        /// Increases the player's XP by a given amount.
        /// Displays the updated XP value to the console.
        /// </summary>
        /// <param name="amount">The amount of XP to add.</param>
        public void AddXP(int amount)
        {
            // Increase XP by the specified amount
            xp += amount;  
            Console.WriteLine($"You gained {amount} XP! Total XP: {xp}");
        }
    }
}
