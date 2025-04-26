using System;

namespace DungeonExplorer
{
    internal class Potion : Item
    {
        private int healAmount;

        public Potion(string name, int healAmount) : base(name)
        {
            this.healAmount = healAmount;
        }

        public override void Use(Player player)
        {
            int oldHealth = player.Health;
            player.Health = Math.Min(player.MaxHealth, player.Health + healAmount);
            Console.WriteLine($"{Name} used! Restored {player.Health - oldHealth} HP.");
        }

        public override string Description => $"A glowing red vial. Restores {healAmount} HP when used.";
    }
}
