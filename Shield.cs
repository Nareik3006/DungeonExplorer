using System;

namespace DungeonExplorer
{
    internal class Shield : Item
    {
        private int damageReduction;
        private int duration;

        public Shield(string name, int reduction, int turns) : base(name)
        {
            damageReduction = reduction;
            duration = turns;
        }

        public override void Use(Player player)
        {
            player.ApplyShield(damageReduction, duration);
            Console.WriteLine($"{Name} equipped! Reduces damage by {damageReduction} for {duration} turns.");
        }

        public override string Description => $"A reinforced shield. Reduces incoming damage by {damageReduction} for {duration} turns.";
    }
}
