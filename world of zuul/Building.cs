using System.Linq;
using world_of_zuul;

namespace WorldOfZuul
{
    interface IBuilding
    {
        string? type { get; set; }
        int cost { get; set; }
        int pollution { get; set; }
        int population { get; set; }
        int energyConsumption { get; set; }
        int DailyCost { get; set; }
    }


    class ResidenceBuilding : IBuilding
    {
        public string? type { get; set; }
        public int cost { get; set; }
        public int pollution { get; set; }
        public int population { get; set; }
        public int energyConsumption { get; set; }
        public int DailyCost { get; set; }
        public ResidenceBuilding(string buildingType, int buildingCost, int buildingPollution, int buildingPopulution, int buildingEnergyConsumption, int dailyCost)
        {
            type = buildingType;
            cost = buildingCost;
            pollution = buildingPollution;
            population = buildingPopulution;
            energyConsumption = buildingEnergyConsumption;
            DailyCost = dailyCost;
        }
    }
    public class EnergyBuilding : IBuilding
    {
        public int energyConsumption { get; set; }
        public int cost { get; set; }
        public string? type { get; set; }
        public int pollution { get; set; }
        public int population { get; set; }
        public int DailyCost { get; set; }

        public List<string> energies = new();

        public List<string> availableEnergies = new List<string> { "coal plant", "oil supply", "wind", "solar", "fission", "fusion" };

        private DayProgress dayCount = new DayProgress(0);

        public Pollution pollute = new Pollution();
        public void AvailableEnergies()
        {
            if (dayCount.currentDay == 0)
            {
                for (int i = 0; i < 1; i++)
                {
                    Console.WriteLine(availableEnergies[i]);
                }
                
            }
            if (dayCount.currentDay == 1)
            {
                for (int i = 0; i < 2; i++)
                {
                    Console.WriteLine(availableEnergies[i]);
                }
                
            }
            if (dayCount.currentDay == 2)
            {
                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine(availableEnergies[i]);
                }
            }
            if (dayCount.currentDay == 3)
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine(availableEnergies[i]);
                }
            }

            if (dayCount.currentDay == 4)
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine(availableEnergies[i]);
                }
            }

            if (dayCount.currentDay == 5)
            {
                for (int i = 0; i < 6; i++)
                {
                    Console.WriteLine(availableEnergies[i]);
                }
            }
            
        }

            public void DisplayEnergies()
            {
                for (int i= 10; i <= energies.Count(); i++)
                {
                    Console.WriteLine(energies[i]);
                }
            }
            public void EnergyHandler()
            {
                string? choice;
                Console.WriteLine("Choose energy type to build:");
                AvailableEnergies();
                Console.Write(">");
                while (true)
                {
                    choice = Console.ReadLine();
                    if (!availableEnergies.Contains(choice))
                    {
                        Console.WriteLine("Not valid. Try again!");
                        Console.WriteLine("Choose energy type to build from the list below:");
                        AvailableEnergies();
                    }
                    else
                    {
                        break;
                    }
                }

                if (choice == "coal plant")
                {
                    energies.Add("coal plant");
                    energyConsumption += 25;
                    pollute.IncreasePollution(100);
                    Console.WriteLine("You successfully built a coal plant.");
                }


                if (choice == "oil supply")
                {
                    energies.Add("oil supply");
                    energyConsumption += 50;
                    Console.WriteLine("You successfully built an oil supply.");
            }
                if (choice == "wind")
                {
                    energies.Add("wind");
                    energyConsumption += 30;
                    Console.WriteLine("You successfully built a wind power plant.");
            }
                if (choice == "solar")
                {
                    energies.Add("solar");
                    energyConsumption += 25;
                    Console.WriteLine("You successfully built a solar power plant.");
            }
                if (choice == "fission")
                {
                    energies.Add("fission");
                    energyConsumption += 100;
                    Console.WriteLine("You successfully built a fission power plant.");
            }
                if (choice == "fusion")
                {
                    energies.Add("fusion");
                    energyConsumption += 200;
                    Console.WriteLine("You successfully built a fusion power plant.");
            }

                if (energyConsumption == 0)
                {
                    Console.WriteLine("You are currently not using any type of energy!");
                }

                else
                {
                    Console.WriteLine($"You have {energyConsumption} kWh of energy.");
                }
            }
    }
}
       /*
       class CommercialBuilding : IBuilding
       {

       }
       */


