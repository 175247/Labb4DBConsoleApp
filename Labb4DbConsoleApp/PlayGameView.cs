using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Labb4DbConsoleApp
{
    class PlayGameView
    {
        public List<Question> questionsList;
        public Func<Answer, bool> ValidateAnswer;
        public Func<List<Question>> GetQuestions;
        public Action Navigation;
        public GameContext context;
        public bool CorrectAnswer { get; set; } = false;
        
        public void UpdateDisplay()
        {
            Console.Clear();
            CorrectAnswer = false;
            Console.WriteLine("Updating resources...");
            questionsList = GetQuestions();

            foreach (var question in questionsList)
            {
                var thisQuestionsAnswerList = question.Answers.ToList();

                do
                {
                    Console.Clear();
                    Console.WriteLine($"Q: {question.TheQuestion}\n" +
                        $"[A]: {thisQuestionsAnswerList[0].TheAnswer}\n" +
                        $"[B]: {thisQuestionsAnswerList[1].TheAnswer}\n" +
                        $"[C]: {thisQuestionsAnswerList[2].TheAnswer}\n" +
                        $"[D]: {thisQuestionsAnswerList[3].TheAnswer}\n\n" +
                        $"[S]: Skip question.\n" +
                        $"[E]: Cancel session and return to main menu.");

                    var userInput = Console.ReadKey().Key;

                    if (userInput == ConsoleKey.S)
                    {
                        Console.Clear();
                        Console.WriteLine("Question skipped.\n" +
                            $"The correct answer was \"{question.CorrectAnswer.TheAnswer}\".\n" +
                            $"Press any key to continue.");
                        Console.ReadKey();
                        break;
                    }
                    else
                    {
                        if (userInput != ConsoleKey.A &&
                            userInput != ConsoleKey.B &&
                            userInput != ConsoleKey.C &&
                            userInput != ConsoleKey.D &&
                            userInput != ConsoleKey.S &&
                            userInput != ConsoleKey.E ||
                            userInput == ConsoleKey.Escape)
                        {
                            Console.WriteLine("\nInvalid input. Press any key to continue.");
                            Console.ReadKey();
                            continue;
                        }
                        CheckUserAnswer(thisQuestionsAnswerList, userInput);
                    }
                } while (CorrectAnswer == false);
            }
            Console.WriteLine("\nEnd of questions. Good job!\n\n" +
                "Loading main menu...");
            Thread.Sleep(3000);
            Console.Clear();
            Navigation();
        }

        public void CheckUserAnswer(List<Answer> thisQuestionsAnswerList, ConsoleKey userInput)
        {
            switch (userInput)
            {
                case ConsoleKey.A:
                    CorrectAnswer = ValidateAnswer(thisQuestionsAnswerList[0]);
                    break;
                case ConsoleKey.B:
                    CorrectAnswer = ValidateAnswer(thisQuestionsAnswerList[1]);
                    break;
                case ConsoleKey.C:
                    CorrectAnswer = ValidateAnswer(thisQuestionsAnswerList[2]);
                    break;
                case ConsoleKey.D:
                    CorrectAnswer = ValidateAnswer(thisQuestionsAnswerList[3]);
                    break;
                case ConsoleKey.E:
                    Navigation();
                    break;
                case ConsoleKey.Escape:
                    Console.WriteLine("\nInvalid input. Press any key to continue.\n");
                    Console.ReadKey();
                    break;
                default:
                    Console.WriteLine("\nInvalid input. Press any key to continue.\n");
                    Console.ReadKey();
                    break;
            }
            if (CorrectAnswer == false)
            {
                Console.Clear();
                Console.WriteLine("\nTry again! Not the correct answer!\n");
            }
            else
            {
                Console.WriteLine("\nCorrect! Press any key to proceed.\n");
                Console.ReadKey(true);
                CorrectAnswer = true;
                Console.Clear();
            }
        }
    }
}
