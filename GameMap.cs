using System;
using System.Collections.Generic;
using static DungeonExplorer.Magic;
using System.Linq;

namespace DungeonExplorer
{
    internal class GameMap
    {
        public bool IsVisited { get; set; } = false;
        private bool runeUsed = false;
        public bool HasTrapdoor { get; set; } = false;

        private static readonly List<string> roomLighting = new List<string>
        {
            "dimly lit",
            "bathed in flickering torchlight",
            "shrouded in heavy mist",
            "glowing faintly with a blue hue",
            "cast in deep shadow",
            "lit by a strange green glow"
        };

        private static readonly List<string> roomFeatures = new List<string>
        {
            "a skeleton slumped against the wall",
            "a pool of dried blood in the corner",
            "a pile of bones and armor",
            "a trail of dead rats",
            "a glowing rune on the floor", // Rune grants magic spells to player
            "a glowing rune on the floor" // Rune grants magic spells to player
        };

        private List<Chest> chests;
        private string lightingDescription;
        private string specialFeature;
        private Monster monster;
        private bool monsterAssigned = false;

        public Dictionary<string, GameMap> Exits { get; set; } = new Dictionary<string, GameMap>();

        public GameMap(Random rand)
        {
            lightingDescription = roomLighting[rand.Next(roomLighting.Count)];
            specialFeature = roomFeatures[rand.Next(roomFeatures.Count)];
            int chestCount = rand.Next(0, 3);

            chests = new List<Chest>();
            for (int i = 0; i < chestCount; i++)
            {
                chests.Add(new Chest());


            }
        }
        public List<Chest> GetChests()
        {
            return chests;
        }


        public void GetDescription(Player player, Action showMinimap)
        {
            bool exploring = true;

            // Spawn monster once per room, only once ever
            Random rand = new Random();
            if (!monsterAssigned)
            {
                if (rand.NextDouble() < 0.5)
                {
                    monster = Monster.GenerateRandom();
                }
                monsterAssigned = true;
            }

            if (monster != null && monster.IsAlive && exploring) 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($">> A wild {monster.Name} appears!");
                Console.ResetColor();
                Console.ReadLine();

                int turnCount = 1;

                while (monster.IsAlive && player.Health > 0)
                {
                    Console.Clear();
                    Console.WriteLine($"--- Turn {turnCount} ---\n");

                    bool itemUsedThisTurn = false;
                    bool playerTurn = true;



                    while (playerTurn)
                    {
                        UIHelper.ShowBattleHUD(player, monster, turnCount, false);

                        string input = Console.ReadLine();

                        if (input == "1")
                        {
                            player.Attack(monster);
                            Console.ReadLine();
                            playerTurn = false;
                        }
                        else if (input == "2")
                        {
                            if (Magic.ChooseSpell(player, monster))
                            {
                                playerTurn = false;
                            }
                            else
                            {
                                UIHelper.ShowBattleHUD(player, monster, turnCount, false);
                                // 🔥 Redraw HUD if canceled
                            }
                        }
                        else if (input == "3")
                        {
                            if (itemUsedThisTurn)
                            {
                                Console.WriteLine("You've already used an item this turn!");
                                Console.ReadLine();
                                UIHelper.ShowBattleHUD(player, monster, turnCount, false);
                                // 🔥 Redraw HUD after warning
                            }
                            else
                            {
                                if (player.OpenItemMenu(true))
                                {
                                    Console.Clear();
                                    playerTurn = false;
                                    itemUsedThisTurn = true;
                                }
                                else
                                {
                                    UIHelper.ShowBattleHUD(player, monster, turnCount, false);
                                    // 🔥 Redraw HUD if canceled
                                }
                            }
                        }
                        else if (input == "4")
                        {
                            Console.WriteLine("You flee from the monster!");
                            Console.ReadLine();
                            Console.Clear();
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice.");
                            Console.ReadLine();
                            UIHelper.ShowBattleHUD(player, monster, turnCount, false);
                            // 🔥 Redraw HUD after invalid input
                        }
                    }


                    if (monster.IsAlive)
                    {
                        monster.TakeTurn(player);
                        Console.ReadLine();
                        Console.Clear();
                    }
                    else
                    {
                        Console.WriteLine($"{monster.Name} is defeated!");
                        player.GainXP(monster.RewardXP());
                        Console.ReadLine();
                        Console.Clear();
                    }

                    turnCount++;
                }
            }

