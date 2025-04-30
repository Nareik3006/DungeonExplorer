using System;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents a weapon item that increases the player's damage temporarily when used.
    /// Inherits from the base Item class.
    /// </summary>
    internal class Weapon : Item
    {
        /// <summary>
        /// Bonus minimum damage the weapon grants.
        /// </summary>
        private int bonusMin;

        /// <summary>
        /// Bonus maximum damage the weapon grants.
        /// </summary>
        private int bonusMax;

        /// <summary>
        /// Constructs a Weapon with a given name and damage bonuses.
        /// </summary>
        /// <param name="name">The name of the weapon.</param>
        /// <param name="bonusMin">The minimum damage bonus provided.</param>
        /// <param name="bonusMax">The maximum damage bonus provided.</param>
        public Weapon(string name, int bonusMin, int bonusMax) : base(name)
        {
            this.bonusMin = bonusMin;
            this.bonusMax = bonusMax;
        }

        /// <summary>
        /// Applies the weapon's damage bonus to the player when used.
        /// </summary>
        /// <param name="player">The player equipping the weapon.</param>
        public override void Use(Player player)
        {
            Console.WriteLine($"{Name} equipped! Adds +{bonusMin}-{bonusMax} damage temporarily.");
            player.ApplyTemporaryWeaponBonus(bonusMin, bonusMax);
        }

        /// <summary>
        /// Provides a description of the weapon for menus or inspection.
        /// </summary>
        public override string Description => $"A sharp-edged weapon. Temporarily adds +{bonusMin}-{bonusMax} to your damage.";
    }
}
