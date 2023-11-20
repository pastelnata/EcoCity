using System;
using world_of_zuul;

namespace WorldOfZuul
{
    public class Population
    {
        Pollution pollution = new Pollution();

        public int populationGrow = 50;
        public int startingPopulation = 500;
        public int currentPopulation;
        
       
        public void DecreasePopulation(int amount = 200)
        {
            Pollution pollution = new Pollution();
            startingPopulation -= amount;

            if (pollution.StartPollution > pollution.MaxPollution)
            {
                Console.WriteLine($"You have exceeded the limit and have lost {currentPopulation}");
            }
        }
        public void IncreasePopulation()
        {
                   
        }
        public void DisplayPopulation()
        {           
            Console.WriteLine($"There is {startingPopulation} people");
        }

    }
}