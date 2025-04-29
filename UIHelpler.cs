using System;

namespace DungeonExplorer
{
    internal static class UIHelper
    {
        //Shows full battle HUD (Turn, HP, Mana, Enemy HP, Action Choices)
        public static void ShowBattleHUD(Player player, Creature monster, int turnCount, bool isBoss)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"--- Turn {turnCount} ---\n");
            Console.ResetColor();

            Console.WriteLine($"{player.Name}: {player.Health}/{player.MaxHealth} HP | {player.Mana}/{player.MaxMana} Mana");
            Console.WriteLine($"{monster.Name}: {monster.Health}/{monster.MaxHealth} HP\n");

            Console.WriteLine("Choose an action:");
            Console.WriteLine("1. Attack");
            Console.WriteLine("2. Magic");
            Console.WriteLine("3. Use Item");

            if (!isBoss)
                Console.WriteLine("4. Flee");

            Console.WriteLine();
        }

        //Shows a mini HUD during spell casting
        public static void ShowMiniBattleHUD(Player caster, Creature target)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("--- Spell Casting ---\n");
            Console.ResetColor();

            Console.WriteLine($"{caster.Name}: {caster.Health}/{caster.MaxHealth} HP | {caster.Mana}/{caster.MaxMana} Mana");
            Console.WriteLine($"{target.Name}: {target.Health}/{target.MaxHealth} HP\n");
        }
    }
}
