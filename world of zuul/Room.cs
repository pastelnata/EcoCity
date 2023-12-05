using System.Security.Cryptography.X509Certificates;
using System;
using static world_of_zuul.Pollution;
using static Building;

namespace WorldOfZuul
{
    public class Room
    {
        public string ShortDescription { get; private set; }
        public string LongDescription { get; private set; }
        public Dictionary<string, Room> Exits { get; private set; } = new();

        public Dictionary<string, int> buildings { get; set; } = new();

        public Room(string shortDesc, string longDesc)
        {
            ShortDescription = shortDesc;
            LongDescription = longDesc;
        }

        public void SetExits(Room? north, Room? east, Room? south, Room? west)
        {
            SetExit("N", north);
            SetExit("E", east); // energy
            SetExit("S", south); //commercial
            SetExit("W", west); //residence
        }

        public void SetExit(string direction, Room? neighbor)
        {
            if (neighbor != null)
                Exits[direction] = neighbor;
        }

        public bool VerifyIfValidBuildingType(string buildingName)
        {
            if (buildings.ContainsKey(buildingName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemoveBuildingFromRoom(string building)
        {
            if (VerifyIfValidBuildingType(building) & buildings[building] > 0)
            {
                buildings[building]--;
                // Pollution decrease
                ChangePollution(-buildingPollution[building]);

                Console.WriteLine($"you have successfully demolished the building {building}!");
                return;
            }
            Console.WriteLine($"{building} is not a valid building to demolish.");
        }

        public void AddBuildingToRoom(string building)
        {
            if (VerifyIfValidBuildingType(building))
            {
                buildings[building]++;
                // Pollution increase
                ChangePollution(buildingPollution[building]);
                
                Console.WriteLine($"you have successfully added the building {building}!");
                return;
            }
            Console.WriteLine($"{building} is not a valid building for this room.");
        }

        public void DisplayBuildingsInTheCurrentRoom()
        {
            Dictionary<string, int>? buildingsInRoom = Game.currentRoom?.buildings;
            if (buildingsInRoom == null || buildingsInRoom.Count() < 1 || buildingsInRoom.Values.All(value => value == -1)) //goes through all values in the dictionary and checks if they're all -1
            {
                Console.WriteLine("There are no buildings here.");
            }
            else
            {
                foreach (KeyValuePair<string, int> building in buildingsInRoom)
                {
                    //if the buildings.Value is -1, it means its not available during that day. 
                    //Check DayProgress.BuildingsDailyUpdater() for clarification.               
                    if (building.Value != -1)
                    {
                        Console.WriteLine("{0}: {1}", building.Key, building.Value);
                    }
                }              
            }
        }

    }

    public class Residence : Room
    {
        public Residence(string shortDesc, string longDesc) : base(shortDesc, longDesc)
        {
            buildings.Add("basic house", 0);
            buildings.Add("hospital", -1);
            buildings.Add("eco house", -1);
            buildings.Add("community center", -1);
            buildings.Add("luxury house", -1);
            buildings.Add("public transport", -1);
        }
    }

    public class Energy : Room
    {
        public Energy(string shortDesc, string longDesc) : base(shortDesc, longDesc)
        {
            buildings.Add("coal energy", 0);
            buildings.Add("oil supply", -1);
            buildings.Add("wind energy", -1);
            buildings.Add("solar energy", -1);
            buildings.Add("fission energy", -1);
            buildings.Add("fusion energy", -1);
        }
    }

    public class Commercial : Room
    {
        public Commercial(string shortDesc, string longDesc) : base(shortDesc, longDesc)
        {
            buildings.Add("food supply", -1);
            buildings.Add("shops", -1);
        }
    }
}
