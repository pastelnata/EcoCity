using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using static world_of_zuul.Pollution;

namespace WorldOfZuul
{
    public class Game
    {
        public static Room? currentRoom;
        private Room? previousRoom;
        private Stack<Room?> roomHistory = new();
        public static Room[] rooms = new Room[9];

        public static int currentMoney = new();
        public static int buildingProfit = new();

        public static DayProgress dayCounter = new DayProgress(0);

        public EnergyBuilding energy = new EnergyBuilding();

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
                    case "4":
                        Console.WriteLine(currentRoom?.LongDescription);
                        break;

                    case "actions":
                    case "3":
                        ActionHandler();
                        break;

                    case "customize":
                    case "2":
                        CustomizeHandler();
                        break;
                    case "move":
                    case "1":
                        MoveHandler();
                        break;

                    case "quit":
                        continuePlaying = false;
                        break;

                    case "help":
                    case "6":
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
            List<string> validActions = new List<string> {"skip day","stats","back","1","2","B"};
            Console.Write("Type: ");
            Console.Write("     < 1 >         < 2 >          < B >  ");
            Console.WriteLine();
            for (int i = 0; i <=2; i++)
            {
                Console.Write($"   |     {validActions[i]}");
       
            }

            Console.Write("     |");
            
            Console.WriteLine();
            Console.Write("> ");
            string? selectedAction = Console.ReadLine();

            selectedAction = ValidateInput(validActions, selectedAction);

            if (selectedAction == "skip day" || selectedAction == "1")
            {
                Console.WriteLine("you have skipped a day.");
                dayCounter.UpdateDay();
            }
            else if (selectedAction == "stats")
            {
                Console.WriteLine($"current money: {currentMoney}");
                Console.Write("Pollution: ");
                DisplayPollution();
                
            }
        }

        private void MoveHandler()
        {
            List<string> validDirections = new List<string> { "north", "east", "south", "west", "back", "N", "E", "S", "W", "B" };
            while (true)
            {
                Console.Write("Choose: ");
                Console.Write(" < N >        < E >         < S >        < W >        < B >  ");
                Console.WriteLine();
                for (int i = 0; i < 5; i++)
                {
                    Console.Write($"   |     {validDirections[i]}");
                }

                Console.Write("   |");
                Console.WriteLine();

                string? selectedDirection = Console.ReadLine();

                selectedDirection = ValidateInput(validDirections, selectedDirection);

                if (selectedDirection != "B")
                {
                    Move(selectedDirection);
                    PrintMap(currentRoom, rooms);
                }
                else
                {
                    break;
                }
            }
        }

        private void CustomizeHandler()
        {
            List<string> validCustomizeOptions = new List<string> {"infrastructure","energy","population","back","1","2","3","B"};
            Console.WriteLine("Choose:   < 1 >       < 2 >      < 3 >     < B > ");
            Console.Write("> ");
            for (int i = 0; i < 4; i++)
            {
                Console.Write($" | {validCustomizeOptions[i]}");
            }
            Console.Write(" |");
            Console.WriteLine();
            string? selectedCustomizeOption = Console.ReadLine();

            selectedCustomizeOption = ValidateInput(validCustomizeOptions, selectedCustomizeOption);

            if (selectedCustomizeOption == "1")
            {
                Console.WriteLine("These are the current buildings you have in this room:");
                currentRoom?.DisplayBuildingsInTheCurrentRoom();
                InfrastructureHandler();
            }
            if (selectedCustomizeOption == "2")
            {
                Console.WriteLine("You currently have these buildings in this room:");
                energy.DisplayEnergies();
                energy.EnergyHandler();
            }
            if (selectedCustomizeOption.ToLower() == "3")
            {
                Population.displayPopulation();
            }

        }
        private void InfrastructureHandler()
        {
            Console.WriteLine("Here you can");
            List<string> validInfrastructureOptions = new List<string> {"build", "demolish", "back", "1", "2", "B"};
            Console.WriteLine("Choose:   < 1 >         < 2 >           < B >");

            for (int i = 0; i < 3; i++)
            {
                Console.Write($"   |     {validInfrastructureOptions[i]}");
            }
            Console.Write("   |   ");
            Console.WriteLine();

            Console.Write("> ");
            string? selectedInfrastructureOption = Console.ReadLine();

            selectedInfrastructureOption = ValidateInput(validInfrastructureOptions, selectedInfrastructureOption);

            if (selectedInfrastructureOption == "1")
            {
                Build();
            }
            if (selectedInfrastructureOption == "2")
            {
                Demolish();
            }
            
        }




        /*-------------------------------------------------------------------------------------
            Executes commands like: move, customize, etc.
        ---------------------------------------------------------------------------------------*/

        private void Build()
        {
            Console.WriteLine("What building do you wish to build out of these options?");

            Dictionary<string, int>? buildingsInRoom = currentRoom?.buildings;

            if (buildingsInRoom == null || buildingsInRoom.Values.All(value => value == -1))
            {
                Console.WriteLine("you can't build here yet.");
                return;
            }
            else
            {
                Building.DisplayBuildingsCosts();
                Console.WriteLine("back");

                Console.Write("> ");
                string? buildingChoice = Console.ReadLine();

                if(buildingChoice == "back")
                {
                    InfrastructureHandler();
                    return;
                }
                while (buildingChoice == null || currentRoom != null && !currentRoom.buildings.ContainsKey(buildingChoice))
                {
                    Console.WriteLine("that is not a valid building.");
                    Console.Write("> ");
                    buildingChoice = Console.ReadLine();
                }
                if (Building.CanPlayerCanAffordBuilding(buildingChoice))
                {
                    Building.UpdateDailyProfit(buildingChoice);
                    Building.DeductPlayersMoney(buildingChoice);
                    currentRoom?.AddBuildingToRoom(buildingChoice);
                    Console.WriteLine($"current money: {currentMoney}");
                }
            }
        }

        public void AvailableBuldings()
        {
            Console.WriteLine();
        }

        private void Demolish()
        {
            Console.WriteLine("What building do you wish to demolish out of these options?");
            currentRoom?.DisplayBuildingsInTheCurrentRoom();
            Console.WriteLine("back");

            Console.Write("> ");
            string? buildingChoice = Console.ReadLine();
            if (buildingChoice != null)
            {
                currentRoom?.RemoveBuildingFromRoom(buildingChoice);
            }
            else if(buildingChoice == "back")
            {
                CustomizeHandler();
                return;
            }
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
            Console.WriteLine("Type '1' to move around");
            Console.WriteLine("Type '2' to customize your city");
            Console.WriteLine("Type '3' to skip a day or check your progress");
            Console.WriteLine("Type '4' to see describtion of a place you are currently located in.");
            Console.WriteLine("Type '6' to print this message again.");
            Console.WriteLine("Type 'B' to go return to options.");
            Console.WriteLine("Type < Quit > to exit the game :'( .");
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
            
            
            for (int i = 0; i < rooms.Length; i++)
            {

                if (rooms[i] == currentRoom)
                {
                    Console.Write("o ");
                }
                else
                {
                    Console.Write("- ");
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


