using System;

namespace DungeonExplorer
{
    /// <summary>
    /// Abstract base class for all creatures in the game, such as Player and Monster.
    /// Defines core shared properties like Health, Name, and behavior like taking damage.
    /// </summary>
    internal abstract class Creature : IDamageable
    {
        /// <summary>
        /// The creature's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Current health value.
        /// </summary>
        public int Health { get; protected set; }

        /// <summary>
        /// Maximum health the creature can have.
        /// </summary>
        public int MaxHealth { get; protected set; }

        /// <summary>
        /// Indicates if the creature is still alive.
        /// </summary>
        public bool IsAlive => Health > 0;

        /// <summary>
        /// Constructor to set the creature's name and health.
        /// </summary>
        /// <param name="name">The name of the creature.</param>
        /// <param name="health">The starting and maximum health of the creature.</param>
        public Creature(string name, int health)
        {
            Name = name;
            MaxHealth = health;
            Health = health;
        }

        /// <summary>
        /// Reduces the creature's health by a specified amount.
        /// </summary>
        /// <param name="amount">Amount of damage to take.</param>
        public virtual void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health < 0) Health = 0;
        }

        /// <summary>
        /// Defines the attack behavior. Must be implemented by subclasses.
        /// </summary>
        /// <param name="target">The target creature to attack.</param>
        public abstract void Attack(Creature target);
    }
}
