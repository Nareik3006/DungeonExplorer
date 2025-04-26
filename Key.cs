using System;

namespace DungeonExplorer
{
    internal class Key : Item
    {
        public Key(string name) : base(name) { }

        public override void Use(Player player)
        {
            Console.WriteLine("You hold the mysterious key. It seems to resonate with something nearby...");
        }

        public override string Description => "A mysterious key that unlocks something important.";
    }
}
