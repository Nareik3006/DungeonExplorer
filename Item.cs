namespace DungeonExplorer
{
    internal abstract class Item : ICollectible
    {
        public string Name { get; set; }

        public Item(string name)
        {
            Name = name;
        }

        public virtual void Collect(Player player)
        {
            player.PickUpItem(this);
        }

        public abstract void Use(Player player);

        public virtual string Description => "No description available.";
    }
}
