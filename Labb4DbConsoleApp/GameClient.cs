using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Labb4DbConsoleApp
{
    public enum ErrorCode { QuestionIsSet, AnswersAreSet, AnswerOptionsNotSet, CorrectAnswerIsSet, CorrectAnswerNotSet, QuestionNotSet }

    public class GameClient
    {
        //public enum ErrorCode { QuestionIsSet, AnswersAreSet, AnswerOptionsNotSet, CorrectAnswerIsSet, CorrectAnswerNotSet, QuestionNotSet }

        GameContext gameContext;
        Game game;
        public List<Question> questionList;
        public List<Answer> answersList;

        public bool IsfinishedQuestion { get; set; } = false;
        public bool IsCorrectAnswerSet { get; set; } = false;
        public GameClient(GameContext gameContext, Game game)
        {
            this.gameContext = gameContext;
            this.game = game;
            this.questionList = gameContext.questions.ToList();
            this.answersList = gameContext.answers.ToList();
        }

        public bool ValidateAnswer(Answer answer)
        {
            return answer.IsCorrectAnswer;
        }

        public void AddAnswers(Question newQuestion, List<Answer> newAnswerList)
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
                Console.WriteLine($"Option {i} set");
            }
        }

        public void SetCorrectAnswer(Question newQuestion, List<Answer> newAnswerList)
        {
            do
            {
                Console.Clear();
                Console.WriteLine("Which is the correct answer?\n" +
                    $"[Q]: {newQuestion.TheQuestion}\n" +
                    $"[A]: {newAnswerList[0].TheAnswer}\n" +
                    $"[B]: {newAnswerList[1].TheAnswer}\n" +
                    $"[C]: {newAnswerList[2].TheAnswer}\n" +
                    $"[D]: {newAnswerList[3].TheAnswer}\n\n" +
                    "Select either A, B, C or D:");

                string userInput = ValidateInput();
                switch (userInput.ToUpper())
                {
                    case "A":
                        newAnswerList[0].IsCorrectAnswer = true;
                        newQuestion.CorrectAnswer = newAnswerList[0];
                        IsCorrectAnswerSet = true;
                        break;
                    case "B":
                        newAnswerList[1].IsCorrectAnswer = true;
                        newQuestion.CorrectAnswer = newAnswerList[1];
                        IsCorrectAnswerSet = true;
                        break;
                    case "C":
                        newAnswerList[2].IsCorrectAnswer = true;
                        newQuestion.CorrectAnswer = newAnswerList[2];
                        IsCorrectAnswerSet = true;
                        break;
                    case "D":
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
            Console.Clear();
            Console.WriteLine("Correct answer set\n");
        }

        public string ValidateInput()
        {
            bool acceptedString = false;
            string userInput = "Unable To Validate Input";
            while (acceptedString == false)
            {
                userInput = Console.ReadLine();

                if (string.IsNullOrEmpty(userInput) ||
                    string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("Invalid Input, try again.");
                    Console.Write("> ");
                }
                else
                {
                    acceptedString = true;
                    return userInput;
                }
            }
            return userInput;
        }

        public void SaveChangesAndUpdateLists()
        {
            Console.WriteLine("Updating database and game resources...");
            gameContext.SaveChanges();
            UpdateGameResources_Questions();
            UpdateGameResources_Answers();
            Console.WriteLine("Finished!\n");
        }

        public IEnumerable<Question> UpdateGameResources_Questions()
        {
            return questionList = gameContext.questions.ToList();
        }

        public IEnumerable<Answer> UpdateGameResources_Answers()
        {
            return answersList = gameContext.answers.ToList();
        }

        public void DisplayErrorCodes(ErrorCode errorCode)
        {
            Console.Clear();
            switch (errorCode)
            {
                case ErrorCode.QuestionIsSet:
                    Console.WriteLine("The question is already set.\n");
                    break;
                case ErrorCode.QuestionNotSet:
                    Console.WriteLine("The question is not set yet.\n");
                    break;
                case ErrorCode.AnswersAreSet:
                    Console.WriteLine("All answers are already set.\n");
                    break;
                case ErrorCode.AnswerOptionsNotSet:
                    Console.WriteLine("The answer options are not set yet.\n");
                    break;
                case ErrorCode.CorrectAnswerIsSet:
                    Console.WriteLine("The correct answer is already set\n");
                    break;
                case ErrorCode.CorrectAnswerNotSet:
                    Console.WriteLine("The correct answer is not set yet.\n");
                    break;
                default:
                    break;
            }
        }
    }
}
