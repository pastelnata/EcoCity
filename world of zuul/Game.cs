using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using static world_of_zuul.Pollution;
using static world_of_zuul.VisualTextWriter;
using static System.ConsoleColor;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;

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

        public Game()
        {
            CreateRooms();
        }
        private void CreateRooms()  
        { 
            
            Room? northwestside = new("Northwestside", "*  The NorthWestSide has been occupied with refugee shelters.");
            Room? northside = new("Northside", "*  The Northside is curently an army facility and cannot be interacted with.");
            Room? northeastside = new("Northeastside", "*  The NorthEastSide has been inhabited by wild life.");
            Residence? residence = new("Residence", "*  In the residential area of your city you can build living homes."); 
            Room? centre = new("Centre", "*  You are standing in the centre.");
            Energy? energy = new("Energy", "*  In the energy area of your city you can build facilities to produce power.");
            Room? southwestside = new("Southwestside", "*  The SouthWestSide was struck by an earthquake and therefore cant be renovated yet"); 
            Commercial? commercial = new("commercial", "*  In the commercial area of your city you can build shops.");
            Room? southeastside = new("Southeastside", "*  The SouthEastSide is currently flooded."); 

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
                
                WriteLine(currentRoom?.ShortDescription);
                Write("> ");

                string? input = Console.ReadLine();

                if (dayCounter.currentDay == 6)
                {
                    WriteLine("Game over! You have reached the day limit.");
                    break;
                }

                if (string.IsNullOrEmpty(input))
                {
                    WriteLine("Please enter a command.");
                    continue;
                }

                Command? command = parser.GetCommand(input);

                if (command == null)
                {
                    WriteLine("I don't know that command.");  
                    continue;
                }
                
                switch (command.Name)
                {
                    case "describe":
                    case "4":
                        SetColor(Red);
                        WriteLine(currentRoom?.LongDescription);
                        ColorReset();
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
                    case "Quit":
                        continuePlaying = false;
                        break;

                    case "help":
                    case "5":
                        PrintHelp();
                        break;

                    default:
                        WriteLine("I don't know that command.");
                        break;
                }
            }
            WriteLine();
            EndGameSummary();
            SetColor(Magenta);
            WriteLine("Your stats are: ");
            SetColor(Gray);
            Write("Pollution: ");
            DisplayPollution();
            WriteLine();
            SetColor(Blue);
            WriteLine($"Happiness: {Happyness.happyness}");
            SetColor(Yellow);
            WriteLine($"Money: {currentMoney}");
            SetColor(Cyan);
            Population.displayPopulation();
            SetColor(Green);
            WriteLine("Thank you for playing EcoCity!");
            ColorReset();
        }

        /*-------------------------------------------------------------------------------------
            Handles user commands.
        ---------------------------------------------------------------------------------------*/

        private void ActionHandler()
        {
            List<string> validActions = new List<string> { "skip day", "stats", "back", "1", "2", "B" };
            Write("Type: ");
            SetColor(Blue);
            Write("     < 1 >         < 2 >          < B >  ");
            ColorReset();
            WriteLine();
            for (int i = 0; i <= 2; i++)
            {
                SetColor(Red);
                Write($"   |     {validActions[i]}");

            }

            Write("     |");
            ColorReset();
            WriteLine();
            Write("> ");
            string? selectedAction = Console.ReadLine();

            selectedAction = ValidateInput(validActions, selectedAction);

            if (selectedAction == "skip day" || selectedAction == "1")
            {
                WriteLine("You have skipped a day.");
                dayCounter.UpdateDay();
            }
            else if (selectedAction == "stats" || selectedAction == "2")
            {
                WriteLine($"current money: {currentMoney}");
                Write("Pollution: ");
                DisplayPollution();
                WriteLine();
                WriteLine($"Happiness: {Happyness.happyness}");
                Population.displayPopulation();
            }
        }

        private void MoveHandler()
        {
            List<string> validDirections = new List<string> { "north", "east", "south", "west", "back", "n", "e", "s", "w", "b" };
            while (true)
            {
                SetColor(White);
                Write("Choose: ");
                ColorReset();
                SetColor(Blue);
                Write(" < N >        < E >         < S >        < W >        < B >  ");
                ColorReset();
                WriteLine();
                for (int i = 0; i < 5; i++)
                {
                    SetColor(Red);
                    Write($"   |     {validDirections[i]}");
                    
                }

                Write("   |");
                ColorReset();
                WriteLine();

                string? selectedDirection = Console.ReadLine();

                selectedDirection = ValidateInput(validDirections, selectedDirection.ToLower());

                if (selectedDirection != "b")
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
            List<string> validCustomizeOptions = new List<string> { "infrastructure", "energy", "back", "1", "2", "3", "b" };
            SetColor(Blue);
            WriteLine("Choose:   < 1 >       < 2 >     < B > ");
            ColorReset();
            Write("> ");
            for (int i = 0; i < 3; i++)
            {
                SetColor(Red);
                Write($" | {validCustomizeOptions[i]}");
            }
            Write(" |");
            ColorReset();
            WriteLine();
            string? selectedCustomizeOption = Console.ReadLine();

            selectedCustomizeOption = ValidateInput(validCustomizeOptions, selectedCustomizeOption.ToLower());

            if (selectedCustomizeOption == "1")
            {
                WriteLine("These are the current buildings you have in this room:");
                currentRoom?.DisplayBuildingsInTheCurrentRoom();
                InfrastructureHandler();
            }
            if (selectedCustomizeOption == "2")
            {
                WriteLine("You currently have these buildings in this room:");
                ENERGY.DisplayEnergy();
            }

        }
        private void InfrastructureHandler()
        {
            WriteLine("Here you can");
            List<string> validInfrastructureOptions = new List<string> { "build", "demolish", "back", "1", "2", "b" };
            SetColor(Blue);
            WriteLine("Choose:   < 1 >         < 2 >           < B >");
            ColorReset();
            for (int i = 0; i < 3; i++)
            {
                SetColor(Red);
                Write($"   |     {validInfrastructureOptions[i]}");
            }
            Write("   |   ");
            ColorReset();
            WriteLine();

            Write("> ");
            string? selectedInfrastructureOption = Console.ReadLine();

            selectedInfrastructureOption = ValidateInput(validInfrastructureOptions, selectedInfrastructureOption.ToLower());

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
            WriteLine("What building do you wish to build out of these options?");

            Dictionary<string, int>? buildingsInRoom = currentRoom?.buildings;

            if (buildingsInRoom == null || buildingsInRoom.Values.All(value => value == -1))
            {
                WriteLine("you can't build here yet.");
                return;
            }
            else
            {
                Building.DisplayBuildingsCosts();
                WriteLine("B");

                Write("> ");
                string? userInput = Console.ReadLine();

                if (userInput == "B")
                {
                    InfrastructureHandler();
                    return;
                }

                int buildingChoice;
                while (!int.TryParse(userInput, out buildingChoice) || userInput == null)
                {
                    WriteLine("That is not a valid building.");
                    Write("> ");
                    userInput = Console.ReadLine();
                }

                string validBuilding = ValidUserInputForBuildings(buildingChoice);

                if (Building.CanPlayerCanAffordBuilding(validBuilding))
                {
                    Building.UpdateDailyProfit(validBuilding);
                    Building.DeductPlayersMoney(validBuilding);
                    ENERGY.DetermineBuildingType(validBuilding);
                    currentRoom?.AddBuildingToRoom(validBuilding);
                    WriteLine($"current money: {currentMoney}");
                }
            }
        }

        private void Demolish()
        {
            WriteLine("What building do you wish to demolish out of these options?");
            currentRoom?.DisplayBuildingsInTheCurrentRoom();
            WriteLine("B");

            Write("> ");
            string? userInput = Console.ReadLine();

            int buildingChoice;

            //if input can't be converted to int or is null
            while (!int.TryParse(userInput, out buildingChoice) || userInput == null)
            {
                WriteLine("That is not a valid building.");
                Write("> ");
                userInput = Console.ReadLine();
            }

            if (userInput != null)
            {
                string buildingToDemolish = ValidUserInputForBuildings(buildingChoice);
                currentRoom?.RemoveBuildingFromRoom(buildingToDemolish);
            }
            else if (userInput == "B")
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
                SetColor(Red);
                WriteLine($"You're on the edge of the map!!");
                ColorReset();
            }
        }

        private static void PrintWelcome()
        {
            WriteLine("Welcome to EcoCity!");
            WriteLine("EcoCity is a game where you can build your own sustainable city!");
            PrintHelp();
            WriteLine();
        }

        private static void PrintHelp()
        {
            WriteLine("Remember: the more sustainable your city is, the more expensive it becomes.");
            WriteLine("However, it attracts more people, which also means more money.");
            ColorReset();
            WriteLine();
            SetColor(Red);
            WriteLine("Helpful tips to improve your game: ");
            ColorReset();
            SetColor(Blue);
            WriteLine("*   In each room you can build different buildings.");
            WriteLine("*   Go to a room by typing 1 and choosing a direction");
            WriteLine("*   If you have chosen a room now you can choose to customize by typing < B > and then < 2 > ");
            ColorReset();
            WriteLine();
            SetColor(Magenta);
            WriteLine("*   If you want to choose a different number from the options");
            WriteLine("    type < B > and then choose another number");
            ColorReset();
            WriteLine();
            Write("Type ");
            SetColor(Red);
            Write("< 1 > ");
            ColorReset();
            WriteLine("to move around");
            Write("Type ");
            SetColor(Red);
            Write("< 2 > ");
            ColorReset();
            WriteLine("to customize your city");
            Write("Type ");
            SetColor(Red);
            Write("< 3 > ");
            ColorReset();
            WriteLine("to skip a day or check your progress");
            Write("Type ");
            SetColor(Red);
            Write("< 4 > ");
            ColorReset();
            WriteLine("to see description of a place you are currently located in");
            Write("Type ");
            SetColor(Red);
            Write("< 5 > ");
            ColorReset();
            WriteLine("to print this message again");
            Write("Type ");
            SetColor(Red);
            Write("< B > ");
            ColorReset();
            WriteLine("to return to options");
            Write("Type ");
            SetColor(Red);
            Write("< Quit > ");
            ColorReset();
            WriteLine("to exit the game");
        }

        private static void EndGameSummary()
        {
            if (Happyness.happyness <= 1000)
            {
                SetColor(Red);
                WriteLine("Environmental Crisis");
                WriteLine();
                WriteLine("In this sad outcome, the city is in trouble because we didn't take care of the environment.");
                WriteLine("The air and water are dirty, and plants and animals are disappearing.");
                WriteLine("This hurts people's health and happiness.");
                WriteLine("This ending shows how important it is to think about the environment when building a city.");
                WriteLine();
                ColorReset();
            }
            else if (Happyness.happyness <= 2000)
            {
                SetColor(DarkMagenta);
                WriteLine("Struggling to be Green");
                WriteLine();
                WriteLine("Even though we tried, the city is still having a hard time being good to the environment.");
                WriteLine("We don't have enough things to help us be green, like clean energy.");
                WriteLine("People in the city are dealing with problems like dirty air and water.");
                WriteLine("We learn that we need to do more to make the city better for the environment.");
                WriteLine();
                ColorReset();
            }
            else if (Happyness.happyness <= 3000)
            {
                SetColor(Yellow);
                WriteLine("Finding the Middle");
                WriteLine();
                WriteLine("In this okay ending, the city is doing better at being green, but we still have some problems.");
                WriteLine("We are getting better at things like handling our trash and using clean energy. ");
                WriteLine("However, there are still issues like too much traffic.");
                WriteLine("This shows that we are learning and trying to make the city a good place for everyone.");
                WriteLine();
                ColorReset();
            }
            else if (Happyness.happyness <= 4000)
            {
                SetColor(Blue);
                WriteLine("Happy and Green City");
                WriteLine();
                WriteLine("Because of good choices, the city is now a happy place with clean air and parks. ");
                WriteLine("We use things like wind and sun to make energy, and people have a good life.");
                WriteLine("The city becomes a role model for other places, showing that being good to the environment can make everyone's life better.");
                WriteLine();
                ColorReset();
            }
            else
            {
                SetColor(Green);
                WriteLine("Super Eco-Friendly City");
                WriteLine();
                WriteLine("In this best ending, the city is amazing because we take care of the environment in every way.");
                WriteLine("We use clean energy, have lots of parks, and teach everyone how to be eco-friendly.");
                WriteLine("People are happy, and the city is a great example for others. ");
                WriteLine("This ending teaches us that learning about and caring for the environment makes the world a better place for everyone.");
                WriteLine();
                ColorReset();
            }

        }

        private static void PrintMap(Room? currentRoom, Room[] rooms)
        {


            for (int i = 0; i < rooms.Length; i++)
            {

                if (rooms[i] == currentRoom)
                {
                    Write("o ");
                }
                else
                {
                    Write("- ");
                }
                if ((i + 1) % 3 == 0)
                {
                    WriteLine();
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
                WriteLine("That is not valid option.");
                Write("> ");
                selectedInput = Console.ReadLine();
            }

            return selectedInput;
        }

        private string ValidUserInputForBuildings(int userInput)
        {
            Dictionary<string, int>? buildingsInRoom = currentRoom?.buildings;
            Dictionary<string, int>? buildingValidInputs = Building.buildingValidInputs;
            
            var commonBuildingNames = buildingsInRoom?.Keys.Intersect(buildingValidInputs.Keys);
            if (commonBuildingNames != null)
            {
                foreach (var buildingName in commonBuildingNames)
                {
                    
                    if (userInput == buildingValidInputs[buildingName])
                    {
                        return buildingName;
                    }
                }
                WriteLine("That is not a valid building.");
                Write("> ");
                Convert.ToInt32(Console.ReadLine());
            }
            return string.Empty;
        }
    }
}


