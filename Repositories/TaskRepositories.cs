using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ToDoList.Models;

namespace ToDoList.Repositories
{
    public class FileTaskRepository : ITaskRepositories
    {
        private readonly string _filePath;

        public FileTaskRepository(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        private List<ToDoTask> LoadTasksFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return new List<ToDoTask>();
            }

            try
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<ToDoTask>>(json) ?? new List<ToDoTask>();
            }
            catch (JsonException)
            {
                Console.WriteLine("Error reading tasks from file. File may be corrupted.");
                return new List<ToDoTask>();
            }
        }

        private void SaveTasksToFile(List<ToDoTask> tasks)
        {
            string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }


        public List<ToDoTask> GetAllTasks()
        {
            return LoadTasksFromFile();
        }

        public ToDoTask GetTaskById(int id)
        {
            return LoadTasksFromFile().FirstOrDefault(t => t.Id == id);
        }

        public void AddTask(ToDoTask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            if (string.IsNullOrWhiteSpace(task.Title))
            {
                throw new ArgumentException("Task title cannot be null or empty.");
            }

            List<ToDoTask> tasks = LoadTasksFromFile();
            task.Id = GetNextId();
            tasks.Add(task);
            SaveTasksToFile(tasks);
        }

        public int GetNextId()
        {
            List<ToDoTask> tasks = LoadTasksFromFile();
            if (tasks.Count == 0)
            {
                return 1;
            }
            return tasks.Max(t => t.Id) + 1;
        }



        public void UpdateTask(ToDoTask updatedTask)
        {
            if (updatedTask == null)
            {
                throw new ArgumentNullException(nameof(updatedTask));
            }

            if (string.IsNullOrWhiteSpace(updatedTask.Title))
            {
                throw new ArgumentException("Task title cannot be null or empty.");
            }

            List<ToDoTask> tasks = LoadTasksFromFile();
            ToDoTask existingTask = tasks.FirstOrDefault(t => t.Id == updatedTask.Id);
            if (existingTask != null)
            {
                existingTask.Title = updatedTask.Title;
                existingTask.Description = updatedTask.Description;
                existingTask.DueDate = updatedTask.DueDate;
                existingTask.TaskPriority = updatedTask.TaskPriority;
                SaveTasksToFile(tasks);
            }
        }

        public void DeleteTask(int id)
        {
            List<ToDoTask> tasks = LoadTasksFromFile();

            if (!tasks.Any(t => t.Id == id))
            {
                throw new ArgumentException("No task found with the specified ID.");
            }

            tasks.RemoveAll(t => t.Id == id);
            SaveTasksToFile(tasks);
        }
    }
}