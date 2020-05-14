using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;
using NLog;

namespace Trello
{

    public class Board
    {
        public delegate void StatusWasChanged();
        public event StatusWasChanged Status;   
        public List<Task> tasksList;
        public List<User> usersList;
        private static readonly NLog.Logger Logger = LogManager.GetCurrentClassLogger();



        public async void configureLog()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "trello.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");           
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);         
            NLog.LogManager.Configuration = config;
        }


        public async System.Threading.Tasks.Task CreateTaskAsync()
        {

            Console.WriteLine("Input task title:");
            var title = Console.ReadLine();

            Console.WriteLine("Input task description:");
            var descr = Console.ReadLine();
            Console.WriteLine("Input task member:");
            var userName = Console.ReadLine();
            Task task = new Task { Title = title, Description = descr, user = new User(userName) };
            try
            {
                using FileStream fs = new FileStream("tasks.json", FileMode.OpenOrCreate);
                await JsonSerializer.SerializeAsync(fs, task);
                fs.Close();
                Logger.Info("Task was created");

            }
            catch(NullReferenceException)
            {
                tasksList = new List<Task>
                {

                };
            }

            
            
        }
        public void CreateUser()
        {
            Console.WriteLine("Provide name of user");
            var userName = Console.ReadLine();
            if(!String.IsNullOrWhiteSpace(userName))
            {
                try
                {
                  usersList.Add(new User(userName));
                    var user = new User(userName);
                    Logger.Info("User was created");

                } catch(NullReferenceException)
                {
                    usersList = new List<User>
                    {

                    };
                }
                
            }

        }
        public async System.Threading.Tasks.Task ShowAllTasksAsync()
        {
            Console.WriteLine("Number |    Title     | Status |    Decsription");
            using (FileStream fs = new FileStream("tasks.json", FileMode.OpenOrCreate))
            {
                Task restoredTask = await JsonSerializer.DeserializeAsync<Task>(fs);
                Console.WriteLine($"{restoredTask.Title}  | {restoredTask.TaskStatus} | {restoredTask.Description}");
            }

            tasksList.ForEach(t => Console.WriteLine($"{tasksList.IndexOf(t)} |   {t.Title}   | {t.TaskStatus} | {t.Description} "));
            Logger.Info("Show all task method was called");

        }
        public void ShowAllUsers()
        {
            usersList.ForEach(u => Console.WriteLine($"{usersList}"));
        }
        private void ShowAllStatuses()
        {
            int i = 0;
            foreach (var enumValue in Enum.GetValues(typeof(TaskStatus)))
                Console.WriteLine($"{i++}. {enumValue}");
        }
        public void ChangeTaskStatus()
        {
            ShowAllTasksAsync();

            Console.WriteLine("Input task number:");
            var numberStr = Console.ReadLine();

            int taskNumber;
            if (!int.TryParse(numberStr, out taskNumber))
                Console.WriteLine("Incorrect task number!");

            ShowAllStatuses();

            Console.WriteLine("Input status number:");
            var statusStr = Console.ReadLine();

            int statusNumber;
            if (!int.TryParse(statusStr, out statusNumber))
                Console.WriteLine("Incorrect status number!");

            tasksList[taskNumber].TaskStatus = (TaskStatus)statusNumber;
            if(tasksList[taskNumber].user != null)
            {
                Status();
                Console.WriteLine("Status of task was changed");
            }

            ShowAllTasksAsync();

        }

        public void ChangeName()
        {
            ShowAllTasksAsync();

            Console.WriteLine("Input task number:");
            var numberStr = Console.ReadLine();

            int taskNumber;
            if (!int.TryParse(numberStr, out taskNumber))
                Console.WriteLine("Incorrect task number!");

            Console.WriteLine("Input new title for task:");
            var newTitle = Console.ReadLine();

            if(!String.IsNullOrWhiteSpace(newTitle))
            {
                tasksList[taskNumber].Title = new string(newTitle);
            } else
            {
                Console.WriteLine("Please provide new title that is not empty and doen't contain spaces");
            }

            ShowAllTasksAsync();

        }
        public void ChangeDescription()
        {
            ShowAllTasksAsync();

            Console.WriteLine("Input task number:");
            var numberStr = Console.ReadLine();

            int taskNumber;
            if (!int.TryParse(numberStr, out taskNumber))
                Console.WriteLine("Incorrect task number!");

            ShowAllStatuses();

            Console.WriteLine("Input new decription:");
            var newDescription = Console.ReadLine();

            if(!String.IsNullOrWhiteSpace(newDescription)) {
                tasksList[taskNumber].Description = new string(newDescription);
            } else
            {
                Console.WriteLine("Please provide new decription that is not empty and doen't contain spaces");
            }

            ShowAllTasksAsync();
        }
        public void ChangeAssignee()
        {
            ShowAllTasksAsync();

            Console.WriteLine("Input  number:");
            var numberStr = Console.ReadLine();

            int taskNumber;
            if (!int.TryParse(numberStr, out taskNumber))
                Console.WriteLine("Incorrect task number!");

            ShowAllUsers();

            Console.WriteLine("Input user number:");
            var userStr = Console.ReadLine();

            int userNumber;
            if (!int.TryParse(userStr, out userNumber))
                Console.WriteLine("Incorrect user number!");

            tasksList[taskNumber].user = usersList[userNumber];

            ShowAllTasksAsync();
        }

        public void ShowTaskByStatus()
        {
            ShowAllStatuses();

            Console.WriteLine("Input status number:");
            var statusStr = Console.ReadLine();

            int statusNumber;
            if (!int.TryParse(statusStr, out statusNumber))
                Console.WriteLine("Incorrect status number!");

            Console.WriteLine(tasksList.Where(t => t.TaskStatus == (TaskStatus)statusNumber));
        }

        public void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("Menu \n 1. Create task \n 2. Change task status \n 3. Show all tasks \n 4. Change title \n 5. Change description \n 6. Show task of selected status ");

                var key = Console.ReadKey().KeyChar;

                switch (key)
                {
                    case '1':
                        CreateTaskAsync();
                        break;

                    case '2':
                        ChangeTaskStatus();
                        break;

                    case '3':
                        ShowAllTasksAsync();
                        break;
                    case '4':
                        ChangeName();
                            break;
                    case '5':
                        ChangeDescription();
                        break;
                    case '6':
                        ShowTaskByStatus();
                        break;

                    default:
                        return;
                }
            }
        }

    }
}