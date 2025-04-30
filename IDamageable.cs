using System;

namespace DungeonExplorer
{
    /// <summary>
    /// Interface that defines damage-taking behavior for any creature or object.
    /// Implemented by classes like Player and Monster.
    /// </summary>
    internal interface IDamageable
    {
        /// <summary>
        /// Method to apply damage to the object.
        /// </summary>
        /// <param name="amount">Amount of damage to inflict.</param>
        void TakeDamage(int amount);
    }
}
