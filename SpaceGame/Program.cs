﻿using System;
using System.Threading;


namespace SpaceGame
{
    class Program
    {


        static void Main(string[] args)
        {
            #region PseudoCode

            {
                //start game
                //show main menu (jason)
                // get username
                // difficulty (jason TODO)
                // initialize planets
                // initialize events
                // initialize ship
                // show game setting/start scenario
                // while (!GameOver(checks fuel, hull, etc))
                //TODO maksim static gameover method()
                //ShowPlanetOptions() "mine, store, sell, buy, getN&R, travel"
                //Get user input
                //if user input == store
                //p1.ShowStore FOR STARTING PLANET
                //if user input == sell
                //sell(name, amount)
                // Event
                //if user input == buy
                //buy(name, amount)
                // Event
                //if user input == mine
                //mine(days), run random event
                //if user input == get name and resources
                //GetNameResources()
                //if user input == travel
                //show all plannets except current
                //Show each planet natural resource and name
                //show DistanceToShip(shipX, shipY) = distance in days to planet
                //get user input
                //if user input == earth
                //if all upgrades TRUE and fuel and hull full then WinGame()
             //change planet, subtract days, run random event, shipX ShipY = PlanetX PlanetY
             }//Collapsed Pseudocode

            #endregion

            Console.SetWindowSize(150, 30);

            //start game
            //show main menu (jason)
            Menu menu = new Menu();
            menu.ShowMainMenu();
            int mMenuOpt = Menu.GetUserInput(3);
            Console.Clear();

            if (mMenuOpt == 1) // NEW GAME, GET USERNAME, get difficulty
            {
                // difficulty (jason TODO)
                Difficulty difficulty = Utility.PromptUserForDifficulty(); //this will be default
                Console.Clear();

                // initialize planets
                Planet p1 = new Planet("Gallifrey", "Gold", difficulty, 15, 100, 100);
                Planet p2 = new Planet("Cadia", "Gold", difficulty, 15, 100, 100);
                Planet p3 = new Planet("Caprica", "Fuel", difficulty, 100, 15, 100);
                Planet p4 = new Planet("Dagobah", "Fuel", difficulty, 100, 15, 100);
                Planet p5 = new Planet("Cybertron", "Hull Material", difficulty, 100, 200, 15);
                Planet[] planetArray = { p1, p2, p3, p4, p5 };

                //TODO run through each planet and remake planets that are too close

                // initialize random object
                Random rng = new Random();

                // initialize events
                FuelEvent fuelEvent = new FuelEvent(difficulty);
                GoldEvent goldEvent = new GoldEvent(difficulty);
                HullEvent hullEvent = new HullEvent(difficulty);
                TimeEvent timeEvent = new TimeEvent(difficulty);
                EasterEggEvent easterEgg = new EasterEggEvent(difficulty);

                // initialize ship
                Ship ship = new Ship(difficulty);
                Planet StartingPlanet = planetArray[rng.Next(5)]; //gets random starting planet
                // show game setting/start scenario
                Utility.ShowSetting(StartingPlanet, ship.Time);
                Console.WriteLine("\nPress any key to contine");
                Console.ReadKey();
                Planet CurrentPlanet = StartingPlanet;
                // while (!GameOver(checks fuel, hull, etc))
                while (!CheckGameOver(ship))
                {
                    Console.Clear();
                    Menu.ShowBanner(CurrentPlanet.PlanetName, ship);
                    Console.WriteLine(CurrentPlanet.ShowPlanetMenu(CurrentPlanet)); //"mine, store, sell, buy, getN&R, travel"

                    int userInput = Menu.GetUserInput(4); //Because there are 4 things to do in the planet menu
                    Console.Clear();
                    if (userInput == (int)PlanetOptions.store)
                    {
                        //ShowStore
                        //TODO buy and sell opt dont show
                        //TODO buy and sell text is backwards in some of the statements
                        Console.WriteLine(CurrentPlanet.ShowPlanetStoreMenu(CurrentPlanet, ship));
                        Console.WriteLine("Select the material you would like to buy or sell: ");
                        //int selection = Menu.GetUserInput(3);
                        //if (selection == 1)

                        userInput = Menu.GetUserInput(2);
                        if (userInput == (int)Store.sell) //input == 1
                        {
                            //get string of material, get amount they want(int)
                            string material = Menu.StoreBuyMenu();
                            int amount = Menu.GetAmount("sell"); //"how much would you like to SELL"
                            Console.WriteLine(CurrentPlanet.Sell(material, amount, ship));//returns string
                            Console.WriteLine(goldEvent.Trigger(ship)); // returns string
                            Console.ReadLine();
                        }
                        else if (userInput == (int)Store.buy) //input == 2
                        {
                            string material = Menu.StoreSellMenu();
                            int amount = Menu.GetAmount("buy"); //"how much would you like to BUY"
                            Console.WriteLine(CurrentPlanet.Buy(material, amount, ship));
                            Console.WriteLine(goldEvent.Trigger(ship));
                            Console.ReadLine();
                        }
                        else //return user to previous menu
                        {
                            Console.WriteLine("Returning to planet menu...");
                            Thread.Sleep(2000);
                        }
                    }
                    else if (userInput == (int)PlanetOptions.mine)
                    {
                        Console.WriteLine("How many days");
                        int NumDaysToMine = Menu.GetUserInput(ship.Time); //do error checking
                        Console.WriteLine(CurrentPlanet.Mine(NumDaysToMine, ship));
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                    }
                    else if (userInput == (int)PlanetOptions.get_name_and_resources)
                    {
                        Console.WriteLine(CurrentPlanet.GetNnameAndResource()); //GetNameResources()
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                    }
                    else if (userInput == (int)PlanetOptions.travel)
                    {
                        Console.WriteLine($"Current Planet: {CurrentPlanet.PlanetName}");
                        Console.WriteLine("Select Planet to travel to");
                        Menu.ShowBanner("", ship);
                        foreach (Planet planet in planetArray)
                        {
                            if (planet != CurrentPlanet)
                            {
                                //Show each planet natural resource and name
                                Console.WriteLine($"[{Array.IndexOf(planetArray, planet)}] Planet: {planet.PlanetName} " +
                                                  $"\tGold: {planet.PlanetGoldCost}" +
                                                  $"\tFuel: {planet.PlanetFuelCost}" +
                                                  $"\tHull: {planet.PlanetHullCost}");
                                //show DistanceToShip(shipX, shipY) = distance in days to planet
                                Console.WriteLine($"Distance from ship in days: {planet.DistanceToShip(planet.PlanetCords.Item1, planet.PlanetCords.Item2, CurrentPlanet)}");
                            }
                        }
                        Console.WriteLine($"[{planetArray.Length}] Planet: Earth");

                        //get user input
                        bool ZeroIndex = true;
                        userInput = Menu.GetUserInput(planetArray.Length, ZeroIndex);
                        if (userInput == planetArray.Length)//user selects earth, the last planet in the options
                        {
                            if (TravelToEarth(ship) == false) //means that not all upgrades are complete
                            {
                                Console.WriteLine(
                                    "Sorry but your ship will not make the journey, select a different planet");
                            }
                            //else//game == win
                        }
                        else
                        {
                            int distanceToPlanet = planetArray[userInput].DistanceToShip(planetArray[userInput].PlanetCords.Item1,
                                    planetArray[userInput].PlanetCords.Item2, CurrentPlanet);

                            Console.WriteLine($"Travelling to planet {planetArray[userInput].PlanetName}....");
                            Menu.LoadingDisplay();
                            Console.WriteLine($"{distanceToPlanet} days later...");
                            //gets number of days travel it takes to get to destination planet
                            //change planet, subtract days, run random event, shipX ShipY = PlanetX PlanetY
                            CurrentPlanet = planetArray[userInput];
                            ship.Time = ship.Time - distanceToPlanet;
                            //TODO change ship coordinates
                            Console.Clear();
                            timeEvent.Trigger(ship);
                            hullEvent.Trigger(ship);
                        }


                    }

                }
            }
            else if (mMenuOpt == 2) // LOAD GAME
            {
                // TODO prompt for username and load game if exists
            }
            else //EXIT
            {
                Console.WriteLine("Thank you for playing!");
                Thread.Sleep(2000);
                Environment.Exit(1);
            }


            static bool CheckGameOver(Ship ship)
            {
                if (ship.Hull <= 0)
                    return true;
                if (ship.Time <= 0)
                    return true;
                else return false;
            }
            static bool TravelToEarth(Ship ship)
            {
                if (ship.HullUpgrade && ship.FuelUpgrade && ship.Fuel == ship.FuelMax && ship.Hull == ship.HullMax)
                    return true;
                else return false;
            }
        }

    }
}
