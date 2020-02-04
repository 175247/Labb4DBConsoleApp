using System;
using System.Collections.Generic;
using System.Text;

namespace Labb4DbConsoleApp
{
    class ViewAddNewData
    {
        public Func<string> ValidateInput;
        public Action<Question> Callback;
        public Question newQuestion;
        public List<Answer> newAnswerList;
        
        public void UpdateDisplay()
        {
            Console.Clear();
            Console.WriteLine("Enter the question:");

            newQuestion.id = Guid.NewGuid().ToString();
            newQuestion.Answers = newAnswerList;
            
            newQuestion.TheQuestion = ValidateInput();
            AddNewAnswer();
            SetCorrectAnswer();
            Callback(newQuestion);
        }

        public void AddNewAnswer()
        {
            for (int i = 1; i <= 4; i++)
            {
                Console.Clear();
                Console.WriteLine($"Question: {newQuestion.TheQuestion}");
                Console.WriteLine($"Enter answer option {i}:");

                newAnswerList.Add(new Answer
                {
                    id = Guid.NewGuid().ToString(),
                    TheAnswer = ValidateInput(),
                    QuestionId = newQuestion.id
                });

                Console.Clear();
                Console.WriteLine($"Option {i} set\n");
            }
        }

        public void SetCorrectAnswer()
        {
            bool IsCorrectAnswerSet = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Which is the correct answer?\n\n" +
                    $"[Q]: {newQuestion.TheQuestion}\n" +
                    $"[A]: {newAnswerList[0].TheAnswer}\n" +
                    $"[B]: {newAnswerList[1].TheAnswer}\n" +
                    $"[C]: {newAnswerList[2].TheAnswer}\n" +
                    $"[D]: {newAnswerList[3].TheAnswer}\n\n" +
                    "Select either A, B, C or D:");

                //string userInput = ValidateInput();
                var userInput = Console.ReadKey().Key;
                switch (userInput)
                {
                    case ConsoleKey.A:
                        newAnswerList[0].IsCorrectAnswer = true;
                        newQuestion.CorrectAnswer = newAnswerList[0];
                        IsCorrectAnswerSet = true;
                        break;
                    case ConsoleKey.B:
                        newAnswerList[1].IsCorrectAnswer = true;
                        newQuestion.CorrectAnswer = newAnswerList[1];
                        IsCorrectAnswerSet = true;
                        break;
                    case ConsoleKey.C:
                        newAnswerList[2].IsCorrectAnswer = true;
                        newQuestion.CorrectAnswer = newAnswerList[2];
                        IsCorrectAnswerSet = true;
                        break;
                    case ConsoleKey.D:
                        newAnswerList[3].IsCorrectAnswer = true;
                        newQuestion.CorrectAnswer = newAnswerList[3];
                        IsCorrectAnswerSet = true;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid answer.");
                        break;
                }
            } while (IsCorrectAnswerSet == false);
        }
    }
}
