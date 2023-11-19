using System.Security.Cryptography.X509Certificates;

namespace WorldOfZuul
{
    public class Room
    {
        public string ShortDescription { get; private set; }
        public string LongDescription { get; private set; }
        public Dictionary<string, Room> Exits { get; private set; } = new();

        public List<string> buildings { get; private set; } = new();
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
            if (buildings.Contains(buildingName))
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
            if (VerifyIfValidBuildingType(building))
            {
                buildings.Remove(building);
                Console.WriteLine($"you have successfully demolished the building {building}!");
                return;
            }
            Console.WriteLine($"{building} is not a valid building to demolish.");
        }

        public void AddBuildingToRoom(string building)
        {
            if (VerifyIfValidBuildingType(building))
            {
                buildings.Add(building);
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
            buildings.Add("basic house");
            buildings.Add("hospital");
            buildings.Add("eco house");
            buildings.Add("community center");
            buildings.Add("luxury house");
            buildings.Add("public transport");
        }
    }

    public class Energy : Room
    {
        public Energy(string shortDesc, string longDesc) : base(shortDesc, longDesc)
        {

        }
    }

    public class Commercial : Room
    {
        public Commercial(string shortDesc, string longDesc) : base(shortDesc, longDesc)
        {
            buildings.Add("food supply");
            buildings.Add("shops");
        }
    }
}
