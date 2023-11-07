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

        private DayProgress dayCounter = new DayProgress(0);

        public Game()
        {
            CreateRooms();
        }
        private void CreateRooms()
        {
            Room? northwestside = new("Northwestside", "You are standing in the Northwestside of your city.");
            Room? northside = new("Northside", "You are standing in the Nothside of your city.");
            Room? northeastside = new("Northeastside", "You are standing in the Northeastside of your city.");
            Room? residence = new("Residence", "You are standing in the residence area of your city");
            Room? centre = new("Centre", "You are standing in the centre.");
            Room? energy = new("Eastside", "You are now located in the energy area of your city.");
            Room? southwestside = new("Southwestside", "You are standing in the Southwestside of your city.");
            Room? moneymaker = new("Moneymaker", "You are now located in the moneymaker area of your city.");
            Room? southeastside = new("Southeastside", "You are now located in the Southeastside of your city.");

            rooms = new Room[] { northwestside, northside, northeastside, residence, centre, energy, southwestside, moneymaker, southeastside };

            // room.SetExits(North, East, South, West)
            northwestside.SetExits(null, northside, residence, null);
            northside.SetExits(null, northeastside, centre, northwestside);
            northeastside.SetExits(null, null, energy, northside);
            residence.SetExits(northwestside, centre, southwestside, null);
            centre.SetExits(northside, energy, moneymaker, residence);
            energy.SetExits(northeastside, null, southeastside, centre);
            southwestside.SetExits(residence, moneymaker, null, null);
            moneymaker.SetExits(centre, southeastside, null, southwestside);
            southeastside.SetExits(energy, null, null, moneymaker);

            currentRoom = centre;
            roomHistory.Push(currentRoom);
        }

        public void Play()
        {
            Parser parser = new();

            PrintWelcome();

            dayCounter.InitializeTimer();

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

        private void OptionPrinter(List<string> Options)
        {
            for (int i = 0; i < Options.Count; i++)
            {
                Console.WriteLine(Options[i]);
            }
        }

        private void ActionHandler()
        {
            List<string> validActions = new List<string> { "skip day", "stats", "back" };
            OptionPrinter(validActions);
            
            Console.Write("> ");
            string? selectedAction = Console.ReadLine();
            
            selectedAction = ValidateInput(validActions, selectedAction);

            if (selectedAction == "skip day")
            {
                Console.WriteLine("you have skipped a day.");
                dayCounter.UpdateDay();
            }
            if (selectedAction == "stats")
            {

            }
        }

        private void MoveHandler()
        {
            List<string> validDirections = new List<string> { "north", "east", "south", "west", "back" };
            OptionPrinter(validDirections);

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
            List<string> validCustomizeOptions = new List<string> {"infrastructure", "energy", "population", "back"};
            OptionPrinter(validCustomizeOptions);

            Console.Write("> ");
            string? selectedCustomizeOption = Console.ReadLine();

            ValidateInput(validCustomizeOptions, selectedCustomizeOption);

            if (selectedCustomizeOption == "infrastructure")
            {
                Infrastructure();
            }
            if (selectedCustomizeOption == "energy")
            {
                Energy();
            }
            if (selectedCustomizeOption == "population")
            {
                Population.DisplayPopulation();
            }

        }


        /*-------------------------------------------------------------------------------------
            Executes commands like: move, customize, etc.
        ---------------------------------------------------------------------------------------*/

        private void Infrastructure()
        {
            List<string> validInfrastructureOptions = new List<string>{"build", "demolish", "back"};
            OptionPrinter(validInfrastructureOptions);

            Console.Write("> ");
            string? selectedInfrastructureOption = Console.ReadLine();

            ValidateInput(validInfrastructureOptions, selectedInfrastructureOption);
            
        }

        private void Energy()
        {
            Console.WriteLine("energy");
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

        private static void Build(string buildingName, int cost, int energyConsumption, int pollution)
        {
            Console.WriteLine($"{buildingName}");
            Console.WriteLine($"Cost: {cost}");
            Console.WriteLine($"Energy consumption: {energyConsumption}");
            Console.WriteLine($"Pollution: {pollution}");
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
    }
}


