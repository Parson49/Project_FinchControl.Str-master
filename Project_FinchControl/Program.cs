using System;
using System.Collections.Generic;
using FinchAPI;
using System.IO;

namespace Project_FinchControl
{

    // ***************************************************
    //
    // Title: Finch Control
    // Description: Programming Finch Robot
    // Application Type: Console
    // Author: Anna Parsons
    // Dated Created: 10/2/19
    // Last Modified: 10/6/19
    //
    // ***************************************************

    class Program
    {
        public enum Command
        {
            NONE,
            MOVEFORWARD,
            MOVEBACKWARD,
            STOPMOTORS,
            WAIT,
            TURNRIGHT,
            TURNLEFT,
            LEDON,
            LEDOFF,
            TEMPERATURE,
            LIGHT,
            CHONJI,
            SOUND,
            SOUNDOFF,
            DONE
        }
        static void Main(string[] args)
        {

            DisplayWelcomeScreen();

            DisplayLoginScreen();

            DisplayMenuScreen();

            DisplayClosingScreen();
        }

        #region LOGIN

        static void DisplayLoginScreen()
        {
            string dataPath = @"Login\UsernamePassword.txt";
            string[] logins;
            logins = File.ReadAllLines(dataPath);
            bool quit = false;
            char menuChoice;
            ConsoleKeyInfo menuChoiceKey;

            do
            {
                DisplayScreenHeader("Account Menu");

                //
                // Get menu choice 
                //
                Console.WriteLine("a) Login");
                Console.WriteLine("b) Register");
                Console.WriteLine("c) Quit");
                menuChoiceKey = Console.ReadKey();
                menuChoice = menuChoiceKey.KeyChar;

                switch (menuChoice)
                {
                    case 'a':
                        DisplayLogin(dataPath, logins);
                        quit = true;
                        break;
                    case 'b':
                        DisplayRegister(dataPath, logins);
                        break;
                    case 'c':
                        quit = true;
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Please enter a valid letter for menu choice");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quit);

        }

        /// <summary>
        /// User login
        /// </summary>
        /// <returns></returns>
        static bool DisplayLogin(string dataPath, string[] logins)
        {
            bool validLogin = false;
            int attemptCap = 4;
            string userResponse;
            logins = File.ReadAllLines(dataPath);

            do
            {
               
                for (int attempts = 1; attempts < attemptCap; attempts++)
                {
                    DisplayScreenHeader("User Login");

                    Console.WriteLine("Please enter username and password in the following format -> username,password ");
                    userResponse = Console.ReadLine();
                    foreach (string login in logins)
                    {
                        if (userResponse == login)
                        {
                            validLogin = true;
                        }
                    }

                    if (validLogin == true)
                    {
                        Console.WriteLine("Valid login");
                        attempts = 4;
                        DisplayContinuePrompt();
                    }
                    else if (attempts >= 3)
                    {
                        Console.WriteLine("Invalid login. Login attempts exceeded. Returning to menu.");
                        DisplayContinuePrompt();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid login. Please enter a valid login.");
                        Console.WriteLine($"Attemps: {attempts}/3");
                        DisplayContinuePrompt();
                    }
                }

            } while (!validLogin);

            return validLogin;
        }

        /// <summary>
        /// Allow user to register account
        /// </summary>
        /// <returns></returns>
        static bool DisplayRegister(string dataPath, string[] logins)
        {
            bool validAccount = true;
            int attemptCap = 4;
            string userResponse;
            logins = File.ReadAllLines(dataPath);

            do
            {

                for (int attempts = 1; attempts < attemptCap; attempts++)
                {
                    DisplayScreenHeader("User Registration");

                    Console.WriteLine("Please enter desired username and password in the following format -> username,password ");
                    userResponse = Console.ReadLine();
                    validAccount = true;
                    foreach (string login in logins)
                    {
                        if (userResponse == login)
                        {
                            validAccount = false;
                        }
                    }

                    if (validAccount == true)
                    {
                        Console.WriteLine("Account accepted");
                        List<string> userInput = new List<string>();
                        foreach (string login in logins)
                        {
                            userInput.Add(login);
                        }
                        userInput.Add(userResponse);
                        File.WriteAllLines(dataPath, userInput.ToArray());
                        attempts = 4;
                        DisplayContinuePrompt();
                    }
                    else if (attempts == 3)
                    {
                        Console.WriteLine("Invalid input. Max attempts exceeded. Returning to menu.");
                        DisplayContinuePrompt();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a unique login.");
                        Console.WriteLine($"Attempts: {attempts}/3");
                        DisplayContinuePrompt();
                    }
                }

            } while (!validAccount);

            return validAccount;
        }

        #endregion LOGIN

        #region USER INTERFACE

        static void DisplayMenuScreen()
        {
            //
            // Instantiate a Finch object
            //
            //Finch finchyBoi;  (can be used with following line for same use as Finch finchyBoi = new Finch)
            //finchyBoi = new Finch();
            Finch finchyBoi = new Finch();

            bool finchRobotConnected = false;
            bool quitApp = false;
            char menuChoice;
            ConsoleKeyInfo menuChoiceKey;

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // Get menu choice
                //
                Console.WriteLine("a) Connect Finch Robot");
                Console.WriteLine("b) Talent Show");
                Console.WriteLine("c) Data Recorder");
                Console.WriteLine("d) Alarm System");
                Console.WriteLine("e) User Programming");
                Console.WriteLine("f) Disconnect Finch Robot");
                Console.WriteLine("q) Quit");
                Console.WriteLine("Enter Choice");
                menuChoiceKey = Console.ReadKey();
                menuChoice = menuChoiceKey.KeyChar;

                //
                // Process menu choice
                //
                switch (menuChoice)
                {
                    case 'a':
                        finchRobotConnected = DisplayConnectFinchRobot(finchyBoi);
                        break;
                    case 'b':
                        if (finchRobotConnected)
                        {
                            DisplayTalentShow(finchyBoi);
                        }
                        else
                        {
                            DisplayScreenHeader("");
                            Console.WriteLine("Finch is not connected. Return to main menu and connect");
                            DisplayContinuePrompt();
                        }
                        break;
                    case 'c':
                        if (finchRobotConnected)
                        {
                            DisplayDataRecorder(finchyBoi);
                        }
                        else
                        {
                            DisplayScreenHeader("");
                            Console.WriteLine("Finch is not connected. Return to main menu and connect");
                            DisplayContinuePrompt();
                        }
                        break;
                    case 'd':
                        if (finchRobotConnected)
                        {
                            DisplayAlarmSystem(finchyBoi);
                        }
                        else
                        {
                            DisplayScreenHeader("");
                            Console.WriteLine("Finch is not connected. Return to main menu and connect");
                            DisplayContinuePrompt();
                        }
                        break;
                    case 'e':
                        if (finchRobotConnected)
                        {
                            DisplayUserProgramming(finchyBoi);
                        }
                        else
                        {
                            DisplayScreenHeader("");
                            Console.WriteLine("Finch is not connected. Return to main menu and connect");
                            DisplayContinuePrompt();
                        }
                        break;
                    case 'f':
                        if (finchRobotConnected)
                        {
                            DisplayDisconnectFinchRobot(finchyBoi);
                        }
                        else
                        {
                            DisplayScreenHeader("");
                            Console.WriteLine("Finch is not connected. Return to main menu and connect");
                            DisplayContinuePrompt();
                        }
                        break;
                    case 'q':
                        quitApp = true;
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Please enter a valid letter for menu choice");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApp);
        }
        #endregion USER INTERFACE

        #region USER PROGRAMMING

        static void DisplayFinchCommands(List<Command> commands)
        {
            DisplayScreenHeader("Finch Commands");

            Console.WriteLine("Commands: ");
            Console.WriteLine();

            foreach (Command command in commands)
            {
                Console.WriteLine(command);
            }

            DisplayContinuePrompt();
        }

        static void DisplayGetFinchCommands(List<Command> commands)
        {
            Command command = Command.NONE;
            bool validCommand;

            do
            {
                do
                {
                    DisplayScreenHeader("Finch Robot Commands");
                    Console.Write("Enter Command: ");
                    validCommand = true;

                    if (!Enum.TryParse(Console.ReadLine().ToUpper(), out command))
                    {
                        validCommand = false;
                        Console.WriteLine("Invalid command entered. Please enter a valid command");
                        DisplayContinuePrompt();
                        Console.Clear();
                    }
                    else if (command != Command.DONE)
                    {
                        Console.WriteLine($"Command {command} will be added to command list");
                        DisplayContinuePrompt();
                        Console.Clear();
                    }
                } while (!validCommand);
                commands.Add(command);
            } while (command != Command.DONE);

            DisplayContinuePrompt();
        }

        static (int motorSpeed, int ledBrightness, int waitSeconds, int patternReps, int moveWait, int lightWait, int soundWait, int soundLevel) DisplayGetCommandParameters()
        {
            (int motorSpeed, int ledBrightness, int waitSeconds, int patternReps, int moveWait, int lightWait, int soundWait, int soundLevel) commandParameters;
            bool validInput;

            do
            {
                DisplayScreenHeader("Command Parameters");
                Console.Write("Enter motor speed [0 - 255]: ");
                validInput = true;

                if (!int.TryParse(Console.ReadLine(), out  commandParameters.motorSpeed))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Input. Please input an integer. Ex [0, 10, 255]: ");
                    validInput = false;
                    DisplayContinuePrompt();
                    Console.Clear();
                }
                else if (commandParameters.motorSpeed > 255 || commandParameters.motorSpeed < 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Input. Please input a valid integer between 0 and 255: ");
                    validInput = false;
                    DisplayContinuePrompt();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"Motor speed is {commandParameters.motorSpeed}.");
                    DisplayContinuePrompt();
                    Console.Clear();
                }


            } while (!validInput);

            do
            {
                Console.Write("Enter a wait time for movement commands in miliseconds: ");
                validInput = true;

                if (!int.TryParse(Console.ReadLine(), out commandParameters.moveWait))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Input. Please input an integer. Ex [0, 10, 255]: ");
                    validInput = false;
                    DisplayContinuePrompt();
                    Console.Clear();
                }
                else if (commandParameters.moveWait < 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Input. Please input a valid integer equal to or greater than 0: ");
                    validInput = false;
                    DisplayContinuePrompt();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"Movement wait time is {commandParameters.moveWait} miliseconds.");
                    DisplayContinuePrompt();
                    Console.Clear();
                }


            } while (!validInput);

