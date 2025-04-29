using System;

namespace DungeonExplorer
{
    internal abstract class Creature : IDamageable
    {
        public string Name { get; set; }
        public int Health { get; protected set; }
        public int MaxHealth { get; protected set; }
        public bool IsAlive => Health > 0;

        public Creature(string name, int health)
        {
            Name = name;
            MaxHealth = health;
            Health = health;
        }

        public virtual void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health < 0) Health = 0;
        }

        public abstract void Attack(Creature target);
    }
}
