using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace world_of_zuul
{
   public class Pollution
    {

            public int StartPollution = 0;
            public int MaxPollution = 150;
            int IncreasePollution(int amount)
            {
                return StartPollution + amount;
            }
            int DecreasePollution(int amount)
            {
                return StartPollution - amount;
            }               
    }
}