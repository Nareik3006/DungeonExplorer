using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    internal class Experience
    {
        private int xp;
        public int XP
        {
            get { return xp; }
        }

        public Experience()
        {
            xp = 0;  // Start with 0 XP
        }

        public void AddXP(int amount)
        {
            xp += amount;
            Console.WriteLine($"You gained {amount} XP! Total XP: {xp}");
        }
    }
}

