using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Labb4DbConsoleApp;
using Microsoft.EntityFrameworkCore;

namespace Labb4DbConsoleApp
{
    public class Game
    {
        GameContext gameContext;
        GameClient gameClient;

        public Game(GameContext gameContext)
        {
            Console.WriteLine("Loading...");
            gameContext.Database.EnsureCreated();
            this.gameContext = gameContext;
            this.gameClient = new GameClient(gameContext, this);
        }

        public void Run()
        {
            Console.Clear();
            int number;
            do
            {
                Console.WriteLine("[1] Start game\n" +
                    "[2] Add your own question\n" +
                    "[3] Delete a question\n" +
                    "[4] Quit");

                var input = Console.ReadLine();
                Int32.TryParse(input, out number);

                switch (number)
                {
                    case 1:
                        PlayGame();
                        break;
                    case 2:
                        AddQuestionsAndAnswers();
                        break;
                    case 3:
                        DeleteQuestion();
                        break;
                    case 4:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid input\n");
                        break;
                }
            } while (number != 4);
        }

        private void AddQuestionsAndAnswers()
        {
            gameClient.newQuestion = new Question();
            gameClient.newAnswerList = new List<Answer>();
            gameClient.ResetForNewQuestion();
        
            Console.Clear();
            do
            {
                Console.WriteLine($"[Q]: Enter a question.\n" +
                    $"[A]: Enter 4 possible answers.\n" +
                    $"[R]: Enter the correct answer to the question.\n" +
                    $"[F]: Finish question and upload to cloud.\n" +
                    $"Answers cannot be editted once submitted.\n\n" +
                    $"Press [E] to exit.\n");
        
                string userInput = Console.ReadLine();
                switch (userInput.ToUpper())
                {
                    case "Q":
                        gameClient.AddNewQuestion();
                        break;
                    case "A":
                        gameClient.AddNewAnswer();
                        break;
                    case "R":
                        gameClient.SetCorrectAnswerIfPossible();
                        break;
                    case "F":
                        gameClient.UploadToDatabase();
                        break;
                    case "E":
                        Console.Clear();
                        Console.WriteLine("Returning to menu.");
                        Run();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid input");
                        break;
        
                }
            } while (gameClient.IsfinishedQuestion == false || gameClient.IsCorrectAnswerSet == false);
        }

        private void DeleteQuestion()
        {
            Console.Clear();
            Console.WriteLine("Which question would you like to delete?\n" +
                "An invalid input will return you to the main menu.");

            var questionList = gameClient.GetQuestions();
            int count = 1;
            foreach (var question in questionList)
            {
                Console.WriteLine($"{count}. {question.TheQuestion}");
                count++;
            }

            gameClient.DeleteQuestion(questionList);
        }

        public void PlayGame()
        {
            Console.Clear();
            gameClient.UpdateGameResources_Questions();
            gameClient.UpdateGameResources_Answers();
            gameClient.CorrectAnswer = false;
            var questionList = gameClient.GetQuestions();
            
            foreach (var question in questionList)
            {
                var thisQuestionsAnswerList = question.Answers.ToList();

                do
                {
                    Console.WriteLine($"Q: {question.TheQuestion}\n" +
                        $"[A]: {thisQuestionsAnswerList[0].TheAnswer}\n" +
                        $"[B]: {thisQuestionsAnswerList[1].TheAnswer}\n" +
                        $"[C]: {thisQuestionsAnswerList[2].TheAnswer}\n" +
                        $"[D]: {thisQuestionsAnswerList[3].TheAnswer}\n\n" +
                        $"[S]: Skip question\n");

                    string userInput = Console.ReadLine();
                    if (userInput.ToUpper() == "S")
                    {
                        Console.Clear();
                        Console.WriteLine("Question skipped.");
                        break;
                    }
                    else
                    {
                        gameClient.CheckUserAnswer(thisQuestionsAnswerList, userInput);
                    }
                } while (gameClient.CorrectAnswer == false);
            }
            Console.WriteLine("End of questions. Good job!\n\n" +
                "Loading main menu...");
            Thread.Sleep(3000);
            Console.Clear();
        }
    }
}
