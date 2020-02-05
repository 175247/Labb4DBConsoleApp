using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Labb4DbConsoleApp
{
    class DeleteQuestionView
    {
        public GameContext modelContext;
        public List<Question> questionsList;
        public Func<List<Question>> GetQuestions;
        public Action<int> PerformDeletion;

        public void UpdateDisplay()
        {
            Console.Clear();
            Console.WriteLine("Which question would you like to delete?\n" +
                "An invalid input will return you to the main menu.");

            questionsList = GetQuestions();

            int count = 1;
            foreach (var question in questionsList)
            {
                Console.WriteLine($"{count}. {question.TheQuestion}");
                count++;
            }
            DeleteData();
        }

        public void DeleteData()
        {
            int choice = 1;
            string userInput = Console.ReadLine();

            try
            {
                Int32.TryParse(userInput, out choice);
                PerformDeletion(choice);
                Console.Clear();
            }
            catch (Exception)
            {
                Console.Clear();
                Console.WriteLine("Invalid input.\n" +
                    "Returning to main menu.\n");
            }
        }
    }
}
