using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace WorldOfZuul
{
    public class Game
    {
        private Room? currentRoom;
        private Room? previousRoom;
        private Stack<Room?> roomHistory = new();
        private Room[] rooms = new Room[9];

        public Game()
        {
            CreateRooms();
        }
        private void CreateRooms()
        {
            Room? northwestside = new("Northwestside", "You are standing in the Northwestside of your city.");
            Room? residence = new("residence", "You are standing in the residence area of your city.");
            Room? northeastside = new("Northeastside", "You are standing in the Northeastside of your city.");
            Room? westside = new("Westside", "You are now located in the Westside of your city.");
            Room? centre = new("Centre", "You are standing in the centre.");
            Room? energy = new("Eastside", "You are now located in the energy area of your city.");
            Room? southwestside = new("Southwestside", "You are standing in the Southwestside of your city.");
            Room? moneymaker = new("moneymaker", "You are now located in the moneymaker area of your city.");
            Room? southeastside = new("Southeastside", "You are now located in the Southeastside of your city.");

            rooms = new Room[] {northwestside, residence, northeastside, westside, centre, energy, southwestside, moneymaker, southeastside};

            // room.SetExits(North, East, South, West)
            northwestside.SetExits(null, residence, westside, null);
            residence.SetExits(null, northeastside, centre, northwestside);
            northeastside.SetExits(null, null, energy, residence);
            westside.SetExits(northwestside, centre, southwestside, null);
            centre.SetExits(residence, energy, moneymaker, westside); 
            energy.SetExits(northeastside, null, southeastside, centre);
            southwestside.SetExits(westside, moneymaker, null, null);
            moneymaker.SetExits(centre, southeastside, null, southwestside);
            southeastside.SetExits(energy,null , null, moneymaker);

            currentRoom = centre;
            roomHistory.Push(currentRoom);
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
                        List<string?> actionsOptions = new List<string?> {"skip day", "stats"};
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
                        List<string?> customizeOptions = new List<string?> {"energy", "infrastructure", "population", "Return"};

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
                            List<string?> infrastructureOptions = new List<string?> {"build", "demolish"};

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
                                        else
                                        {
                                            Console.WriteLine("You cannot build here yet. Try going into another room.");
                                            Console.WriteLine("Return");
                                            
                                            Return();
                                        }
                                        break;
                                    case 1:
                                        if (currentRoom == rooms[1])
                                        {
                                            Build("basic house", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("eco house", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("hospital", 0, 0, 0);
                                        }
                                        else if (currentRoom == rooms[7])
                                        {
                                            Build("food supply", 0, 0, 0);
                                        }
                                        break;
                                    case 2:
                                        if (currentRoom == rooms[1])
                                        {
                                            Build("basic house", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("eco house", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("hospital", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("community center", 0, 0, 0);
                                        }
                                        else if (currentRoom == rooms[7])
                                        {
                                            Build("food supply", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("shops", 0, 0, 0);
                                        }                                    
                                        break;
                                    case 3:
                                        if (currentRoom == rooms[1])
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
                                        else if (currentRoom == rooms[7])
                                        {
                                            Build("food supply", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("shops", 0, 0, 0);
                                        }
                                        break;
                                    case 4:
                                        if (currentRoom == rooms[1])
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
                                        else if (currentRoom == rooms[7])
                                        {
                                            Build("food supply", 0, 0, 0);
                                            Console.WriteLine(" ");
                                            Build("shops", 0, 0, 0);
                                        }
                                        break;
                                    case 5:
                                        if (currentRoom == rooms[1])
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
                                        else if (currentRoom == rooms[7])
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
                            Population.DisplayPopulation();
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

                    case "return":
                        Return();
                        PrintMap(currentRoom, rooms);
                        break;

                    case "move":
                        Console.WriteLine("which area of your city do you wish to go to?");
                        Console.WriteLine("residence");
                        Console.WriteLine("moneymaker");
                        Console.WriteLine("energy");
                        
                        Console.Write("> ");

                        string? inputLine = Console.ReadLine();
                        List<string> directions = new List<string> {"energy", "moneymaker", "residence", "west" };

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
                roomHistory.Push(currentRoom);
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
            Console.WriteLine("Type 'actions' to skip a day or check your progress");
            Console.WriteLine("Type 'look' for more details.");
            Console.WriteLine("Type 'return' to go to the previous room.");
            Console.WriteLine("Type 'back' to go back.");
            Console.WriteLine("Type 'help' to print this message again.");
            Console.WriteLine("Type 'quit' to exit the game.");
        }
        
        public static void Build(string buildingName, int cost, int energyConsumption, int pollution)
        {
            Console.WriteLine($"{buildingName}");
            Console.WriteLine($"Cost: {cost}");
            Console.WriteLine($"Energy consumption: {energyConsumption}");
            Console.WriteLine($"Pollution: {pollution}");
        }

         static void PlaceBuilding(string buildingName)
        {
            Console.WriteLine($"you have successfuly built {buildingName}");
        }


        private void Return()
        {
            if(roomHistory.Count == 1)
                {
                    Console.WriteLine("You can't return from here!");
                }
            else
            {
                roomHistory.Pop();
                currentRoom = roomHistory.Peek();
            }  
        }
        private static void PrintMap(Room? currentRoom,Room[] rooms)
        {
            Console.WriteLine();
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

