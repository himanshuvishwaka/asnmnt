using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json;

class TaskManager
{
    private List<Task> tasks;
    private string file = "C:\\Users\\himanshu\\source\\repos\\tasks.json";

    public TaskManager()
    {
        tasks = new List<Task>();
    }

    public void AddTask(string name, string description, DateTime dueDate)
    {
        tasks.Add(new Task(name, description, dueDate));
    }

    public void ViewTasks()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks found.");
            return;
        }

        foreach (var task in tasks)
        {
            Console.WriteLine($"Task Name: {task.Name}");
            Console.WriteLine($"Description: {task.Description}");
            Console.WriteLine($"Due Date: {task.DueDate}");
            Console.WriteLine($"Status: {(task.IsCompleted ? "Completed" : "Pending")}");
            Console.WriteLine("------------------------");
        }
    }

    public void MarkCompleted(int taskId)
    {
        if (taskId < 1 || taskId > tasks.Count)
        {
            Console.WriteLine("Invalid task ID.");
            return;
        }

        tasks[taskId - 1].IsCompleted = true;
    }

    public void DeleteTask(int taskId)
    {
        if (taskId < 1 || taskId > tasks.Count)
        {
            Console.WriteLine("Invalid task ID.");
            return;
        }

        tasks.RemoveAt(taskId - 1);
    }

    public void SaveTasks()
    {
        try
        {
            string jsonData = System.Text.Json.JsonSerializer.Serialize(tasks);
            File.WriteAllText(file, jsonData);
            Console.WriteLine("Tasks saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while saving tasks: {ex.Message}");
        }
    }

    public void LoadTasks()
    {
        if (File.Exists(file))
        {
            try
            {
                string jsonData = File.ReadAllText(file);
                tasks = System.Text.Json.JsonSerializer.Deserialize<List<Task>>(jsonData);
                Console.WriteLine("Tasks loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while loading tasks: {ex.Message}");
            }
        }
    }

    public void SortTasksByDueDate()
    {
        tasks = tasks.OrderBy(task => task.DueDate).ToList();
    }

    public void FilterTasksByCompletionStatus(bool showCompleted)
    {
        tasks = tasks.Where(task => task.IsCompleted == showCompleted).ToList();
    }
}

class Task
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }

    public Task(string name, string description, DateTime dueDate)
    {
        Name = name;
        Description = description;
        DueDate = dueDate;
        IsCompleted = false;
    }
}

class Program
{
    static void Main()
    {
        TaskManager taskManager = new TaskManager();

        // Load tasks from file on application start
        taskManager.LoadTasks();

        while (true)
        {
            Console.WriteLine("Task Management Application");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. View Tasks");
            Console.WriteLine("3. Mark Completed");
            Console.WriteLine("4. Delete Task");
            Console.WriteLine("5. Save Tasks");
            Console.WriteLine("6. Sort Tasks by Due Date");
            Console.WriteLine("7. Filter Tasks by Completion Status");
            Console.WriteLine("8. Exit");
            Console.Write("Enter your choice: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Write("Enter task name: ");
                    string name = Console.ReadLine();
                    Console.Write("Enter task description: ");
                    string description = Console.ReadLine();
                    Console.Write("Enter task due date (YYYY-MM-DD): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime dueDate))
                    {
                        taskManager.AddTask(name, description, dueDate);
                        Console.WriteLine("Task added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid due date format.");
                    }
                    break;
                case "2":
                    taskManager.ViewTasks();
                    break;
                case "3":
                    Console.Write("Enter task ID to mark as completed: ");
                    if (int.TryParse(Console.ReadLine(), out int taskId))
                    {
                        taskManager.MarkCompleted(taskId);
                        Console.WriteLine("Task marked as completed.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid task ID format.");
                    }
                    break;
                case "4":
                    Console.Write("Enter task ID to delete: ");
                    if (int.TryParse(Console.ReadLine(), out int taskIdToDelete))
                    {
                        taskManager.DeleteTask(taskIdToDelete);
                        Console.WriteLine("Task deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid task ID format.");
                    }
                    break;
                case "5":
                    taskManager.SaveTasks();
                    break;
                case "6":
                    taskManager.SortTasksByDueDate();
                    Console.WriteLine("Tasks sorted by due date.");
                    break;
                case "7":
                    Console.Write("Show completed tasks? (y/n): ");
                    string showCompletedInput = Console.ReadLine();
                    bool showCompleted = showCompletedInput.ToLower() == "y";
                    taskManager.FilterTasksByCompletionStatus(showCompleted);
                    Console.WriteLine($"Tasks filtered by completion status (Show Completed: {showCompleted}).");
                    break;
                case "8":
                    taskManager.SaveTasks(); // Save tasks before exiting
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Console.WriteLine("------------------------");
        }
    }
}
