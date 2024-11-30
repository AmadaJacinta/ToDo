using System;
using System.Collections.Generic;
using ToDoList.Models;
using ToDoList.Repositories;

namespace ToDoList.Services
{
    public class TaskService
    {
        private readonly ITaskRepositories _taskRepository;

        public TaskService(ITaskRepositories taskRepository)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        }

        public List<ToDoTask> GetAllTasks()
        {
            return _taskRepository.GetAllTasks();
        }

        public ToDoTask GetTaskById(int id)
        {
            return _taskRepository.GetTaskById(id);
        }


        public void AddTask(string title, string description, DateTime? dueDate, ToDoTask.Priority priority)
        {
            if (title == null)
            {
                throw new ArgumentException("Task title cannot be null or empty.");
            }

            var newTask = new ToDoTask(title, description, dueDate, priority);
            _taskRepository.AddTask(newTask);
        }




        public void UpdateTask(int id, string title, string description, DateTime? dueDate, ToDoTask.Priority priority)
        {

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Task title cannot be null or empty.");
            }

            ToDoTask existingTask = _taskRepository.GetTaskById(id);

            if (existingTask == null)
            {
                throw new ArgumentException("Task with the specified ID not found.");
            }
            existingTask.Title = title;
            existingTask.Description = description;
            existingTask.DueDate = dueDate;
            existingTask.TaskPriority = priority;



            _taskRepository.UpdateTask(existingTask);



        }


        public void DeleteTask(int id)
        {
            try
            {
                _taskRepository.DeleteTask(id);
            }
            catch (ArgumentException ex)
            {

                throw new ArgumentException($"Failed to delete task: {ex.Message}");

            }

        }
        public List<ToDoTask> GetFilteredTasks(DateTime? dueDateFilter, ToDoTask.Priority? priorityFilter)
        {
            var tasks = _taskRepository.GetAllTasks();

            if (dueDateFilter.HasValue)
            {
                tasks = tasks.Where(t => t.DueDate == dueDateFilter.Value).ToList();
            }

            if (priorityFilter.HasValue)
            {
                tasks = tasks.Where(t => t.TaskPriority == priorityFilter.Value).ToList();
            }

            return tasks;
        }

        public List<ToDoTask> GetSortedTasks(string sortBy = "DueDate", bool ascending = true)
        {
            var tasks = _taskRepository.GetAllTasks();

            switch (sortBy)
            {
                case "DueDate":
                    tasks = ascending
                        ? tasks.OrderBy(t => t.DueDate).ToList()
                        : tasks.OrderByDescending(t => t.DueDate).ToList();
                    break;
                case "Priority":
                    tasks = ascending
                        ? tasks.OrderBy(t => t.TaskPriority).ToList()
                        : tasks.OrderByDescending(t => t.TaskPriority).ToList();
                    break;
                default: // Сортировка по Id, если sortBy некорректный
                    tasks = ascending
                        ? tasks.OrderBy(t => t.Id).ToList()
                        : tasks.OrderByDescending(t => t.Id).ToList();
                    break;
            }

            return tasks;
        }


    }
}