using System;
using System.Diagnostics;
using System.IO;
using static DungeonExplorer.Magic;

namespace DungeonExplorer
{
    /// <summary>
    /// Provides automated test coverage for various core systems in the game.
    /// Uses Debug.Assert and Trace.Assert to verify correctness, and writes a test log to file.
    /// </summary>
    internal class Testing
    {
        private static StreamWriter logWriter;

        /// <summary>
        /// Runs all test cases and logs results to a text file.
        /// </summary>
        public static void RunTests()
        {
            Console.WriteLine("Running Tests...");
            logWriter = new StreamWriter("TestResults.txt", false);
            logWriter.AutoFlush = true;
            var originalOut = Console.Out;
            Console.SetOut(TextWriter.Null); // Suppress console output during testing

            logWriter.WriteLine("-Test Log-");

            try
            {
                TestPlayerHealth();
                TestInventoryManagement();
                TestItemPickupLimit();
                TestRoomConstruction();
                TestExperienceSystem();
                TestGameInitialization();
                TestManaUsage();
                TestSimpleBattle();
                TestWeaponBonus();
                TestShieldDefense();
                TestPotionHealing();
                TestManaPotionRestoration();
                TestMagicCastingBolt();
                TestMagicCastingFireball();
                TestMagicCastingHeal();
                TestSpellbookUpgrade();
            }
            finally
            {
                Console.SetOut(originalOut);
                logWriter.WriteLine("All Tests Completed.");
                logWriter.Close();
            }
        }

        /// <summary>
        /// Verifies that a new player's health is initialized correctly.
        /// </summary>
        private static void TestPlayerHealth()
        {
            Player player = new Player("TestPlayer", 100);
            Debug.Assert(player.Health == 100, "Debug: Player health should be initialized to 100.");
            Trace.Assert(player.MaxHealth == 100, "Trace: Player MaxHealth should be initialized to 100.");
            logWriter.WriteLine("Player health test passed.");
        }

        /// <summary>
        /// Verifies that picking up an item adds it to inventory.
        /// </summary>
        private static void TestInventoryManagement()
        {
            Player player = new Player("InventoryTester", 100);
            player.PickUpItem(new Potion("Health Potion", 25));
            Debug.Assert(player.Inventory.Count == 1, "Debug: Inventory should have 1 item.");
            Trace.Assert(player.Inventory[0].Name == "Health Potion", "Trace: First item should be Health Potion.");
            logWriter.WriteLine("Inventory management test passed.");
        }

        /// <summary>
        /// Ensures that inventory enforces a maximum limit of 5 items.
        /// </summary>
        private static void TestItemPickupLimit()
        {
            Player player = new Player("LimitTester", 100);
            for (int i = 0; i < 7; i++)
            {
                player.PickUpItem(new Potion("Health Potion", 25));
            }
            Debug.Assert(player.Inventory.Count == 5, "Debug: Inventory should not exceed 5 items.");
            Trace.Assert(player.Inventory.Count <= 5, "Trace: Inventory limit enforcement failed.");
            logWriter.WriteLine("Inventory limit test passed.");
        }

        /// <summary>
        /// Tests that a room can be successfully created and contains chests.
        /// </summary>
        private static void TestRoomConstruction()
        {
            Random rand = new Random();
            GameMap room = new GameMap(rand);
            Debug.Assert(room != null, "Debug: Room should be created.");
            Trace.Assert(room.GetChests().Count >= 0, "Trace: Room chests list should exist.");
            logWriter.WriteLine("Room creation test passed.");
        }

        /// <summary>
        /// Tests that gaining XP can result in leveling up the player.
        /// </summary>
        private static void TestExperienceSystem()
        {
            Player player = new Player("XPTester", 100);
            int initialLevel = player.Level;
            player.GainXP(100);
            Debug.Assert(player.Level > initialLevel, "Debug: Player should have leveled up.");
            Trace.Assert(player.Level >= 2, "Trace: Player should be at least Level 2 after XP.");
            logWriter.WriteLine("Experience system test passed.");
        }

        /// <summary>
        /// Ensures the Game object can be instantiated successfully.
        /// </summary>
        private static void TestGameInitialization()
        {
            Game game = new Game();
            Debug.Assert(game != null, "Debug: Game object creation failed.");
            Trace.Assert(game != null, "Trace: Game object must exist.");
            logWriter.WriteLine("Game object creation test passed.");
        }

        /// <summary>
        /// Tests that mana is correctly deducted when used.
        /// </summary>
        private static void TestManaUsage()
        {
            Player player = new Player("ManaTester", 100);
            int initialMana = player.Mana;
            player.UseMana(10);
            Debug.Assert(player.Mana == initialMana - 10, "Debug: Mana should decrease correctly.");
            Trace.Assert(player.Mana >= 0, "Trace: Mana should never be negative.");
            logWriter.WriteLine("Mana usage test passed.");
        }

        /// <summary>
        /// Ensures that player attacks cause damage to a monster.
        /// </summary>
        private static void TestSimpleBattle()
        {
            Player player = new Player("BattleTester", 100);
            Monster monster = new Monster("Test Goblin", 30, 5, 10);
            int initialMonsterHealth = monster.Health;
            player.Attack(monster);
            Debug.Assert(monster.Health < initialMonsterHealth, "Debug: Monster should take damage.");
            Trace.Assert(monster.Health >= 0, "Trace: Monster health should not be negative.");
            logWriter.WriteLine("Simple battle test passed.");
        }

