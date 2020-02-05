using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Labb4DbConsoleApp
{
    class ViewDeleteQuestion
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
                //var answersToDelete = modelContext.Answers. //vyn ska inte arbeta mot modelle, endast delegates ifrån controllern.
                //    Where(a => a.QuestionId == questionsList[choice - 1].id);
                //
                //foreach (var answer in answersToDelete)
                //{
                //    modelContext.Answers.Remove(answer);
                //}
                //
                //modelContext.Questions.Remove(questionsList[choice - 1]); //detta stycket bör ta in userinput efter tryparse
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
