using System.Collections.Generic;
using System;
using System.Linq;

namespace CastleGrimtol.Project
{
    public class Game : IGame
    {
        public Room CurrentRoom { get; set; }
        public Player CurrentPlayer { get; set; }
        public bool playing = false;
        public Game()
        {
            bool inStartMenu = true;
            Console.Clear();
            System.Console.WriteLine("Welcome to Castle Grimtol!");
            System.Console.WriteLine("Would you like to start a New Game? (Y/N)");
            while (inStartMenu)
            {
                string input = Console.ReadLine().ToLower();
                if (input == "y" || input == "yes")
                {
                    inStartMenu = false;
                    Console.Clear();
                    System.Console.WriteLine("What is thy name brave one?");
                    var name = Console.ReadLine();
                    CurrentPlayer = new Player(name);
                    Setup();
                    Console.Clear();
                    System.Console.WriteLine($@"{CurrentPlayer.Name}, Brave Young Warrior. 
Our forces are failing and the enemy grows stronger everyday. 
I fear if we don't act now our people will be driven from their homes. 
These dark times have left us with one final course of action. 
We must cut the head off of the snake by assasinating the Dark Lord of Grimtol...  
Our sources have identified a small tunnel that leads into the rear of the castle. 
Hurry! (type RUN)");
                    string running = "";
                    do
                    {
                        running = Console.ReadLine().ToLower();
                        if (running == "run")
                        {
                            Console.Clear();
                            System.Console.WriteLine($@"You've made it in without being spotted! 
You must now find a way to kill the Dark Lord without out being discovered. GOOD LUCK!");
                            Console.ReadKey();
                            Console.Clear();
                            System.Console.WriteLine("Type HELP at anytime during the game to get a list of commands");
                            Console.ReadKey();
                        }
                        else
                        {
                            System.Console.WriteLine($"Hurry {CurrentPlayer.Name} you dont have much time.");
                        }
                    } while (running != "run");
                    playing = true;
                    StartGame();
                }
                else if (input == "n" || input == "no")
                {
                    Console.Clear();
                    System.Console.WriteLine("See you soon ol' sport!");
                    inStartMenu = false;
                }
                else if (input != "n" || input != "y")
                {
                    System.Console.WriteLine("Invalid input. Please type yes or no.");
                }
            }
        }

        public void Reset()
        {
            CurrentPlayer.Score = 0;
            CurrentPlayer.Inventory.Clear();
            Setup();
            StartGame();
        }
        public void StartGame()
        {
            while (playing)
            {
                Console.Clear();
                CurrentLocation();
                string[] actions = UserInput();

                switch (actions[0])
                {
                    case "move":
                        Go(actions[1]);
                        break;
                    case "go":
                        Go(actions[1]);
                        break;
                    case "take":
                        Take(actions[1]);
                        actions = null;
                        Console.ReadKey();
                        break;
                    case "pickup":
                        Take(actions[1]);
                        actions = null;
                        Console.ReadKey();
                        break;
                    case "use":
                        UseItem(actions[1]);
                        actions = null;
                        Console.ReadKey();
                        break;
                    case "look":
                        Look();
                        Console.ReadKey();
                        break;
                    case "inventory":
                        Inventory();
                        actions = null;
                        Console.ReadKey();
                        break;
                    case "help":
                        Help();
                        Console.ReadKey();
                        break;
                    case "quit":
                        System.Console.WriteLine("\nThe evil Lord has won this time!");
                        Console.ReadKey();
                        System.Environment.Exit(0);
                        break;
                    case "restart":
                        System.Console.WriteLine("\n Are you sure you want to restart? (Y/N)");
                        string input = Console.ReadLine().ToLower();
                        if (input == "y")
                        {
                            Reset();
                        }
                        else
                        {
                            continue;
                        }
                        break;
                    default:
                        System.Console.WriteLine("What?");
                        break;
                }
            }
        }
        public void Setup()
        {
            // Rooms in the Game
            Room barracks = new Room("Barrack's", "You see a room with several sleeping guards, The room smells of sweaty men. The bed closest to you is empty and there are several uniforms tossed about.");
            Room captainsQuarters = new Room("Captain's Quarters", "As you approach the captains Quarters you swallow hard and notice your lips are dry, Stepping into the room you see a few small tables and maps of the countryside sprawled out.");
            Room castleCourtyard = new Room("Castle Courtyard", "You step into the large castle courtyard there is a flowing fountain in the middle of the grounds and a few guards patrolling the area.");
            // Room councilRoom = new Room("Council Room", "This is the Council Room.");
            Room dungeon = new Room("Dungeon", "As you descend the stairs to the dungeon you notice a harsh chill to the air. Landing a the base of the stairs you see what the remains of a previous prisoner.");
            Room guardRoom = new Room("Guard Room", "Pushing open the door of the guard room you look around and notice the room is empty, There are a few small tools in the corner and a chair propped against the wall near the that likely leads to the dungeon.");
            Room northHall = new Room("North Hallway", "This is the North Hallway.");
            Room southHall = new Room("South Hallway", "This is the North Hallway.");
            Room squireTower = new Room("Squire Tower", "As you finish climbing the stairs to the squire tower you see a messenger nestled in his bed. His messenger overcoat is hanging from his bed post. ");
            Room throneRoom = new Room("Throne Room", "As you unlock the door and swing it wide you see an enormous hall stretching out before you. At the opposite end of the hall sitting on his throne you see the dark lord.");
            Room warRoom = new Room("War Room", "Steping into the war room you see several maps spread across tables. On the maps many of the villages have been marked for purification. You also notice several dishes of prepared food to the side perhaps the war council will be meeting soon.");
            Room westHall = new Room("West Hallway", "You find yourself in a small corridor, there doesnt appear to be anything of interest here.");

            //Exits
            barracks.Exits.Add("SOUTH".ToLower(), westHall);
            captainsQuarters.Exits.Add("NORTH".ToLower(), westHall);
            captainsQuarters.Exits.Add("EAST".ToLower(), southHall);
            castleCourtyard.Exits.Add("SOUTH".ToLower(), southHall);
            castleCourtyard.Exits.Add("WEST".ToLower(), westHall);
            castleCourtyard.Exits.Add("NORTH".ToLower(), northHall);
            dungeon.Exits.Add("SOUTH".ToLower(), guardRoom);
            guardRoom.Exits.Add("NORTH".ToLower(), dungeon);
            guardRoom.Exits.Add("WEST".ToLower(), southHall);
            northHall.Exits.Add("NORTH".ToLower(), throneRoom);
            northHall.Exits.Add("EAST".ToLower(), squireTower);
            northHall.Exits.Add("SOUTH".ToLower(), castleCourtyard);
            southHall.Exits.Add("NORTH".ToLower(), castleCourtyard);
            southHall.Exits.Add("EAST".ToLower(), guardRoom);
            southHall.Exits.Add("WEST".ToLower(), captainsQuarters);
            squireTower.Exits.Add("NORTH".ToLower(),warRoom);
            squireTower.Exits.Add("WEST".ToLower(),northHall);
            throneRoom.Exits.Add("SOUTH".ToLower(), northHall);
            warRoom.Exits.Add("SOUTH".ToLower(), squireTower);
            westHall.Exits.Add("NORTH".ToLower(), barracks);
            westHall.Exits.Add("EAST".ToLower(), castleCourtyard);
            westHall.Exits.Add("SOUTH".ToLower(), captainsQuarters);

            //Items
            Item guardUniform = new Item("GUARD_UNIFORM".ToLower(), "Uniform to desguise yourself.", barracks, true);
            Item hammer = new Item("HAMMER".ToLower(), "Old Rusty hammer.", dungeon, true);
            Item key = new Item("KEY".ToLower(), "An old skull key", castleCourtyard, true);
            Item note = new Item("NOTE".ToLower(), "A note", squireTower, true);
            Item vial = new Item("VIAL".ToLower(), "A vial with green liquid", warRoom, true);
            Item brokenLock = new Item("BROKEN_LOCK".ToLower(), "Old broken lock", captainsQuarters, true);
            Item messengerOvercoat = new Item("MESSENGER_OVERCOAT".ToLower(), "A messengers overcoat", dungeon, true);
            Item Window = new Item("WINDOW", "Window in War Room".ToLower(), warRoom, true);

            barracks.Items.Add(guardUniform);
            captainsQuarters.Items.Add(key);
            captainsQuarters.Items.Add(note);
            captainsQuarters.Items.Add(vial);
            guardRoom.Items.Add(hammer);
            dungeon.Items.Add(brokenLock);
            squireTower.Items.Add(messengerOvercoat);
            warRoom.Items.Add(Window);

            CurrentRoom = westHall;

        }

        public void Go(string direction)
        {

            if (CurrentRoom.Exits.ContainsKey(direction))
            {
                if (CurrentRoom.Exits[direction].Name == "Throne Room")
                {
                    for (int i = 0; i < CurrentPlayer.Inventory.Count; i++)
                    {
                        Item inventoryItem = CurrentPlayer.Inventory[i];
                        if (inventoryItem.Name == "key")
                        {
                            CurrentRoom = CurrentRoom.Exits[direction];
                            Console.Clear();
                            CurrentLocation();
                            Events();
                            return;
                        }
                    }

                    System.Console.WriteLine("\nCrap! This room is locked.");
                    Console.ReadLine();
                    return;

                }

                CurrentRoom = CurrentRoom.Exits[direction];
                Events();
            }
            else
            {
                Console.WriteLine("Can't go that way...");
                Console.ReadKey();
            }
        }
        public void Take(string itemName)
        {
            Console.Clear();
            itemName.ToLower();
            // Find Item
            for (int i = 0; i < CurrentRoom.Items.Count; i++)
            {
                Item item = CurrentRoom.Items[i];
                if (item.Name == itemName)
                {
                    if (item.Available)
                    {
                        CurrentPlayer.Inventory.Add(item);
                        CurrentPlayer.Score += 50;
                        Console.WriteLine($"\n You have taken the {item.Name} and added it to your inventory. ");
                        if (item.Name == "guard_uniform")
                        {
                            Console.WriteLine(" This will keep you from being discovered!");
                        }
                        CurrentRoom.Items.Remove(item);
                    }
                }
                else
                {
                    System.Console.WriteLine("Hmm... that item doesnt appear to be here.");
                }
            }
            Console.ReadKey();
        }
        public void UseItem(string itemName)
        {
            Console.Clear();
            // Check to see if the player has the item
            for (int i = 0; i < CurrentPlayer.Inventory.Count; i++)
            {
                Item item = CurrentPlayer.Inventory[i];
                if (item.Name == itemName)
                {

                    if (item.UseableRoom == CurrentRoom)
                    {
                        System.Console.WriteLine($"\nUsing {item.Name}\n");
                        if (item.Name == "guard_uniform")
                        {
                                CurrentPlayer.Camouflage = true;
                                CurrentPlayer.Score += 100;
                                System.Console.WriteLine("You look like one of them now.");
                                System.Console.WriteLine($"Your score just went up to {CurrentPlayer.Score}");
                                return;
                        }

                    }
                    else
                    {
                        System.Console.WriteLine($"You cant use the {item.Name} in this room.");
                    }
                    return;
                }
            }

            System.Console.WriteLine("You don't have that item or item is not in the room.");
        }
        public void Look()
        {
            Console.Clear();
            Console.WriteLine($@" You are in the {CurrentRoom.Name}.
 {CurrentRoom.Description}.");

            if (CurrentRoom.Items.Count > 0)
            {
                if (CurrentRoom.Items.Count > 2)
                {
                    Console.WriteLine("\n These might come in handy! \n");

                }
                else
                {
                    Console.WriteLine("\n This might come in handy! \n");
                }
                foreach (var item in CurrentRoom.Items)
                {
                    if (item.Available)
                    {
                        Console.WriteLine($" {item.Name} : {item.Description}\n");
                    }
                };
            }
            else
            {
                Console.WriteLine("\n There are no items in this room \n");
            }

            if (CurrentRoom.Exits.Count > 2)
            {
                System.Console.WriteLine(" There appears to be multiple exits that lead to different rooms.\n");

            }
            else
            {
                System.Console.WriteLine(" There appears to be an exit that leads to a different room.\n");
            }
            foreach (var exit in CurrentRoom.Exits)
            {
                System.Console.WriteLine($" {exit.Key.ToUpper()}: {exit.Value.Name}");
            }

        }
        public void Inventory()
        {
            int itemIndex = 1;
            Console.WriteLine();
            if (CurrentPlayer.Inventory.Count > 0)
            {
                System.Console.WriteLine("Your Items:");
                CurrentPlayer.Inventory.ForEach(item =>
                {
                    System.Console.WriteLine($"* {item.Name}: {item.Description}");
                    itemIndex++;
                });
            }
            else
            {
                System.Console.WriteLine("You have no items yet.");
            }
        }
        public string[] UserInput()
        {
            System.Console.WriteLine(" \n What would you like to do?\n");
            string[] UserInput;
            string Input = Console.ReadLine().ToLower();
            return UserInput = Input.Split(' ');
        }
        private void CurrentLocation()
        {
            Console.WriteLine($@" {CurrentPlayer.Name} You have entered the {CurrentRoom.Name}.
 {CurrentRoom.Description}");

        }
        private void Events()
        {
            if (CurrentPlayer.Camouflage)
            {

                switch (CurrentRoom.Name)
                {
                    case "Throne Room":
                        if (CurrentPlayer.Inventory.Any(item => item.Name.Contains("hammer")))
                        {
                            System.Console.WriteLine("\n You quietly sneak up behind him and take the Hammer to his head. Congratulations! You have defeated the Dark Lord");
                            Console.ReadKey();
                            Console.Clear();
                            System.Console.WriteLine("------------------------------ YOU WON ------------------------------");
                            PlayAgain();
                            return;
                        }
                        else
                        {
                            System.Console.WriteLine("\nThe Dark Lord shouts at you demanding why you dared to interrupt him during his Ritual of Evil Summoning... Dumbfounded you mutter an incoherent response. Becoming more enraged the Dark Lord complains that you just ruined his concentration and he will now have to start the ritual over... Quickly striding towards you he smirks at least I know have a sacrificial volunteer. Plunging his jewel encrusted dagger into your heart your world slowly fades away.\n");
                            Console.ReadKey();
                            GameOver();
                            return;
                        }
                }
            }
            else
            {
                if (CurrentRoom.Name == "Barrack's")
                {
                    return;
                }
                Console.Clear();
                System.Console.WriteLine(@"Oh no! You've been spotted!
'Hey! You're not supposed to be here!' The Guard rings the alarm....seconds later you're captured");
                Console.ReadKey();
                GameOver();
            }
        }

        private void GameOver()
        {
            Console.Clear();
            System.Console.WriteLine("------------------------------ GAME OVER ------------------------------");
            PlayAgain();
        }
        private void PlayAgain()
        {
            bool showMenu = true;
            while (showMenu)
            {
                Console.WriteLine("\nDo you want to play again? (Y/N)");
                string input = Console.ReadLine().ToLower();
                if (input == "y" || input == "yes")
                {
                    Reset();
                }
                if (input == "n" || input == "no")
                {
                    Environment.Exit(0);
                }
                else
                {
                    System.Console.WriteLine("what?");
                }
            }
        }
        private void Help()
        {
            Console.Clear();
            Console.WriteLine($@" Here is how to play:

 Type: GO/MOVE North, East, South, or West to navigate.
 Type: TAKE/PICKUP 'Item Name' to take Item.
 Type: USE 'Item Name' to use Item.
 Type: LOOK to look for Items in room and Exits.
 Type: INVENTORY to display your Inventory
 Type: RESTART to Restart the game.
 Type: QUIT to quit the game.
 Type: HELP to bring up this menu.");
        }
    }
}