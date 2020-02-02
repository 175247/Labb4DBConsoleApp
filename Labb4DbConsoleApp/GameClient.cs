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
        private List<Question> questionList;
        private List<Answer> answersList;
        public Question newQuestion;
        public List<Answer> newAnswerList;

        public bool IsfinishedQuestion { get; set; } = false;
        public bool IsCorrectAnswerSet { get; set; } = false;
        public bool IsQuestionSet { get; set; } = false;
        public bool IsAllAnswersSet { get; set; } = false;
        public bool CorrectAnswer { get; set; } = false;

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

        internal void ResetForNewQuestion()
        {
            IsfinishedQuestion = false;
            IsCorrectAnswerSet = false;
            IsQuestionSet = false;
            IsAllAnswersSet = false;
            newQuestion.id = Guid.NewGuid().ToString();
            newQuestion.Answers = newAnswerList;
        }

        public void SetCorrectAnswer()
        {
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

        public List<Question> UpdateGameResources_Questions()
        {
            return questionList = gameContext.questions.ToList();
        }

        public List<Answer> UpdateGameResources_Answers()
        {
            return answersList = gameContext.answers.ToList();
        }

        public List<Question> GetQuestions()
        {
            return questionList;
        }

        public List<Answer> GetAnswers()
        {
            return answersList;
        }


        public void CheckIfCanAddQuestion()
        {
            if (IsQuestionSet == false)
            {
                Console.Clear();
                Console.WriteLine("Enter the question:");
                newQuestion.TheQuestion = ValidateInput();
                Console.Clear();
                Console.WriteLine("Question set\n");
                IsQuestionSet = true;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("The question is already set.\n");
                DisplayErrorCodes(ErrorCode.QuestionIsSet);
            }
        }

        public void CheckIfCanAddAnswer()
        {
            if (IsQuestionSet == false)
            {
                DisplayErrorCodes(ErrorCode.QuestionNotSet);
            }
            else if (IsAllAnswersSet == false)
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
                IsAllAnswersSet = true;
            }
            else
            {
                DisplayErrorCodes(ErrorCode.AnswersAreSet);
            }
        }

        public void CheckIfCanSetCorrectAnswer()
        {
            if (newQuestion.Answers.Count == 0)
            {
                DisplayErrorCodes(ErrorCode.AnswerOptionsNotSet);
            }
            else if (IsCorrectAnswerSet == true)
            {
                DisplayErrorCodes(ErrorCode.CorrectAnswerIsSet);
            }
            else
            {
                SetCorrectAnswer();
            }
        }

        public void CheckIfCanUploadToDatabase()
        {
            if (IsAllAnswersSet == false)
            {
                DisplayErrorCodes(ErrorCode.AnswerOptionsNotSet);
                return;
            }
            else if (newQuestion.CorrectAnswer == null)
            {
                DisplayErrorCodes(ErrorCode.CorrectAnswerNotSet);
                return;
            }
            else if (newQuestion.TheQuestion == null)
            {
                DisplayErrorCodes(ErrorCode.QuestionNotSet);
                return;
            }
            else
            {
                gameContext.questions.Add(newQuestion);

                SaveChangesAndUpdateLists();
                IsfinishedQuestion = true;
                Console.Clear();
                Console.WriteLine("Question added to database.\n");
            }
        }

        public void DeleteQuestion(List<Question> questions)
        {
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
                SaveChangesAndUpdateLists();
            }
            catch (Exception)
            {
                Console.Clear();
                Console.WriteLine("Invalid input\n" +
                    "Returning to main menu.\n");
            }
        }

        public void CheckUserAnswer(List<Answer> thisQuestionsAnswerList, string userInput)
        {
            switch (userInput.ToUpper())
            {
                case "A":
                    CorrectAnswer = ValidateAnswer(thisQuestionsAnswerList[0]);
                    break;
                case "B":
                    CorrectAnswer = ValidateAnswer(thisQuestionsAnswerList[1]);
                    break;
                case "C":
                    CorrectAnswer = ValidateAnswer(thisQuestionsAnswerList[2]);
                    break;
                case "D":
                    CorrectAnswer = ValidateAnswer(thisQuestionsAnswerList[3]);
                    break;
                case "E":
                    game.Run();
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
