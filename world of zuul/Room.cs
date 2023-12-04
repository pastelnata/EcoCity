using System.Security.Cryptography.X509Certificates;

namespace WorldOfZuul
{
    public class Room
    {
        public string ShortDescription { get; private set; }
        public string LongDescription { get; private set; }
        public Dictionary<string, Room> Exits { get; private set; } = new();

        public Dictionary<string, int> buildings { get; private set; } = new();

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
                Console.WriteLine($"you have successfully added the building {building}!");
                return;
            }
            Console.WriteLine($"{building} is not a valid building for this room.");
        }
    }

    public class Residence : Room
    {
        public Residence(string shortDesc, string longDesc) : base(shortDesc, longDesc)
        {
            buildings.Add("basic house", 0);
            buildings.Add("hospital", 0);
            buildings.Add("eco house", 0);
            buildings.Add("community center", 0);
            buildings.Add("luxury house", 0);
            buildings.Add("public transport", 0);
        }
    }

    public class Energy : Room
    {
        public Energy(string shortDesc, string longDesc) : base(shortDesc, longDesc)
        {
            buildings.Add("coal energy", 0);
            buildings.Add("oil supply", 0);
            buildings.Add("wind energy", 0);
            buildings.Add("solar energy", 0);
            buildings.Add("fision energy", 0);
            buildings.Add("fussion energy", 0);
        }
    }

    public class Commercial : Room
    {
        public Commercial(string shortDesc, string longDesc) : base(shortDesc, longDesc)
        {
            buildings.Add("food supply", 0);
            buildings.Add("shops", 0);
        }
    }
}
