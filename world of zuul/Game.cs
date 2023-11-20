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
        public static Room? currentRoom;
        private Room? previousRoom;
        private Stack<Room?> roomHistory = new();
        private Room[] rooms = new Room[9];

        public static int currentMoney = new();
        public static int buildingProfit = new();

        private static DayProgress dayCounter = new DayProgress(0);

        public Game()
        {
            CreateRooms();
        }
        private void CreateRooms()
        {
            Room? northwestside = new("Northwestside", "You are standing in the Northwestside of your city.");
            Room? northside = new("Northside", "You are standing in the Nothside of your city.");
            Room? northeastside = new("Northeastside", "You are standing in the Northeastside of your city.");
            Residence? residence = new("Residence", "You are standing in the residencial area of your city");
            Room? centre = new("Centre", "You are standing in the centre.");
            Energy? energy = new("Energy", "You are now located in the energy area of your city.");
            Room? southwestside = new("Southwestside", "You are standing in the Southwestside of your city.");
            Commercial? commercial = new("commercial", "You are now located in the commercial area of your city.");
            Room? southeastside = new("Southeastside", "You are now located in the Southeastside of your city.");

            rooms = new Room[] { northwestside, northside, northeastside, residence, centre, energy, southwestside, commercial, southeastside };

            // room.SetExits(North, East, South, West)
            northwestside.SetExits(null, northside, residence, null);
            northside.SetExits(null, northeastside, centre, northwestside);
            northeastside.SetExits(null, null, energy, northside);
            residence.SetExits(northwestside, centre, southwestside, null);
            centre.SetExits(northside, energy, commercial, residence);
            energy.SetExits(northeastside, null, southeastside, centre);
            southwestside.SetExits(residence, commercial, null, null);
            commercial.SetExits(centre, southeastside, null, southwestside);
            southeastside.SetExits(energy, null, null, commercial);

            currentRoom = centre;
            roomHistory.Push(currentRoom);
        }
        public void Play()
        {
            Parser parser = new();

            PrintWelcome();

            dayCounter.InitializeTimer();

            currentMoney = 10000;

            buildingProfit = 0;

            bool continuePlaying = true;
            while (continuePlaying)
            {
                Console.WriteLine(currentRoom?.ShortDescription);
                Console.Write("> ");

                string? input = Console.ReadLine();

                if (dayCounter.currentDay == 6)
                {
                    Console.WriteLine("Game over! You have reached the day limit.");
                    break;
                }

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

                switch (command.Name)
                {
                    case "describe":
                        Console.WriteLine(currentRoom?.LongDescription);
                        break;

                    case "actions":
                        ActionHandler();
                        break;

                    case "customize":
                        CustomizeHandler();
                        break;

                    case "return":
                        Return();
                        PrintMap(currentRoom, rooms);
                        break;

                    case "move":
                        MoveHandler();
                        break;

                    case "quit":
                        continuePlaying = false;
                        break;

                    case "help":
                        PrintHelp();
                        break;

                    default:
                        Console.WriteLine("I don't know that command.");
                        break;
                }
            }

            Console.WriteLine("Thank you for playing EcoCity!");
        }

        /*-------------------------------------------------------------------------------------
            Handles user commands.
        ---------------------------------------------------------------------------------------*/

        private void ActionHandler()
        {
            List<string> validActions = new List<string> { "skip day", "stats", "back" };
            PrintUserOptions(validActions);

            Console.Write("> ");
            string? selectedAction = Console.ReadLine();

            selectedAction = ValidateInput(validActions, selectedAction);

            if (selectedAction == "skip day")
            {
                Console.WriteLine("you have skipped a day.");
                dayCounter.UpdateDay();
            }
            else if (selectedAction == "stats")
            {
                Console.WriteLine($"current money: {currentMoney}");
            }
        }

        private void MoveHandler()
        {
            List<string> validDirections = new List<string> { "north", "east", "south", "west", "back" };
            PrintUserOptions(validDirections);

            Console.Write("> ");
            string? selectedDirection = Console.ReadLine();

            selectedDirection = ValidateInput(validDirections, selectedDirection);

            if (selectedDirection != "back")
            {
                Move(selectedDirection);
                PrintMap(currentRoom, rooms);
            }
        }

        private void CustomizeHandler()
        {
            List<string> validCustomizeOptions = new List<string> { "infrastructure", "energy", "population", "back" };
            PrintUserOptions(validCustomizeOptions);

            Console.Write("> ");
            string? selectedCustomizeOption = Console.ReadLine();

            selectedCustomizeOption = ValidateInput(validCustomizeOptions, selectedCustomizeOption);

            if (selectedCustomizeOption == "infrastructure")
            {
                Console.WriteLine("These are the current buildings you have in this room:");
                Room.DisplayBuildingsInTheCurrentRoom();
                InfrastructureHandler();
            }
            if (selectedCustomizeOption == "energy")
            {
                EnergyHandler();
            }
            if (selectedCustomizeOption == "population")
            {
                Population.DisplayPopulation();
            }

        }
        private void InfrastructureHandler()
        {
            List<string> validInfrastructureOptions = new List<string> { "build", "demolish", "back" };
            PrintUserOptions(validInfrastructureOptions);

            Console.Write("> ");
            string? selectedInfrastructureOption = Console.ReadLine();

            selectedInfrastructureOption = ValidateInput(validInfrastructureOptions, selectedInfrastructureOption);

            if (selectedInfrastructureOption == "build")
            {
                Build();
            }
            if (selectedInfrastructureOption == "demolish")
            {
                Demolish();
            }
            if (selectedInfrastructureOption == "back")
            {
                CustomizeHandler();
            }
        }



        /*-------------------------------------------------------------------------------------
            Executes commands like: move, customize, etc.
        ---------------------------------------------------------------------------------------*/
        private void Build()
        {
            Console.WriteLine("What building do you wish to build out of these options?");
            Building.DisplayBuildingsCosts();
            Console.WriteLine("back");

            Console.Write("> ");
            string? buildingChoice = Console.ReadLine();
            

            while (buildingChoice == null || currentRoom != null && !currentRoom.buildings.ContainsKey(buildingChoice))
            {
                Console.WriteLine("that is not a valid building.");
                Console.Write("> ");
                buildingChoice = Console.ReadLine();
            }
            if (Building.CheckIfPlayerHasEnoughMoney(buildingChoice))
            {
                Building.AddBuildingProfitToDailyMoneyManager(buildingChoice);
                Building.DeductPlayersMoney(buildingChoice);
                currentRoom?.AddBuildingToRoom(buildingChoice);
                Console.WriteLine($"current money: {currentMoney}");
            }
            if(buildingChoice == "back")
            {
                InfrastructureHandler();
            }
        }

        private void Demolish()
        {
            Console.WriteLine("What building do you wish to demolish out of these options?");
            Room.DisplayBuildingsInTheCurrentRoom();
            Console.WriteLine("back");

            Console.Write("> ");
            string? buildingChoice = Console.ReadLine();
            if (buildingChoice != null)
            {
                currentRoom?.RemoveBuildingFromRoom(buildingChoice);
            }
            else if(buildingChoice == "back")
            {
                InfrastructureHandler();
            }
        }

        private void EnergyHandler()
        {

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
            Console.WriteLine("Type 'describe' to see describtion of a place you are currently located in.");
            Console.WriteLine("Type 'return' to go to the previous room.");
            Console.WriteLine("Type 'back' to go back.");
            Console.WriteLine("Type 'help' to print this message again.");
            Console.WriteLine("Type 'quit' to exit the game.");
        }

        public static void GetRoomBuildingsInfo(Dictionary<string, int> buildings)
        {
            foreach (var building in buildings)
            {
                Console.WriteLine(buildings.Keys);
            }
        }

        private void Return()
        {
            if (roomHistory.Count == 1)
            {
                Console.WriteLine("You can't return from here!");
            }
            else
            {
                roomHistory.Pop();
                currentRoom = roomHistory.Peek();
            }
        }

        private static void PrintMap(Room? currentRoom, Room[] rooms)
        {
            Console.WriteLine();
            Console.WriteLine("Map:");
            for (int i = 0; i < rooms.Length; i++)
            {

                if (rooms[i] == currentRoom)
                {
                    Console.Write("X ");
                }
                else
                {
                    Console.Write(". ");
                }
                if ((i + 1) % 3 == 0)
                {
                    Console.WriteLine();
                }
            }
        }

        /*-------------------------------------------------------------------------------------
           Util - functions to avoid code repetition.
       ---------------------------------------------------------------------------------------*/

        private string ValidateInput(List<string> validInputs, string? selectedInput)
        {

            while (selectedInput == null || !validInputs.Contains(selectedInput))
            {
                Console.WriteLine("That is not valid option.");
                Console.Write("> ");
                selectedInput = Console.ReadLine();
            }

            return selectedInput;
        }

        private void PrintUserOptions(List<string> Options)
        {
            for (int i = 0; i < Options.Count; i++)
            {
                Console.WriteLine(Options[i]);
            }
        }
    }
}


