using System;

namespace DungeonExplorer
{
    internal class ManaPotion : Item
    {
        private int manaRestoreAmount;

        public ManaPotion(string name, int manaRestoreAmount) : base(name)
        {
            this.manaRestoreAmount = manaRestoreAmount;
        }

        public override void Use(Player player)
        {
            int oldMana = player.Mana;
            player.Mana = Math.Min(player.MaxMana, player.Mana + manaRestoreAmount);
            Console.WriteLine($"{Name} used! Restored {player.Mana - oldMana} Mana.");
        }

        public override string Description => $"A shimmering blue potion. Restores {manaRestoreAmount} Mana when consumed.";
    }
}
