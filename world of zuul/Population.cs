using System;
using world_of_zuul;
using static WorldOfZuul.Happyness;
using static world_of_zuul.VisualTextWriter;
using static System.ConsoleColor;

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
            WriteLine($"There is {PopulationLevel} people  |");
        }

        public static void displayPopulation()
        {
            WriteLine($"There is {PopulationLevel} people");
        }

    }
}