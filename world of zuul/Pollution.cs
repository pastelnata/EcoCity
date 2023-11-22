using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// POLLUTION CLASS


// To increase or decrease pollution use: 
// --- ChangePollution(integer); to increase pollution integer has to be positive, to decrease integer negative

// To print out current level of pollution use:
// --- DisplayPollution(); prints number to console

// Everytime you change pollutionLevel the setter will check if you haven't exceeded the maximum level

namespace world_of_zuul
{
    public static class Pollution
    {

        private static int pollutionLevel = 0;
        private static int maxPollution = 6075;
        public static int PollutionLevel
        {
            get { return pollutionLevel; }
            set
            {
                // Checks if pollution limit was exceeded each time the pollution level is changed
                if (value > maxPollution)
                {
                    // Here put the code which you want to be executed when the Pollution level is exceeded
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