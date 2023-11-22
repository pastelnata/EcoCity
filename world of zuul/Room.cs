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

        static List<string> day1Buildings = new List<string> {"food supply", "hospital", "oil supply", "eco house"};
        static List<string> day2Buildings = new List<string> {"wind energy", "solar energy", "community center", "shops"};
        static List<string> day3Buildings = new List<string> {"luxury house", "public transport", "fission"};
        static List<string> day5Buildings = new List<string> {"fusion"};

        public Room(string shortDesc, string longDesc)
        {
            ShortDescription = shortDesc;
            LongDescription = longDesc;
        }

        public void SetExits(Room? north, Room? east, Room? south, Room? west)
        {
            SetExit("north", north);
            SetExit("east", east); // energy
            SetExit("south", south); //commercial
            SetExit("west", west); //residence
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
            if (buildingsInRoom == null || buildingsInRoom.Count() < 1 || buildingsInRoom.Values.All(value => value == -1))
            {
                Console.WriteLine("There are no buildings here.");
            }
            else
            {
                foreach (KeyValuePair<string, int> building in buildingsInRoom)
                {               
                    if (building.Value != -1)
                    {
                        Console.WriteLine("{0}: {1}", building.Key, building.Value);
                    }
                }              
            }
        }

        public void DisplaysBuildingsAvailable()
        {
            if (Game.currentRoom != null)
            {
                BuildingsAccordingToDay(1, day1Buildings);
                BuildingsAccordingToDay(2, day2Buildings);
                BuildingsAccordingToDay(3, day3Buildings);
                BuildingsAccordingToDay(5, day5Buildings);
            }
            DisplayBuildingsInTheCurrentRoom();
        }

        private void BuildingsAccordingToDay(int day, List<string> newBuildings)
        {
            if (Game.dayCounter.currentDay == day)
            {
                UpdateBuildingAvailability(newBuildings);
            }
        }

        private void UpdateBuildingAvailability(List<string> newAvailableBuildings) 
        {
            foreach (var newBuilding in newAvailableBuildings)
            {
                if (Game.currentRoom != null && Game.currentRoom.buildings.ContainsKey(newBuilding))
                {
                    buildings[newBuilding] = 0;
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
