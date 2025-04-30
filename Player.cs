using System;
using System.Collections.Generic;
using System.Linq;
using static DungeonExplorer.Magic;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents the player character in the game.
    /// Inherits from Creature and contains all logic for stats, combat, leveling, inventory, and magic.
    /// </summary>
    internal class Player : Creature
    {
        private List<Item> inventory;
        public List<Spell> Spellbook { get; private set; }

        public int Level { get; private set; } = 1;
        private int currentXP = 0;
        private int xpToNextLevel = 50;

        private int baseMinDamage = 5;
        private int baseMaxDamage = 10;
        private int bonusMinDamage = 0;
        private int bonusMaxDamage = 0;

        private int shieldReduction = 0;
        private int shieldTurnsRemaining = 0;

        public int Mana { get; set; }
        public int MaxMana { get; private set; }

        public int DamageMin => baseMinDamage + bonusMinDamage;
        public int DamageMax => baseMaxDamage + bonusMaxDamage;

        public List<Item> Inventory => inventory;

        /// <summary>
        /// Constructs the player with a name and starting health.
        /// </summary>
        public Player(string name, int health) : base(name, health)
        {
            MaxMana = 50;
            Mana = MaxMana;
            inventory = new List<Item>();
            Spellbook = new List<Spell> { Magic.GetBoltSpell() };
        }

        /// <summary>
        /// Performs a basic attack against a target.
        /// </summary>
        public override void Attack(Creature target)
        {
            Random rand = new Random();
            int damage = rand.Next(DamageMin, DamageMax + 1);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{Name} attacks and deals {damage} damage!");
            Console.ResetColor();

            target.TakeDamage(damage);
        }

        /// <summary>
        /// Handles taking damage with shield reduction if active.
        /// </summary>
        public override void TakeDamage(int amount)
        {
            if (shieldTurnsRemaining > 0)
            {
                Console.WriteLine($"Shield absorbs {shieldReduction} damage!");
                amount = Math.Max(0, amount - shieldReduction);
                shieldTurnsRemaining--;
            }

            base.TakeDamage(amount);

            if (Health <= 0)
            {
                HandleDeath();
            }
        }

        /// <summary>
        /// Triggers the death sequence and ends the game.
        /// </summary>
        public void HandleDeath()
        {
            Console.ReadLine();
            Console.Clear();
            Console.WriteLine($"{Name} has fallen in battle...");
            Console.WriteLine("Game Over.");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Would you like to try again? (y/n)");
            string input = Console.ReadLine()?.Trim().ToLower();

            if (input == "y")
            {
                Console.Clear();
                Program.Main(null); // Restarts the game
            }
            else
            {
                Console.WriteLine("Thanks for playing!");
                Console.ReadLine();
                Environment.Exit(0);
            }

        }

        /// <summary>
        /// Applies a temporary shield that reduces incoming damage for a set number of turns.
        /// </summary>
        public void ApplyShield(int reduction, int turns)
        {
            shieldReduction = reduction;
            shieldTurnsRemaining = turns;
        }

        /// <summary>
        /// Grants XP and checks for level-up.
        /// </summary>
        public void GainXP(int amount)
        {
            currentXP += amount;
            Console.WriteLine($"+{amount} XP! Total XP: {currentXP}/{xpToNextLevel}");
            if (currentXP >= xpToNextLevel)
            {
                LevelUp();
            }
        }

        /// <summary>
        /// Increases stats when the player levels up.
        /// </summary>
        private void LevelUp()
        {
            Level++;
            currentXP = 0;
            xpToNextLevel += 25;
            MaxHealth += 10;
            MaxMana += 10;
            Health = MaxHealth;
            Mana = MaxMana;
            baseMinDamage += 2;
            baseMaxDamage += 3;

            Console.WriteLine($"*** {Name} leveled up to Level {Level}! ***");
            Console.WriteLine($"Max Health: {MaxHealth}, Max Mana: {MaxMana}, Damage: {baseMinDamage}-{baseMaxDamage}");
        }

        /// <summary>
        /// Adds an item to the player's inventory if space allows.
        /// </summary>
        public void PickUpItem(Item item)
        {
            if (inventory.Count < 5)
            {
                inventory.Add(item);
                Console.WriteLine($"{item.Name} added to inventory.");
            }
            else
            {
                Console.WriteLine("Inventory full! Cannot pick up more items.");
            }
        }

        /// <summary>
        /// Attempts to spend mana. Returns true if successful.
        /// </summary>
        public bool UseMana(int amount)
        {
            if (Mana >= amount)
            {
                Mana -= amount;
                return true;
            }
            else
            {
                Console.WriteLine("Not enough mana!");
                Console.ReadLine();
                return false;
            }
        }

        /// <summary>
        /// Applies a temporary weapon bonus (used by weapon items).
        /// </summary>
        public void ApplyTemporaryWeaponBonus(int minBonus, int maxBonus)
        {
            bonusMinDamage = minBonus;
            bonusMaxDamage = maxBonus;
        }

        /// <summary>
        /// Displays and manages the item menu during or outside of battle.
        /// </summary>
        public bool OpenItemMenu(bool battleMode = false)
        {
            while (true)
            {
                Console.Clear();

                if (inventory.Count == 0)
                {
                    Console.WriteLine("You have no items.");
                    Console.ReadLine();
                    return false;
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Your Inventory:");
                Console.ResetColor();

                for (int i = 0; i < inventory.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {inventory[i].Name}");
                }

                Console.WriteLine($"{inventory.Count + 1}. Show Potions");
                Console.WriteLine($"{inventory.Count + 2}. Exit");
                Console.WriteLine("\nSelect an item by number:");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice >= 1 && choice <= inventory.Count)
                    {
                        Item selectedItem = inventory[choice - 1];
                        Console.Clear();

                        Console.WriteLine($"Selected: {selectedItem.Name}");
                        Console.WriteLine("1. Inspect\n2. Use\n3. Delete\n4. Cancel");

                        string action = Console.ReadLine();
                        Console.Clear();

                        switch (action)
                        {
                            case "1":
                                Console.WriteLine($"Name: {selectedItem.Name}");
                                Console.WriteLine($"Type: {selectedItem.GetType().Name}");
                                Console.WriteLine($"Description: {selectedItem.Description}");
                                Console.WriteLine("Press Enter to return.");
                                Console.ReadLine();
                                break;

                            case "2":
                                selectedItem.Use(this);
                                inventory.Remove(selectedItem);
                                Console.WriteLine("Item used.");
                                Console.ReadLine();
                                return battleMode ? true : false;

                            case "3":
                                if (selectedItem is Key)
                                {
                                    Console.WriteLine("You cannot delete a Key item!");
                                    Console.ReadLine();
                                }
                                else
                                {
                                    inventory.Remove(selectedItem);
                                    Console.WriteLine("Item deleted.");
                                    Console.ReadLine();
                                }
                                break;

                            case "4":
                                break;

                            default:
                                Console.WriteLine("Invalid choice.");
                                Console.ReadLine();
                                break;
                        }
                    }
                    else if (choice == inventory.Count + 1)
                    {
                        ShowAllPotions();
                    }
                    else if (choice == inventory.Count + 2)
                    {
                        break; // Exit item menu
                    }
                    else
                    {
                        Console.WriteLine("Invalid input.");
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                    Console.ReadLine();
                }
            }

            return false;
        }

        /// <summary>
        /// Allows the player to inspect known spells.
        /// </summary>
        public void OpenSpellbook()
        {
            while (true)
            {
                Console.Clear();

                if (Spellbook.Count == 0)
                {
                    Console.WriteLine("You have no spells yet.");
                    Console.ReadLine();
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Your Spellbook:");
                Console.ResetColor();

                for (int i = 0; i < Spellbook.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {Spellbook[i].Name} ({Spellbook[i].ManaCost} Mana)");
                }

                Console.WriteLine($"{Spellbook.Count + 1}. Exit");
                Console.WriteLine("\nSelect a spell by number to inspect:");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice >= 1 && choice <= Spellbook.Count)
                    {
                        Spell selectedSpell = Spellbook[choice - 1];
                        Console.Clear();
                        Console.WriteLine($"Name: {selectedSpell.Name}");
                        Console.WriteLine($"Mana Cost: {selectedSpell.ManaCost}");
                        Console.WriteLine($"Description: {selectedSpell.Description}");
                        Console.WriteLine($"Power Bonus: +{selectedSpell.PowerBonus}");
                        Console.WriteLine("Press Enter to return.");
                        Console.ReadLine();
                    }
                    else if (choice == Spellbook.Count + 1)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice.");
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// Sets the player's health (used for testing and healing).
        /// </summary>
        public void SetHealth(int value)
        {
            Health = Math.Min(value, MaxHealth);
        }

        /// <summary>
        /// Displays all player stats: name, health, mana, level, XP, damage, shield, and items.
        /// </summary>
        public void Stats()
        {
            Console.WriteLine("====================");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Level: {Level}");
            Console.WriteLine($"Health: {Health}/{MaxHealth}");
            Console.WriteLine($"Mana: {Mana}/{MaxMana}");
            Console.WriteLine($"XP: {currentXP}/{xpToNextLevel}");
            Console.WriteLine($"Damage: {DamageMin}-{DamageMax}");

            if (shieldTurnsRemaining > 0)
                Console.WriteLine($"Shield: -{shieldReduction} damage ({shieldTurnsRemaining} turns remaining)");

            Console.WriteLine("Items: " + (inventory.Count > 0 ? string.Join(", ", inventory.Select(i => i.Name)) : "No items"));
        }

        /// <summary>
        /// Lists all health and mana potions in the inventory.
        /// </summary>
        public void ShowAllPotions()
        {
            Console.Clear();
            var potions = inventory.Where(item => item is Potion || item is ManaPotion);

            if (!potions.Any())
            {
                Console.WriteLine("You have no potions.");
            }
            else
            {
                Console.WriteLine("Your Potions:");
                foreach (var potion in potions)
                {
                    Console.WriteLine($"- {potion.Name}");
                }
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
}
