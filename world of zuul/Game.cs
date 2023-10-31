namespace WorldOfZuul
{
    public class Game
    {
        private Room? currentRoom;
        private Room? previousRoom;

        public Game()
        {
            CreateRooms();
        }

        private void CreateRooms()
        {
  
            Room? outside = new("Outside", "You are standing outside the main entrance of the university. To the east is a large building, to the south is a computing lab, and to the west is the campus pub.");
            Room? eastside = new("Eastside", "You are now located in the Eastside of your city.");
            Room? westside = new("Pub", "You've entered the campus pub. It's a cozy place, with a few students chatting over drinks. There's a bar near you and some pool tables at the far end.");
            Room? southside = new("Lab", "You're in a computing lab. Desks with computers line the walls, and there's an office to the east. The hum of machines fills the room.");
            Room? office = new("Office", "You've entered what seems to be an administration office. There's a large desk with a computer on it, and some bookshelves lining one wall.");

            outside.SetExits(null, eastside, southside, westside); // North, East, South, West

            eastside.SetExit("west", outside);

            westside.SetExit("east", outside);

            southside.SetExits(outside, office, null, null);

            office.SetExit("west", southside);

            currentRoom = outside;
        }

        public void Play()
        {
            Parser parser = new();

            PrintWelcome();

            bool continuePlaying = true;
            while (continuePlaying)
            {
                Console.WriteLine(currentRoom?.ShortDescription);
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
                        Console.WriteLine("you entered actions");
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
                            List<string> infrastructureOptions = new List<string> {"build", "demolish"};

                            string userInput1 = Console.ReadLine();

                            while (!infrastructureOptions.Contains(userInput1))
                            {
                                Console.WriteLine("That is not a valid option.");
                                Console.Write("> ");
                                userInput1 = Console.ReadLine();  
                            }

                            if (userInput1 == "build")
                            {
                                string building1 = "Build X";
                                int costBuilding1 = 0;
                                int energyBuilding1 = 0;
                                int pollutionBuilding1 = 0;

                                string building2 = "Build Y";
                                int costBuilding2 = 0;
                                int energyBuilding2 = 0;
                                int pollutionBuilding2 = 0;

                                Build(building1, costBuilding1, energyBuilding1, pollutionBuilding1);
                                Build(building2, costBuilding2, energyBuilding2, pollutionBuilding2);
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

                        while (!directions.Contains(inputLine))
                        {
                            Console.WriteLine("That is not valid direction.");
                            Console.Write("> ");
                            inputLine = Console.ReadLine();
                        }

                        Move(inputLine);
                        
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
            Console.WriteLine("What do you wish to build?");
            Console.WriteLine("");
            Console.WriteLine($"Build {buildingName}");
            Console.WriteLine($"Cost: {cost}");
            Console.WriteLine($"Energy consumption: {energyConsumption}");
            Console.WriteLine($"Pollution: {pollution}");
        }
    }
}
