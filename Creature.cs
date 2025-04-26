using System;

namespace DungeonExplorer
{
    /// <summary>
    /// Abstract base class for all living creatures.
    /// </summary>
    internal abstract class Creature : IDamageable
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }

        public abstract void Attack(Creature target);

        public virtual void TakeDamage(int amount)
        {
            Health = Math.Max(0, Health - amount);
        }

        public bool IsAlive => Health > 0;
    }
}
