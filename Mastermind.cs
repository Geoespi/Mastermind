using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Mastermind
{
    public class Mastermind
    {
        public static void Run()
        {
            bool inGame = true;
            bool victory = true;
            string difficulty = MMMainMenu();
            int difficultyTurns = 6;
            int difficultyColors = 4;

            Console.CursorVisible = false;

            switch (difficulty)
            {
                case "easy":
                    difficultyColors = 2;
                    difficultyTurns = 8;
                    break;
                case "medium":
                    difficultyColors = 4;
                    difficultyTurns = 6;
                    break;
                case "hard":
                    difficultyColors = 6;
                    difficultyTurns = 4;
                    break;
                default:
                    break;
            }
            string[] solution = MMGenerateColors(difficultyColors);
            MMDrawBoard(difficultyTurns);
            //MMDrawColor(0, solution);
            for (int i = difficultyTurns; i > 0 && inGame; i--)
            {
                string[] answer = MMUserSelectColor(i);
                if (answer.Length != 4)
                {
                    i++;
                    continue;
                }
                string[] clues = MMCheckAnswer(answer, solution);
                MMDrawColor(i, answer);
                MMDrawClue(i, clues);
                if (i == 1)
                {
                    inGame = false;
                    victory = false;
                }
                if (clues.Where(x => x == "black").Count() == 4)
                {
                    inGame = false;
                    victory = true;
                }
            }

            MMDrawColor(0, solution);
            Console.ResetColor();
            Console.SetCursorPosition(0, Console.WindowHeight - 7);
            Console.WriteLine(victory ? "Has ganado!!" : "Has perdido :(");
        }
        public static string[] MMGenerateColors(int maxColors = 4)
        {
            Random rnd = new Random();
            string[] PossibleColors = ["red", "green", "yellow", "blue", "magenta", "cyan"];
            string[] response = new string[4];
            for (int i = 0; i < 4; i++)
            {
                response[i] = PossibleColors[rnd.Next(0, maxColors)];
            }
            return response;
        }
        public static string[] MMCheckAnswer(string[] answer, string[] solution)
        {
            List<string> response = new List<string>();
            bool[] answerChecked = [false, false, false, false];
            bool[] solutionChecked = [false, false, false, false];

            for (int i = 0; i < solution.Length; i++)
            {
                if (answer[i] == solution[i] && !solutionChecked[i] && !answerChecked[i])
                {
                    response.Add("black");
                    answerChecked[i] = true;
                    solutionChecked[i] = true;
                }

                for (int j = 0; j < answer.Length; j++)
                {
                    if (answer[j] == solution[i] && !solutionChecked[i] && !answerChecked[j])
                    {
                        response.Add("white");
                        answerChecked[j] = true;
                        solutionChecked[i] = true;
                    }
                }
            }

            return response.ToArray();
        }
        public static string MMMainMenu()
        {
            bool difficultySelected = false;
            int previousPositionY = 1;
            int positionY = 1;

            Console.Clear();
            Console.WriteLine("Bienvenido a Mastermind");
            Console.WriteLine("Para comenzar selecciona un nivel de dificultad:");
            Console.WriteLine();
            Console.WriteLine("Fácil ---> 2 colores 8 intentos:");
            Console.WriteLine("Medio ---> 4 colores 6 intentos: *");
            Console.WriteLine("Dificil -> 6 colores 4 intentos:");

            do
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo consoleKeyPressed = Console.ReadKey(true);
                    switch (consoleKeyPressed.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (positionY != 0)
                            {
                                previousPositionY = positionY;
                                positionY--;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if (positionY != 2)
                            {
                                previousPositionY = positionY;
                                positionY++;
                            }
                            break;
                        case ConsoleKey.Enter:
                            difficultySelected = true;
                            break;
                        default:
                            break;
                    }
                    MMDrawDifficultySelector(positionY, previousPositionY);
                }
            } while (!difficultySelected);
            Console.Clear();
            switch (positionY)
            {
                case 0:
                    return "easy";
                    break;
                case 1:
                    return "medium";
                    break;
                case 2:
                    return "hard";
                    break;
                default:
                    return "medium";
                    break;
            }
        }

        public static string MMDetermineColorFromInt(int color)
        {
            switch (color)
            {
                case 0:
                    return "red";
                case 1:
                    return "green";
                case 2:
                    return "yellow";
                case 3:
                    return "blue";
                case 4:
                    return "magenta";
                case 5:
                    return "cyan";
                default:
                    return "";
            }
        }

        //funciones de dibujado

        public static void MMDrawBoard(int turns)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("     ┌────┐");
            Console.WriteLine("     │■■■■│");
            Console.WriteLine("┌────┼────┤");
            for (int i = 0; i < turns - 1; i++)
            {
                Console.WriteLine("│    │    │");
                Console.WriteLine("├────┼────┤");
            }
            Console.WriteLine("│    │    │");
            Console.WriteLine("└────┴────┘");
        }
        public static void MMDrawClue(int level, string[] clues)
        {
            Console.SetCursorPosition(1, 2 * level + 1);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            foreach (string clue in clues)
            {
                if (clue == "black")
                    Console.ForegroundColor = ConsoleColor.Black;
                else if (clue == "white")
                    Console.ForegroundColor = ConsoleColor.White;
                else
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("*");
            }
            Console.ResetColor();
        }
        public static void MMDrawColor(int level, string[] colors)
        {
            Console.SetCursorPosition(6, 2 * level + 1);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            foreach (string color in colors)
            {
                switch (color)
                {
                    case "red":
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case "green":
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case "blue":
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                    case "yellow":
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case "magenta":
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        break;
                    case "cyan":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                    default:
                        break;
                }
                Console.Write("■");
            }
            Console.ResetColor();
        }

        public static void MMDrawDifficultySelector(int CurrentY, int PreviousY)
        {
            Console.SetCursorPosition(33, PreviousY + 3);
            Console.Write(" ");
            Console.SetCursorPosition(33, CurrentY + 3);
            Console.Write("*");
        }

        public static void MMDrawColorsToBeSelected()
        {
            Console.ResetColor();
            Console.SetCursorPosition(0, Console.WindowHeight - 3);
            Console.Write("                                                               ");
            Console.SetCursorPosition(0, Console.WindowHeight - 4);
            Console.WriteLine("Selecciona el color a poner:");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("█");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("█");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("█");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("█");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("█");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("█\n");
            Console.ResetColor();
        }

        public static void MMDrawUserColorSelector(int currentValue, int previousValue)
        {
            Console.SetCursorPosition(previousValue, Console.WindowHeight - 2);
            Console.Write(" ");
            Console.SetCursorPosition(currentValue, Console.WindowHeight - 2);
            Console.Write("*");
        }

        //funciones de controles con flechas

        public static string[] MMUserSelectColor(int level)
        {
            MMDrawColorsToBeSelected();
            List<string> colors = new List<string>();
            int currentValue = 0;
            int previousValue = 0;
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.Write("*      ");
            while (colors.Count != 4)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo consoleKeyPressed = Console.ReadKey(true);
                    switch (consoleKeyPressed.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            if (currentValue != 0)
                            {
                                previousValue = currentValue;
                                currentValue--;
                                MMDrawUserColorSelector(currentValue, previousValue);
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            if (currentValue != 5)
                            {
                                previousValue = currentValue;
                                currentValue++;
                                MMDrawUserColorSelector(currentValue, previousValue);
                            }
                            break;
                        case ConsoleKey.Enter:
                            colors.Add(MMDetermineColorFromInt(currentValue));
                            MMDrawColor(level, colors.ToArray());
                            break;
                        default:
                            break;
                    }
                }
            }

            return colors.ToArray();

        }
    }
}