            if (monsterAssigned && monster != null && !monster.IsAlive)
            {
                Console.WriteLine("The corpse of the slain monster lies still.");
            }

            while (exploring)
            {
                Console.Clear();
                if (specialFeature == "a glowing rune on the floor")
                {
                    Console.Write($"The room is {lightingDescription} and you notice ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"{specialFeature}");
                    Console.ResetColor();
                    Console.WriteLine(".");
                }
                else
                {
                    Console.WriteLine($"The room is {lightingDescription} and you notice {specialFeature}.");
                }

                Console.WriteLine("====================");
                showMinimap();
                player.Stats();

                Console.WriteLine("====================");
                Console.WriteLine("What would you like to inspect?");

                if (specialFeature == "a glowing rune on the floor")
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("1. The special feature");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("1. The special feature");
                }

                for (int i = 0; i < chests.Count; i++)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"{i + 2}. Open Chest #{i + 1}");
                    Console.ResetColor();
                }

                int itemMenuIndex = chests.Count + 2;
                int spellbookMenuIndex = chests.Count + 3;
                int exitIndex = chests.Count + 4;

                Console.WriteLine($"{itemMenuIndex}. Open item menu");
                Console.WriteLine($"{spellbookMenuIndex}. Open spellbook");
                Console.WriteLine($"{exitIndex}. Leave the room");


                string input = Console.ReadLine();
                int choice;

                if (int.TryParse(input, out choice))
                {
                    if (choice == 1)
                    {
                        if (specialFeature == "a glowing rune on the floor")
                        {
                            if (!runeUsed)
                            {
                                Console.WriteLine("You approach the glowing rune...");

                                int spellChoice = rand.Next(2); // 0 = Heal, 1 = Fireball
                                Spell newSpell = (spellChoice == 0) ? Magic.GetHealSpell() : Magic.GetFireballSpell();

                                var knownSpell = player.Spellbook.FirstOrDefault(spell => spell.Name == newSpell.Name);
                                if (knownSpell != null)
                                {
                                    knownSpell.PowerBonus += 3;
                                    Console.WriteLine($"The rune strengthens your {knownSpell.Name} spell! (+3 bonus power!)");
                                }
                                else
                                {
                                    player.Spellbook.Add(newSpell);
                                    Console.WriteLine($"You absorb the power of the rune and learn {newSpell.Name}!");
                                }

                                runeUsed = true;
                            }
                            else
                            {
                                Console.WriteLine("The rune has faded and holds no more power.");
                            }
                            Console.ReadLine();
                            Console.Clear();
                        }
                        else
                        {
                            Console.WriteLine("You inspect the feature closely. It seems eerie but harmless.");
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }



                    else if (choice >= 2 && choice < 2 + chests.Count)
                    {
                        int chestIndex = choice - 2;
                        chests[chestIndex].Loot(player);
                    }
                    else if (choice == itemMenuIndex)
                    {
                        player.OpenItemMenu();
                    }
                    else if (choice == spellbookMenuIndex)
                    {
                        player.OpenSpellbook();
                    }
                    else if (choice == exitIndex)
                    {
                        Console.WriteLine("You leave the room.");
                        Console.ReadLine();
                        Console.Clear();
                        exploring = false;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Try again.");
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Try again.");
                    Console.ReadLine();
                }
            }
        }
    }
}
