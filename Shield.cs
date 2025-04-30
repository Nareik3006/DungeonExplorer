using System;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents a shield item that reduces incoming damage for a set number of turns.
    /// Inherits from the base Item class.
    /// </summary>
    internal class Shield : Item
    {
        /// <summary>
        /// Amount of damage reduced per hit.
        /// </summary>
        private int damageReduction;

        /// <summary>
        /// How many turns the shield effect lasts.
        /// </summary>
        private int duration;

        /// <summary>
        /// Constructs a Shield with a name, damage reduction, and duration.
        /// </summary>
        /// <param name="name">The name of the shield.</param>
        /// <param name="reduction">How much damage the shield blocks.</param>
        /// <param name="turns">How many turns the shield effect lasts.</param>
        public Shield(string name, int reduction, int turns) : base(name)
        {
            damageReduction = reduction;
            duration = turns;
        }

        /// <summary>
        /// Applies the shield effect to the player when used.
        /// </summary>
        /// <param name="player">The player equipping the shield.</param>
        public override void Use(Player player)
        {
            player.ApplyShield(damageReduction, duration);
            Console.WriteLine($"{Name} equipped! Reduces damage by {damageReduction} for {duration} turns.");
        }

        /// <summary>
        /// Provides a description of the shield for menus or inspection.
        /// </summary>
        public override string Description => $"A reinforced shield. Reduces incoming damage by {damageReduction} for {duration} turns.";
    }
}
