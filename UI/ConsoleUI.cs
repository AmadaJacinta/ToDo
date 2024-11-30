using System;
using System.Collections.Generic;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.UI
{
    public class ConsoleUI
    {
        private readonly TaskService _taskService;

        public ConsoleUI(TaskService taskService)
        {
            _taskService = taskService;
        }

        public void Run()
        {
            while (true)
            {
                DisplayMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTask();
                        break;
                    case "2":
                        ViewTasks();
                        break;
                    case "3":
                        UpdateTask();
                        break;
                    case "4":
                        DeleteTask();
                        break;
                    case "5":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }


        private void DisplayMenu()
        {
            Console.WriteLine("\nTODO List Menu:");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. View Tasks");
            Console.WriteLine("3. Update Task");
            Console.WriteLine("4. Delete Task");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");
        }

        private void AddTask()
        {
            Console.Write("Enter task title: ");
            string title = Console.ReadLine();

            Console.Write("Enter task description (optional): ");
            string description = Console.ReadLine();

            Console.Write("Enter due date (yyyy-MM-dd, or press Enter for no due date): ");
            DateTime? dueDate = null;
            if (DateTime.TryParse(Console.ReadLine(), out DateTime parsedDate))
            {
                dueDate = parsedDate;
            }


            Console.Write("Enter priority (Low, Medium, High, or press Enter for default Low):");
            ToDoTask.Priority priority;

            if (Enum.TryParse(Console.ReadLine(), true, out priority))
            {
                //
            }
            else
            {
                priority = ToDoTask.Priority.Low;
            }


            _taskService.AddTask(title, description, dueDate, priority);
            Console.WriteLine("Task added successfully!");


        }




        private void ViewTasks()
        {
            // Фильтрация
            DateTime? dueDateFilter = null;
            Console.Write("Filter by due date (yyyy-MM-dd, or press Enter for no filter): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime parsedDate))
            {
                dueDateFilter = parsedDate;
            }


            ToDoTask.Priority? priorityFilter = null;

            Console.Write("Filter by priority (Low, Medium, High or press Enter for no filter): ");
            if (Enum.TryParse<ToDoTask.Priority>(Console.ReadLine(), out var parsedPriority))
            {
                priorityFilter = parsedPriority;
            }






            // Сортировка
            Console.WriteLine("Sort by (DueDate, Priority, or press Enter for default Id):");
            string sortBy = Console.ReadLine();

            Console.WriteLine("Ascending (y/n, or press Enter for default ascending):");
            string ascendingStr = Console.ReadLine();
            bool ascending = ascendingStr.ToLower() != "n";

            List<ToDoTask> tasks = string.IsNullOrEmpty(sortBy) && dueDateFilter == null && priorityFilter == null
                    ? _taskService.GetSortedTasks(sortBy, ascending)
                    : _taskService.GetFilteredTasks(dueDateFilter, priorityFilter);



            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks found.");
            }
            else
            {
                Console.WriteLine("Tasks:");
                foreach (var task in tasks)
                {
                    Console.WriteLine(task);
                }

            }




        }


        private void UpdateTask()
        {
            Console.Write("Enter the ID of the task to update: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                ToDoTask existingTask = _taskService.GetTaskById(id);
                if (existingTask == null)
                {
                    Console.WriteLine("Task not found.");
                    return;
                }



                Console.Write($"Enter new title (current: {existingTask.Title}): ");
                string title = Console.ReadLine();
                if (string.IsNullOrEmpty(title)) title = existingTask.Title;

                Console.Write($"Enter new description (current: {existingTask.Description}): ");
                string description = Console.ReadLine();
                if (string.IsNullOrEmpty(description)) description = existingTask.Description;


                Console.Write($"Enter new due date (yyyy-MM-dd, or press Enter to keep current: {existingTask.DueDate}): ");

                DateTime? dueDate = null;
                string dueDateInput = Console.ReadLine();

                if (string.IsNullOrEmpty(dueDateInput))
                {
                    dueDate = existingTask.DueDate;
                }
                else if (DateTime.TryParse(dueDateInput, out DateTime parsedDueDate))
                {
                    dueDate = parsedDueDate;
                }





                Console.Write($"Enter new priority (current: {existingTask.TaskPriority}): ");
                if (Enum.TryParse<ToDoTask.Priority>(Console.ReadLine(), out var priority))
                {
                    _taskService.UpdateTask(id, title, description, dueDate, priority);

                }
                else
                {
                    _taskService.UpdateTask(id, title, description, dueDate, existingTask.TaskPriority);
                }




                Console.WriteLine("Task updated successfully!");


            }
            else
            {
                Console.WriteLine("Invalid task ID.");
            }

        }

        private void DeleteTask()
        {
            Console.Write("Enter the ID of the task to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    _taskService.DeleteTask(id);
                    Console.WriteLine("Task deleted successfully!");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Invalid task ID.");
            }
        }

    }
}