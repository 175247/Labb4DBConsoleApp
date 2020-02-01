using System;
using System.Collections.Generic;
using System.Text;

namespace Labb4DbConsoleApp
{
    public class GameClient
    {
        public enum ErrorCode { QuestionIsSet, AnswersAreSet, AnswerOptionsNotSet, CorrectAnswerIsSet, CorrectAnswerNotSet, QuestionNotSet }

        GameContext gameContext;
        GameClient gameClient;
        List<Question> questionList;
        List<Answer> answersList;

        bool finishedQuestion = false;
        bool isCorrectAnswerSet = false;
        bool correctAnswer = false;
        public Game(GameContext gameContext, GameClient gameClient)
        {
            gameContext.Database.EnsureCreated();
            this.gameContext = gameContext;
            this.gameClient = gameClient;
            questionList = gameContext.questions.ToList();
            answersList = gameContext.answers.ToList();
        }

        public void PlayGame()
        {
            Console.Clear();
            questionList = gameContext.questions.ToList();
            answersList = gameContext.answers.ToList();
            correctAnswer = false;

            foreach (var question in questionList)
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
                            correctAnswer = ValidateAnswer(thisQuestionsAnswerList[0]);
                            break;
                        case "B":
                            correctAnswer = ValidateAnswer(thisQuestionsAnswerList[1]);
                            break;
                        case "C":
                            correctAnswer = ValidateAnswer(thisQuestionsAnswerList[2]);
                            break;
                        case "D":
                            correctAnswer = ValidateAnswer(thisQuestionsAnswerList[3]);
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
                        Console.WriteLine("Try again! Not the correct answer!");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Correct!");
                        correctAnswer = true;
                    }
                } while (correctAnswer == false);
            }
            Console.WriteLine($"End of questions. Good job!");
            Thread.Sleep(3000);
            Console.Clear();
        }

        private bool ValidateAnswer(Answer answer)
        {
            return answer.IsCorrectAnswer;
        }

        public void AddQuestions()
        {
            finishedQuestion = false;
            isCorrectAnswerSet = false;
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
                            newQuestion.TheQuestion = ValidateInput();
                            Console.Clear();
                            Console.WriteLine("Question set");
                            isQuestionSet = true;
                        }
                        else
                        {

                            Console.Clear();
                            Console.WriteLine("The question is already set.");
                            DisplayErrorCodes(ErrorCode.QuestionIsSet);
                        }
                        break;
                    case "A":
                        if (isAllAnswersSet == false)
                        {
                            AddAnswers(newQuestion, newAnswerList);
                            isAllAnswersSet = true;
                        }
                        else
                        {
                            DisplayErrorCodes(ErrorCode.AnswersAreSet);
                        }
                        break;
                    case "R":
                        if (newQuestion.Answers.Count == 0)
                        {
                            DisplayErrorCodes(ErrorCode.AnswerOptionsNotSet);
                        }
                        else if (isCorrectAnswerSet == true)
                        {
                            DisplayErrorCodes(ErrorCode.CorrectAnswerIsSet);
                        }
                        else
                        {
                            SetCorrectAnswer(newQuestion, newAnswerList);
                        }
                        break;
                    case "F":
                        if (isAllAnswersSet == false)
                        {
                            DisplayErrorCodes(ErrorCode.AnswerOptionsNotSet);
                            break;
                        }
                        else if (newQuestion.CorrectAnswer == null)
                        {
                            DisplayErrorCodes(ErrorCode.CorrectAnswerNotSet);
                            break;
                        }
                        else if (newQuestion.TheQuestion == null)
                        {
                            DisplayErrorCodes(ErrorCode.QuestionNotSet);
                            break;
                        }
                        else
                        {
                            gameContext.questions.Add(newQuestion);

                            SaveChangesAndUpdateLists();
                            finishedQuestion = true;
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
            } while (finishedQuestion == false || isCorrectAnswerSet == false);
        }

        private void AddAnswers(Question newQuestion, List<Answer> newAnswerList)
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

        private void SetCorrectAnswer(Question newQuestion, List<Answer> newAnswerList)
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
                        isCorrectAnswerSet = true;
                        break;
                    case "B":
                        newAnswerList[1].IsCorrectAnswer = true;
                        newQuestion.CorrectAnswer = newAnswerList[1];
                        isCorrectAnswerSet = true;
                        break;
                    case "C":
                        newAnswerList[2].IsCorrectAnswer = true;
                        newQuestion.CorrectAnswer = newAnswerList[2];
                        isCorrectAnswerSet = true;
                        break;
                    case "D":
                        newAnswerList[3].IsCorrectAnswer = true;
                        newQuestion.CorrectAnswer = newAnswerList[3];
                        isCorrectAnswerSet = true;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid answer.");
                        break;
                }

            } while (isCorrectAnswerSet == false);
            Console.Clear();
            Console.WriteLine("Correct answer set\n");
        }

        public void DeleteQuestion(List<Question> questions)
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
            }
            catch (Exception)
            {
                Console.Clear();
                Console.WriteLine("Invalid input!\n" +
                    "Returning to main menu.\n");
            }
            SaveChangesAndUpdateLists();
        }
        private string ValidateInput()
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


        private void SaveChangesAndUpdateLists()
        {
            Console.WriteLine("Updating database...");
            gameContext.SaveChanges();
            questionList = gameContext.questions.ToList();
            answersList = gameContext.answers.ToList();
            Console.WriteLine("Finished!\n");
        }



        private void DisplayErrorCodes(ErrorCode errorCode)
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
