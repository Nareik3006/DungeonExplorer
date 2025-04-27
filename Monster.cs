using System;

namespace DungeonExplorer
{
    internal class Monster : Creature
    {
        private static Random rand = new Random();

        private int minDamage;
        private int maxDamage;

        private int burnTurnsRemaining = 0;
        private int burnDamagePerTurn = 0;

        public Monster(string name, int health, int minDamage, int maxDamage)
        {
            Name = name;
            MaxHealth = health;
            Health = health;
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
            burnTurnsRemaining = rand.Next(2, 4); // 2 or 3 turns
            burnDamagePerTurn = Math.Max(1, MaxHealth / 10); // 10% of Max HP
        }

        public void TakeTurn(Player player)
        {
            if (burnTurnsRemaining > 0)
            {
                Console.WriteLine($"{Name} suffers {burnDamagePerTurn} burn damage from the flames!");
                TakeDamage(burnDamagePerTurn);
                burnTurnsRemaining--;

                if (burnTurnsRemaining == 0)
                {
                    Console.WriteLine($"{Name} is no longer burned!");
                }
            }

            if (IsAlive)
            {
                Attack(player);
            }
        }

        public static Monster GenerateRandom()
        {
            int roll = rand.Next(3);
            if (roll == 0)
                return new Monster("Goblin", 30, 5, 10);
            else if (roll == 1)
                return new Monster("Troll", 50, 8, 15);
            else
                return new Monster("Bat", 20, 3, 7);
        }
        public int RewardXP()
        {
            if (Name == "Goblin") return 20;
            if (Name == "Troll") return 40;
            if (Name == "Bat") return 15;
            return 10; // default
        }

    }
}
