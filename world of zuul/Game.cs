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
            Residence? residence = new("Residence", "You are standing in the residence area of your city");
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
            if (selectedAction == "stats")
            {

            }
        }

        private void MoveHandler()
        {
            bool move = true;
            List<string> validDirections = new List<string> { "north", "east", "south", "west", "back" };
            while (move = true)
            {
                Console.WriteLine("Move: ");

                foreach (string word in validDirections)
                {
                    Console.Write(word + " ");          
                }
                Console.WriteLine();


                string? selectedDirection = Console.ReadLine();

                selectedDirection = ValidateInput(validDirections, selectedDirection);

                if (selectedDirection != "back")
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
            List<string> validCustomizeOptions = new List<string> { "infrastructure", "energy", "population", "back" };
            
            PrintUserOptions(validCustomizeOptions);

            Console.Write("> ");
            string? selectedCustomizeOption = Console.ReadLine();

            selectedCustomizeOption = ValidateInput(validCustomizeOptions, selectedCustomizeOption);

            if (selectedCustomizeOption == "infrastructure")
            {
                Console.WriteLine("These are the current buildings you have in this room:");
                DisplayBuildingsInTheCurrentRoom();
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
            
        }



        /*-------------------------------------------------------------------------------------
            Executes commands like: move, customize, etc.
        ---------------------------------------------------------------------------------------*/
        private void Build()
        {
            Console.WriteLine("What building do you wish to build out of these options?");
            DisplayBuildingsInTheCurrentRoom();

            Console.Write("> ");
            string? buildingChoice = Console.ReadLine();
            if (buildingChoice != null)
            {
                currentRoom?.AddBuildingToRoom(buildingChoice);
            }
        }

        private void Demolish()
        {
            Console.WriteLine("What building do you wish to demolish out of these options?");
            DisplayBuildingsInTheCurrentRoom();

            Console.Write("> ");
            string? buildingChoice = Console.ReadLine();
            if (buildingChoice != null)
            {
                currentRoom?.RemoveBuildingFromRoom(buildingChoice);
            }
        }

        private void EnergyHandler()
        {

            //TODO FIX THISS PLEASEEEEEEEEEEEEEEEEEEEEEEEEE
            /*Console.WriteLine("energy");
            
            Energy coalPlant = new Energy();
            coalPlant.DailyPriceCalc(100);
            coalPlant.BuildPrice(3000);
            coalPlant.IncomeCalc(25);

            Energy oilSupply = new Energy();
            oilSupply.DailyPriceCalc(150);
            oilSupply.BuildPrice(4000);
            oilSupply.IncomeCalc(50);

            Energy wind = new Energy();
            wind.DailyPriceCalc(30);
            wind.BuildPrice(2500);
            wind.IncomeCalc(30);

            Energy solar = new Energy();
            solar.DailyPriceCalc(20);
            solar.BuildPrice(2000);
            solar.IncomeCalc(25);

            Energy fission = new Energy();
            fission.DailyPriceCalc(200);
            fission.BuildPrice(5000);
            fission.IncomeCalc(100);

            Energy fusion = new Energy();
            fusion.DailyPriceCalc(100);
            fusion.BuildPrice(10000);
            fusion.IncomeCalc(200);

            if(coalPlant.Income == 0 &&  oilSupply.Income == 0 && wind.Income == 0 && solar.Income == 0 && fission.Income == 0 && fusion.Income == 0)
            {
                Console.WriteLine("You are currently not using any type of energy");
            }
        */
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
            Console.WriteLine("Type 'back' to go return to options.");
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

        private static void DisplayBuildingsInTheCurrentRoom()
        {
            Dictionary<string, int>? buildingsInRoom = currentRoom?.buildings;
            if (buildingsInRoom == null || buildingsInRoom.Count() < 1)
            {
                Console.WriteLine("There are no buildings here.");
            }
            else
            {
                foreach (KeyValuePair<string, int> building in buildingsInRoom)
                {
                    Console.WriteLine("{0}: {1}", building.Key, building.Value);
                }
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


