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
            Room? outside = new Room("Outside", "You are standing outside the main entrance of the university. To the east is a large building, to the south is a computing lab, and to the west is the campus pub.");
            Room? eastside = new Room("Eastside", "You are now located in the Eastside of your city.");
            Room? pub = new Room("Pub", "You've entered the campus pub. It's a cozy place, with a few students chatting over drinks. There's a bar near you and some pool tables at the far end.");
            Room? lab = new Room("Lab", "You're in a computing lab. Desks with computers line the walls, and there's an office to the east. The hum of machines fills the room.");
            Room? office = new Room("Office", "You've entered what seems to be an administration office. There's a large desk with a computer on it, and some bookshelves lining one wall.");

            outside.SetExits(null, eastside, lab, pub); // North, East, South, West

            eastside.SetExit("west", outside);

            pub.SetExit("east", outside);

            lab.SetExits(outside, office, null, null);

            office.SetExit("west", lab);

            currentRoom = outside;
        }

        public void Play()
        {
            Parser parser = new Parser();

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

                switch (command.Name)
                {
                    case "look":
                        Console.WriteLine(currentRoom?.LongDescription);
                        break;

                    case "actions":
                        Console.WriteLine("you entered actions");
                        break;

                    case "customize":
                    
                        string[] buildings = new string[0];
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
                            Console.WriteLine("You are currently not using any type of energy."); 
                        }
                        Console.WriteLine($"You have {buildings.Length} buildings.");
                        Console.WriteLine(statHolder1);
                        Console.WriteLine(statHolder2);
                        Console.WriteLine(statHolder3);
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
                        List<string> directions = new List<string> { "east", "south", "north", "west" };

                        while (!directions.Contains(inputLine))
                        {
                            Console.WriteLine("that is not valid direction.");
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

            Console.WriteLine("Thank you for playing World of Zuul!");
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
            Console.WriteLine("Welcome to the World of Zuul!");
            Console.WriteLine("World of Zuul is a new, incredibly boring adventure game.");
            PrintHelp();
            Console.WriteLine();
        }

        private static void PrintHelp()
        {
            Console.WriteLine("You are lost. You are alone. You wander");
            Console.WriteLine("around the university.");
            Console.WriteLine();
            Console.WriteLine("Type 'move' to navigate");
            Console.WriteLine("Type 'customize' to customize your city");
            Console.WriteLine("Type 'actions' to "); //TODO: ADD DESCRIPTION FOR ACTIONS
            Console.WriteLine("Type 'look' for more details.");
            Console.WriteLine("Type 'back' to go to the previous room.");
            Console.WriteLine("Type 'help' to print this message again.");
            Console.WriteLine("Type 'quit' to exit the game.");
        }
    }
}
