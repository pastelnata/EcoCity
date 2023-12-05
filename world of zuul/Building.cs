using System.ComponentModel.DataAnnotations;
using WorldOfZuul;

class Building
{
    public static Dictionary<string,int> buildingCosts { get; } = new Dictionary<string, int>()
    {
        {"basic house", 500},
        {"eco house", 800},
        {"luxury house", 1500},
        {"hospital", 2000},
        {"community center", 1000},
        {"public transport", 2500},
        {"food supply", 800},
        {"shops", 1200},
        {"coal energy", 3000},
        {"oil supply", 4000},
        {"wind energy", 2500},
        {"solar energy", 2000},
        {"fission energy", 5000},
        {"fusion energy", 10000},
    };

    // Dictionary with buildings and their corresponding amounts of increased pollution when built
    public static Dictionary<string,int> buildingPollution { get; } = new Dictionary<string, int>()
    {
        {"basic house", 10},
        {"eco house", 1},
        {"luxury house", 50},
        {"hospital", 30},
        {"community center", 10},
        {"public transport", -100},
        {"food supply", 200},
        {"shops", 300},
        {"coal energy", 200},
        {"oil supply", 400},
        {"wind energy", 10},
        {"solar energy", 20},
        {"fission energy", 50},
        {"fusion energy", 0},
    };

    public static bool CanPlayerCanAffordBuilding(string buildingToBuy)
    {
        if (Game.currentMoney >= buildingCosts[buildingToBuy])
        {
            return true;
        }
        else
        {
            Console.WriteLine("you do not have enough money for this building.");
            return false;
        }
    }

    public static void UpdateDailyProfit(string buildingBought)
    {
        Dictionary<string, int>? buildingCount = Game.currentRoom?.buildings;
        Game.buildingProfit += Convert.ToInt32(buildingCosts[buildingBought] * 0.2);
    }

    public static void DeductPlayersMoney(string buildingBought)
    {
        Game.currentMoney -= Convert.ToInt32(buildingCosts[buildingBought]);
    }

    public static void DailyMoneyManager()
    {
        Game.currentMoney += Game.buildingProfit;
    }

    public static void DisplayBuildingsCosts()
    {
        Dictionary<string, int>? buildingsInRoom = Game.currentRoom?.buildings;

        if(buildingsInRoom != null)
        {
            var buildingsInRoomAndCost = buildingsInRoom.Keys.Intersect(buildingCosts.Keys);
            foreach (var building in buildingsInRoomAndCost)
            {
                if (buildingsInRoom[building] != -1)
                {
                    Console.WriteLine("{0}: {1} euros", building, buildingCosts[building]);
                }
            }
        }
        else
        {
            Console.WriteLine("you can't build here yet.");
        }
    }
}



