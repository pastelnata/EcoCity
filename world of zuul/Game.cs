using System.ComponentModel;
using System.Runtime.CompilerServices;

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
            Room? northwestside = new("Northwestside", "You are standing in the Northwestside of your city.");
            Room? northside = new("Northside", "You are standing in the Northside of your city.");
            Room? northeastside = new("Northeastside", "You are standing in the Northeastside of your city.");
            Room? westside = new("Westside", "You are now located in the Westside of your city.");
            Room? centre = new("Centre", "You are standing in the centre.");
            Room? eastside = new("Eastside", "You are now located in the Eastside of your city.");
            Room? southwestside = new("Southwestside", "You are standing in the Southwestside of your city.");
            Room? southside = new("Southside", "You are now located in the Southside of your city.");
            Room? southeastside = new("Southeastside", "You are now located in the Southeastside of your city.");


            // room.SetExits(North, East, South, West)
            northwestside.SetExits(null, northside, westside, null);
            northside.SetExits(null, northeastside, centre, northwestside);
            northeastside.SetExits(null, null, eastside, northside);
            westside.SetExits(northwestside, centre, southwestside, null);
            centre.SetExits(northside, eastside, southside, westside); 
            eastside.SetExits(northeastside, null, southeastside, centre);
            southwestside.SetExits(southwestside, southside, null, null);
            southside.SetExits(centre, southeastside, null, southwestside);
            southeastside.SetExits(eastside,null , null, southside);

            currentRoom = centre;
        }

        public void Play()
        {
            Parser parser = new();
            int day = 0;
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
                        Console.WriteLine($"current day: {day}");
                        Console.WriteLine("skip day");

                        string? actionOptions = Console.ReadLine();
                        List<string> actionsOptions = new List<string> {"skip day"};
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

                            string userInput1 = Console.ReadLine();

                            while (!infrastructureOptions.Contains(userInput1))
                            {
                                Console.WriteLine("That is not a valid option.");
                                Console.Write("> ");
                                userInput1 = Console.ReadLine();  
                            }

                            if (userInput1 == "build")
                            {
                                
                                Console.WriteLine("What do you wish to build?");
                                Console.WriteLine(" ");

                                string building1 = "building X";
                                string building2 = "building Y";

                                Build("building X", 0, 0, 0);
                                Console.WriteLine("");
                                Build("building Y", 0, 0, 0);
                                Console.Write("> ");

                                List<string> building = new List<string> {building1, building2};                                
                                string chosenBuilding = Console.ReadLine();

                                while (!building.Contains(chosenBuilding))
                                {
                                    Console.WriteLine("That building does not exist.");
                                    Console.Write("> ");
                                    chosenBuilding = Console.ReadLine();
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
            Console.WriteLine($"{buildingName}");
            Console.WriteLine($"Cost: {cost}");
            Console.WriteLine($"Energy consumption: {energyConsumption}");
            Console.WriteLine($"Pollution: {pollution}");

        }

        private static void BuildPlace()
        {
            
            
        }
        }
    }

