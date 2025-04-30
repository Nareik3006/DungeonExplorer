using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonExplorer
{
    /// <summary>
    /// Static class that defines and manages magic spells in the game.
    /// </summary>
    internal static class Magic
    {
        /// <summary>
        /// Represents a single spell that can be cast by the player.
        /// </summary>
        internal class Spell
        {
            public string Name { get; }
            public int ManaCost { get; }
            public string Description { get; }
            public Action<Player, Creature> Effect { get; }
            public int PowerBonus { get; set; } = 0; // Increases with rune upgrades

            public Spell(string name, int manaCost, string description, Action<Player, Creature> effect)
            {
                Name = name;
                ManaCost = manaCost;
                Description = description;
                Effect = effect;
            }
        }

        /// <summary>
        /// Displays the player's known spells, allows inspection or casting.
        /// </summary>
        /// <param name="caster">The player casting the spell.</param>
        /// <param name="target">The target of the spell (usually a monster).</param>
        /// <returns>True if a spell was cast, false if cancelled or failed.</returns>
        public static bool ChooseSpell(Player caster, Creature target)
        {
            while (true)
            {
                Console.Clear();

                if (caster.Spellbook.Count == 0)
                {
                    Console.WriteLine("You don't know any spells yet.");
                    Console.ReadLine();
                    return false;
                }

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Your Spells:");
                Console.ResetColor();

                for (int i = 0; i < caster.Spellbook.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {caster.Spellbook[i].Name} ({caster.Spellbook[i].ManaCost} Mana)");
                }
                Console.WriteLine($"{caster.Spellbook.Count + 1}. Cancel");

                Console.WriteLine("\nSelect a spell by number:");
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice >= 1 && choice <= caster.Spellbook.Count)
                    {
                        Spell selectedSpell = caster.Spellbook[choice - 1];
                        Console.Clear();
                        Console.WriteLine($"Selected: {selectedSpell.Name}");
                        Console.WriteLine("1. Inspect\n2. Cast\n3. Cancel");

                        string action = Console.ReadLine();
                        Console.Clear();

                        if (action == "1")
                        {
                            Console.WriteLine($"Name: {selectedSpell.Name}");
                            Console.WriteLine($"Mana Cost: {selectedSpell.ManaCost}");
                            Console.WriteLine($"Description: {selectedSpell.Description}");
                            Console.WriteLine($"Power Bonus: +{selectedSpell.PowerBonus}");
                            Console.WriteLine("Press Enter to return.");
                            Console.ReadLine();
                        }
                        else if (action == "2")
                        {
                            if (caster.UseMana(selectedSpell.ManaCost))
                            {
                                selectedSpell.Effect(caster, target);
                                Console.ReadLine();
                                return true;
                            }
                        }
                        else if (action == "3")
                        {
                            return false;
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice.");
                            Console.ReadLine();
                        }
                    }
                    else if (choice == caster.Spellbook.Count + 1)
                    {
                        return false;
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
        /// Returns the Bolt spell, a basic lightning attack.
        /// </summary>
        public static Spell GetBoltSpell()
        {
            return new Spell(
                "Bolt",
                10,
                "A basic lightning attack dealing 10–20 magical damage.",
                (caster, target) =>
                {
                    Random rand = new Random();
                    int boltDamage = rand.Next(10, 20);

                    var known = caster.Spellbook.FirstOrDefault(spell => spell.Name == "Bolt");
                    int bonus = known != null ? known.PowerBonus : 0;
                    boltDamage += bonus;

                    UIHelper.ShowMiniBattleHUD(caster, target);

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"{caster.Name} casts Bolt and strikes {target.Name} for {boltDamage} magical damage!");
                    Console.ResetColor();
                    target.TakeDamage(boltDamage);
                }
            );
        }

        /// <summary>
        /// Returns the Heal spell, restoring health to the caster.
        /// </summary>
        public static Spell GetHealSpell()
        {
            return new Spell(
                "Heal",
                20,
                "Restores 40 health to the caster.",
                (caster, target) =>
                {
                    int healAmount = 40;
                    var known = caster.Spellbook.FirstOrDefault(spell => spell.Name == "Heal");
                    int bonus = known != null ? known.PowerBonus : 0;
                    healAmount += bonus;

                    int oldHealth = caster.Health;
                    caster.SetHealth(caster.Health + healAmount);

                    UIHelper.ShowMiniBattleHUD(caster, target);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{caster.Name} casts Heal and restores {caster.Health - oldHealth} HP!");
                    Console.ResetColor();
                }
            );
        }

        /// <summary>
        /// Returns the Fireball spell, which deals damage and may inflict burn.
        /// </summary>
        public static Spell GetFireballSpell()
        {
            return new Spell(
                "Fireball",
                15,
                "Deals 5–12 damage and has a 50% chance to inflict burn.",
                (caster, target) =>
                {
                    Random rand = new Random();
                    int fireDamage = rand.Next(5, 13);

                    var known = caster.Spellbook.FirstOrDefault(spell => spell.Name == "Fireball");
                    int bonus = known != null ? known.PowerBonus : 0;
                    fireDamage += bonus;

                    UIHelper.ShowMiniBattleHUD(caster, target);

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"{caster.Name} casts Fireball and hits {target.Name} for {fireDamage} fire damage!");
                    Console.ResetColor();
                    target.TakeDamage(fireDamage);

                    if (rand.NextDouble() < 0.5)
                    {
                        if (target is Monster monsterTarget)
                        {
                            monsterTarget.ApplyBurn();
                        }
                        Console.WriteLine($"{target.Name} is burned!");
                    }
                    else
                    {
                        Console.WriteLine($"{target.Name} resists the burn!");
                    }
                }
            );
        }
    }
}
