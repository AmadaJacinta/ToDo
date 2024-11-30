using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.Repositories
{
    public interface ITaskRepositories
    {
        List<ToDoTask> GetAllTasks();
        ToDoTask GetTaskById(int id);
        void AddTask(ToDoTask task);
        void UpdateTask(ToDoTask task);
        void DeleteTask(int id);
        int GetNextId(); // Метод для получения следующего ID
    }
}