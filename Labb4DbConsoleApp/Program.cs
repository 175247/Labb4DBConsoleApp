using System;

namespace Labb4DbConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameContext = new GameContext();
            var game = new Game(gameContext);
            game.Run();
        }

        /*
         * Make sure all answers are deleted once their respective questions are.
         * Make sure there are no duplicate answers to the same question in the database.
         * Make a gameClient and separate text to minimize it, then call it with parameters if required.
         * Make sure all answers have a Questionid linked to it.
         * Currently not all do, could be a bug appearing if you don't restart "the game" ;) after adding a question already.
         * Drop all questions and answers and restart to attempt it.
         */
    }
}
