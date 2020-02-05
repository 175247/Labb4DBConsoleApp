using System;
using System.Collections.Generic;
using System.Linq;

namespace Labb4DbConsoleApp
{
    class Controller
    {
        private GameContext modelContext;
        private ViewAddNewData viewAddNewData;
        private ViewDeleteQuestion viewDeleteQuestion;
        private ViewMainMenu viewMainMenu;
        private ViewPlayGame viewPlayGame;
        private List<Question> questionsList;

        public Controller(GameContext modelContext)
        {
            this.modelContext = modelContext;
        }

        public void Run()
        {
            Console.WriteLine("Starting up...");
            modelContext.Database.EnsureCreated();
            Initialize();
            MainMenu();
        }

        public void Initialize()
        {
            viewMainMenu = new ViewMainMenu
            {
                PlayGame = PlayGame,
                AddNewData = AddNewData,
                DeleteQuestion = DeleteQuestion
            };

            viewPlayGame = new ViewPlayGame
            {
                questionsList = UpdateGameResources_Questions(),
                GetQuestions = GetQuestions,
                Navigation = MainMenu,
                ValidateAnswer = ValidateAnswer
            };

            viewDeleteQuestion = new ViewDeleteQuestion
            {
                modelContext = modelContext,
                GetQuestions = GetQuestions,
                Navigation = SaveChangesAndUpdateLists
            };
        }

        private void MainMenu()
        {
            UpdateGameResources_Questions();
            viewMainMenu.UpdateDisplay();
        }

        private void PlayGame()
        {
            viewPlayGame.UpdateDisplay();
        }

        private void AddNewData()
        {
            viewAddNewData = new ViewAddNewData
            {
                newQuestion = new Question(),
                newAnswerList = new List<Answer>(),
                ValidateInput = ValidateInput,
                Callback = UploadToDatabase
            };
            viewAddNewData.UpdateDisplay();
        }

        private void DeleteQuestion()
        {
            viewDeleteQuestion.UpdateDisplay();
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
                    if (userInput.Length == 1)
                    {
                        acceptedString = true;
                        return userInput.ToUpper();
                    }
                    else
                    {
                        acceptedString = true;
                        return userInput;
                    }
                }
            }
            return userInput;
        }
        
        public void UploadToDatabase(Question newQuestion)
        {
            modelContext.Questions.Add(newQuestion);
            SaveChangesAndUpdateLists();
            Console.Clear();
            Console.WriteLine("Question added to database.\n");
        }

        private void SaveChangesAndUpdateLists()
        {
            modelContext.SaveChanges();
            UpdateGameResources_Questions();
            MainMenu();
        }

        private List<Question> UpdateGameResources_Questions()
        {
            Console.WriteLine("Loading resources...");
            return questionsList = modelContext.Questions.ToList();
        }

        public List<Question> GetQuestions()
        {
            return questionsList;
        }

        public bool ValidateAnswer(Answer answer)
        {
            return answer.IsCorrectAnswer;
        }
    }
}
