using System;

namespace Trello
{
    class Program
    {
        static void Main(string[] args)
        {
            User alex = new User("Alex");
            User kolya = new User("Kolya");
            Board board = new Board();
            board.configureLog();
            board.Status += board.ChangeTaskStatus;
            board.ShowMainMenu();
            
        }
    }

}
