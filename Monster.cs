using System;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents an enemy monster in the game.
    /// Inherits from the Creature base class.
    /// </summary>
    internal class Monster : Creature
    {
        private readonly Random rand = new Random();
        private int minDamage;
        private int maxDamage;

        /// <summary>
        /// Number of turns this monster will take burn damage.
        /// </summary>
        public int BurnTurns { get; private set; } = 0;

        /// <summary>
        /// Creates a monster with a name, health, and a damage range.
        /// </summary>
        public Monster(string name, int health, int minDamage, int maxDamage) : base(name, health)
        {
            this.minDamage = minDamage;
            this.maxDamage = maxDamage;
        }

        /// <summary>
        /// Monster attacks a target, dealing random damage within its range.
        /// </summary>
        public override void Attack(Creature target)
        {
            int damage = rand.Next(minDamage, maxDamage + 1);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{Name} attacks and deals {damage} damage!");
            Console.ResetColor();
            target.TakeDamage(damage);
        }

        /// <summary>
        /// Applies a burning effect to the monster that lasts 2–3 turns.
        /// Has a 50% chance to apply (handled externally).
        /// </summary>
        public void ApplyBurn()
        {
            if (rand.NextDouble() < 0.5)
            {
                BurnTurns = rand.Next(2, 4);
            }
        }

        /// <summary>
        /// Executes the monster's turn:
        /// applies burn damage if burning, then performs a normal attack if still alive.
        /// </summary>
        public void TakeTurn(Player player)
        {
            if (BurnTurns > 0)
            {
                int burnDamage = (int)(0.1 * MaxHealth);
                TakeDamage(burnDamage);
                BurnTurns--;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{Name} suffers {burnDamage} burn damage!");
                Console.ResetColor();
            }

            if (IsAlive)
            {
                Attack(player);
            }
        }

        /// <summary>
        /// Randomly generates a type of monster (Goblin, Troll, Bat).
        /// </summary>
        public static Monster GenerateRandom()
        {
            Random rand = new Random();
            int choice = rand.Next(3);
            switch (choice)
            {
                case 0:
                    return new Monster("Goblin", 40, 5, 10);
                case 1:
                    return new Monster("Troll", 60, 8, 12);
                case 2:
                    return new Monster("Bat", 30, 3, 6);
                default:
                    return new Monster("Slime", 20, 2, 4); // Fallback
            }
        }

        /// <summary>
        /// Returns the amount of XP rewarded for defeating this monster.
        /// </summary>
        public int RewardXP()
        {
            switch (Name)
            {
                case "Goblin":
                    return 20;
                case "Troll":
                    return 40;
                case "Bat":
                    return 15;
                default:
                    return 10;
            }
        }
    }
}