            do
            {
                Console.Write("Enter LED brightness [0 - 255]: ");
                validInput = true;

                if (!int.TryParse(Console.ReadLine(), out commandParameters.ledBrightness))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Input. Please input an integer. Ex [0, 10, 255]: ");
                    validInput = false;
                    DisplayContinuePrompt();
                    Console.Clear();
                }
                else if (commandParameters.ledBrightness > 255 || commandParameters.ledBrightness < 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Input. Please input a valid integer between 0 and 255: ");
                    validInput = false;
                    DisplayContinuePrompt();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"LED brightness is {commandParameters.ledBrightness}.");
                    DisplayContinuePrompt();
                    Console.Clear();
                }
            } while (!validInput);

            do
            {
                Console.Write("Enter wait time for brightness displays in miliseconds: ");
                validInput = true;

                if (!int.TryParse(Console.ReadLine(), out commandParameters.lightWait))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Input. Please input an integer. Ex [0, 10, 255]: ");
                    validInput = false;
                    DisplayContinuePrompt();
                    Console.Clear();
                }
                else if (commandParameters.lightWait < 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Input. Please input a valid integer equal to or greater than 0: ");
                    validInput = false;
                    DisplayContinuePrompt();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"Brightness display wait time is {commandParameters.lightWait} miliseconds.");
                    DisplayContinuePrompt();
                    Console.Clear();
                }
            } while (!validInput);

            do
            {
                Console.Write("Enter a sound level frequency [100 - 10000]: ");
                validInput = true;

                if (!int.TryParse(Console.ReadLine(), out commandParameters.soundLevel))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Input. Please input an integer. Ex [100, 500, 1000]: ");
                    validInput = false;
                    DisplayContinuePrompt();
                    Console.Clear();
                }
                else if (commandParameters.soundLevel < 100 || commandParameters.soundLevel > 10000)
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Input. Please input a valid integer between 100 and 10000: ");
                    validInput = false;
                    DisplayContinuePrompt();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"Sound level is {commandParameters.soundLevel}.");
                    DisplayContinuePrompt();
                    Console.Clear();
                }
            } while (!validInput);

            do
            {
                Console.Write("Enter a sound wait time in miliseconds: ");
                validInput = true;

                if (!int.TryParse(Console.ReadLine(), out commandParameters.soundWait))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Input. Please input an integer. Ex [100, 500, 1000]: ");
                    validInput = false;
                    DisplayContinuePrompt();
                    Console.Clear();
                }
                else if (commandParameters.soundWait < 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Input. Please input a valid integer equal to or greater than 0: ");
                    validInput = false;
                    DisplayContinuePrompt();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"Sound wait time is {commandParameters.soundWait} miliseconds.");
                    DisplayContinuePrompt();
                    Console.Clear();
                }
            } while (!validInput);

            do
            {
                Console.Write("Enter Wait command in miliseconds: ");
                validInput = true;

                if (!int.TryParse(Console.ReadLine(), out commandParameters.waitSeconds))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Input. Please input an integer. Ex [0, 10, 255]: ");
                    validInput = false;
                    DisplayContinuePrompt();
                    Console.Clear();
                }
                else if (commandParameters.waitSeconds < 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Input. Please input a valid integer equal to or greater than 0 seconds: ");
                    validInput = false;
                    DisplayContinuePrompt();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"Wait is {commandParameters.waitSeconds} miliseconds.");
                    DisplayContinuePrompt();
                    Console.Clear();
                }

            } while (!validInput);

            do
            {
                Console.Write("Enter # of pattern repitions [maximum of 5]: ");
                validInput = true;

                if (!int.TryParse(Console.ReadLine(), out commandParameters.patternReps))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Input. Please input an integer. Ex [0, 1, 5]: ");
                    validInput = false;
                    DisplayContinuePrompt();
                    Console.Clear();
                }
                else if (commandParameters.patternReps < 0 || commandParameters.patternReps > 5)
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Input. Please input a valid integer between 0 and 5: ");
                    validInput = false;
                    DisplayContinuePrompt();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"# of pattern repititions is {commandParameters.patternReps}.");
                    DisplayContinuePrompt();
                    Console.Clear();
                }

            } while (!validInput);

            return commandParameters;
        }

        static void DisplayUserProgramming(Finch finchyBoi)
        {
            (int motorSpeed, int ledBrightness, int waitSeconds, int patternReps, int moveWait, int lightWait, int soundWait, int soundLevel) commandParameter;
            commandParameter.motorSpeed = 0;
            commandParameter.ledBrightness = 0;
            commandParameter.waitSeconds = 0;
            commandParameter.patternReps = 0;
            commandParameter.moveWait = 0;
            commandParameter.lightWait = 0;
            commandParameter.soundWait = 0;
            commandParameter.soundLevel = 0;
            List<Command> commands = new List<Command>();
            bool quitApplication = false;

            char menuChoice;
            ConsoleKeyInfo menuChoiceKey;

            do
            {
                DisplayScreenHeader("User Programming");

                //
                // get user menu choice
                //
                Console.WriteLine("a) Set Command Paramenters");
                Console.WriteLine("b) Add Commands");
                Console.WriteLine("c) View Commands");
                Console.WriteLine("d) Execute Commands");
                Console.WriteLine("e) Write Commands to Data File");
                Console.WriteLine("f) Read Commands from Data File");
                Console.WriteLine("q) Return to Main Menu");
                Console.Write("Enter Choice:");
                menuChoiceKey = Console.ReadKey();
                menuChoice = menuChoiceKey.KeyChar;

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case 'a':
                        commandParameter = DisplayGetCommandParameters();
                        break;

                    case 'b':
                        DisplayGetFinchCommands(commands);
                        break;

                    case 'c':
                        DisplayFinchCommands(commands);
                        break;

                    case 'd':
                        DisplayExecuteFinchCommands(finchyBoi, commands, commandParameter);
                        break;
                    case 'e':
                        DisplayWriteUserProgrammingData(commands);
                        break;
                    case 'f':
                        commands = DisplayReadUserProgrammingData();
                        break;
                    case 'q':
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("Please enter a valid letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }


            } while (!quitApplication);
        }

        static List<Command> DisplayReadUserProgrammingData()
        {
            string dataPath = @"Data\Data.txt";
            List<Command> commands = new List<Command>();
            string[] commandsString;

            DisplayScreenHeader("Read Data from File");

            Console.WriteLine("Prepared to read from the data file.");
            DisplayContinuePrompt();

            commandsString = File.ReadAllLines(dataPath);

            //
            // Create list of command enum
            //
            Command command;
            foreach (string commandString in commandsString)
            {
                Enum.TryParse(commandString, out command);
                commands.Add(command);
            }

            return commands;
        }

        static void DisplayWriteUserProgrammingData(List<Command> commands)
        {
            string dataPath = @"Data\Data.txt";

            List<string> commandsString = new List<string>();

            DisplayScreenHeader("Write Data to File");

            //
            // Create list of command strings
            //
            foreach (Command command in commands)
            {
                commandsString.Add(command.ToString());
            }

            Console.WriteLine("Prepared to write to the data file.");
            DisplayContinuePrompt();

            File.WriteAllLines(dataPath, commandsString.ToArray());

            DisplayContinuePrompt();
        }

        static void DisplayExecuteFinchCommands(
            Finch finchyBoi,
            List<Command> commands,
            (int motorSpeed, int ledBrightness, int waitSeconds, int patternReps, int moveWait, int lightWait, int soundWait, int soundLevel) commandParameter)

        {
            int motorSpeed = commandParameter.motorSpeed;
            int ledBrightness = commandParameter.ledBrightness;
            int waitSeconds = commandParameter.waitSeconds;
            int soundLevel = commandParameter.soundLevel;
            int reps = commandParameter.patternReps;
            int moveWait = commandParameter.moveWait;
            int lightWait = commandParameter.lightWait;
            int soundWait = commandParameter.soundWait;
            double temp = 0;
            double light = 0;
            DisplayScreenHeader("Execute Finch Commands");

            Console.WriteLine("Press any key to continue");

            foreach (Command command in commands)
            {
                switch (command)
                {
                    case Command.NONE:
                        break;
                    case Command.MOVEFORWARD:
                        Console.WriteLine(command);
                        finchyBoi.setMotors(motorSpeed, motorSpeed);
                        finchyBoi.wait(moveWait);
                        break;
                    case Command.MOVEBACKWARD:
                        Console.WriteLine(command);
                        finchyBoi.setMotors(-motorSpeed, -motorSpeed);
                        finchyBoi.wait(moveWait);
                        break;
                    case Command.STOPMOTORS:
                        Console.WriteLine(command);
                        finchyBoi.setMotors(0, 0);
                        break;
                    case Command.WAIT:
                        Console.WriteLine(command);
                        finchyBoi.wait(waitSeconds);
                        break;
                    case Command.TURNRIGHT:
                        Console.WriteLine(command);
                        finchyBoi.setMotors(motorSpeed, 0);
                        finchyBoi.wait(moveWait);
                        break;
                    case Command.TURNLEFT:
                        Console.WriteLine(command);
                        finchyBoi.setMotors(0, motorSpeed);
                        finchyBoi.wait(moveWait);
                        break;
                    case Command.LEDON:
                        Console.WriteLine(command);
                        finchyBoi.setLED(ledBrightness, ledBrightness, ledBrightness);
                        finchyBoi.wait(lightWait);
                        break;
                    case Command.LEDOFF:
                        Console.WriteLine(command);
                        finchyBoi.setLED(0, 0, 0);
                        break;
                    case Command.TEMPERATURE:
                        Console.WriteLine(command);
                        DisplayGetTemp(finchyBoi, temp);
                        break;
                    case Command.LIGHT:
                        Console.WriteLine(command);
                        DisplayGetLight(finchyBoi, light);
                        break;
                    case Command.CHONJI:
                        for (int attempts = 0; attempts <= reps; attempts++)
                        {
                        Console.WriteLine(command);
                            DisplayPerformChonji(finchyBoi);
                        }
                        break;
                    case Command.SOUND:
                        Console.WriteLine(command);
                        finchyBoi.noteOn(soundLevel);
                        finchyBoi.wait(soundWait);
                        break;
                    case Command.SOUNDOFF:
                        Console.WriteLine(command);
                        finchyBoi.noteOff();
                        break;
                    case Command.DONE:
                        break;
                    default:
                        break;
                }
            }

            Console.ReadKey();

            DisplayContinuePrompt();
        }

        static void DisplayPerformChonji(Finch finchyBoi)
        {

            //
            // Pattern Chon-ji pt1
            //
            finchyBoi.setMotors(0, 100);
            finchyBoi.wait(1400);
            finchyBoi.setMotors(100, 100);
            finchyBoi.wait(500);
            finchyBoi.noteOn(600);
            finchyBoi.wait(100);
            finchyBoi.setLED(255, 0, 0);
            finchyBoi.noteOff();

            finchyBoi.setMotors(100, 100);
            finchyBoi.wait(500);
            finchyBoi.noteOn(300);
            finchyBoi.setLED(255, 255, 0);
            finchyBoi.noteOff();

            finchyBoi.setMotors(200, 0);
            finchyBoi.wait(1200);
            finchyBoi.setMotors(100, 100);
            finchyBoi.wait(500);
            finchyBoi.noteOn(300);
            finchyBoi.setLED(255, 255, 255);
            finchyBoi.noteOff();

            finchyBoi.setMotors(100, 100);
            finchyBoi.wait(500);
            finchyBoi.noteOn(300);
            finchyBoi.setLED(255, 255, 0);
            finchyBoi.noteOff();

            finchyBoi.setMotors(0, 100);
            finchyBoi.wait(1400);
            finchyBoi.setMotors(100, 100);
            finchyBoi.wait(500);
            finchyBoi.noteOn(300);
            finchyBoi.setLED(255, 0, 0);
            finchyBoi.noteOff();

            finchyBoi.setMotors(100, 100);
            finchyBoi.wait(500);
            finchyBoi.noteOn(300);
            finchyBoi.setLED(0, 255, 0);
            finchyBoi.noteOff();

            finchyBoi.setMotors(200, 0);
            finchyBoi.wait(1200);
            finchyBoi.setMotors(100, 100);
            finchyBoi.wait(500);
            finchyBoi.noteOn(300);
            finchyBoi.setLED(0, 255, 255);
            finchyBoi.noteOff();

            finchyBoi.setMotors(100, 100);
            finchyBoi.wait(500);
            finchyBoi.noteOn(300);
            finchyBoi.setLED(255, 255, 0);
            finchyBoi.noteOff();

            //
            // Pattern Chon-ji pt2
            //
            finchyBoi.setMotors(0, 100);
            finchyBoi.wait(1400);
            finchyBoi.noteOn(300);
            finchyBoi.setLED(0, 0, 255);
            finchyBoi.noteOff();

            finchyBoi.setMotors(100, 100);
            finchyBoi.wait(500);
            finchyBoi.noteOn(300);
            finchyBoi.setLED(255, 0, 0);
            finchyBoi.noteOff();

            finchyBoi.setMotors(200, 0);
            finchyBoi.wait(1200);
            finchyBoi.noteOn(300);
            finchyBoi.setLED(255, 255, 0);
            finchyBoi.noteOff();

            finchyBoi.setMotors(100, 100);
            finchyBoi.wait(500);
            finchyBoi.noteOn(300);
            finchyBoi.setLED(255, 255, 255);
            finchyBoi.noteOff();

            finchyBoi.setMotors(0, 100);
            finchyBoi.wait(1400);
            finchyBoi.noteOn(300);
            finchyBoi.setLED(0, 255, 255);
            finchyBoi.noteOff();

            finchyBoi.setMotors(100, 100);
            finchyBoi.wait(500);
            finchyBoi.noteOn(300);
            finchyBoi.setLED(0, 255, 0);
            finchyBoi.noteOff();

            finchyBoi.setMotors(200, 0);
            finchyBoi.wait(1200);
            finchyBoi.noteOn(300);
            finchyBoi.setLED(255, 255, 0);
            finchyBoi.noteOff();

            finchyBoi.setMotors(100, 100);
            finchyBoi.wait(500);
            finchyBoi.noteOn(300);
            finchyBoi.setLED(0, 255, 255);
            finchyBoi.noteOff();

            finchyBoi.setMotors(100, 100);
            finchyBoi.wait(500);
            finchyBoi.noteOn(300);
            finchyBoi.setLED(0, 0, 255);
            finchyBoi.noteOff();

            finchyBoi.setMotors(-100, -100);
            finchyBoi.wait(500);
            finchyBoi.noteOn(300);
            finchyBoi.setLED(0, 255, 255);
            finchyBoi.noteOff();

            finchyBoi.setMotors(-100, -100);
            finchyBoi.wait(500);
            finchyBoi.setMotors(0, 0);
            finchyBoi.noteOn(600);
            finchyBoi.wait(100);
            finchyBoi.setLED(255, 255, 255);
            finchyBoi.noteOff();
            finchyBoi.setLED(0, 0, 0);
        }

        static void DisplayGetTemp(Finch finchyBoi, double temp)
        {
            temp = ((finchyBoi.getTemperature() * 9) / 5) + 32;
            Console.WriteLine($"Current temperature is {temp}.");
        }

        static void DisplayGetLight(Finch finchyBoi, double light)
        {
            light = (finchyBoi.getLeftLightSensor() + finchyBoi.getRightLightSensor()) / 2;
            Console.WriteLine($"Current light level is {light}");
        }

        #endregion USER PROGRAMMING

        #region DATA RECORDER
        static void DisplayDataRecorder(Finch finchyBoi)
        {
            double freq;
            int numDataP;
            double CelsiusTemp = 0;
            double lightAverage = 0;
            bool LightTemp = true;

            DisplayScreenHeader("Finch Data Recorder");
            Console.WriteLine();
            Console.WriteLine("User will enter frequeny of recordings and number of recordings");

            freq = DisplayGetDataRecorderFreq(finchyBoi);
            numDataP = DisplayGetNumberDataPoints(finchyBoi);

            LightTemp = DisplayUserLightOrTemp(LightTemp);
            if (!LightTemp)
            {
                //
                // User selected temperature
                // Instantiate temp array
                //
                double[] temperatures = new double[numDataP];
                Console.WriteLine("Program will now record temperature data");
                DisplayContinuePrompt();
                DisplayGetDataSet(finchyBoi, numDataP, freq, temperatures);
                ConvertCToF(CelsiusTemp, numDataP, temperatures);
                DisplayRecorderData(temperatures);
            }
            else 
            {
                //
                // User selected light
                // Instantiate light array 
                //
                double[] lightsL = new double[numDataP];
                double[] lightsR = new double[numDataP];
                double[] lights = new double[numDataP];
                Console.WriteLine("Program will now display light data");
                DisplayContinuePrompt();
                DisplayGetLightDataL(finchyBoi, numDataP, freq, lightsL);
                DisplayGetLightDataR(finchyBoi, numDataP, freq, lightsR);
                AverageLight(lightAverage, numDataP, lightsL, lightsR, lights);
                DisplayLightData(lights);
            }

            DisplayMenuPrompt();
        }

        /// <summary>
        /// Ask user if they want to record temperature or light data
        /// </summary>
        /// <param name="LightTemp"></param>
        /// <returns></returns>
        static bool DisplayUserLightOrTemp(bool LightTemp)
        {
            string userResponse;
            bool validResponse;

            do
            {
                DisplayScreenHeader("Data Type");
            Console.WriteLine();
            Console.WriteLine("Would you like to record temperature data or light data? [temperature , light]");
            userResponse = Console.ReadLine().ToLower();

            validResponse = true;

            switch (userResponse)
            {
                case "light":
                    LightTemp = true;
                    validResponse = true;
                    break;
                case "temperature":
                    LightTemp = false;
                    validResponse = true;
                    break;
                default:
                    Console.WriteLine();
                    Console.WriteLine("Invalid response. Please enter a valid answer [temperature , light]");
                    DisplayContinuePrompt();
                        validResponse = false;
                    break;
            }
        } while (!validResponse);


            return LightTemp;
        }

        /// <summary>
        /// Display average light levels
        /// </summary>
        /// <param name="lights"></param>
        static void DisplayLightData(double[] lights)
        {
            DisplayScreenHeader("Light Levels");

            Console.WriteLine("Program will now display average Light Levels");
            Console.WriteLine();
            Console.WriteLine("Data Set");
            Console.WriteLine();

            for (int index = 0; index < lights.Length; index++)
            {
                Console.WriteLine($"Average light levels {index + 1}: {lights[index]}");
            }

        }

        /// <summary>
        /// Compute average light levels
        /// </summary>
        /// <param name="lightAverage"></param>
        /// <param name="numDataP"></param>
        /// <param name="lightsL"></param>
        /// <param name="lightsR"></param>
        /// <param name="lights"></param>
        /// <returns></returns>
        static double AverageLight(double lightAverage, int numDataP, double[] lightsL, double[] lightsR, double[] lights)
        {

            for (int index = 0; index < numDataP; index++)
            {
                lights[index] = (lightsL[index] + lightsR[index]) / 2;
                lightAverage = lights[index];
            }

            return lightAverage;
        }

        /// <summary>
        /// Record light levels from left sensor
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <param name="numDataP"></param>
        /// <param name="freq"></param>
        /// <param name="lightsL"></param>
        static void DisplayGetLightDataL(Finch finchyBoi,
            int numDataP,
            double freq,
            double[] lightsL)
        {
            DisplayScreenHeader("Get Left Sensor Light Level Readings");

            Console.WriteLine();
            Console.WriteLine("Continue to get light level readings from the left sensor.");
            Console.WriteLine();
            DisplayContinuePrompt();
            Console.WriteLine();

            for (int index = 0; index < numDataP; index++)
            {
                lightsL[index] = finchyBoi.getLeftLightSensor();
                int milliSecs = (int)(freq * 1000);
                finchyBoi.wait(milliSecs);
                Console.WriteLine($"Left sensor light levels {index + 1}: {lightsL[index]}");
            }

            DisplayContinuePrompt();

        }

        /// <summary>
        /// Record light levels from right sensor
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <param name="numDataP"></param>
        /// <param name="freq"></param>
        /// <param name="lightsR"></param>
        static void DisplayGetLightDataR(Finch finchyBoi,
            int numDataP,
            double freq,
            double[] lightsR)
        {
            DisplayScreenHeader("Get Right Sensor Light Level Readings");

            Console.WriteLine();
            Console.WriteLine("Continue to get light level readings from right sensor.");
            Console.WriteLine();
            DisplayContinuePrompt();
            Console.WriteLine();

            for (int index = 0; index < numDataP; index++)
            {
                lightsR[index] = finchyBoi.getRightLightSensor();
                int milliSecs = (int)(freq * 1000);
                finchyBoi.wait(milliSecs);
                Console.WriteLine($"Right sensor light levels {index + 1}: {lightsR[index]}");
            }

            DisplayContinuePrompt();

        }

        /// <summary>
        /// Dispay temperatures in degrees Fahrenheit
        /// </summary>
        /// <param name="temperatures"></param>
        static void DisplayRecorderData(double[] temperatures)
        {
            DisplayScreenHeader("Temperatures");

            Console.WriteLine("Program will now display temperatures in degrees Fahrenheit");
            Console.WriteLine();
            Console.WriteLine("Data Set");
            Console.WriteLine();

            for (int index = 0; index < temperatures.Length; index++)
            {
                Console.WriteLine($"Temperature {index + 1}: {temperatures[index]}°F");
            }

        }

        /// <summary>
        /// Get temperature readings in degrees Celsius
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <param name="numDataP"></param>
        /// <param name="freq"></param>
        /// <param name="temperatures"></param>
        static void DisplayGetDataSet(
            Finch finchyBoi, 
            int numDataP, 
            double freq, 
            double[] temperatures)
        {
            DisplayScreenHeader("Get Temperature Readings");

            Console.WriteLine();
            Console.WriteLine("Continue to get temperature readings in degrees Celsius.");
            Console.WriteLine();
            DisplayContinuePrompt();
            Console.WriteLine();

            for (int index = 0; index < numDataP; index++)
            { 
                temperatures[index] = finchyBoi.getTemperature();
                int milliSecs = (int)(freq * 1000);
                finchyBoi.wait(milliSecs);
                Console.WriteLine($"Temperatures {index + 1}: {temperatures[index]}°C");
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Convert temperature readings from degrees Celsius to degrees Fahrenheit
        /// </summary>
        /// <param name="CelsiusTemp"></param>
        /// <param name="numDataP"></param>
        /// <param name="temperatures"></param>
        /// <returns></returns>
        static double ConvertCToF(double CelsiusTemp,           
            int numDataP,
            double[] temperatures)
        {
            double temp = 0;

            for (int index = 0; index < numDataP; index++)
            {
                temp = temperatures[index];
                temperatures[index] = ((temp * 9) / 5) + 32;
                CelsiusTemp = temperatures[index];
            }

            return CelsiusTemp;
        }

        /// <summary>
        /// Get frequency of data recording
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <returns></returns>
        static double DisplayGetDataRecorderFreq(Finch finchyBoi)
        {
            double freq;
            bool validInput = false;
            bool validNum = true;

            do
            {

                do
                {
                    DisplayScreenHeader("Get Frequency of Recordings");

                    Console.Write("Enter frequency [in seconds]: ");

                    validInput = true;

                    if (!double.TryParse(Console.ReadLine(), out freq))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Please input a number Ex: [0, .5, 2.5, 5]");
                        DisplayContinuePrompt();
                        validInput = false;
                    }
                } while (!validInput);

                if (freq <= 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a valid frequency. (Must be greater than 0)");
                    DisplayContinuePrompt();
                    validNum = false;
                }
                else
                {
                    validNum = true;
                }
            } while (!validNum);
            DisplayContinuePrompt();

            return freq;
        }

        /// <summary>
        /// Get number of data points to be collected
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <returns></returns>
        static int DisplayGetNumberDataPoints(Finch finchyBoi)
        {
            int numDataP;
            bool validInput = false;
            bool validNum = true;
            do
            {

            do
            {
                DisplayScreenHeader("Get Number of Data Points");

                Console.Write("Enter the number of data points: ");

                validInput = true;

                if (!int.TryParse(Console.ReadLine(), out numDataP))
                {
                    Console.WriteLine();
                    Console.WriteLine("Please input a whole number Ex: [0, 1, 3, 5]");
                    DisplayContinuePrompt();
                    validInput = false;

                }

            } while (!validInput);

                if (numDataP <= 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a valid number of data points. (Must be greater than 0)");
                    DisplayContinuePrompt();
                    validNum = false;
                }
                else
                {
                    validNum = true;
                }

            } while (!validNum);


            DisplayContinuePrompt();

            return numDataP;
        }
        #endregion DATA RECORDER

        #region ALARM SYSTEM
        static void DisplayAlarmSystem(Finch finchyBoi)
        {
            string alarmType;
            int maxSeconds;
            double lightUpperThreshold;
            double lightMin;
            string lightThresholdExceeded;
            double tempUpperThreshold;
            double tempMin;
            string tempThresholdExceeded;
            string exceeded;


            DisplayScreenHeader("Alarm System");

            alarmType = DisplayGetAlarmType();
            maxSeconds = DisplayGetMaxSeconds();

            //
            // Get thresholds and monitor based on user input
            //
            switch (alarmType)
            {
                case "light":
                    lightUpperThreshold = DisplayGetLightThreshold(finchyBoi, alarmType);
                    lightMin = DisplayGetLightMin(finchyBoi, alarmType);

                    Console.Clear();
                    Console.WriteLine("Finch will now monitor light levels.");
                    DisplayContinuePrompt();

                    lightThresholdExceeded = MonitorLightLevels(finchyBoi, lightUpperThreshold, lightMin, maxSeconds);

                    //
                    // Display appropriate message based on monitoring results
                    //
                    if (lightThresholdExceeded == "max")
                    {
                        Console.WriteLine();
                        Console.WriteLine("WARNING: Maximum light level exceeded.");
                    }
                    else if (lightThresholdExceeded == "min")
                    {
                        Console.WriteLine();
                        Console.WriteLine("WARNING: Minimum light level exceeded.");
                    }
                    else if (lightThresholdExceeded == "safe")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Light level within safe range.");
                    }
                    else
                    {
                        Console.WriteLine("Monitor Error. Returning to main menu.");
                    }
                    break;
                case "temperature":
                    tempUpperThreshold = DisplayGetTempThreshold(finchyBoi, alarmType);
                    tempMin = DisplayGetTempMin(finchyBoi, alarmType);

                    Console.Clear();
                    Console.WriteLine("Finch will now monitor temperature.");
                    DisplayContinuePrompt();

                    tempThresholdExceeded = MonitorTempLevels(finchyBoi, tempUpperThreshold, tempMin, maxSeconds);

                    //
                    // Display appropriate message based on monitoring results
                    //
                    if (tempThresholdExceeded == "max")
                    {
                        Console.WriteLine();
                        Console.WriteLine("WARNING: Maximum temperature exceeded.");
                    }
                    else if (tempThresholdExceeded == "min")
                    {
                        Console.WriteLine();
                        Console.WriteLine("WARNING: Minimum temperature exceeded.");
                    }
                    else if (tempThresholdExceeded == "safe")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Temperature within safe range.");
                    }
                    else
                    {
                        Console.WriteLine("Monitor Error. Returning to main menu.");
                    }
                    break;
                case "both":
                    lightUpperThreshold = DisplayGetLightThreshold(finchyBoi, alarmType);
                    lightMin = DisplayGetLightMin(finchyBoi, alarmType);
                    tempUpperThreshold = DisplayGetTempThreshold(finchyBoi, alarmType);
                    tempMin = DisplayGetTempMin(finchyBoi, alarmType);

                    Console.Clear();
                    Console.WriteLine("Finch will now monitor light levels and temperature.");
                    DisplayContinuePrompt();

                    exceeded = MonitorTempLight(finchyBoi, tempUpperThreshold, tempMin, lightUpperThreshold, lightMin, maxSeconds);

                    //
                    // Display appropriate message based on monitoring results
                    //
                    if (exceeded == "lightmax")
                    {
                        Console.WriteLine();
                        Console.WriteLine("WARNING: Maximum light level exceeded.");
                    }
                    else if (exceeded == "lightmin")
                    {
                        Console.WriteLine();
                        Console.WriteLine("WARNING: Minimum light level exceeded.");
                    }
                    if (exceeded == "tempmax")
                    {
                        Console.WriteLine();
                        Console.WriteLine("WARNING: Maximum temperature exceeded.");
                    }
                    else if (exceeded == "tempmin")
                    {
                        Console.WriteLine();
                        Console.WriteLine("WARNING: Minimum temperature exceeded.");
                    }
                    else if (exceeded == "safe")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Temperature and light level within safe range.");
                    }
                    break;
                default:
                    Console.WriteLine("Invalid alarm type entered. Returning to menu.");
                    DisplayMenuPrompt();
                    break;
            }

            DisplayMenuPrompt();
        }

        /// <summary>
        /// Monitoring light levels and temperature simulataneously 
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <param name="tempUpperThreshold"></param>
        /// <param name="tempMin"></param>
        /// <param name="lightUpperThreshold"></param>
        /// <param name="lightMin"></param>
        /// <param name="maxSeconds"></param>
        /// <returns></returns>
        static string MonitorTempLight(Finch finchyBoi, double tempUpperThreshold, double tempMin, double lightUpperThreshold, double lightMin, int maxSeconds)
        {
            bool tempUpperThresholdExceeded = false;
            bool lightUpperThresholdExceeded = false;
            bool tempMinExceeded = false;
            bool lightMinExceeded = false;
            double seconds = 0;
            double currentTemp;
            double lightLevel;
            string exceeded = "safe";
            double currentTempF;

            finchyBoi.setLED(0, 255, 0);

            while ((!tempUpperThresholdExceeded && !tempMinExceeded && seconds <= maxSeconds) && (!lightUpperThresholdExceeded && !lightMinExceeded && seconds <maxSeconds))
            {
                currentTemp = finchyBoi.getTemperature();
                currentTempF = ((currentTemp * 9) / 5) + 32;
                lightLevel = (finchyBoi.getLeftLightSensor() + finchyBoi.getRightLightSensor()) / 2;

                DisplayScreenHeader("Monitor Temperature & Light Level");
                Console.WriteLine($"Maximum Temperature: {(int)tempUpperThreshold}");
                Console.WriteLine($"Minimum Temperature: {(int)tempMin}");
                Console.WriteLine($"Current Temperature: {currentTempF}");

                Console.WriteLine($"Maximum Light Level: {(int)lightUpperThreshold}");
                Console.WriteLine($"Minimum Light Level: {(int)lightMin}");
                Console.WriteLine($"Current Light Level: {lightLevel}");

                if (currentTempF >= tempUpperThreshold) tempUpperThresholdExceeded = true;
                else if (currentTempF <= tempMin) tempMinExceeded = true;
                else if (lightLevel >= lightUpperThreshold) lightUpperThresholdExceeded = true;
                else if (lightLevel <= lightMin) lightMinExceeded = true;

                finchyBoi.wait(500);
                seconds += 0.5;
            }

            if (tempUpperThresholdExceeded == true)
            {
                exceeded = "tempmax";
                finchyBoi.setLED(255, 0, 0);
            }
            else if (tempMinExceeded == true)
            {
                exceeded = "tempmin";
                finchyBoi.setLED(255, 0, 0);
            }
            else if (lightUpperThresholdExceeded == true)
            {
                exceeded = "lightmax";
                finchyBoi.setLED(255, 0, 0);
            }
            else if (lightMinExceeded == true)
            {
                exceeded = "lightmin";
                finchyBoi.setLED(255, 0, 0);
            }

            return exceeded;
        }

        /// <summary>
        /// Monitoring temperature upper and lower thresholds
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <param name="tempUpperThreshold"></param>
        /// <param name="tempMin"></param>
        /// <param name="maxSeconds"></param>
        /// <returns></returns>
        static string MonitorTempLevels(Finch finchyBoi, double tempUpperThreshold, double tempMin, int maxSeconds)
        {
            bool tempUpperThresholdExceeded = false;
            bool tempMinExceeded = false;
            double seconds = 0;
            double currentTemp;
            double currentTempF;
            string tempThresholdExceeded = "safe";

            finchyBoi.setLED(0, 255, 0);

            while (!tempUpperThresholdExceeded && !tempMinExceeded && seconds <= maxSeconds)
            {
                currentTemp = finchyBoi.getTemperature();
                currentTempF = ((currentTemp * 9) / 5) + 32;

                DisplayScreenHeader("Monitor Temperature");
                Console.WriteLine($"Maximum Temperature: {(int)tempUpperThreshold}");
                Console.WriteLine($"Maximum Temperature: {(int)tempMin}");
                Console.WriteLine($"Current Temperature: {currentTempF}");

                if (currentTempF >= tempUpperThreshold) tempUpperThresholdExceeded = true;
                else if (currentTempF <= tempMin) tempMinExceeded = true;

                finchyBoi.wait(500);
                seconds += 0.5;
            }

            if (tempUpperThresholdExceeded == true)
            {
                tempThresholdExceeded = "max";
                finchyBoi.setLED(255, 0, 0);
            }
            else if (tempMinExceeded == true)
            {
                tempThresholdExceeded = "min";
                finchyBoi.setLED(255, 0, 0);
            }

            return tempThresholdExceeded;
        }

        /// <summary>
        /// Monitoring light level upper and lower thresholds
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <param name="lightUpperThreshold"></param>
        /// <param name="lightMin"></param>
        /// <param name="maxSeconds"></param>
        /// <returns></returns>
        static string MonitorLightLevels(Finch finchyBoi, double lightUpperThreshold, double lightMin, int maxSeconds)
        {
            bool lightUpperThresholdExceeded = false;
            bool lightMinExceeded = false;
            double seconds = 0;
            int currentLightLevel;
            string lightThresholdExceeded = "safe";

            finchyBoi.setLED(0, 255, 0);

            while (!lightUpperThresholdExceeded && !lightMinExceeded && seconds <= maxSeconds)
            {
                currentLightLevel = (finchyBoi.getLeftLightSensor() + finchyBoi.getRightLightSensor()) / 2;

                DisplayScreenHeader("Monitor Light Levels");
                Console.WriteLine($"Maximum Light Level: {(int)lightUpperThreshold}");
                Console.WriteLine($"Maximum Light Level: {(int)lightMin}");
                Console.WriteLine($"Current Light Level: {currentLightLevel}");

                if (currentLightLevel >= lightUpperThreshold) lightUpperThresholdExceeded = true;
                else if (currentLightLevel <= lightMin) lightMinExceeded = true;

                finchyBoi.wait(500);
                seconds += 0.5;
            }

            if (lightUpperThresholdExceeded == true)
            {
                lightThresholdExceeded = "max";
                finchyBoi.setLED(255, 0, 0);
            }
            else if (lightMinExceeded == true)
            {
                lightThresholdExceeded = "min";
                finchyBoi.setLED(255, 0, 0);
            }

            return lightThresholdExceeded;
        }

        /// <summary>
        /// Getting light level upper threshold from user
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <param name="alarmType"></param>
        /// <returns></returns>
        static double DisplayGetLightThreshold(Finch finchyBoi, string alarmType)
        {
            double lightUpperThreshold = 0;
            double lightLevel = (finchyBoi.getLeftLightSensor() + finchyBoi.getRightLightSensor()) / 2;
            string userResponse;
            bool validInput = false;

            do
            {
                DisplayScreenHeader("Light Level Maximum Value");

                Console.WriteLine($"Current Light Level: {lightLevel}");
                Console.Write("Enter Maximum Light Level: ");
                userResponse = Console.ReadLine();

                if (!double.TryParse(userResponse, out lightUpperThreshold))
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a valid number [-1, 0, 1, 5]: ");
                    DisplayContinuePrompt();
                }
                else if (lightUpperThreshold <= 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a valid maximum light level greater than zero: ");
                    DisplayContinuePrompt();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"Is {lightUpperThreshold} correct? [yes , no]: ");
                    userResponse = Console.ReadLine().ToLower();

                    if (userResponse != "yes")
                    {
                        Console.WriteLine("Please enter the correct maximum light level: ");
                        DisplayContinuePrompt();
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Maximum light level is: {lightUpperThreshold}");
                        DisplayContinuePrompt();
                        validInput = true;
                    }
                }
            } while (!validInput);

            return lightUpperThreshold;
        }

        /// <summary>
        /// Getting light level lower threshold from user
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <param name="alarmType"></param>
        /// <returns></returns>
        static double DisplayGetLightMin(Finch finchyBoi, string alarmType)
        {
            double lightMin = 0;
            double lightLevel = (finchyBoi.getLeftLightSensor() + finchyBoi.getRightLightSensor()) / 2;
            string userResponse;
            bool validInput = false;

            do
            {
                DisplayScreenHeader("Light Level Minimum Value");

                Console.WriteLine($"Current Light Level: {lightLevel}");
                Console.Write("Enter Minimum Light Level: ");
                userResponse = Console.ReadLine();

                if (!double.TryParse(userResponse, out lightMin))
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a valid number [-1, 0, 1, 5]: ");
                    DisplayContinuePrompt();
                }
                else if (lightMin < 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a valid minimum light level greater than or equal to zero: ");
                    DisplayContinuePrompt();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"Is {lightMin} correct? [yes , no]: ");
                    userResponse = Console.ReadLine().ToLower();

                    if (userResponse != "yes")
                    {
                        Console.WriteLine("Please enter the correct minimum light level: ");
                        DisplayContinuePrompt();
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Minimum light level is: {lightMin}");
                        DisplayContinuePrompt();
                        validInput = true;
                    }
                }
            } while (!validInput);

            return lightMin;
        }

        /// <summary>
        /// Getting temperature upper threshold from user
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <param name="alarmType"></param>
        /// <returns></returns>
        static double DisplayGetTempThreshold(Finch finchyBoi, string alarmType)
        {
            double tempUpperThreshold = 0;
            string userResponse;
            bool validInput = false;

            do
            {
                DisplayScreenHeader("Temperature Maximum Value");

                Console.WriteLine($"Current Temperature: {((finchyBoi.getTemperature() * 9) / 5) + 32}");
                Console.Write("Enter Maximum Temperature [in degrees Fahrenheit]: ");
                userResponse = Console.ReadLine();

                if (!double.TryParse(userResponse, out tempUpperThreshold))
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a valid number [-1, 0, 1, 5]: ");
                    DisplayContinuePrompt();
                }
                else if (tempUpperThreshold <= -459.67)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a valid maximum temperature above absolute zero: ");
                    DisplayContinuePrompt();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"Is {tempUpperThreshold} correct? [yes , no]: ");
                    userResponse = Console.ReadLine().ToLower();

                    if (userResponse != "yes")
                    {
                        Console.WriteLine("Please enter the correct maximum temperature: ");
                        DisplayContinuePrompt();
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Maximum temperature is: {tempUpperThreshold}");
                        DisplayContinuePrompt();
                        validInput = true;
                    }
                }
            } while (!validInput);

            return tempUpperThreshold;
        }

        /// <summary>
        /// Getting temperature lower threshold from user
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <param name="alarmType"></param>
        /// <returns></returns>
        static double DisplayGetTempMin(Finch finchyBoi, string alarmType)
        {
            double tempMin = 0;
            string userResponse;
            bool validInput = false;
            do
            {
                DisplayScreenHeader("Temperature Minimum Value");

                Console.WriteLine($"Current Temperature: {((finchyBoi.getTemperature() * 9) / 5) + 32}");
                Console.Write("Enter Minimum Temperature [in degrees Fahrenheit]: ");
                userResponse = Console.ReadLine();

                if (!double.TryParse(userResponse, out tempMin))
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a valid number [-1, 0, 1, 5]: ");
                    DisplayContinuePrompt();
                }
                else if (tempMin < -459.67)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a valid minimum temperature at or above absolute zero: ");
                    DisplayContinuePrompt();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"Is {tempMin} correct? [yes , no]: ");
                    userResponse = Console.ReadLine().ToLower();

                    if (userResponse != "yes")
                    {
                        Console.WriteLine("Please enter the correct minimum temperature: ");
                        DisplayContinuePrompt();
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Minimum temperature is: {tempMin}");
                        validInput = true;
                    }
                }       
            } while (!validInput);


            DisplayContinuePrompt();

            return tempMin;
        }

        /// <summary>
        /// Getting maximum monitoring time from user
        /// </summary>
        /// <returns></returns>
        static int DisplayGetMaxSeconds()
        {
            bool validInput = false;
            int maxSeconds;
            string userResponse;
           
            do
            {
                DisplayScreenHeader("Monitoring Time");

                Console.WriteLine();
                Console.Write("Please enter a maximum monitoring time in seconds [integer]: ");
                userResponse = Console.ReadLine();

                if (!int.TryParse(userResponse, out maxSeconds))
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a valid integer [0, 1, 5]: ");
                    DisplayContinuePrompt();
                }
                else
                {
                    if (maxSeconds < 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Please enter a valid integer zero or greater [1, 5, 9]: ");
                        DisplayContinuePrompt();
                    }
                    else if (maxSeconds >= 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Is {maxSeconds} correct? [yes , no]: ");
                        userResponse = Console.ReadLine().ToLower();

                        if (userResponse != "yes")
                        {
                            Console.WriteLine();
                            Console.WriteLine("Please enter the correct maximum monitoring time");
                            DisplayContinuePrompt();
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Maximum monitoring time is {maxSeconds}.");
                            validInput = true;
                            DisplayContinuePrompt();
                        }
                    }
                }
            } while (!validInput);

            return maxSeconds;
        }

        /// <summary>
        /// Getting type of monitoring alarm from user
        /// </summary>
        /// <returns></returns>
        static string DisplayGetAlarmType()
        {
            bool validInput = false;
            string alarmType;
            string userResponse;

            do
            {
                DisplayScreenHeader("Alarm Type");

                Console.WriteLine();
                Console.Write("Please enter an alarm type to measure [light, temperature, both]: ");
                alarmType = Console.ReadLine();

                if (alarmType == "light" || alarmType == "temperature" || alarmType == "both")
                {
                    Console.WriteLine();
                    Console.WriteLine($"Is {alarmType} correct? [yes , no]: ");
                    userResponse = Console.ReadLine().ToLower();

                    if (userResponse != "yes")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Please enter the correct alarm type.");
                        DisplayContinuePrompt();
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Alarm type: {alarmType}");
                        DisplayContinuePrompt();
                        validInput = true;
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a valid alarm type [light, temperature, both]: ");
                    DisplayContinuePrompt();
                }
            } while (!validInput);
 
            return alarmType;
        }

        #endregion

        #region TALENT SHOW

        /// <summary>
        /// Finch displays talents
        /// </summary>
        /// <param name="finchyBoi"></param>
        static void DisplayTalentShow(Finch finchyBoi)
        {
            int blueLight = 0;
            int redLight = 0;
            int greenLight = 0;
            int audioA = 0;
            int audioB = 0;
            int audioC = 0;

            DisplayScreenHeader("Finch Talent Show");

            Console.WriteLine("Finch will perform talents");
            DisplayContinuePrompt();

            DisplayScreenHeader("Testing Visuals");
            DisplayContinuePrompt();

            redLight = DisplayGetRed(redLight);
            DisplayRedLight(finchyBoi, redLight);
            greenLight = DisplayGetGreen(greenLight);
            DisplayGreenLight(finchyBoi, greenLight);
            blueLight = DisplayGetBlue(blueLight);
            DisplayBlueLight(finchyBoi, blueLight);

            DisplayScreenHeader("Testing Audio");
            DisplayContinuePrompt(); 

            audioA = DisplayGetFreqA(audioA);
            DisplayFreqA(finchyBoi, audioA);
            audioB = DisplayGetFreqB(audioB);
            DisplayFreqB(finchyBoi, audioB);
            audioC = DisplayGetFreqC(audioC);
            DisplayFreqC(finchyBoi, audioC);

            DisplaySingDance(finchyBoi);

            DisplayContinuePrompt();
        }
        /// <summary>
        /// Finch plays 'London Bridge' and dances along
        /// </summary>
        /// <param name="finchyBoi"></param>
        static void DisplaySingDance(Finch finchyBoi)
        {
            DisplayScreenHeader("Finch will now sing 'London Bridge' and dance");

            finchyBoi.setLED(255, 255, 255);
            finchyBoi.setMotors(50, 50);
            finchyBoi.noteOn(392);
            finchyBoi.wait(250);
            finchyBoi.setLED(0, 255, 255);
            finchyBoi.noteOn(440);
            finchyBoi.wait(250);
            finchyBoi.setLED(255, 255, 255);
            finchyBoi.noteOn(392);
            finchyBoi.wait(250);
            finchyBoi.setLED(0, 255, 0);
            finchyBoi.noteOn(349);
            finchyBoi.wait(250);

            finchyBoi.setLED(0, 0, 255);
            finchyBoi.setMotors(65, 0);
            finchyBoi.noteOn(329);
            finchyBoi.wait(250);
            finchyBoi.setLED(0, 255, 0);
            finchyBoi.noteOn(349);
            finchyBoi.wait(250);
            finchyBoi.setLED(255, 255, 255);
            finchyBoi.noteOn(392);
            finchyBoi.wait(500);

            finchyBoi.setLED(255, 0, 255);
            finchyBoi.setMotors(-70, -70);
            finchyBoi.noteOn(293);
            finchyBoi.wait(250);
            finchyBoi.setLED(0, 0, 255);
            finchyBoi.noteOn(329);
            finchyBoi.wait(250);
            finchyBoi.setLED(0, 255, 0);
            finchyBoi.noteOn(349);
            finchyBoi.wait(500);

            finchyBoi.setLED(0, 0, 255);
            finchyBoi.setMotors(0, 65);
            finchyBoi.noteOn(329);
            finchyBoi.wait(250);
            finchyBoi.setLED(0, 255, 0);
            finchyBoi.noteOn(349);
            finchyBoi.wait(250);
            finchyBoi.setLED(255, 255, 255);
            finchyBoi.noteOn(392);
            finchyBoi.wait(500);


            finchyBoi.setLED(255, 255, 255);
            finchyBoi.setMotors(-70, -70);
            finchyBoi.noteOn(392);
            finchyBoi.wait(250);
            finchyBoi.setLED(0, 255, 255);
            finchyBoi.noteOn(440);
            finchyBoi.wait(250);
            finchyBoi.setLED(255, 255, 255);
            finchyBoi.noteOn(392);
            finchyBoi.wait(250);
            finchyBoi.setLED(0, 255, 0);
            finchyBoi.noteOn(349);
            finchyBoi.wait(250);

            finchyBoi.setLED(0, 0, 255);
            finchyBoi.setMotors(-65, 0);
            finchyBoi.noteOn(329);
            finchyBoi.wait(250);
            finchyBoi.setLED(0, 255, 0);
            finchyBoi.noteOn(349);
            finchyBoi.wait(250);
            finchyBoi.setLED(255, 255, 255);
            finchyBoi.noteOn(392);
            finchyBoi.wait(500);

            finchyBoi.setLED(255, 0, 255);
            finchyBoi.setMotors(0, -65);
            finchyBoi.noteOn(293);
            finchyBoi.wait(500);
            finchyBoi.setLED(255, 255, 255);
            finchyBoi.noteOn(392);
            finchyBoi.wait(500);

            finchyBoi.setLED(0, 0, 255);
            finchyBoi.setMotors(60, 60);
            finchyBoi.noteOn(329);
            finchyBoi.wait(250);
            finchyBoi.setLED(255, 0, 0);
            finchyBoi.noteOn(130);
            finchyBoi.wait(250);
            finchyBoi.setLED(255, 0, 0);
            finchyBoi.setMotors(0, 65);
            finchyBoi.noteOn(130);
            finchyBoi.wait(500);
            finchyBoi.noteOff();
            finchyBoi.setLED(0, 0, 0);
            finchyBoi.setMotors(0, 0);

        }

        #region LIGHTS W USER INPUT

        /// <summary>
        /// Get red brightness level from user
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <returns></returns>
        static int DisplayGetRed(int redLight)
        {
            string userResponse;
            bool validLight = false;
            bool validNum;
            do
            {
                do
                {
                    DisplayScreenHeader("Finch Red Light");

                    Console.WriteLine("Choose a brightness level for the Red light [0-255]");
                    userResponse = Console.ReadLine();

                    validNum = true;

                    if (!int.TryParse(userResponse, out redLight))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Please input a number Ex: [0, 100, 255]: ");
                        DisplayContinuePrompt();
                        validNum = false;
                    }
                } while (!validNum);

                if ((redLight > 255) || (redLight < 0))
                {
                    Console.WriteLine("Please enter a valid brightness level from 0 to 255");
                    DisplayContinuePrompt();
                }
                else
                {
                    validLight = true;
                }

            } while (!validLight);

            return redLight;
        }

        /// <summary>
        /// Display red light with user input
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <param name="redLight"></param>
        static void DisplayRedLight(Finch finchyBoi, int redLight)
        {
            DisplayScreenHeader($"Displaying Red Light Level [{redLight}]");
            finchyBoi.setLED(redLight, 0, 0);
            finchyBoi.wait(250);
            finchyBoi.setLED(0, 0, 0);

            DisplayContinuePrompt();

        }

        /// <summary>
        /// Get green brightness level from user
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <returns></returns>
        static int DisplayGetGreen(int greenLight)
        {
            string userResponse;
            bool validLight = false;
            bool validNum;
            do
            {
                do
                {
                    DisplayScreenHeader("Finch Green Light");

                    Console.WriteLine("Choose a brightness level for the Green light [0-255]");
                    userResponse = Console.ReadLine();

                    validNum = true;

                    if (!int.TryParse(userResponse, out greenLight))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Please input a number Ex: [0, 100, 255]: ");
                        DisplayContinuePrompt();
                        validNum = false;
                    }
                } while (!validNum);

                if ((greenLight > 255) || (greenLight < 0))
                {
                    Console.WriteLine("Please enter a valid brightness level from 0 to 255");
                    DisplayContinuePrompt();
                }
                else
                {
                    validLight = true;
                }

            } while (!validLight);

            return greenLight;
        }

        /// <summary>
        /// Display green light with user input
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <param name="greenLight"></param>
        static void DisplayGreenLight(Finch finchyBoi, int greenLight)
        {
            DisplayScreenHeader($"Displaying Green Light Level [{greenLight}]");
            finchyBoi.setLED(0, greenLight, 0);
            finchyBoi.wait(500);
            finchyBoi.setLED(0, 0, 0);

            DisplayContinuePrompt();

        }

        /// <summary>
        /// Get blue brightness level from user 
        /// </summary>
        /// <param name="blueLight"></param>
        /// <returns></returns>
        static int DisplayGetBlue(int blueLight)
        {
            string userResponse;
            bool validLight = false;
            bool validNum;
            do
            {
                do
                {
                    DisplayScreenHeader("Finch Blue Light");

                    Console.WriteLine("Choose a brightness level for the blue light [0-255]");
                    userResponse = Console.ReadLine();

                    validNum = true;

                    if (!int.TryParse(userResponse, out blueLight))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Please input a number Ex: [0, 100, 255]: ");
                        DisplayContinuePrompt();
                        validNum = false;
                    }
                } while (!validNum);

                if ((blueLight > 255) || (blueLight < 0))
                {
                    Console.WriteLine("Please enter a valid brightness level from 0 to 255");
                    DisplayContinuePrompt();
                }
                else
                {
                    validLight = true;
                }

            } while (!validLight);

            return blueLight;
        }

        /// <summary>
        /// Display blue light with user input
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <returns></returns>
        static void DisplayBlueLight(Finch finchyBoi, int blueLight)
        {
            DisplayScreenHeader($"Displaying Blue Light Level [{blueLight}]");
            finchyBoi.setLED(0, 0, blueLight);
            finchyBoi.wait(750);
            finchyBoi.setLED(0, 0, 0);

            DisplayContinuePrompt();

        }

        #endregion LIGHT W USER INPUT

        #region SOUND W USER INPUT

        /// <summary>
        /// Get frequency 1 from user
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <returns></returns>
        static int DisplayGetFreqA(int audioA)
        {
            string userResponse;
            bool validFreq = false;
            bool validNum;
            do
            {
                do
                {
                    DisplayScreenHeader("Finch Audio");

                    Console.WriteLine("Choose a frequency for audio test 1 [100-10000]");
                    userResponse = Console.ReadLine();

                    validNum = true;

                    if (!int.TryParse(userResponse, out audioA))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Please input a number Ex: [100, 1000, 10000]: ");
                        DisplayContinuePrompt();
                        validNum = false;
                    }
                } while (!validNum);

                if ((audioA < 100) || (audioA > 10000))
                {
                    Console.WriteLine("Please enter a valid frequency from 100 to 10000");
                    DisplayContinuePrompt();
                }
                else
                {
                    validFreq = true;
                }

            } while (!validFreq);

            return audioA;
        }

        /// <summary>
        /// Play freq 1 with user input
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <param name="audioA"></param>
        static void DisplayFreqA(Finch finchyBoi, int audioA)
        {
            DisplayScreenHeader($"Playing Frequency 1 [{audioA}]Hz");
            finchyBoi.noteOn(audioA);
            finchyBoi.wait(250);
            finchyBoi.noteOff();

            DisplayContinuePrompt();

        }

        /// <summary>
        /// Get frequency 1 from user
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <returns></returns>
        static int DisplayGetFreqB(int audioB)
        {
            string userResponse;
            bool validFreq = false;
            bool validNum;
            do
            {
                do
                {
                    DisplayScreenHeader("Finch Audio");

                    Console.WriteLine("Choose a frequency for audio test 2 [100-10000]");
                    userResponse = Console.ReadLine();

                    validNum = true;

                    if (!int.TryParse(userResponse, out audioB))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Please input a number Ex: [100, 1000, 10000]: ");
                        DisplayContinuePrompt();
                        validNum = false;
                    }
                } while (!validNum);
                
                if ((audioB < 100) || (audioB > 10000))
                {
                    Console.WriteLine("Please enter a valid frequency from 100 to 10000");
                    DisplayContinuePrompt();
                }
                else
                {
                    validFreq = true;
                }

            } while (!validFreq);

            return audioB;
        }

        /// <summary>
        /// Play freq 2 with user input
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <param name="audioA"></param>
        static void DisplayFreqB(Finch finchyBoi, int audioB)
        {
            DisplayScreenHeader($"Playing Frequency 2 [{audioB}]Hz");
            finchyBoi.noteOn(audioB);
            finchyBoi.wait(500);
            finchyBoi.noteOff();

            DisplayContinuePrompt();

        }

        /// <summary>
        /// Get frequency 3 from user
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <returns></returns>
        static int DisplayGetFreqC(int audioC)
        {
            string userResponse;
            bool validFreq = false;
            bool validNum;
            do
            {
                do
                {
                    DisplayScreenHeader("Finch Audio");

                    Console.WriteLine("Choose a frequency for audio test 3 [100-10000]");
                    userResponse = Console.ReadLine();

                    validNum = true;
                    if (!int.TryParse(userResponse, out audioC))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Please input a number Ex: [100, 1000, 10000]: ");
                        DisplayContinuePrompt();
                        validNum = false;
                    }
                } while (!validNum);

                if ((audioC < 100) || (audioC > 10000))
                {
                    Console.WriteLine("Please enter a valid frequency from 100 to 10000");
                    DisplayContinuePrompt();
                }
                else
                {
                    validFreq = true;
                }

            } while (!validFreq);

            return audioC;
        }

        /// <summary>
        /// Play freq 3 with user input
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <param name="audioC"></param>
        static void DisplayFreqC(Finch finchyBoi, int audioC)
        {
            DisplayScreenHeader($"Playing Frequency 3 [{audioC}]Hz");
            finchyBoi.noteOn(audioC);
            finchyBoi.wait(750);
            finchyBoi.noteOff();

            DisplayContinuePrompt();

        }

        #endregion SOUND W USER INPUT

        #endregion TALENT SHOW

        #region FINCH COMMAND
        static void DisplayDisconnectFinchRobot(Finch finchyBoi)
        {
            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("Disconnecting from Finch Robot");
            DisplayContinuePrompt();

            finchyBoi.disConnect();

            Console.WriteLine("Finch is now disconnected");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Connect to Finch robot
        /// </summary>
        /// <param name="finchyBoi"></param>
        /// <returns></returns>
        static bool DisplayConnectFinchRobot(Finch finchyBoi)
        {
            bool robotConnected;
            const int CAP_ATTEMPTS = 4;
            
            robotConnected = finchyBoi.connect();

            for (int attempts = 1; attempts < CAP_ATTEMPTS; attempts++)
            {
                DisplayScreenHeader("Connect Finch Robot");

                Console.WriteLine("Connecting to the Finch robot. Confirm all USB cords are connected to Finch and Computer.");
                Console.WriteLine();

                DisplayContinuePrompt();

                if (robotConnected == true)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Attempts [{attempts}]");
                    finchyBoi.setLED(0, 255, 0);
                    finchyBoi.noteOn(15000);
                    finchyBoi.wait(1000);
                    finchyBoi.noteOff();
                    Console.WriteLine();
                    Console.WriteLine("Finch robot is now connected");
                    attempts = 3;
                    DisplayContinuePrompt();
                }
                else if (attempts == 3)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Attempts [{attempts}]");
                    Console.WriteLine();
                    Console.WriteLine("Connection to finch failed. Returning to menu. Please ensure Finch and connection USB are functional.");
                    DisplayContinuePrompt();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"Attempts [{attempts}]");
                    Console.WriteLine();
                    Console.WriteLine("Unable to connect to Finch");
                    DisplayContinuePrompt();
                }
            } 

            return robotConnected;
        }

        /// <summary>
        /// display welcome screen
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display closing screen
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayEndPrompt();
        }

        #endregion FINCH COMMAND

        #region HELPER METHODS

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// Display return to menu prompt
        /// </summary>
        static void DisplayMenuPrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to return to menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display exit prompt
        /// </summary>
        static void DisplayEndPrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion
    }
}
