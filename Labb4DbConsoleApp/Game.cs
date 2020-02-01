using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace consoleLabb4Db
{
    public class Game
    {
        GameContext gameContext;
        List<Question> questionList;
        List<Answer> answersList;

        bool finishedQuestion = false;
        bool isCorrectAnswerSet = false;
        bool correctAnswer = false;
        public Game(GameContext gameContext)
        {
            gameContext.Database.EnsureCreated();
            this.gameContext = gameContext;
            questionList = gameContext.questions.ToList();
            answersList = gameContext.answers.ToList();
            /*
            var answer = new Answer();
            var question = new Question
            {
                id = Guid.NewGuid().ToString(),
                TheQuestion = "What is the colour of the sea?"
            };
            question.Answers = new List<Answer>();
            gameContext.answers.Add(new Answer
                {
                    id = Guid.NewGuid().ToString(),
                    TheAnswer = "Green",
                    IsCorrectAnswer = false,
                    QuestionId = question.id
                });
            gameContext.answers.Add(new Answer
            {
                id = Guid.NewGuid().ToString(),
                TheAnswer = "Red",
                IsCorrectAnswer = false,
                QuestionId = question.id
            });
            gameContext.answers.Add(new Answer
            {
                id = Guid.NewGuid().ToString(),
                TheAnswer = "Blue",
                IsCorrectAnswer = true,
                QuestionId = question.id
            });
            gameContext.answers.Add(new Answer
            {
                id = Guid.NewGuid().ToString(),
                TheAnswer = "Purple",
                IsCorrectAnswer = false,
                QuestionId = question.id,
            });
            gameContext.questions.Add(question);
            gameContext.SaveChanges();
            */
        }

        public void Run()
        {
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
                        AddQuestions();
                        break;
                    case 3:
                        DeleteQuestion(questionList);
                        break;
                    case 4:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid input...");
                        break;
                }
            } while (number != 4);
        }

        private void AddQuestions()
        {
            finishedQuestion = false;
            isCorrectAnswerSet = false;
            var newQuestion = new Question();
            var newAnswerList = new List<Answer>();
            newQuestion.id = Guid.NewGuid().ToString();
            newQuestion.Answers = newAnswerList;

            Console.Clear();
            do
            {
                Console.WriteLine($"[Q]: Enter or edit question.\n" +
                    $"[A]: Enter/Edit answer option A.\n" +
                    $"[B]: Enter/Edit answer option B.\n" +
                    $"[C]: Enter/Edit answer option C.\n" +
                    $"[D]: Enter/Edit answer option D.\n" +
                    $"[R]: Enter/Edit the correct answer to the question.\n" +
                    $"[F]: Finish question and upload to cloud.\n" +
                    $"Press [E] to exit.\n");

                string userInput = Console.ReadLine();

                switch (userInput.ToUpper())
                {
                    case "Q":
                        Console.WriteLine("Enter the question:");
                        newQuestion.TheQuestion = ValidateInput();
                        Console.Clear();
                        Console.WriteLine("Question set");
                        break;
                    case "A":
                        Console.WriteLine("Enter answer option A:");
                        AddNewAnswer(newQuestion, newAnswerList);
                        Console.WriteLine("Option A set");
                        break;
                    case "B":
                        Console.WriteLine("Enter answer option B:");
                        AddNewAnswer(newQuestion, newAnswerList);
                        Console.WriteLine("Option B set");
                        break;
                    case "C":
                        Console.WriteLine("Enter answer option C:");
                        AddNewAnswer(newQuestion, newAnswerList);
                        Console.WriteLine("Option C set");
                        break;
                    case "D":
                        Console.WriteLine("Enter answer option D:");
                        AddNewAnswer(newQuestion, newAnswerList);
                        Console.WriteLine("Option D set");
                        break;
                    case "R":
                        SetCorrectAnswer(newQuestion, newAnswerList);
                        Console.Clear();
                        Console.WriteLine("Correct answer set");
                        break;
                    case "F":
                        if (newQuestion.Answers.Contains(null))
                        {
                            Console.WriteLine("All answer options are not set.");
                            break;
                        }
                        else if (newQuestion.CorrectAnswer == null)
                        {
                            Console.WriteLine("The correct answer is not set.");
                            break;
                        }
                        else if (newQuestion.TheQuestion == null)
                        {
                            Console.WriteLine("The question is not set.");
                            break;
                        }
                        else
                        {
                            gameContext.questions.Add(newQuestion);
                            gameContext.SaveChanges();
                            questionList = gameContext.questions.ToList();
                            finishedQuestion = true;
                            Console.Clear();
                            Console.WriteLine("Question added to database.");
                        }
                        break;
                    case "E":
                        newQuestion = null;
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

        private void AddNewAnswer(Question newQuestion, List<Answer> newAnswerList)
        {
            newAnswerList.Add(new Answer
            {
                id = Guid.NewGuid().ToString(),
                TheAnswer = ValidateInput(),
                Question = newQuestion,
                QuestionId = newQuestion.id
            });
            Console.Clear();
        }

        private void SetCorrectAnswer(Question newQuestion, List<Answer> newAnswerList)
        {
            do
            {
                Console.WriteLine("Which is the correct answer?\n" +
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
        }

        private void DeleteQuestion(List<Question> questions)
        {
            Console.Clear();
            Console.WriteLine("Which question would you like to delete?");

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

                gameContext.answers.RemoveRange(answersToDelete);
                gameContext.questions.Remove(questions[choice - 1]);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input");
            }
            gameContext.SaveChanges();
            questionList = gameContext.questions.ToList();
            answersList = gameContext.answers.ToList();
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
        public void PlayGame()
        {
            questionList = gameContext.questions.ToList();
            answersList = gameContext.answers.ToList();

                correctAnswer = false;
            foreach (var question in questionList)
            {
                var answerList = question.Answers.ToList();

                do
                {
                    Console.WriteLine($"Q: {question.TheQuestion}\n\n");
                    foreach (var answer in answerList)
                    {
                        Console.WriteLine(answer.TheAnswer);
                    }

                    string userInput = Console.ReadLine();
                    switch (userInput.ToUpper())
                    {
                        case "A":
                            correctAnswer = ValidateAnswer(answerList[0]);
                            Console.WriteLine($"\n\n***THIS WAS THE ANSWER: {correctAnswer} ***\n\n");
                            break;
                        case "B":
                            correctAnswer = ValidateAnswer(answerList[1]);
                            break;
                        case "C":
                            correctAnswer = ValidateAnswer(answerList[2]);
                            break;
                        case "D":
                            correctAnswer = ValidateAnswer(answerList[3]);
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
                        Console.WriteLine("Correct!");
                        correctAnswer = true;
                    }
                } while (correctAnswer == false);
            }
            Console.Clear();
        }

        private bool ValidateAnswer(Answer answer)
        {
            return answer.IsCorrectAnswer;
        }
    }
}
