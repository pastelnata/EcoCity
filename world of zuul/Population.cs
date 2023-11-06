using System;

namespace WorldOfZuul
{
    public class Population
    {
        public int currentPopulation { get; private set; }

        public Population(int initialPopulation)
        {
            currentPopulation = initialPopulation;
        }

        public Population()
        {
        }

        public void IncreasePopulation(int amount)
        {
            currentPopulation += amount;
        }

        public void DecreasePopulation(int amount)
        {
            currentPopulation -= amount;
        }
    }
}
