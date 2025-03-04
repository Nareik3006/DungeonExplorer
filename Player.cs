using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_ASSIGNMENT_TEST2
{
    internal class Player
    {
        private string name;
        private int health;
        private List<string> inventory;

        public string Name
        {
            get { return name; }
            set { name = string.IsNullOrWhiteSpace(value) ? "Unknown" : value; }
        }

        public int Health
        {
            get { return health; }
            set { health = Math.Max(0, Math.Min(100, value)); } // Health between 0 and 100
        }

        public List<string> Inventory { get { return inventory; } }

        public Player(string name, int health)
        {
            Name = name;
            Health = health;
            inventory = new List<string>();
        }

        public void PickUpItem(string item)
        {
            if (inventory.Count < 3)
            {
                inventory.Add(item);
                Console.WriteLine($"{item} added to inventory.");
            }
            else
            {
                Console.WriteLine("Inventory full! Cannot pick up more items.");
            }
        }

        public void Stats()
        {
            Console.WriteLine("====================");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Health: {Health}/100");
            Console.WriteLine("Items: " + (inventory.Count > 0 ? string.Join(", ", inventory) : "No items"));
            Console.WriteLine("====================");
        }
    }

}
