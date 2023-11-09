using System.Text.Json;

interface CliTodoApp
{
    
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsDone { get; set; }
    }
    
    class TaskManager
    {
        private List<TaskItem> tasks;
        private string Filename = "data.json";
        
        public TaskManager()
        {
            tasks = new List<TaskItem>();
        }
        
        public void AddTask(string Title)
        {
            int newId = tasks.Count + 1;
            TaskItem newTask = new TaskItem
            {
                Id = newId,
                Title = Title,
                IsDone = false
            };
            tasks.Add(newTask);
        }
        
        public void RemoveTask(int Id)
        {
            TaskItem selected = tasks.FirstOrDefault(data => data.Id == Id);
            
            if (selected != null)
            {
                tasks.Remove(selected);
                Console.WriteLine($"Task with id {Id} successfuly deleted..");
            }
            else
            {
                Console.WriteLine("No task found with id given.");
            }
        }
        
        public void ChangeStatus(int Id, bool Status)
        {
            TaskItem selected = tasks.FirstOrDefault(data => data.Id == Id);
            if (selected != null)
            {
                selected.IsDone = Status;
                Console.WriteLine("Task status successfuly changed.");
            }
            else
            {
                Console.WriteLine("No task found with id given.");
            }
        }
        
        public void ShowTask()
        {
            string format = "{0,-5} | {1,-25} | {2,-15}";
            Console.WriteLine(string.Format(format, "No", "Name", "Status"));
            Console.WriteLine(new string('-', 50));

            foreach (var task in tasks)
            {
                string status = task.IsDone ? "Done" : "On Progress";
                Console.WriteLine(string.Format(format, task.Id, task.Title, status));
            }
        }
        
        public void SaveTask ()
        {
            string json = JsonSerializer.Serialize(tasks);
            File.WriteAllText(Filename, json);
        }
        
        public void LoadTask ()
        {
            if (File.Exists(Filename))
            {
                string json = File.ReadAllText(Filename);
                tasks = JsonSerializer.Deserialize<List<TaskItem>>(json);
            }
        }
    }
    
    class Program 
    {
        static void Main(string[] args)
        {
            TaskManager taskManager = new TaskManager();
            taskManager.LoadTask();
            
            Console.WriteLine("Welcome to To-do App writen on c-sharp");
            while (true)
            {
                Console.WriteLine("Use [E]xit [L]ist [A]dd [R]emove [S]ave [C]hange");
                Console.Write(">> ");
                string choice = Console.ReadLine();
                
                switch (choice.ToLower())
                {
                    case "a":
                    case "add":
                        Console.Write("Enter title name: ");
                        string title = Console.ReadLine();
                        if (title.Length >= 3)
                        {
                            taskManager.AddTask(title);
                            Console.WriteLine("Task Successfuly added.");
                        }
                        else
                        {
                            Console.WriteLine("Title must >= 3 character.");
                        }
                        break;
                    case "l":
                    case "list":
                        taskManager.ShowTask();
                        break;
                    case "e":
                    case "exit":
                        Console.WriteLine("Saving all tasks before exiting");
                        taskManager.SaveTask();
                        Console.WriteLine("All tasks saved. Exiting program.");
                        Environment.Exit(0);
                        break;
                    case "s":
                    case "save":
                        Console.WriteLine("Saving all task.");
                        taskManager.SaveTask();
                        Console.WriteLine("Saved.");
                        break;
                    case "r":
                    case "remove":
                        Console.Write("Id task to be deleted: ");
                        string input = Console.ReadLine();
                        int id;
                        if (int.TryParse(input, out id))
                        {
                            if (id >= 1)
                            {
                                taskManager.RemoveTask(id);
                            }
                            else
                            {
                                Console.WriteLine($"Id must not {id}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Please input a number!");
                        }
                        break;
                    case "c":
                    case "change":
                        Console.Write("Id task to be changed: ");
                        string i = Console.ReadLine();
                        Console.Write("Status task true/false: ");
                        string i2 = Console.ReadLine();
                        int id2;
                        bool status;
                        if (int.TryParse(i, out id2) && bool.TryParse(i2, out status))
                        {
                            if (id2 >= 1)
                            {
                                taskManager.ChangeStatus(id2, status);
                            }
                            else
                            {
                                Console.WriteLine($"Id must not {id2}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Please id as a number or status a boolean!");
                        }
                        break;
                    default:
                        Console.WriteLine("Command not found.");
                        break;
                }
            }
        }
    }
}