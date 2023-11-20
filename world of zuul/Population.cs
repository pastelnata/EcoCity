using System;
using world_of_zuul;

namespace WorldOfZuul
{
    public class Population
    {

        public int populationGrow = 50;
        public int startingPopulation = 500;
        public int currentPopulation;
        
       
        public void DecreasePopulation(int amount = 200)
        {
           
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