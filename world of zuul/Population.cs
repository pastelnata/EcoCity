using System;
using world_of_zuul;

namespace WorldOfZuul
{
    public static class Population
    {     
        public static int populationGrow = 50;
        public static int startingPopulation = 500;
        
       /* public void DecreasePopulation(int amount = 200)
        {
            Pollution pollution = new Pollution();
            startingPopulation -= amount;

            if (pollution.StartPollution > pollution.MaxPollution)
            {
                Console.WriteLine($"You have exceeded the limit and have lost {currentPopulation}");
            }
        }*/
        public static void increasePopulation()
        {
            startingPopulation += 50;
            Console.WriteLine($"There is {startingPopulation} people  |");
        }

        public static void displayPopulation()
        {
            Console.WriteLine($"There is {startingPopulation} people");
        }

    }
}