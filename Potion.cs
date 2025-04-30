using System;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents a health potion item that restores the player's health.
    /// Inherits from the base Item class.
    /// </summary>
    internal class Potion : Item
    {
        /// <summary>
        /// Amount of health this potion restores when used.
        /// </summary>
        private int healAmount;

        /// <summary>
        /// Constructs a new Potion with a name and healing amount.
        /// </summary>
        /// <param name="name">The name of the potion (e.g., "Health Potion").</param>
        /// <param name="healAmount">The amount of HP the potion restores.</param>
        public Potion(string name, int healAmount) : base(name)
        {
            this.healAmount = healAmount;
        }

        /// <summary>
        /// Applies the potion's healing effect to the player, restoring HP.
        /// </summary>
        /// <param name="player">The player using the potion.</param>
        public override void Use(Player player)
        {
            int oldHealth = player.Health;
            player.SetHealth(player.Health + healAmount + 5);
            Console.WriteLine($"{Name} used! Restored {player.Health - oldHealth} HP.");
        }

        /// <summary>
        /// Returns a description of the potion, displayed in menus or inspection.
        /// </summary>
        public override string Description => $"A glowing red vial. Restores {healAmount + 5} HP when used.";
    }
}
