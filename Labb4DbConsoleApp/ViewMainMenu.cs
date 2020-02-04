using System;
using System.Collections.Generic;
using System.Text;

namespace Labb4DbConsoleApp
{
    class ViewMainMenu
    {
        public Action PlayGame, AddNewData, DeleteQuestion;

        public void UpdateDisplay()
        {
            Console.Clear();
            ConsoleKey input;
            do
            {
                Console.WriteLine("[P] Play game.\n" +
                    "[A] Add your own question.\n" +
                    "[D] Delete a question.\n" +
                    "[E] Exit program.");

                input = Console.ReadKey().Key;
                switch (input)
                {
                    case ConsoleKey.P:
                        PlayGame();
                        break;
                    case ConsoleKey.A:
                        AddNewData();
                        break;
                    case ConsoleKey.D:
                        DeleteQuestion();
                        break;
                    case ConsoleKey.E:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid input.\n");
                        break;
                }
            } while (input != ConsoleKey.E);
        }
    }
}
