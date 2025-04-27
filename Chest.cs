using System;

namespace DungeonExplorer
{
    internal class Chest
    {
        public Item Item { get; private set; }
        public bool IsLooted { get; private set; }
        private static readonly Random rand = new Random();

        public Chest()
        {
            // Item will be generated later when the chest is opened
        }

        public void Loot(Player player)
        {
            if (!IsLooted)
            {
                if (Item == null)
                {
                    GenerateItem(player); // 🛠 Generate loot only when opening
                }

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"You open the chest and find: {Item.Name}");
                Console.ResetColor();

                if (player.Inventory.Count < 5)
                {
                    Item.Collect(player);
                    player.GainXP(10);
                    IsLooted = true;
                }
                else
                {
                    Console.WriteLine("Your inventory is full! You cannot pick up the item.");
                }
            }
            else
            {
                Console.WriteLine("The chest is empty.");
            }

            Console.ReadLine();
            Console.Clear();
        }
        public void ForceKey()
        {
            Item = new Key("Trapdoor Key");
        }

        private void GenerateItem(Player player)
        {
            int roll = rand.Next(0, 5); // 0 = potion, 1 = weapon, 2 = shield, 3 = mana potion, 4 = key

            if (roll == 0)
                Item = new Potion("Health Potion", 25);
            else if (roll == 1)
                Item = new Weapon("Iron Sword", 3, 6);
            else if (roll == 2)
                Item = new Shield("Wooden Shield", 3, 2);
            else if (roll == 3)
                Item = new ManaPotion("Mana Potion", 25);
            else
            {
                if (!player.Inventory.Exists(item => item is Key))
                    Item = new Key("Trapdoor Key");
                else
                    Item = new Potion("Health Potion", 25);
            }
        }
    }
}
