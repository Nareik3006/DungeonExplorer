using System;

namespace DungeonExplorer
{
    internal class Monster : Creature
    {
        private readonly Random rand = new Random();
        private int minDamage;
        private int maxDamage;

        public int BurnTurns { get; private set; } = 0;

        public Monster(string name, int health, int minDamage, int maxDamage) : base(name, health)
        {
            this.minDamage = minDamage;
            this.maxDamage = maxDamage;
        }

        public override void Attack(Creature target)
        {
            int damage = rand.Next(minDamage, maxDamage + 1);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{Name} attacks and deals {damage} damage!");
            Console.ResetColor();
            target.TakeDamage(damage);
        }

        public void ApplyBurn()
        {
            if (rand.NextDouble() < 0.5)
            {
                BurnTurns = rand.Next(2, 4);
            }
        }

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
                    return new Monster("Slime", 20, 2, 4);
            }
        }

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
