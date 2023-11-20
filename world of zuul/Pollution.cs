using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace world_of_zuul
{
    public static class Pollution
    {

        public static int pollutionLevel = 0;
        public static int maxPollution = 6075;
        public static int PollutionLevel
        {
            get { return pollutionLevel; }
            set
            {
                // Checks if pollution limit was exceeded each time the pollution level is changed
                if (value > maxPollution)
                {
                    Console.WriteLine("Pollution limit exceeded! You lost...");
                }

                pollutionLevel = value;
            }
        }

        //Changes Amount of pollution by "amount"
        public static void ChangePollution(int amount)
        {
            PollutionLevel += amount;
        }

        public static void DisplayPollution()
        {
            Console.WriteLine(pollutionLevel);
        }
    }
}