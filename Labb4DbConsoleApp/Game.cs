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

        //bool finishedQuestion = false;
        //bool isCorrectAnswerSet = false;
        bool correctAnswer = false;
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
                        DeleteQuestion(gameClient.questionList);
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
            gameClient.IsfinishedQuestion = false;
            gameClient.IsCorrectAnswerSet = false;
            bool isQuestionSet = false;
            bool isAllAnswersSet = false;
            var newQuestion = new Question();
            var newAnswerList = new List<Answer>();
            newQuestion.id = Guid.NewGuid().ToString();
            newQuestion.Answers = newAnswerList;
        
            Console.Clear();
            do
            {
                Console.WriteLine($"[Q]: Enter a question.\n" +
                    $"[A]: Enter 4 possible answers.\n" +
                    $"[R]: Enter the correct answer to the question.\n" +
                    $"[F]: Finish question and upload to cloud.\n" +
                    $"Answers cannot be editted once submitted.\n" +
                    $"Press [E] to exit.\n");
        
                string userInput = Console.ReadLine();
                switch (userInput.ToUpper())
                {
                    case "Q":
                        if (isQuestionSet == false)
                        {
                            Console.Clear();
                            Console.WriteLine("Enter the question:");
                            newQuestion.TheQuestion = gameClient.ValidateInput();
                            Console.Clear();
                            Console.WriteLine("Question set\n");
                            isQuestionSet = true;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("The question is already set.");
                            gameClient.DisplayErrorCodes(ErrorCode.QuestionIsSet);
                        }
                        break;
                    case "A":
                        if (isAllAnswersSet == false)
                        {
                            gameClient.AddAnswers(newQuestion, newAnswerList);
                            isAllAnswersSet = true;
                        }
                        else
                        {
                            gameClient.DisplayErrorCodes(ErrorCode.AnswersAreSet);
                        }
                        break;
                    case "R":
                        if (newQuestion.Answers.Count == 0)
                        {
                            gameClient.DisplayErrorCodes(ErrorCode.AnswerOptionsNotSet);
                        }
                        else if (gameClient.IsCorrectAnswerSet == true)
                        {
                            gameClient.DisplayErrorCodes(ErrorCode.CorrectAnswerIsSet);
                        }
                        else
                        {
                            gameClient.SetCorrectAnswer(newQuestion, newAnswerList);
                        }
                        break;
                    case "F":
                        if (isAllAnswersSet == false)
                        {
                            gameClient.DisplayErrorCodes(ErrorCode.AnswerOptionsNotSet);
                            break;
                        }
                        else if (newQuestion.CorrectAnswer == null)
                        {
                            gameClient.DisplayErrorCodes(ErrorCode.CorrectAnswerNotSet);
                            break;
                        }
                        else if (newQuestion.TheQuestion == null)
                        {
                            gameClient.DisplayErrorCodes(ErrorCode.QuestionNotSet);
                            break;
                        }
                        else
                        {
                            gameContext.questions.Add(newQuestion);

                            gameClient.SaveChangesAndUpdateLists();
                            gameClient.IsfinishedQuestion = true;
                            Console.Clear();
                            Console.WriteLine("Question added to database.\n");
                        }
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

        private void DeleteQuestion(List<Question> questions)
        {
            Console.Clear();
            Console.WriteLine("Which question would you like to delete?\n" +
                "An invalid input will return you to the main menu.");

            int count = 1;
            foreach (var question in questions)
            {
                Console.WriteLine($"{count}. {question.TheQuestion}");
                count++;
            }

            int choice = 1;
            string userInput = Console.ReadLine();

            try
            {
                Int32.TryParse(userInput, out choice);

                var answersToDelete = gameContext.answers.
                    Where(a => a.QuestionId == questions[choice - 1].id);

                foreach (var answer in answersToDelete)
                {
                    gameContext.answers.Remove(answer);
                }

                gameContext.questions.Remove(questions[choice - 1]);
                Console.Clear();
                gameClient.SaveChangesAndUpdateLists();
            }
            catch (Exception)
            {
                Console.Clear();
                Console.WriteLine("Invalid input!\n" +
                    "Returning to main menu.\n");
            }
        }

        public void PlayGame()
        {
            Console.Clear();
            gameClient.UpdateGameResources_Questions();
            gameClient.UpdateGameResources_Answers();
            correctAnswer = false;
            
            foreach (var question in gameClient.questionList)
            {
                var thisQuestionsAnswerList = question.Answers.ToList();

                do
                {
                    Console.WriteLine($"Q: {question.TheQuestion}\n" +
                        $"[A]: {thisQuestionsAnswerList[0].TheAnswer}\n" +
                        $"[B]: {thisQuestionsAnswerList[1].TheAnswer}\n" +
                        $"[C]: {thisQuestionsAnswerList[2].TheAnswer}\n" +
                        $"[D]: {thisQuestionsAnswerList[3].TheAnswer}");


                    string userInput = Console.ReadLine();
                    switch (userInput.ToUpper())
                    {
                        case "A":
                            correctAnswer = gameClient.ValidateAnswer(thisQuestionsAnswerList[0]);
                            break;
                        case "B":
                            correctAnswer = gameClient.ValidateAnswer(thisQuestionsAnswerList[1]);
                            break;
                        case "C":
                            correctAnswer = gameClient.ValidateAnswer(thisQuestionsAnswerList[2]);
                            break;
                        case "D":
                            correctAnswer = gameClient.ValidateAnswer(thisQuestionsAnswerList[3]);
                            break;
                        case "E":
                            Run();
                            break;
                        default:
                            Console.WriteLine("Invalid input");
                            break;
                    }
                    if (correctAnswer == false)
                    {
                        Console.WriteLine("Try again! Not the correct answer!\n");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Correct!\n");
                        correctAnswer = true;
                    }
                } while (correctAnswer == false);
            }
            Console.WriteLine($"End of questions. Good job!" +
                $"Loading main menu...");
            Thread.Sleep(3000);
            Console.Clear();
        }
    }
}
