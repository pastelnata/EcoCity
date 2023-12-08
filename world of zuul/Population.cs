using System;
using world_of_zuul;
using static WorldOfZuul.Happyness;

namespace WorldOfZuul
{
    public static class Population
    {
        private static int[] PopulationGrow = { 100, 300, 700, 1500, 2400};
        private static int PopulationLevel = 350;


        public static void IncreasePopulation()
        {
            int index = HappynessLimit();
            if(index != -1 )
            {
                PopulationLevel += PopulationGrow[index];
            }
            Console.WriteLine($"There is {PopulationLevel} people  |");
        }

        public static void displayPopulation()
        {
            Console.WriteLine($"There is {PopulationLevel} people");
        }

    }
}