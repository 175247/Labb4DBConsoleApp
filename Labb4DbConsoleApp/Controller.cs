using System;
using System.Collections.Generic;
using System.Linq;

namespace Labb4DbConsoleApp
{
    class Controller
    {
        private GameContext modelContext;
        private AddNewDataView viewAddNewData;
        private DeleteQuestionView deleteQuestionView;
        private MainMenuView mainMenuView;
        private PlayGameView playGameView;
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
            mainMenuView = new MainMenuView
            {
                PlayGame = PlayGame,
                AddNewData = AddNewData,
                DeleteQuestion = DeleteQuestion
            };

            playGameView = new PlayGameView
            {
                questionsList = UpdateGameResourcesQuestions(),
                GetQuestions = UpdateGameResourcesQuestions,
                Navigation = MainMenu,
                ValidateAnswer = ValidateAnswer
            };

            deleteQuestionView = new DeleteQuestionView
            {
                modelContext = modelContext,
                GetQuestions = UpdateGameResourcesQuestions,
                PerformDeletion = PerformDeletion
            };
        }

        private void MainMenu()
        {
            UpdateGameResourcesQuestions();
            mainMenuView.UpdateDisplay();
        }

        private void PlayGame()
        {
            playGameView.UpdateDisplay();
        }

        private void AddNewData()
        {
            viewAddNewData = new AddNewDataView
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
            deleteQuestionView.UpdateDisplay();
        }

        public string ValidateInput()
        {
            bool acceptedString = false;
            string userInput = "Unable To Validate Input.";
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

        private void PerformDeletion(int choice)
        {
            try
            {
                var answersToDelete = modelContext.Answers.
                        Where(a => a.QuestionId == questionsList[choice - 1].id);

                foreach (var answer in answersToDelete)
                {
                    modelContext.Answers.Remove(answer);
                }

                modelContext.Questions.Remove(questionsList[choice - 1]);
                SaveChangesAndUpdateLists();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to delete question.\n" +
                    "Make sure the index exists.");
            }
            finally
            {
                MainMenu();
            }
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
            UpdateGameResourcesQuestions();
            MainMenu();
        }

        private List<Question> UpdateGameResourcesQuestions()
        {
            return questionsList = modelContext.Questions.ToList();
        }

        public bool ValidateAnswer(Answer answer)
        {
            return answer.IsCorrectAnswer;
        }
    }
}
