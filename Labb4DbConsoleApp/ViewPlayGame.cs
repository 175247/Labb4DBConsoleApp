using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Labb4DbConsoleApp
{
    class ViewPlayGame
    {
        public List<Question> questionsList;
        public Func<Answer, bool> ValidateAnswer;
        public Func<List<Question>> GetQuestions;
        public Action Navigation;
        public bool CorrectAnswer { get; set; } = false;
        
        public void UpdateDisplay()
        {
            Console.Clear();
            CorrectAnswer = false;
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
                        $"[S]: Skip question\n" +
                        $"[E]: Abort and return to main menu.");

                    var userInput = Console.ReadKey().Key;

                    if (userInput == ConsoleKey.S)
                    {
                        Console.Clear();
                        Console.WriteLine("Question skipped.\n" +
                            $"Correct answer was \"{question.CorrectAnswer.TheAnswer}\"\n");
                        Thread.Sleep(3000);
                        break;
                    }
                    else
                    {
                        CheckUserAnswer(thisQuestionsAnswerList, userInput);
                    }
                } while (CorrectAnswer == false);
            }
            Console.WriteLine("End of questions. Good job!\n\n" +
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
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
            if (CorrectAnswer == false)
            {
                Console.Clear();
                Console.WriteLine("Try again! Not the correct answer!\n");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Correct!\n");
                CorrectAnswer = true;
            }
        }
    }
}