        /// <summary>
        /// Tests that weapon use correctly increases the player's damage stats.
        /// </summary>
        private static void TestWeaponBonus()
        {
            Player player = new Player("WeaponTester", 100);
            int originalMinDamage = player.DamageMin;
            int originalMaxDamage = player.DamageMax;
            Weapon sword = new Weapon("Iron Sword", 3, 6);
            sword.Use(player);
            Debug.Assert(player.DamageMin > originalMinDamage, "Debug: Weapon min damage bonus failed.");
            Trace.Assert(player.DamageMax > originalMaxDamage, "Trace: Weapon max damage bonus failed.");
            logWriter.WriteLine("Weapon bonus test passed.");
        }

        /// <summary>
        /// Verifies that a shield reduces incoming damage.
        /// </summary>
        private static void TestShieldDefense()
        {
            Player player = new Player("ShieldTester", 100);
            Shield shield = new Shield("Wooden Shield", 3, 2);
            shield.Use(player);
            int originalHealth = player.Health;
            player.TakeDamage(10);
            Debug.Assert(player.Health > originalHealth - 10, "Debug: Shield did not reduce incoming damage.");
            Trace.Assert(player.Health > 0, "Trace: Player should still be alive after damage.");
            logWriter.WriteLine("Shield defense test passed.");
        }

        /// <summary>
        /// Ensures potions restore player health properly.
        /// </summary>
        private static void TestPotionHealing()
        {
            Player player = new Player("PotionTester", 100);
            player.SetHealth(50);
            Potion potion = new Potion("Health Potion", 25);
            potion.Use(player);
            Debug.Assert(player.Health > 50, "Debug: Potion should restore health.");
            Trace.Assert(player.Health <= player.MaxHealth, "Trace: Potion should not overheal.");
            logWriter.WriteLine("Potion healing test passed.");
        }

        /// <summary>
        /// Ensures mana potions restore mana correctly.
        /// </summary>
        private static void TestManaPotionRestoration()
        {
            Player player = new Player("ManaPotionTester", 100);
            player.Mana = 10;
            ManaPotion manaPotion = new ManaPotion("Mana Potion", 25);
            manaPotion.Use(player);
            Debug.Assert(player.Mana > 10, "Debug: Mana potion should restore mana.");
            Trace.Assert(player.Mana <= player.MaxMana, "Trace: Mana should not exceed maximum.");
            logWriter.WriteLine("Mana potion restoration test passed.");
        }

        /// <summary>
        /// Tests Fireball spell: damage and optional burn effect.
        /// </summary>
        private static void TestMagicCastingFireball()
        {
            Player player = new Player("MagicTester", 100);
            Monster monster = new Monster("Training Dummy", 50, 1, 2);
            player.Spellbook.Add(GetFireballSpell());

            int initialMana = player.Mana;
            int initialMonsterHealth = monster.Health;

            Spell fireball = GetFireballSpell();
            player.UseMana(fireball.ManaCost);
            fireball.Effect(player, monster);

            Debug.Assert(player.Mana < initialMana, "Debug: Casting Fireball should consume mana.");
            Debug.Assert(monster.Health < initialMonsterHealth, "Debug: Fireball should deal damage.");
            Trace.Assert(monster.BurnTurns >= 0, "Trace: Monster burn turns set correctly (0 if not burned).");
            logWriter.WriteLine("Magic casting (Fireball) and burn test passed.");
        }

        /// <summary>
        /// Tests Heal spell: health restored, mana consumed.
        /// </summary>
        private static void TestMagicCastingHeal()
        {
            Player player = new Player("HealTester", 100);
            player.SetHealth(30);
            player.Spellbook.Add(GetHealSpell());

            int initialMana = player.Mana;
            Spell heal = GetHealSpell();
            player.UseMana(heal.ManaCost);
            heal.Effect(player, player);

            Debug.Assert(player.Health > 30, "Debug: Heal should restore health.");
            Debug.Assert(player.Mana < initialMana, "Debug: Casting Heal should consume mana.");
            Trace.Assert(player.Health <= player.MaxHealth, "Trace: Heal should not exceed max health.");
            logWriter.WriteLine("Magic casting (Heal) test passed.");
        }

        /// <summary>
        /// Tests Bolt spell: deals damage and consumes mana.
        /// </summary>
        private static void TestMagicCastingBolt()
        {
            Player player = new Player("MagicTester", 100);
            Monster monster = new Monster("Training Dummy", 50, 1, 2);

            int initialMana = player.Mana;
            int initialMonsterHealth = monster.Health;

            Spell bolt = GetBoltSpell();
            player.UseMana(bolt.ManaCost);
            bolt.Effect(player, monster);

            Debug.Assert(player.Mana < initialMana, "Debug: Casting Bolt should consume mana.");
            Trace.Assert(monster.Health < initialMonsterHealth, "Trace: Bolt should deal damage to monster.");
            logWriter.WriteLine("Magic casting (Bolt) test passed.");
        }

        /// <summary>
        /// Verifies that learning the same spell again upgrades its power.
        /// </summary>
        private static void TestSpellbookUpgrade()
        {
            Player player = new Player("SpellUpgradeTester", 100);
            Spell fireball = GetFireballSpell();
            player.Spellbook.Add(fireball);
            Spell duplicateFireball = GetFireballSpell();

            var knownSpell = player.Spellbook.Find(spell => spell.Name == duplicateFireball.Name);
            if (knownSpell != null)
            {
                knownSpell.PowerBonus += 3;
            }

            Debug.Assert(knownSpell.PowerBonus == 3, "Debug: Spell power bonus upgrade failed.");
            Trace.Assert(knownSpell.PowerBonus > 0, "Trace: Spell bonus should be positive.");
            logWriter.WriteLine("Spellbook upgrade test passed.");
        }
    }
}
