using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace WorldOfZuul
{
    public class Game
    {
        private Room? currentRoom;
        private Room? previousRoom;
        private Room[] rooms = new Room[9];

        public Game()
        {
            CreateRooms();
        }

        private void CreateRooms()
        {
            Room? northwestside = new("Northwestside", "You are standing in the Northwestside of your city.");
            Room? northside = new("Northside", "You are standing in the Northside of your city.");
            Room? northeastside = new("Northeastside", "You are standing in the Northeastside of your city.");
            Room? westside = new("Westside", "You are now located in the Westside of your city.");
            Room? centre = new("Centre", "You are standing in the centre.");
            Room? eastside = new("Eastside", "You are now located in the Eastside of your city.");
            Room? southwestside = new("Southwestside", "You are standing in the Southwestside of your city.");
            Room? southside = new("Southside", "You are now located in the Southside of your city.");
            Room? southeastside = new("Southeastside", "You are now located in the Southeastside of your city.");

            rooms = new Room[] {northwestside, northside, northeastside, westside, centre, eastside, southwestside, southside, southeastside};

            // room.SetExits(North, East, South, West)
            northwestside.SetExits(null, northside, westside, null);
            northside.SetExits(null, northeastside, centre, northwestside);
            northeastside.SetExits(null, null, eastside, northside);
            westside.SetExits(northwestside, centre, southwestside, null);
            centre.SetExits(northside, eastside, southside, westside); 
            eastside.SetExits(northeastside, null, southeastside, centre);
            southwestside.SetExits(westside, southside, null, null);
            southside.SetExits(centre, southeastside, null, southwestside);
            southeastside.SetExits(eastside,null , null, southside);

            currentRoom = centre;
        }

        public void Play()
        {
            Parser parser = new();

            int day = 0;

            Dictionary<string, int> buildingCounters = new Dictionary<string, int>
            {
                { "basic house", 0 },
                { "coal energy", 0 },
                { "food supply", 0 },
                { "hospital", 0 },
                { "oil supply", 0 },
                { "eco house", 0 },
                { "wind energy", 0 },
                { "solar energy", 0 },
                { "community center", 0 },
                { "shops", 0 },
                { "luxury house", 0 },
                { "public transport", 0 },
                { "fission energy", 0 },
                { "fussion energy", 0 }
            };

            PrintWelcome();

            bool continuePlaying = true;
            while (continuePlaying)
            {
                Console.WriteLine(currentRoom?.ShortDescription);
                Console.WriteLine(buildingCounters);
                Console.Write("> ");

                string? input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Please enter a command.");
                    continue;
                }

                Command? command = parser.GetCommand(input);

                if (command == null)
                {
                    Console.WriteLine("I don't know that command.");
                    continue;
                }

                switch(command.Name)
                {
                    case "look":
                        Console.WriteLine(currentRoom?.LongDescription);
                        break;

                    case "actions":
                        Console.WriteLine($"current day: {day}");
                        Console.WriteLine("skip day");
                        Console.WriteLine("stats");

                        string? actionOptions = Console.ReadLine();
                        List<string> actionsOptions = new List<string> {"skip day", "stats"};
                        while (!actionsOptions.Contains(actionOptions))
                        {
                            Console.WriteLine("That is not a valid option.");
                            Console.Write("> ");
                            actionOptions = Console.ReadLine();
                        }

                        if (actionOptions == "skip day")
                        {
                            day++;
                            Console.WriteLine("you have skipped a day.");
                            Console.WriteLine($"current day: {day}");
                        }
                        break;

                    case "customize":
                        Console.WriteLine("energy");
                        Console.WriteLine("infrastructure");
                        Console.WriteLine("population");
                        Console.WriteLine("back");
                        Console.Write("> ");
                        
                        string? userInput = Console.ReadLine();
                        List<string> customizeOptions = new List<string> {"energy", "infrastructure", "population"};

                        while (!customizeOptions.Contains(userInput))
                        {
                            Console.WriteLine("That is not a valid option.");
                            Console.Write("> ");
                            userInput = Console.ReadLine();
                        }

                        if (userInput == "infrastructure")
                        {
                            Console.WriteLine("build");
                            Console.WriteLine("demolish");
                            Console.Write("> ");
                            List<string> infrastructureOptions = new List<string> {"build", "demolish"};

                            string? userInput1 = Console.ReadLine();

                            while (!infrastructureOptions.Contains(userInput1))
                            {
                                Console.WriteLine("That is not a valid option.");
                                Console.Write("> ");
                                userInput1 = Console.ReadLine();  
                            }

                            if (userInput1 == "build")
                            {
                                List<string> buildings = new List<string> {"basic house", "coal energy", "food supply", "hospital", "oil supply", "eco house", "wind energy", "solar energy", "community center", "shops", "luxury house", "public transport", "fission energy", "fusion energy"};
                                Console.WriteLine("What do you wish to build?");

                                switch (day)
                                {
                                    case 0:
                                        if (currentRoom == rooms[0])
                                        {
                                            Build("basic house", 0, 0, 0);
                                        }
                                        else if (currentRoom == rooms[1])
                                        {
                                            Build("coal energy", 0, 0, 0);
                                        }
                                        else
                                        {
                                            Console.WriteLine("You cannot build here yet. Try going into another room.");
                                        }
                                        break;
                                    case 1:
                                        if (currentRoom == rooms[0])
                                        {
                                            Build("basic house", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("eco house", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("hospital", 0, 0, 0);
                                        }
                                        else if (currentRoom == rooms[1])
                                        {
                                            Build("coal energy", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("oil supply", 0, 0, 0);
                                        }
                                        else if (currentRoom == rooms[2])
                                        {
                                            Build("food supply", 0, 0, 0);
                                        }
                                        break;
                                    case 2:
                                        if (currentRoom == rooms[0])
                                        {
                                            Build("basic house", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("eco house", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("hospital", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("community center", 0, 0, 0);
                                        }
                                        else if (currentRoom == rooms[1])
                                        {
                                            Build("coal energy", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("oil supply", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("wind energy", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("solar energy", 0, 0, 0);
                                        }
                                        else if (currentRoom == rooms[2])
                                        {
                                            Build("food supply", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("shops", 0, 0, 0);
                                        }                                    
                                        break;
                                    case 3:
                                        if (currentRoom == rooms[0])
                                        {
                                            Build("basic house", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("eco house", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("hospital", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("community center", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("luxury house", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("public transport", 0, 0, 0);
                                        }
                                        else if (currentRoom == rooms[1])
                                        {
                                            Build("coal energy", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("oil supply", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("wind energy", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("solar energy", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("fission", 0, 0, 0);
                                        }
                                        else if (currentRoom == rooms[2])
                                        {
                                            Build("food supply", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("shops", 0, 0, 0);
                                        }
                                        break;
                                    case 4:
                                        if (currentRoom == rooms[0])
                                        {
                                            Build("basic house", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("eco house", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("hospital", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("community center", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("luxury house", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("public transport", 0, 0, 0);
                                        }
                                        else if (currentRoom == rooms[1])
                                        {
                                            Build("coal energy", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("oil supply", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("wind energy", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("solar energy", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("fission", 0, 0, 0);
                                        }
                                        else if (currentRoom == rooms[2])
                                        {
                                            Build("food supply", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("shops", 0, 0, 0);
                                        }
                                        break;
                                    case 5:
                                        if (currentRoom == rooms[0])
                                        {
                                            Build("basic house", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("eco house", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("hospital", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("community center", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("luxury house", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("public transport", 0, 0, 0);
                                        }
                                        else if (currentRoom == rooms[1])
                                        {
                                            Build("coal energy", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("oil supply", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("wind energy", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("solar energy", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("fission", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("fusion energy", 0, 0, 0);
                                        }
                                        else if (currentRoom == rooms[2])
                                        {
                                            Build("food supply", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("shops", 0, 0, 0);
                                        }
                                        break;
                                }

                                string? buildingOption = Console.ReadLine();

                                while (!buildings.Contains(buildingOption))
                                    {
                                        Console.WriteLine("that is not a valid building.");
                                        Console.Write("> ");
                                        buildingOption = Console.ReadLine();
                                    }

                                if (buildingCounters.ContainsKey(buildingOption))
                                    {
                                        buildingCounters[buildingOption]++;
                                        Console.WriteLine($"you have sucessfully built {buildingOption}");
                                    }                       
                            }

                            if (userInput1 == "demolish")
                            {
                                Console.WriteLine("Demolish");
                            }
                        }

                        if (userInput == "population")
                        {
                            Console.WriteLine("you have entered population");
                        }

                        if (userInput == "energy")
                        {
                            List<string> buildings = new List<string>();
                            int statHolder1 = 0;
                            string statHolder2 = "";
                            int statHolder3 = 0;
                            double energy1Consumption = 0;
                            double energy1Income = 0;
                            double energy2Consumption = 0;
                            double energy2Income = 0;
                            double energy3Consumption = 0;
                            double energy3Income = 0;
                            if (energy1Consumption == 0 && energy2Consumption == 0 && energy3Consumption == 0)
                            {
                                Console.WriteLine($"You are currently not using any type of energy.");
                            }
                            Console.WriteLine($"You have {buildings.Count} buildings.");
                            Console.WriteLine(statHolder1);
                            Console.WriteLine(statHolder2);
                            Console.WriteLine(statHolder3);
                        }
                        
                        break;

                    case "back":
                        if (previousRoom == null)
                            Console.WriteLine("You can't go back from here!");
                        else
                            currentRoom = previousRoom;
                        break;

                    case "move":
                        Console.WriteLine("write one of the following directions:");
                        Console.WriteLine("north");
                        Console.WriteLine("south");
                        Console.WriteLine("east");
                        Console.WriteLine("west");
                        
                        Console.Write("> ");

                        string? inputLine = Console.ReadLine();
                        List<string> directions = new List<string> {"east", "south", "north", "west" };

                        while (!directions.Contains(inputLine) || currentRoom == null)
                        {
                            Console.WriteLine("That is not valid direction.");
                            Console.Write("> ");
                            inputLine = Console.ReadLine();
                        }

                        Move(inputLine);
                        PrintMap(currentRoom,rooms);
                        break;

                    case "quit":
                        continuePlaying = false;
                        break;

                    case "help":
                        PrintHelp();
                        break;

                    default:
                        Console.WriteLine("I don't know what command.");
                        break;
                }
            }

            Console.WriteLine("Thank you for playing EcoCity!");
        }

        private void Move(string direction)
        {
            if (currentRoom?.Exits.ContainsKey(direction) == true)
            {
                previousRoom = currentRoom;
                currentRoom = currentRoom?.Exits[direction];
            }
            else
            {
                Console.WriteLine($"You can't go {direction}!");
            }
        }

        private static void PrintWelcome()
        {
            Console.WriteLine("Welcome to EcoCity!");
            Console.WriteLine("EcoCity is a game where you can build your own sustainable city!");
            PrintHelp();
            Console.WriteLine();
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Remember: the more sustainable your city is, the more expensive it becomes.");
            Console.WriteLine("However, it attracts more people, which also means more money.");
            Console.WriteLine();
            Console.WriteLine("Type 'move' to navigate");
            Console.WriteLine("Type 'customize' to customize your city");
            Console.WriteLine("Type 'actions' to "); //TODO: ADD DESCRIPTION FOR ACTIONS
            Console.WriteLine("Type 'look' for more details.");
            Console.WriteLine("Type 'back' to go to the previous room.");
            Console.WriteLine("Type 'help' to print this message again.");
            Console.WriteLine("Type 'quit' to exit the game.");
        }
        
        private static void Build(string buildingName, int cost, int energyConsumption, int pollution)
        {
            Console.WriteLine($"{buildingName}");
            Console.WriteLine($"Cost: {cost}");
            Console.WriteLine($"Energy consumption: {energyConsumption}");
            Console.WriteLine($"Pollution: {pollution}");
        }

        private static void PlaceBuilding(string buildingName)
        {
            Console.WriteLine($"you have successfuly built {buildingName}");
        }

        private static void PrintMap(Room currentRoom,Room[] rooms)
        {
            Console.WriteLine("Map:");
            for(int i=0; i<rooms.Length; i++)
            {
                
                if(rooms[i]== currentRoom)
                {
                    Console.Write("X ");
                }
                else
                {
                    Console.Write(". ");
                }
                if((i+1)%3 == 0)
                {
                    Console.WriteLine();
                }
            }
        }
        }
    }

