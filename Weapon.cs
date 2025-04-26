using System;

namespace DungeonExplorer
{
    internal class Weapon : Item
    {
        private int bonusMin;
        private int bonusMax;

        public Weapon(string name, int bonusMin, int bonusMax) : base(name)
        {
            this.bonusMin = bonusMin;
            this.bonusMax = bonusMax;
        }

        public override void Use(Player player)
        {
            Console.WriteLine($"{Name} equipped! Adds +{bonusMin}-{bonusMax} damage temporarily.");
            player.ApplyTemporaryWeaponBonus(bonusMin, bonusMax);
        }

        public override string Description => $"A sharp-edged weapon. Temporarily adds +{bonusMin}-{bonusMax} to your damage.";
    }
}
