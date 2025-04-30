using System;

namespace DungeonExplorer
{
    /// <summary>
    /// Provides static methods for rendering battle-related UI in the console.
    /// Used to keep the display logic separate from the gameplay logic.
    /// </summary>
    internal static class UIHelper
    {
        /// <summary>
        /// Displays the full battle HUD during combat, including player and enemy health,
        /// mana, turn number, and action options.
        /// </summary>
        /// <param name="player">The player character.</param>
        /// <param name="monster">The enemy monster or boss.</param>
        /// <param name="turnCount">The current turn number.</param>
        /// <param name="isBoss">Whether the fight is against a boss (disables flee option).</param>
        public static void ShowBattleHUD(Player player, Creature monster, int turnCount, bool isBoss)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"--- Turn {turnCount} ---\n");
            Console.ResetColor();

            Console.WriteLine($"{player.Name}: {player.Health}/{player.MaxHealth} HP | {player.Mana}/{player.MaxMana} Mana");
            Console.WriteLine($"{monster.Name}: {monster.Health}/{monster.MaxHealth} HP\n");

            Console.WriteLine("Choose an action:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("1. Attack");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("2. Magic");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("3. Use Item");
            Console.ResetColor();

            // Only show flee option if not in a boss fight
            if (!isBoss)
                Console.WriteLine("4. Flee");

            Console.WriteLine();
        }

        /// <summary>
        /// Displays a simplified version of the battle HUD, used when casting spells.
        /// Shows updated player and target stats.
        /// </summary>
        /// <param name="caster">The player casting the spell.</param>
        /// <param name="target">The creature targeted by the spell.</param>
        public static void ShowMiniBattleHUD(Player caster, Creature target)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("--- Spell Casting ---\n");
            Console.ResetColor();

            Console.WriteLine($"{caster.Name}: {caster.Health}/{caster.MaxHealth} HP | {caster.Mana}/{caster.MaxMana} Mana");
            Console.WriteLine($"{target.Name}: {target.Health}/{target.MaxHealth} HP\n");
        }
    }
}
