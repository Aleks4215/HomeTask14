using System;
namespace Trello
{
    public class User
    {
        public string Name { get; set; }

        public User(string name)
        {
            this.Name = name;
        }

        public void Message()
        {
            Console.WriteLine($"{Name} получил сообщение");
        }

    }
}
