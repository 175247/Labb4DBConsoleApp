using System;
using System.Collections.Generic;
using System.Text;

namespace Labb4DbConsoleApp
{
    class AddNewDataView
    {
        public Func<string> ValidateInput;
        public Action<Question> Callback;
        public Question newQuestion;
        public List<Answer> newAnswerList;
        private bool IsCorrectAnswerSet = false;

        public void UpdateDisplay()
        {
            Console.Clear();
            Console.WriteLine("Enter the question:");

            newQuestion.id = Guid.NewGuid().ToString();
            newQuestion.Answers = newAnswerList;

            newQuestion.TheQuestion = ValidateInput();
            AddNewAnswer();
            ChooseCorrectAnswer();
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
                Console.WriteLine($"Option {i} set.\n");
            }
        }

        public void ChooseCorrectAnswer()
        {
            IsCorrectAnswerSet = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Which is the correct answer?\n\n" +
                    $"[Q]: {newQuestion.TheQuestion}\n" +
                    $"[1]: {newAnswerList[0].TheAnswer}\n" +
                    $"[2]: {newAnswerList[1].TheAnswer}\n" +
                    $"[3]: {newAnswerList[2].TheAnswer}\n" +
                    $"[4]: {newAnswerList[3].TheAnswer}\n\n" +
                    "Select either 1, 2, 3 or 4:");

                var key = Console.ReadKey().Key;

                SetCorrectAnswer(key);
            } while (IsCorrectAnswerSet == false);
        }

        private void SetCorrectAnswer(ConsoleKey key)
        {
            if (key != ConsoleKey.D1 &&
                key != ConsoleKey.D2 &&
                key != ConsoleKey.D3 &&
                key != ConsoleKey.D4)
            {
                Console.Clear();
                Console.WriteLine("\nInvalid input.");
                Console.ReadKey();
            }
            else
            {
                Console.ReadKey();
                var userInputString = key.ToString();
                int index = Int32.Parse(userInputString.Substring(userInputString.Length - 1));
                newAnswerList[index - 1].IsCorrectAnswer = true;
                newQuestion.CorrectAnswer = newAnswerList[index - 1];
                IsCorrectAnswerSet = true;
            }
        }
    }
}
