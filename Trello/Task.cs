using System;
namespace Trello
{
    public class Task
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public User user { get; set; }
        public TaskStatus TaskStatus { get; set; }
    }

    public enum TaskStatus
    {
        ToDo,
        OnTeacher,
        OnStudent,
        Done
    }

}
