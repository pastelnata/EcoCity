using world_of_zuul;

namespace WorldOfZuul
{
    public class ENERGY
    {
        public static int energyIncome;
        public static int limit;

        public static void DisplayEnergy()
        {
            EnergyLimit();
            Console.WriteLine($"You have {energyIncome} kWh of energy.");
            Console.WriteLine($"You have to achieve {limit} kWh of energy today, otherwise you lose half of your money.");
        }

        public static void EnergyLimit()
        {
            switch (Game.dayCounter.currentDay)
            {
                case 1:
                    limit = 50;
                    break;
                case 2:
                    limit = 150;
                    break;
                case 3:
                    limit = 350;
                    break;
                case 4:
                    limit = 750;
                    break;
                case 5:
                    limit = 1200;
                    break;
            }
        }

        public static void EnergyIsEnough()
        {
            if(energyIncome < limit)
            {
                Game.currentMoney = Game.currentMoney / 2;
            }
        }

        public static void DetermineBuildingType(string building)
        {
            switch (building)
            {
                case "basic house":
                    Happyness.happyness += 50;
                    break;
                case "eco house":
                    Happyness.happyness += 500;
                    break;
                case "luxury house":
                    Happyness.happyness += 250;
                    break;
                case "hospital":
                    Happyness.happyness += 250;
                    break;
                case "community center":
                    Happyness.happyness += 500;
                    break;
                case "public transport":
                    Happyness.happyness += 1000;
                    break;
                case "food supply":
                    Happyness.happyness += 250;
                    break;
                case "shops":
                    Happyness.happyness += 50;
                    break;
                case "coal energy":
                    Happyness.happyness += 50;
                    energyIncome += 50;
                    break;
                case "oil supply":
                    Happyness.happyness += 50;
                    energyIncome += 100;
                    break;
                case "wind energy":
                    Happyness.happyness += 250;
                    energyIncome += 40;
                    break;
                case "solar energy":
                    Happyness.happyness += 250;
                    energyIncome += 35;
                    break;
                case "fission energy":
                    Happyness.happyness += 50;
                    energyIncome += 500;
                    break;
                case "fusion energy":
                    Happyness.happyness += 1000;
                    energyIncome += 1000;
                    break;
            }
        }
    }
} 
