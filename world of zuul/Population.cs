using System;
using world_of_zuul;

namespace WorldOfZuul
{
    public class Population
    {
        Pollution pollution = new Pollution();
        public static int currentPopulation { get; private set; }
        public void IncreasePopulation(int amount)
        {
            currentPopulation += amount;
            //if (Day = Passed)
            {
                Console.WriteLine("The day passed and your town got 50 more people");
            }
        }

        public void DecreasePopulation(int amount)
        {
            Pollution pollution = new Pollution();
            amount = 200;
            currentPopulation -= amount;

            if (pollution.StartPollution > pollution.MaxPollution)
            {
                Console.WriteLine($"You have exceeded the limit and have lost {currentPopulation}");
            }
        }
        public static void DisplayPopulation()
        {
            currentPopulation = 500;
            Console.WriteLine($"There is {Population.currentPopulation} people");
        }
    }
}