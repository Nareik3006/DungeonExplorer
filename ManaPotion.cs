using System;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents a mana potion item that restores the player's mana when used.
    /// Inherits from the base Item class.
    /// </summary>
    internal class ManaPotion : Item
    {
        private int manaRestoreAmount;

        /// <summary>
        /// Creates a new ManaPotion with a specified name and restore amount.
        /// </summary>
        /// <param name="name">The name of the potion (e.g., \"Mana Potion\").</param>
        /// <param name="manaRestoreAmount">How much mana the potion restores.</param>
        public ManaPotion(string name, int manaRestoreAmount) : base(name)
        {
            this.manaRestoreAmount = manaRestoreAmount;
        }

        /// <summary>
        /// When used, the potion restores the player's mana (without exceeding MaxMana).
        /// </summary>
        /// <param name="player">The player using the potion.</param>
        public override void Use(Player player)
        {
            int oldMana = player.Mana;
            player.Mana = Math.Min(player.MaxMana, player.Mana + manaRestoreAmount);
            Console.WriteLine($"{Name} used! Restored {player.Mana - oldMana} Mana.");
        }

        /// <summary>
        /// Returns a description of the potion for UI or inspection.
        /// </summary>
        public override string Description => $"A shimmering blue potion. Restores {manaRestoreAmount} Mana when consumed.";
    }
}
